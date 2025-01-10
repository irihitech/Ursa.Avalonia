using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Ursa.Controls;

public class Avatar : Button
{
    public static readonly StyledProperty<IImage?> SourceProperty = AvaloniaProperty.Register<Avatar, IImage?>(
        nameof(Source));

    public static readonly StyledProperty<object?> HoverMaskProperty = AvaloniaProperty.Register<Avatar, object?>(
        nameof(HoverMask));

    [ExcludeFromCodeCoverage]
    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    [ExcludeFromCodeCoverage]
    public object? HoverMask
    {
        get => GetValue(HoverMaskProperty);
        set => SetValue(HoverMaskProperty, value);
    }
}