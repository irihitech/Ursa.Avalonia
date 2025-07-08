using Avalonia.LogicalTree;
using Ursa.Controls;

namespace Ursa.Common;

public static class LogicalHelpers
{
    public static int CalculateDistanceFromLogicalParent<T, TItem>(TItem? item, int defaultValue = -1) 
        where T : class 
        where TItem : ILogical
    {
        var result = 0;
        ILogical? logical = item;
        while (logical is not null and not T)
        {
            if (logical is TItem) result++;
            logical = logical.LogicalParent;
        }
        return item is not null ? result : defaultValue;
    }
}