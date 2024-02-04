using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Ursa.EventArgs;

namespace Ursa.Controls.OverlayShared;

public abstract class OverlayFeedbackElement: ContentControl
{
    static OverlayFeedbackElement()
    {
        DataContextProperty.Changed.AddClassHandler<CustomDialogControl, object?>((o, e) => o.OnDataContextChange(e));
    }
    
    public static readonly RoutedEvent<ResultEventArgs> ClosedEvent = RoutedEvent.Register<DrawerControlBase, ResultEventArgs>(
        nameof(Closed), RoutingStrategies.Bubble);
    
    public event EventHandler<ResultEventArgs> Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }
    
    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogContext oldContext)
        {
            oldContext.RequestClose -= OnContextRequestClose;
        }
        if (args.NewValue.Value is IDialogContext newContext)
        {
            newContext.RequestClose += OnContextRequestClose;
        }
    }
    
    protected virtual void OnElementClosing(object sender, object? args)
    {
        RaiseEvent(new ResultEventArgs(ClosedEvent, args));
    }
    
    private void OnContextRequestClose(object sender, object? args)
    {
        RaiseEvent(new ResultEventArgs(ClosedEvent, args));
    }
    
    public Task<T?> ShowAsync<T>(CancellationToken? token = default)
    { 
        var tcs = new TaskCompletionSource<T?>();
        token?.Register(() =>
        {
            Dispatcher.UIThread.Invoke(Close);
        });
        
        void OnCloseHandler(object sender, ResultEventArgs? args)
        {
            if (args?.Result is T result)
            {
                tcs.SetResult(result);
            }
            else
            {
                tcs.SetResult(default);
            }
            RemoveHandler(ClosedEvent, OnCloseHandler);
        }

        AddHandler(ClosedEvent, OnCloseHandler);
        return tcs.Task;
    }
    
    protected static void SetVisibility(Button? button, bool visible)
    {
        if (button is not null) button.IsVisible = visible;
    }
    
    public abstract void Close();
}