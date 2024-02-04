using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Ursa.Common;
using Ursa.EventArgs;

namespace Ursa.Controls;

public abstract class DrawerControlBase: ContentControl
{
    internal bool CanClickOnMaskToClose { get; set; }

    
    public static readonly StyledProperty<Position> PositionProperty = AvaloniaProperty.Register<DrawerControlBase, Position>(
        nameof(Position));

    public Position Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsOpenProperty = AvaloniaProperty.Register<DrawerControlBase, bool>(
        nameof(IsOpen));

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public static readonly StyledProperty<bool> IsCloseButtonVisibleProperty =
        AvaloniaProperty.Register<DrawerControlBase, bool>(
            nameof(IsCloseButtonVisible));

    public bool IsCloseButtonVisible
    {
        get => GetValue(IsCloseButtonVisibleProperty);
        set => SetValue(IsCloseButtonVisibleProperty, value);
    }
    
    public static readonly RoutedEvent<ResultEventArgs> ClosedEvent = RoutedEvent.Register<DrawerControlBase, ResultEventArgs>(
        nameof(Closed), RoutingStrategies.Bubble);
    
    public event EventHandler<ResultEventArgs> Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }
    
    static DrawerControlBase()
    {
        DataContextProperty.Changed.AddClassHandler<DrawerControlBase, object?>((o, e) => o.OnDataContextChange(e));
    }

    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if(args.OldValue.Value is IDialogContext oldContext)
        {
            oldContext.RequestClose -= OnContextRequestClose;
        }
        if(args.NewValue.Value is IDialogContext newContext)
        {
            newContext.RequestClose += OnContextRequestClose;
        }
    }

    private void OnContextRequestClose(object sender, object? e)
    {
        RaiseEvent(new ResultEventArgs(ClosedEvent, e));
    }

    public Task<T?> ShowAsync<T>(CancellationToken? token = default)
    { 
        var tcs = new TaskCompletionSource<T?>();
        token?.Register(() =>
        {
            Dispatcher.UIThread.Invoke(CloseDrawer);
        });
        
        void OnCloseHandler(object sender, ResultEventArgs args)
        {
            if (args.Result is T result)
            {
                tcs.SetResult(result);
            }
            else
            {
                tcs.SetResult(default(T));
            }
            RemoveHandler(ClosedEvent, OnCloseHandler);
        }
        AddHandler(ClosedEvent, OnCloseHandler);
        return tcs.Task;
    }

    internal virtual void CloseDrawer()
    {
        RaiseEvent(new ResultEventArgs(ClosedEvent, null));
    }
}