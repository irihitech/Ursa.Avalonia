using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media;

namespace Ursa.Controls;

public class TimelineItem: HeaderedContentControl
{
    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<TimelineItem, object?>(
        nameof(Icon));

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty = AvaloniaProperty.Register<TimelineItem, IDataTemplate?>(
        nameof(IconTemplate));

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly StyledProperty<TimelineItemType> TypeProperty = AvaloniaProperty.Register<TimelineItem, TimelineItemType>(
        nameof(Type));

    public TimelineItemType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }
    
    
}