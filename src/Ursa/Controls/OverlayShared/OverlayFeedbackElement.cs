using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;
using Ursa.EventArgs;

namespace Ursa.Controls.OverlayShared;

public abstract class OverlayFeedbackElement : ContentControl
{
    public static readonly StyledProperty<bool> IsClosedProperty =
        AvaloniaProperty.Register<OverlayFeedbackElement, bool>(nameof(IsClosed), true);

    public static readonly RoutedEvent<ResultEventArgs> ClosedEvent =
        RoutedEvent.Register<DrawerControlBase, ResultEventArgs>(
            nameof(Closed), RoutingStrategies.Bubble);

    private bool _resizeDragging;
    private bool _moveDragging;
    
    private Panel? _containerPanel;
    private Rect _resizeDragStartBounds;
    private Point _resizeDragStartPoint;
    
    private WindowEdge? _windowEdge;

    static OverlayFeedbackElement()
    {
        FocusableProperty.OverrideDefaultValue<OverlayFeedbackElement>(false);
        DataContextProperty.Changed.AddClassHandler<OverlayFeedbackElement, object?>((o, e) =>
            o.OnDataContextChange(e));
        ClosedEvent.AddClassHandler<OverlayFeedbackElement>((o, e) => o.OnClosed(e));
    }

    public bool IsClosed
    {
        get => GetValue(IsClosedProperty);
        set => SetValue(IsClosedProperty, value);
    }

    private void OnClosed(ResultEventArgs _)
    {
        SetCurrentValue(IsClosedProperty, true);
    }

    public event EventHandler<ResultEventArgs> Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogContext oldContext) oldContext.RequestClose -= OnContextRequestClose;
        if (args.NewValue.Value is IDialogContext newContext) newContext.RequestClose += OnContextRequestClose;
    }

    protected virtual void OnElementClosing(object? sender, object? args)
    {
        RaiseEvent(new ResultEventArgs(ClosedEvent, args));
    }

    private void OnContextRequestClose(object? sender, object? args)
    {
        RaiseEvent(new ResultEventArgs(ClosedEvent, args));
    }

    public Task<T?> ShowAsync<T>(CancellationToken? token = default)
    {
        var tcs = new TaskCompletionSource<T?>();
        token?.Register(() => { Dispatcher.UIThread.Invoke(Close); });

        void OnCloseHandler(object? sender, ResultEventArgs? args)
        {
            if (args?.Result is T result)
                tcs.SetResult(result);
            else
                tcs.SetResult(default);
            RemoveHandler(ClosedEvent, OnCloseHandler);
        }

        AddHandler(ClosedEvent, OnCloseHandler);
        return tcs.Task;
    }

    public abstract void Close();

    internal void BeginResizeDrag(WindowEdge windowEdge, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        _resizeDragging = true;
        _resizeDragStartPoint = e.GetPosition(this);
        _resizeDragStartBounds = Bounds;
        _windowEdge = windowEdge;
    }
    
    internal void BeginMoveDrag(PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        _resizeDragging = true;
        _resizeDragStartPoint = e.GetPosition(this);
        _resizeDragStartBounds = Bounds;
        _windowEdge = null;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _containerPanel = this.FindAncestorOfType<Panel>();
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        _resizeDragging = false;
    }

    protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
    {
        base.OnPointerCaptureLost(e);
        _resizeDragging = false;
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (!_resizeDragging || _windowEdge is null) return;
        var point = e.GetPosition(this);
        var diff = point - _resizeDragStartPoint;
        var left = Canvas.GetLeft(this);
        var top = Canvas.GetTop(this);
        var width = _windowEdge is WindowEdge.West or WindowEdge.NorthWest or WindowEdge.SouthWest
            ? Bounds.Width : _resizeDragStartBounds.Width;
        var height = _windowEdge is WindowEdge.North or WindowEdge.NorthEast or WindowEdge.NorthWest
            ? Bounds.Height : _resizeDragStartBounds.Height;
        var newBounds = CalculateNewBounds(left, top, width, height, diff, _containerPanel?.Bounds, _windowEdge.Value);
        Canvas.SetLeft(this, newBounds.Left);
        Canvas.SetTop(this, newBounds.Top);
        SetCurrentValue(WidthProperty, newBounds.Width);
        SetCurrentValue(HeightProperty, newBounds.Height);
    }

    private Rect CalculateNewBounds(double left, double top, double width, double height, Point diff, Rect? containerBounds,
        WindowEdge windowEdge)
    {
        if (containerBounds is not null)
        {
            var minX = windowEdge is WindowEdge.West or WindowEdge.NorthWest or WindowEdge.SouthWest
                ? -left
                : double.NegativeInfinity; 
            var minY = windowEdge is WindowEdge.North or WindowEdge.NorthEast or WindowEdge.NorthWest
                ? -top
                : double.NegativeInfinity;
            var maxX = containerBounds.Value.Width - left - MinWidth;
            var maxY = containerBounds.Value.Height - top - MinHeight;
            diff = new Point(MathHelpers.SafeClamp(diff.X, minX, maxX), MathHelpers.SafeClamp(diff.Y, minY, maxY));
        }
        switch (windowEdge)
        {
            case WindowEdge.North:
                top += diff.Y;
                height -= diff.Y;
                top = Math.Max(0, top);
                break;
            case WindowEdge.NorthEast:
                top += diff.Y;
                width += diff.X;
                height -= diff.Y;
                break;
            case WindowEdge.East:
                width += diff.X;
                break;
            case WindowEdge.SouthEast:
                width += diff.X;
                height += diff.Y;
                break;
            case WindowEdge.South:
                height += diff.Y;
                break;
            case WindowEdge.SouthWest:
                left += diff.X;
                width -= diff.X;
                height += diff.Y;
                break;
            case WindowEdge.West:
                left += diff.X;
                width -= diff.X;
                break;
            case WindowEdge.NorthWest:
                left += diff.X;
                top += diff.Y;
                width -= diff.X;
                height -= diff.Y;
                break;
        }
        return new Rect(left, top, width, height);
    }
}