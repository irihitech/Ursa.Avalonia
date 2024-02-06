using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Ursa.Common;

internal static class EventHelper
{
    public static void RegisterClickEvent(EventHandler<RoutedEventArgs> handler, params Button?[] buttons)
    {
        foreach (var button in buttons)
        {
            if(button is not null) button.Click += handler;
        }
    }
    
    public static void UnregisterClickEvent(EventHandler<RoutedEventArgs> handler, params Button?[] buttons)
    {
        foreach (var button in buttons)
        {
            if(button is not null) button.Click -= handler;
        }
    }
    
    public static void RegisterEvent<TArgs>(RoutedEvent<TArgs> routedEvent, EventHandler<TArgs> handler, params Button?[] controls)
        where TArgs : RoutedEventArgs
    {
        foreach (var control in controls)
        {
            control?.AddHandler(routedEvent, handler);
        }
    }
    
    public static void UnregisterEvent<TArgs>(RoutedEvent<TArgs> routedEvent, EventHandler<TArgs> handler, params Button?[] controls)
        where TArgs : RoutedEventArgs
    {
        foreach (var control in controls)
        {
            control?.RemoveHandler(routedEvent, handler);
        }
    }
}