using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Ursa.Controls.OverlayShared;

namespace Ursa.Controls;

public class DialogResizerThumb: Thumb
{
    private OverlayFeedbackElement? _dialog;
    
    public static readonly StyledProperty<ResizeDirection> ResizeDirectionProperty = AvaloniaProperty.Register<DialogResizerThumb, ResizeDirection>(
        nameof(ResizeDirection));

    public ResizeDirection ResizeDirection
    {
        get => GetValue(ResizeDirectionProperty);
        set => SetValue(ResizeDirectionProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _dialog = this.FindLogicalAncestorOfType<OverlayFeedbackElement>();
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (_dialog is null) return;
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        var windowEdge = ResizeDirection switch
        {
            ResizeDirection.Top => WindowEdge.North,
            ResizeDirection.TopRight => WindowEdge.NorthEast,
            ResizeDirection.Right => WindowEdge.East,
            ResizeDirection.BottomRight => WindowEdge.SouthEast,
            ResizeDirection.Bottom => WindowEdge.South,
            ResizeDirection.BottomLeft => WindowEdge.SouthWest,
            ResizeDirection.Left => WindowEdge.West,
            ResizeDirection.TopLeft => WindowEdge.NorthWest,
            _ => throw new ArgumentOutOfRangeException()
        };
        _dialog.BeginResizeDrag(windowEdge, e);
    }
}