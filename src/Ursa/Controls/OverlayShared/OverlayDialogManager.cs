using System.Collections.Concurrent;
using System.Diagnostics;

namespace Ursa.Controls;

internal record struct HostKey(string? Id, int? Hash);

internal static class OverlayDialogManager
{
    private static readonly ConcurrentDictionary<HostKey, OverlayDialogHost> Hosts = new();
    
    public static void RegisterHost(OverlayDialogHost host, string? id, int? hash)
    {
        Debug.WriteLine("Count: "+Hosts.Count);
        Hosts.TryAdd(new HostKey(id, hash), host);
    }
    
    public static void UnregisterHost(string? id, int? hash)
    {
        Hosts.TryRemove(new HostKey(id, hash), out _);
    }
    
    public static OverlayDialogHost? GetHost(string? id, int? hash)
    {
        HostKey? key = hash is null ? Hosts.Keys.Where(k => k.Id == id).ToArray().FirstOrDefault() : Hosts.Keys.FirstOrDefault(k => k.Id == id && k.Hash == hash);
        if (key is null) return null;
        return Hosts.TryGetValue(key.Value, out var host) ? host : null;
    }
}

