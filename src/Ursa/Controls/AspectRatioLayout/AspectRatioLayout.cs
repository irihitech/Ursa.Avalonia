using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Metadata;

namespace Ursa.Controls;

public class AspectRatioLayout : TransitioningContentControl
{
    public static readonly StyledProperty<List<AspectRatioLayoutItem>> ItemsProperty =
        AvaloniaProperty.Register<AspectRatioLayout, List<AspectRatioLayoutItem>>(
            nameof(Items));

    public static readonly StyledProperty<double> AspectRatioToleranceProperty =
        AvaloniaProperty.Register<AspectRatioLayout, double>(
            nameof(AspectRatioTolerance), 0.2);

    private AspectRatioMode _currentAspectRatioMode;

    public static readonly DirectProperty<AspectRatioLayout, AspectRatioMode> CurrentAspectRatioModeProperty =
        AvaloniaProperty.RegisterDirect<AspectRatioLayout, AspectRatioMode>(
            nameof(CurrentAspectRatioMode), o => o.CurrentAspectRatioMode);

    private readonly Queue<bool> _history = new();

    static AspectRatioLayout()
    {
        UrsaCrossFade ursaCrossFade = new()
        {
            Duration = TimeSpan.FromSeconds(0.55),
            FadeInEasing = new QuadraticEaseInOut(),
            FadeOutEasing = new QuadraticEaseInOut()
        };
        PageTransitionProperty.OverrideDefaultValue<AspectRatioLayout>(ursaCrossFade);
    }

    public AspectRatioLayout()
    {
        Items = new List<AspectRatioLayoutItem>();
    }

    public AspectRatioMode CurrentAspectRatioMode
    {
        get => GetValue(CurrentAspectRatioModeProperty);
        set => SetAndRaise(CurrentAspectRatioModeProperty, ref _currentAspectRatioMode, value);
    }

    public static readonly StyledProperty<double> AspectRatioValueProperty =
        AvaloniaProperty.Register<AspectRatioLayout, double>(
            nameof(AspectRatioValue));

    public double AspectRatioValue
    {
        get => GetValue(AspectRatioValueProperty);
        set => SetValue(AspectRatioValueProperty, value);
    }

    protected override Type StyleKeyOverride => typeof(TransitioningContentControl);

    [Content]
    public List<AspectRatioLayoutItem> Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public double AspectRatioTolerance
    {
        get => GetValue(AspectRatioToleranceProperty);
        set => SetValue(AspectRatioToleranceProperty, value);
    }

    private void UpdateHistory(bool value)
    {
        _history.Enqueue(value);
        while (_history.Count > 3)
            _history.Dequeue();
    }

    private bool IsRightChanges()
    {
        //if (_history.Count < 3) return false;
        return _history.All(x => x) || _history.All(x => !x);
    }

    private double GetAspectRatio(Rect rect)
    {
        return Math.Round(Math.Truncate(Math.Abs(rect.Width)) / Math.Truncate(Math.Abs(rect.Height)), 3);
    }

    private AspectRatioMode GetScaleMode(Rect rect)
    {
        var scale = GetAspectRatio(rect);
        var absA = Math.Abs(AspectRatioTolerance);
        var h = 1d + absA;
        var v = 1d - absA;
        if (scale >= h) return AspectRatioMode.HorizontalRectangle;
        if (v < scale && scale < h) return AspectRatioMode.Square;
        if (scale <= v) return AspectRatioMode.VerticalRectangle;
        return AspectRatioMode.None;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ItemsProperty ||
            change.Property == AspectRatioToleranceProperty ||
            change.Property == BoundsProperty)
        {
            if (change.Property == BoundsProperty)
            {
                var o = (Rect)change.OldValue!;
                var n = (Rect)change.NewValue!;
                UpdateHistory(GetAspectRatio(o) <= GetAspectRatio(n));
                if (!IsRightChanges()) return;
                CurrentAspectRatioMode = GetScaleMode(n);
            }

            AspectRatioValue = GetAspectRatio(Bounds);
            var c =
                Items
                    .Where(x => x.IsUseAspectRatioRange)
                    .FirstOrDefault(x =>
                        x.StartAspectRatioValue <= AspectRatioValue
                        && AspectRatioValue <= x.EndAspectRatioValue);

            c ??= Items.FirstOrDefault(x => x.AcceptAspectRatioMode == GetScaleMode(Bounds));
            if (c == null)
            {
                if (Items.Count == 0) return;
                c = Items.First();
            }

            Content = c;
        }
    }
}