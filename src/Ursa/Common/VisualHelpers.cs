using Avalonia;
using Avalonia.VisualTree;

namespace Ursa.Common;

public static class VisualHelpers
{
    public static T? GetContainerFromEventSource<T>(this Visual? source)
    {
        var item = source.GetSelfAndVisualAncestors().OfType<T>().FirstOrDefault();
        return item;
    }
}