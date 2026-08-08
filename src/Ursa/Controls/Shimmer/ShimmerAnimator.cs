using System.Diagnostics;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;

namespace Ursa.Controls.Shimmer;

/// <summary>
///     Shared implementation for shimmer-animated visuals: takes over one of the
///     target's brush properties with an animated gradient and runs the sweep
///     animation, throttled to ~30 fps and paused while hidden.
/// </summary>
internal sealed class ShimmerAnimator
{
    private static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(1.5);

    /// <summary>
    ///     Minimum interval between brush position updates (≈30 fps).
    /// </summary>
    private static readonly double FrameIntervalSeconds = 1.0 / 30.0;

    private readonly AvaloniaProperty<Color?> _baseColorProperty;
    private readonly AvaloniaProperty<IBrush?> _brushProperty;
    private readonly AvaloniaProperty<TimeSpan> _durationProperty;
    private readonly AvaloniaProperty<Color?> _highlightColorProperty;
    private readonly AvaloniaProperty<bool> _isActiveProperty;

    private readonly Visual _owner;

    private readonly LinearGradientBrush _shimmerBrush = new();
    private readonly AvaloniaProperty<double> _shimmerOffsetProperty;
    private CancellationTokenSource? _animationCts;
    private Task? _animationTask;
    private Color? _fallbackBaseColor;
    private SolidColorBrush? _inactiveBrush;
    private Color _inactiveBrushColor;
    private bool _isAttached;
    private long _lastBrushUpdateTimestamp;
    private bool _updatingBrush;

    /// <summary>
    ///     Creates an animator for <paramref name="owner" />, subscribes to its
    ///     property and lifetime changes, and applies the initial state.
    /// </summary>
    /// <param name="owner">The visual whose brush property is animated.</param>
    /// <param name="brushProperty">The <see cref="IBrush" /> property to take over.</param>
    /// <param name="baseColorProperty">The base (resting) color property.</param>
    /// <param name="highlightColorProperty">The highlight color property.</param>
    /// <param name="durationProperty">The sweep duration property.</param>
    /// <param name="isActiveProperty">The active flag property.</param>
    /// <param name="shimmerOffsetProperty">The internal gradient offset property.</param>
    public ShimmerAnimator(
        Visual owner,
        AvaloniaProperty<IBrush?> brushProperty,
        AvaloniaProperty<Color?> baseColorProperty,
        AvaloniaProperty<Color?> highlightColorProperty,
        AvaloniaProperty<TimeSpan> durationProperty,
        AvaloniaProperty<bool> isActiveProperty,
        AvaloniaProperty<double> shimmerOffsetProperty)
    {
        _owner = owner;
        _brushProperty = brushProperty;
        _baseColorProperty = baseColorProperty;
        _highlightColorProperty = highlightColorProperty;
        _durationProperty = durationProperty;
        _isActiveProperty = isActiveProperty;
        _shimmerOffsetProperty = shimmerOffsetProperty;

        // The animator and the owner reference each other; this self-cycle is
        // collectable by the GC once the control is no longer referenced, so no
        // explicit unsubscribe is required.
        owner.PropertyChanged += OnPropertyChanged;
        owner.AttachedToVisualTree += OnAttachedToVisualTree;
        owner.DetachedFromVisualTree += OnDetachedFromVisualTree;

        // Optimistic: an animator created after the owner was already attached
        // (e.g. runtime SetBrushProperty / code-behind after add) has missed the
        // AttachedToVisualTree event, so assume attached; the detach event
        // corrects the state.
        _isAttached = true;

        RebuildBrush();
        UpdateAnimationState();
    }

    // ── Property access ───────────────────────────────────────────────────────

    private Color? BaseColor
    {
        get => (Color?)_owner.GetValue(_baseColorProperty);
    }

    private Color? HighlightColor
    {
        get => (Color?)_owner.GetValue(_highlightColorProperty);
    }

    private TimeSpan Duration
    {
        get => _owner.GetValue(_durationProperty) is TimeSpan d ? d : default;
    }

    private bool IsActive
    {
        get => _owner.GetValue(_isActiveProperty) is bool b && b;
    }

    private double ShimmerOffset
    {
        get => _owner.GetValue(_shimmerOffsetProperty) is double o ? o : 0;
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        _isAttached = true;
        RebuildBrush();
        UpdateAnimationState();
    }

