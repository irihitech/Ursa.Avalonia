using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
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
            var scroll = newValue.GetSelfAndVisualDescendants().OfType<ScrollViewer>().FirstOrDefault();
            if (_scroll is not null)
            {
                _disposable?.Dispose();
                _scroll = null;
            }
            _scroll = scroll;

            _disposable = _scroll?.GetObservable(ScrollViewer.OffsetProperty).Subscribe(OnScrollChanged);
            SetVisibility(Direction, _scroll?.Offset);
        }
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        _disposable?.Dispose();
    }

    protected override void OnClick()
    {
        if (_scroll is null) return;
        var vector = Direction switch
        {
            Position.Top => new Vector(0, double.NegativeInfinity),
            Position.Bottom => new Vector(0, double.PositiveInfinity),
            Position.Left => new Vector(double.NegativeInfinity, 0),
            Position.Right => new Vector(double.PositiveInfinity, 0),
            _ => new Vector(0, 0)
        };
        _scroll.SetCurrentValue(ScrollViewer.OffsetProperty, vector);
    }
    
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var scroll = Target.GetSelfAndVisualDescendants().OfType<ScrollViewer>().FirstOrDefault();
        if (_scroll is not null)
        {
            _disposable?.Dispose();
            _scroll = null;
        }
        _scroll = scroll;
        _disposable = _scroll?.GetObservable(ScrollViewer.OffsetProperty).Subscribe(OnScrollChanged);
        SetVisibility(Direction, _scroll?.Offset);
    }

    private void OnScrollChanged(Vector arg2)
    {
        SetVisibility(Direction, arg2);
    }

    private void SetVisibility(Position direction, Vector? vector)
    {
        if (vector is null || _scroll is null) return;
        if (direction == Position.Bottom && vector.Value.Y < _scroll.Extent.Height - _scroll.Bounds.Height)
        {
            IsVisible = true;
        }
        else if (direction == Position.Top && vector.Value.Y > 0)
        {
            IsVisible = true;
        }
        else if (direction == Position.Left && vector.Value.X > 0)
        {
            IsVisible = true;
        }
        else if (direction == Position.Right && vector.Value.X < _scroll.Extent.Width - _scroll.Bounds.Width)
        {
            IsVisible = true;
        }
        else
        {
            IsVisible = false;
        }
    }
}