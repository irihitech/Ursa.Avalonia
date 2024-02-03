using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Ursa.Common;
using Ursa.EventArgs;

namespace Ursa.Controls;

public class DrawerControl: ContentControl
{
    public static readonly StyledProperty<Position> PositionProperty = AvaloniaProperty.Register<DrawerControl, Position>(
        nameof(Position));

    public Position Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsOpenProperty = AvaloniaProperty.Register<DrawerControl, bool>(
        nameof(IsOpen));

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }
    
    public static readonly RoutedEvent<ResultEventArgs> CloseEvent = RoutedEvent.Register<DrawerControl, ResultEventArgs>(
        "Close", RoutingStrategies.Bubble);
    
    public event EventHandler<ResultEventArgs> Close
    {
        add => AddHandler(CloseEvent, value);
        remove => RemoveHandler(CloseEvent, value);
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
            if (args is T result)
            {
                tcs.SetResult(result);
                Close -= OnCloseHandler;
            }
            else
            {
                tcs.SetResult(default(T));
                Close -= OnCloseHandler;
            }
        }

        this.Close += OnCloseHandler;
        return tcs.Task;
    }

    internal void CloseDrawer()
    {
        
    }
}