    private void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        _isAttached = false;
        StopAnimation();
    }

    private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == _baseColorProperty ||
            e.Property == _highlightColorProperty ||
            e.Property == _brushProperty)
            RebuildBrush();
        else if (e.Property == _durationProperty)
            StartAnimation();
        else if (e.Property == _isActiveProperty)
            OnIsActiveChanged();
        else if (e.Property == Visual.IsVisibleProperty)
            UpdateAnimationState();
        else if (e.Property == _shimmerOffsetProperty) UpdateBrushPosition();
    }

    // Note: Control.IsLoaded becomes true asynchronously (via Dispatcher.Post) and
    // has no change notification, so it cannot drive the animation state machine.
    // The visual tree attach event is synchronous and the rendering clock only
    // runs while attached, so it is the correct gate for starting the animation.

    // ── Brush construction ────────────────────────────────────────────────────

    /// <summary>
    ///     Rebuilds <see cref="_shimmerBrush" /> from the base/highlight colors and
    ///     applies it to the target's brush property (or a plain solid color when
    ///     inactive).
    /// </summary>
    private void RebuildBrush()
    {
        if (_updatingBrush)
            return;

        _updatingBrush = true;
        try
        {
            var baseColor = BaseColor ?? ResolveBaseColor();
            var highlightColor = HighlightColor ?? GetDefaultHighlightColor();

            _shimmerBrush.GradientStops.Clear();
            _shimmerBrush.GradientStops.Add(new GradientStop(baseColor, 0d));
            _shimmerBrush.GradientStops.Add(new GradientStop(highlightColor, 0.5d));
            _shimmerBrush.GradientStops.Add(new GradientStop(baseColor, 1d));

            UpdateBrushPosition();

            _owner.SetValue(_brushProperty, IsActive ? _shimmerBrush : GetInactiveBrush(baseColor));
        }
        finally
        {
            _updatingBrush = false;
        }
    }

    /// <summary>
    ///     Moves the gradient's start/end points to follow the shimmer offset and
    ///     requests a re-render.
    /// </summary>
    private void UpdateBrushPosition()
    {
        if (!IsActive)
            return;

        // Throttle re-renders: the sweep is linear, so only repaint every ~33 ms.
        var timestamp = Stopwatch.GetTimestamp();
        var elapsed = (timestamp - _lastBrushUpdateTimestamp) / (double)Stopwatch.Frequency;
        if (elapsed < FrameIntervalSeconds)
            return;
        _lastBrushUpdateTimestamp = timestamp;

        var offset = ShimmerOffset;
        _shimmerBrush.StartPoint = new RelativePoint(offset, 0.5, RelativeUnit.Relative);
        _shimmerBrush.EndPoint = new RelativePoint(offset + 1, 0.5, RelativeUnit.Relative);
        _owner.InvalidateVisual();
    }

    /// <summary>
    ///     Resolves the resting color when <see cref="BaseColor" /> is unset: the target
    ///     brush's solid color when available (cached, so later rebuilds — after the
    ///     brush has been replaced by the gradient — keep the original color), else the
    ///     theme default.
    /// </summary>
    private Color ResolveBaseColor()
    {
        if (_owner.GetValue(_brushProperty) is ISolidColorBrush solid)
        {
            _fallbackBaseColor = solid.Color;
            return solid.Color;
        }

        if (_fallbackBaseColor is { } cached)
            return cached;

        return GetDefaultBaseColor();
    }

    /// <summary>Resolves the default resting text color: theme text color, else black.</summary>
    private static Color GetDefaultBaseColor()
    {
        if (Application.Current?.TryFindResource("TextControlForeground", out var value) == true &&
            value is ISolidColorBrush brush)
            return brush.Color;

        return Colors.Black;
    }

    /// <summary>Returns a cached solid brush for the inactive state.</summary>
    private SolidColorBrush GetInactiveBrush(Color color)
    {
        if (_inactiveBrush is null || _inactiveBrushColor != color)
        {
            _inactiveBrush = new SolidColorBrush(color);
            _inactiveBrushColor = color;
        }

        return _inactiveBrush;
    }

    /// <summary>Resolves the default highlight color: theme accent, else white.</summary>
    private static Color GetDefaultHighlightColor()
    {
        if (Application.Current?.TryFindResource("SystemAccentColor", out var value) == true &&
            value is Color color)
            return color;

        return Colors.White;
    }

    // ── Animation ─────────────────────────────────────────────────────────────

    private void OnIsActiveChanged()
    {
        RebuildBrush();
        UpdateAnimationState();
    }

    /// <summary>
    ///     Starts or stops the animation based on the current active, visible and
    ///     loaded state, so a hidden control does not keep burning CPU on an
    ///     invisible animation.
    /// </summary>
    private void UpdateAnimationState()
    {
        if (IsActive && _owner.GetValue(Visual.IsVisibleProperty) && _isAttached)
            StartAnimation();
        else
            StopAnimation();
    }

    private void StartAnimation()
    {
        StopAnimation();

        if (!IsActive || !_owner.GetValue(Visual.IsVisibleProperty) || !_isAttached)
            return;

        var duration = Duration == default ? DefaultDuration : Duration;
        _animationCts = new CancellationTokenSource();

        var animation = new Animation
        {
            Duration = duration,
            IterationCount = IterationCount.Infinite,
            Children =
            {
                new KeyFrame { Cue = new Cue(0d), Setters = { new Setter(_shimmerOffsetProperty, -1d) } },
                new KeyFrame { Cue = new Cue(1d), Setters = { new Setter(_shimmerOffsetProperty, 2d) } }
            }
        };

        _animationTask = animation.RunAsync(_owner, _animationCts.Token);
    }

    private void StopAnimation()
    {
        if (_animationCts is null)
            return;

        var cts = _animationCts;
        var task = _animationTask;
        _animationCts = null;
        _animationTask = null;

        cts.Cancel();
        _ = ObserveAnimationTaskAsync(task, cts);
    }

    /// <summary>
    ///     Observes the animation task so a fault cannot surface as an unobserved
    ///     exception, and disposes the cancellation source only after the task
    ///     (which holds a registration on the token) has completed.
    /// </summary>
    private static async Task ObserveAnimationTaskAsync(Task? task, CancellationTokenSource cts)
    {
        try
        {
            if (task is not null)
                await task;
        }
        catch
        {
            // Cancellation completes the task normally; a fault here is best-effort
            // and must not crash the application.
        }
        finally
        {
            cts.Dispose();
        }
    }
}
