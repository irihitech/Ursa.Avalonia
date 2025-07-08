using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Ursa.Common;

public static class VisualHelpers
{
    public static T? GetContainerFromEventSource<T>(this Visual? source) where T: Control
    {
        var item = source?.GetSelfAndVisualAncestors().OfType<T>().FirstOrDefault();
        return item;
    }
}