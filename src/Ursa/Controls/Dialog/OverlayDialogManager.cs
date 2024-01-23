using System.Collections.Concurrent;

namespace Ursa.Controls;

internal static class OverlayDialogManager
{
    private static OverlayDialogHost? _defaultHost;
    private static readonly ConcurrentDictionary<string, OverlayDialogHost> Hosts = new();
    
    public static void RegisterHost(OverlayDialogHost host, string? id)
    {
        if (id == null)
        {
            if (_defaultHost != null)
            {
                throw new InvalidOperationException("Cannot register multiple OverlayDialogHost with empty HostId");
            }
            _defaultHost = host;
            return;
        }
        Hosts.TryAdd(id, host);
    }
    
    public static void UnregisterHost(string? id)
    {
        if (id is null)
        {
            _defaultHost = null;
            return;
        }
        Hosts.TryRemove(id, out _);
    }
    
    public static OverlayDialogHost? GetHost(string? id)
    {
        if (id is null)
        {
            return _defaultHost;
        }
        return Hosts.TryGetValue(id, out var host) ? host : null;
    }
}