using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Irihi.Avalonia.Shared.Contracts;
using Ursa.EventArgs;

namespace Ursa.Controls.OverlayShared;

public abstract class OverlayFeedbackElement: ContentControl
{
    public static readonly StyledProperty<bool> IsClosedProperty =
        AvaloniaProperty.Register<OverlayFeedbackElement, bool>(nameof(IsClosed), defaultValue: true);

    public bool IsClosed
    {
        get => GetValue(IsClosedProperty);
        set => SetValue(IsClosedProperty, value);
    }
    
    static OverlayFeedbackElement()
    {
        DataContextProperty.Changed.AddClassHandler<OverlayFeedbackElement, object?>((o, e) => o.OnDataContextChange(e));
        ClosedEvent.AddClassHandler<OverlayFeedbackElement>((o,e)=>o.OnClosed(e));
    }

    private void OnClosed(ResultEventArgs _)
    {
        SetCurrentValue(IsClosedProperty,true);
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
        token?.Register(() =>
        {
            Dispatcher.UIThread.Invoke(Close);
        });
        
        void OnCloseHandler(object? sender, ResultEventArgs? args)
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
    
    public abstract void Close();
}