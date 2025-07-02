using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.VisualTree;

namespace Ursa.Controls;

public class Anchor: SelectingItemsControl
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
        if (TargetContainer is null)
            return;
        var target = TargetContainer.GetVisualDescendants().FirstOrDefault(a=>Anchor.GetAnchorId(a) == anchorId);
        if (target is null) return;
        ScrollToAnchor(target);
    }
    
    internal void ScrollToAnchor(Visual target)
    {
        if (TargetContainer is null)
            return;
        TargetContainer.Loaded += OnTargetLoaded;
        var targetPosition = target.TranslatePoint(new Point(0, 0), TargetContainer);
        if (targetPosition.HasValue)
        {
            var from = TargetContainer.Offset.Y;
            var to = TargetContainer.Offset.Y + targetPosition.Value.Y;
            if(to > TargetContainer.Extent.Height - TargetContainer.Bounds.Height)
            {
                to = TargetContainer.Extent.Height - TargetContainer.Bounds.Height;
            }
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
            animation.RunAsync(TargetContainer);
            // TargetContainer.Offset = TargetContainer.Offset.WithY(TargetContainer.Offset.Y + targetPosition.Value.Y);
        }
    }

    private void OnTargetLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is ScrollViewer scrollViewer)
        {
            scrollViewer.Loaded -= OnTargetLoaded;
            if (scrollViewer.Content is Visual target)
            {
                var anchorId = GetAnchorId(target);
                if (!string.IsNullOrEmpty(anchorId))
                {
                    ScrollToAnchor(anchorId);
                }
            }
        }
    }

    public void InvalidatePositions()
    {
        
    }
}