using System.Collections.Concurrent;

namespace Ursa.Controls;

internal static class OverlayDialogManager
{
    private static ConcurrentDictionary<string, OverlayDialogHost> _hosts = new();
    
    public static void RegisterOverlayDialogHost(OverlayDialogHost host, string id)
    {
        _hosts.TryAdd(id, host);
    }
    
    public static void UnregisterOverlayDialogHost(string id)
    {
        _hosts.TryRemove(id, out _);
    }
    
    public static OverlayDialogHost? GetOverlayDialogHost(string id)
    {
        return _hosts.TryGetValue(id, out var host) ? host : null;
    }
}