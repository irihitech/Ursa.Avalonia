using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.VisualTree;
using Ursa.Common;

namespace Ursa.Controls;

/// <summary>
///     Some basic assumptions: This should not be a regular SelectingItemsControl, because it does not support multiple
///     selections.
///     Selection should not be exposed to the user, it is only used to determine which item is currently selected.
///     The manipulation of container selection should be simplified.
///     Scroll event of TargetContainer also triggers selection change.
/// </summary>
public class Anchor : ItemsControl
{
    public static readonly StyledProperty<ScrollViewer?> TargetContainerProperty =
        AvaloniaProperty.Register<Anchor, ScrollViewer?>(
            nameof(TargetContainer));

    public static readonly AttachedProperty<string?> IdProperty =
        AvaloniaProperty.RegisterAttached<Anchor, Visual, string?>("Id");

    private CancellationTokenSource _cts = new();

    private List<(string, double)> _positions = [];
    private bool _scrollingFromSelection;

    private AnchorItem? _selectedContainer;

    public ScrollViewer? TargetContainer
    {
        get => GetValue(TargetContainerProperty);
        set => SetValue(TargetContainerProperty, value);
    }

    public static void SetId(Visual obj, string? value)
    {
        obj.SetValue(IdProperty, value);
    }

    public static string? GetId(Visual obj)
    {
        return obj.GetValue(IdProperty);
    }

    public static readonly StyledProperty<double> TopOffsetProperty = AvaloniaProperty.Register<Anchor, double>(
        nameof(TopOffset));

    public double TopOffset
    {
        get => GetValue(TopOffsetProperty);
        set => SetValue(TopOffsetProperty, value);
    }
    
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<AnchorItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        var i = new AnchorItem();
        return i;
    }

    private void ScrollToAnchor(Visual target)
    {
        if (TargetContainer is null)
            return;

        var targetPosition = target.TranslatePoint(new Point(0, 0), TargetContainer);
        if (targetPosition.HasValue)
        {
            var from = TargetContainer.Offset.Y;
            var to = TargetContainer.Offset.Y + targetPosition.Value.Y - TopOffset;
            if (to > TargetContainer.Extent.Height - TargetContainer.Bounds.Height)
                to = TargetContainer.Extent.Height - TargetContainer.Bounds.Height;
            if (from == to) return;
            var animation = new Animation
            {
                Duration = TimeSpan.FromSeconds(0.3),
                Easing = new QuadraticEaseOut(),
                Children =
                {
                    new KeyFrame
                    {
                        Setters = { new Setter(ScrollViewer.OffsetProperty, new Vector(0, from)) },
                        Cue = new Cue(0.0)
                    },
                    new KeyFrame
                    {
                        Setters = { new Setter(ScrollViewer.OffsetProperty, new Vector(0, to)) },
                        Cue = new Cue(1.0)
                    }
                }
            };
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            token.Register(_ => _scrollingFromSelection = false, null);
            _scrollingFromSelection = true;
            animation.RunAsync(TargetContainer, token).ContinueWith(_ => _scrollingFromSelection = false, token);
        }
    }

    public void InvalidatePositions()
    {
        InvalidateAnchorPositions();
        MarkSelectedContainerByPosition();
    }

    internal void InvalidateAnchorPositions()
    {
        if (TargetContainer is null) return;
        var items = TargetContainer.GetVisualDescendants().Where(a => GetId(a) is not null);
        var positions = new List<(string, double)>();
        foreach (var item in items)
        {
            var anchorId = GetId(item);
            if (anchorId is null) continue;
            var position = item.TransformToVisual(TargetContainer)?.M32 + TargetContainer.Offset.Y;
            if (position.HasValue) positions.Add((anchorId, position.Value));
        }

        positions.Sort((a, b) => a.Item2.CompareTo(b.Item2));
        _positions = positions;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var target = TargetContainer;
        if (target is null) return;
        TargetContainer?.AddHandler(ScrollViewer.ScrollChangedEvent, OnScrollChanged);
        TargetContainer?.AddHandler(LoadedEvent, OnTargetContainerLoaded);
        if (TargetContainer?.IsLoaded == true) InvalidateAnchorPositions();
        MarkSelectedContainerByPosition();
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (_scrollingFromSelection) return;
        MarkSelectedContainerByPosition();
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        var source = (e.Source as Visual).GetContainerFromEventSource<AnchorItem>();
        if (source is null) return;
        MarkSelectedContainer(source);
        var target = TargetContainer?.GetVisualDescendants()
                                    .FirstOrDefault(a => GetId(a) == source.AnchorId);
        if (target is null) return;
        ScrollToAnchor(target);
    }

    /// <summary>
    ///     This method is used to expose the protected CreateContainerForItemOverride method to the AnchorItem class.
    /// </summary>
    internal Control CreateContainerForItemOverrideInternal(object? item, int index, object? recycleKey)
    {
        return CreateContainerForItemOverride(item, index, recycleKey);
    }

    internal bool NeedsContainerOverrideInternal(object? item, int index, out object? recycleKey)
    {
        return NeedsContainerOverride(item, index, out recycleKey);
    }

    internal void PrepareContainerForItemOverrideInternal(Control container, object? item, int index)
    {
        PrepareContainerForItemOverride(container, item, index);
    }

    internal void ContainerForItemPreparedOverrideInternal(Control container, object? item, int index)
    {
        ContainerForItemPreparedOverride(container, item, index);
    }

    internal void MarkSelectedContainer(AnchorItem? item)
    {
        var oldValue = _selectedContainer;
        var newValue = item;
        if (oldValue == newValue) return;
        _selectedContainer?.SetValue(AnchorItem.IsSelectedProperty, false);
        _selectedContainer = newValue;
        _selectedContainer?.SetValue(AnchorItem.IsSelectedProperty, true);
    }

    internal void MarkSelectedContainerByPosition()
    {
        if (TargetContainer is null) return;
        var top = TargetContainer.Offset.Y + TopOffset;
        var topAnchorId = _positions.LastOrDefault(a => a.Item2 <= top).Item1;
        if (topAnchorId is null) return;
        var item = this.GetVisualDescendants().OfType<AnchorItem>()
                       .FirstOrDefault(a => a.AnchorId == topAnchorId);
        if (item is null) return;
        MarkSelectedContainer(item);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        TargetContainer?.RemoveHandler(LoadedEvent, OnTargetContainerLoaded);
        TargetContainer?.RemoveHandler(ScrollViewer.ScrollChangedEvent, OnScrollChanged);
    }

    private void OnTargetContainerLoaded(object? sender, RoutedEventArgs e)
    {
        InvalidateAnchorPositions();
    }
}