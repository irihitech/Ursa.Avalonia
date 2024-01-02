using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media;

namespace Ursa.Controls;

[PseudoClasses(PC_First, PC_Last, PC_EmptyIcon)]
public class TimelineItem: HeaderedContentControl
{
    public const string PC_First = ":first";
    public const string PC_Last = ":last";
    public const string PC_EmptyIcon = ":empty-icon";
    
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
    
    public static readonly DirectProperty<TimelineItem, double> LeftWidthProperty = AvaloniaProperty.RegisterDirect<TimelineItem, double>(
        nameof(LeftWidth), o => o.LeftWidth, (o, v) => o.LeftWidth = v);
    private double _leftWidth;
    public double LeftWidth
    {
        get => _leftWidth;
        set => SetAndRaise(LeftWidthProperty, ref _leftWidth, value);
    }
    
    public static readonly DirectProperty<TimelineItem, double> IconWidthProperty = AvaloniaProperty.RegisterDirect<TimelineItem, double>(
        nameof(IconWidth), o => o.IconWidth, (o, v) => o.IconWidth = v);
    private double _iconWidth;
    public double IconWidth
    {
        get => _iconWidth;
        set => SetAndRaise(IconWidthProperty, ref _iconWidth, value);
    }
    
    public static readonly DirectProperty<TimelineItem, double> RightWidthProperty = AvaloniaProperty.RegisterDirect<TimelineItem, double>(
        nameof(RightWidth), o => o.RightWidth, (o, v) => o.RightWidth = v);
    private double _rightWidth;
    public double RightWidth
    {
        get => _rightWidth;
        set => SetAndRaise(RightWidthProperty, ref _rightWidth, value);
    }

    static TimelineItem()
    {
        IconProperty.Changed.AddClassHandler<TimelineItem, object?>((item, args) => { item.OnIconChanged(args); });
    }

    private void OnIconChanged(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        PseudoClasses.Set(PC_EmptyIcon, args.NewValue.Value is null);
    }

    internal void SetEnd(bool start, bool end)
    {
        PseudoClasses.Set(PC_First, start);
        PseudoClasses.Set(PC_Last, end);
    }
}