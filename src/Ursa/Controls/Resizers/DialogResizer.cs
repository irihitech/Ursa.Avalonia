using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class DialogResizer: TemplatedControl
{
    public const string PART_Top = "PART_Top";
    public const string PART_Bottom = "PART_Bottom";
    public const string PART_Left = "PART_Left";
    public const string PART_Right = "PART_Right";
    public const string PART_TopLeft = "PART_TopLeft";
    public const string PART_TopRight = "PART_TopRight";
    public const string PART_BottomLeft = "PART_BottomLeft";
    public const string PART_BottomRight = "PART_BottomRight";
    
    private Thumb? _top;
    private Thumb? _bottom;
    private Thumb? _left;
    private Thumb? _right;
    private Thumb? _topLeft;
    private Thumb? _topRight;
    private Thumb? _bottomLeft;
    private Thumb? _bottomRight;
    
    public static readonly StyledProperty<ResizeDirection> ResizeDirectionProperty = AvaloniaProperty.Register<DialogResizer, ResizeDirection>(
        nameof(ResizeDirection), ResizeDirection.All);

    /// <summary>
    /// Defines what direction the dialog is allowed to be resized.
    /// </summary>
    public ResizeDirection ResizeDirection
    {
        get => GetValue(ResizeDirectionProperty);
        set => SetValue(ResizeDirectionProperty, value);
    }
    
    static DialogResizer()
    {
        ResizeDirectionProperty.Changed.AddClassHandler<DialogResizer, ResizeDirection>((resizer, e) => resizer.OnResizeDirectionChanged(e));
    }

    private void OnResizeDirectionChanged(AvaloniaPropertyChangedEventArgs<ResizeDirection> args)
    {
        UpdateThumbVisibility(args.NewValue.Value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _top = e.NameScope.Find<Thumb>(PART_Top);
        _bottom = e.NameScope.Find<Thumb>(PART_Bottom);
        _left = e.NameScope.Find<Thumb>(PART_Left);
        _right = e.NameScope.Find<Thumb>(PART_Right);
        _topLeft = e.NameScope.Find<Thumb>(PART_TopLeft);
        _topRight = e.NameScope.Find<Thumb>(PART_TopRight);
        _bottomLeft = e.NameScope.Find<Thumb>(PART_BottomLeft);
        _bottomRight = e.NameScope.Find<Thumb>(PART_BottomRight);
        UpdateThumbVisibility(ResizeDirection);
    }

    private void UpdateThumbVisibility(ResizeDirection direction)
    {
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.Top), _top);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.Bottom), _bottom);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.Left), _left);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.Right), _right);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.TopLeft), _topLeft);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.TopRight), _topRight);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.BottomLeft), _bottomLeft);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.BottomRight), _bottomRight);
    }
}