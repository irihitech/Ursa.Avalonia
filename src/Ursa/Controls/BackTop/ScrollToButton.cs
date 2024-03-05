using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Irihi.Avalonia.Shared.Helpers;
using Ursa.Common;

namespace Ursa.Controls;

public class ScrollToButton: Button
{
    private ScrollViewer? _scroll;
    private IDisposable? _disposable;
    
    public static readonly StyledProperty<Control> TargetProperty = AvaloniaProperty.Register<ScrollToButton, Control>(
        nameof(Target));

    public Control Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public static readonly StyledProperty<Position> DirectionProperty = AvaloniaProperty.Register<ScrollToButton, Position>(
        nameof(Direction));

    public Position Direction
    {
        get => GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    static ScrollToButton()
    {
        TargetProperty.Changed.AddClassHandler<ScrollToButton, Control>((o,e)=>o.OnTargetChanged(e));
        DirectionProperty.Changed.AddClassHandler<ScrollToButton, Position>((o,e)=>o.OnDirectionChanged(e));
    }

    private void OnDirectionChanged(AvaloniaPropertyChangedEventArgs<Position> avaloniaPropertyChangedEventArgs)
    {
        if (_scroll is null) return;
        SetVisibility(avaloniaPropertyChangedEventArgs.NewValue.Value, _scroll.Offset);
    }

    private void OnTargetChanged(AvaloniaPropertyChangedEventArgs<Control> arg2)
    {
        _disposable?.Dispose();
        if (arg2.NewValue.Value is { } newValue)
        {
            var scroll = newValue.GetSelfAndLogicalDescendants().OfType<ScrollViewer>().FirstOrDefault();
            if (_scroll is not null)
            {
                _disposable?.Dispose();
            }
            _scroll = scroll;
            _disposable = ScrollViewer.OffsetProperty.Changed.AddClassHandler<ScrollViewer, Vector>(OnScrollChanged);
            SetVisibility(Direction, _scroll?.Offset);
        }
    }

    protected override void OnClick()
    {
        var vector = Direction switch
        {
            Position.Top => new Vector(0, double.NegativeInfinity),
            Position.Bottom => new Vector(0, double.PositiveInfinity),
            Position.Left => new Vector(double.NegativeInfinity, 0),
            Position.Right => new Vector(double.PositiveInfinity, 0),
            _ => new Vector(0, 0)
        };
        _scroll?.SetCurrentValue(ScrollViewer.OffsetProperty, vector);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var scroll = Target.GetSelfAndLogicalDescendants().OfType<ScrollViewer>().FirstOrDefault();
        if (_scroll is not null)
        {
            _disposable?.Dispose();
        }
        _scroll = scroll;
        _disposable = ScrollViewer.OffsetProperty.Changed.AddClassHandler<ScrollViewer, Vector>(OnScrollChanged);
        SetVisibility(Direction, _scroll?.Offset);
    }

    private void OnScrollChanged(ScrollViewer arg1, AvaloniaPropertyChangedEventArgs<Vector> arg2)
    {
        if (arg1 != _scroll) return;
        SetVisibility(Direction, arg2.NewValue.Value);
    }

    private void SetVisibility(Position direction, Vector? vector)
    {
        if (vector is null) return;
        if (direction == Position.Bottom && vector.Value.Y < 0)
        {
            IsVisible = true;
        }
        else if (direction == Position.Top && vector.Value.Y > 0)
        {
            IsVisible = true;
        }
        else if (direction == Position.Left && vector.Value.X < 0)
        {
            IsVisible = true;
        }
        else if (direction == Position.Right && vector.Value.X > 0)
        {
            IsVisible = true;
        }
        else
        {
            IsVisible = false;
        }
    }
}