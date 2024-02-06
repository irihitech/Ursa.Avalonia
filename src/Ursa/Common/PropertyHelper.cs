using Avalonia;

namespace Ursa.Common;

public static class PropertyHelper
{
    public static void SetValue<TValue>(AvaloniaProperty<TValue> property, TValue value, params AvaloniaObject?[] elements)
    {
        foreach (var element in elements)
        {
            element?.SetValue(property, value);
        }
    }
}