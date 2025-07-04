using System.Diagnostics;
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
/// Some basic assumptions: This should not be a regular SelectingItemsControl, because it does not support multiple selections.
/// Selection should not be exposed to the user, it is only used to determine which item is currently selected.
/// The manipulation of container selection should be simplified.
/// Scroll event of TargetContainer also triggers selection change. 
/// </summary>
public class Anchor: ItemsControl
{
    public static readonly StyledProperty<ScrollViewer?> TargetContainerProperty = AvaloniaProperty.Register<Anchor, ScrollViewer?>(
        nameof(TargetContainer));

    public ScrollViewer? TargetContainer
    {
        get => GetValue(TargetContainerProperty);
        set => SetValue(TargetContainerProperty, value);
    }

    public static readonly AttachedProperty<string?> AnchorIdProperty =
        AvaloniaProperty.RegisterAttached<Anchor, Visual, string?>("AnchorId");

    public static void SetAnchorId(Visual obj, string? value) => obj.SetValue(AnchorIdProperty, value);
    public static string? GetAnchorId(Visual obj) => obj.GetValue(AnchorIdProperty);
    

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<AnchorItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        var i = new AnchorItem();
        return i;
    }
    
    internal void ScrollToAnchor(string anchorId)
    {
        return;
        if (TargetContainer is null)
            return;
        var target = TargetContainer.GetVisualDescendants().FirstOrDefault(a=>Anchor.GetAnchorId(a) == anchorId);
        if (target is null) return;
        ScrollToAnchor(target);
    }

    private CancellationTokenSource _cts = new();
    private bool _scrollingFromSelection = false;
    
    private void ScrollToAnchor(Visual target)
    {
        if (TargetContainer is null)
            return;
        
        var targetPosition = target.TranslatePoint(new Point(0, 0), TargetContainer);
        if (targetPosition.HasValue)
        {
            var from = TargetContainer.Offset.Y;
            var to = TargetContainer.Offset.Y + targetPosition.Value.Y;
            if(to > TargetContainer.Extent.Height - TargetContainer.Bounds.Height)
            {
                to = TargetContainer.Extent.Height - TargetContainer.Bounds.Height;
            }
            if (from == to) return;
            Animation animation = new Animation()
            {
                Duration = TimeSpan.FromSeconds(0.3),
                Easing = new QuadraticEaseOut(),
                Children =
                {
                    new KeyFrame(){ 
                        Setters = 
                        {
                            new Setter(ScrollViewer.OffsetProperty, new Vector(0, from)),
                        },
                        Cue = new Cue(0.0)
                    },
                    new KeyFrame()
                    {
                        Setters =
                        {
                            new Setter(ScrollViewer.OffsetProperty, new Vector(0, to))
                        },
                        Cue = new Cue(1.0)
                    }
                
                }
            };
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            var token  = _cts.Token;
            token.Register(_ => _scrollingFromSelection = false, null);
            _scrollingFromSelection = true;
            animation.RunAsync(TargetContainer, token).ContinueWith(_ => _scrollingFromSelection = false, token);
        }
    }
    
    public void InvalidatePositions()
    {
        
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var items = this.GetVisualDescendants().OfType<AnchorItem>().ToList();
        var target = this.TargetContainer;
        if (target is null) return;
        var targetItems = target.GetVisualDescendants().Where(a => Anchor.GetAnchorId(a) is not null).ToList();
        var tops = targetItems.Select(a => (a.TransformToVisual(target)?.M32, GetAnchorId(a)));
        var isloaded = TargetContainer?.IsLoaded; 
        TargetContainer?.AddHandler(ScrollViewer.ScrollChangedEvent, OnScrollChanged);
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (_scrollingFromSelection) return;
        Debug.WriteLine("Scroll changed");
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        var source = (e.Source as Visual).GetContainerFromEventSource<AnchorItem>();
        if (source is null) return;
        if (_selectedContainer is not null)
        {
            _selectedContainer.IsSelected = false;
        }
        source.IsSelected = true;
        _selectedContainer = source;
        var target = TargetContainer?.GetVisualDescendants()
                                    .FirstOrDefault(a => Anchor.GetAnchorId(a) == source?.AnchorId);
        if (target is null) return;
        ScrollToAnchor(target);
    }

    /// <summary>
    /// This method is used to expose the protected CreateContainerForItemOverride method to the AnchorItem class.
    /// </summary>
    internal Control CreateContainerForItemOverride_INTERNAL(object? item, int index, object? recycleKey)
    {
        return CreateContainerForItemOverride(item, index, recycleKey);
    }
    
    internal bool NeedsContainerOverride_INTERNAL(object? item, int index, out object? recycleKey)
    {
        return NeedsContainerOverride(item, index, out recycleKey);
    }
    
    internal void PrepareContainerForItemOverride_INTERNAL(Control container, object? item, int index)
    {
        PrepareContainerForItemOverride(container, item, index);
    }

    internal void ContainerForItemPreparedOverride_INTERNAL(Control container, object? item, int index)
    {
        ContainerForItemPreparedOverride(container, item, index);
    }

    internal AnchorItem? _selectedContainer;
    
    
}