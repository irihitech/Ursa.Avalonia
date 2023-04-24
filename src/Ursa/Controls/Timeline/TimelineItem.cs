using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media;

namespace Ursa.Controls;

[PseudoClasses(PC_First, PC_Last, PC_Default, PC_Ongoing, PC_Success, PC_Warning, PC_Error, PC_None)]
public class TimelineItem: ContentControl
{
    private const string PC_First = ":first";
    private const string PC_Last = ":last";
    private const string PC_Default = ":default";
    private const string PC_Ongoing = ":ongoing";
    private const string PC_Success = ":success";
    private const string PC_Warning = ":warning";
    private const string PC_Error = ":error";
    private const string PC_None = ":none";

    private static readonly IReadOnlyDictionary<TimelineItemType, string> _itemTypeMapping = new Dictionary<TimelineItemType, string>
    {
        {TimelineItemType.Default, PC_Default},
        {TimelineItemType.Ongoing, PC_Ongoing},
        {TimelineItemType.Success, PC_Success},
        {TimelineItemType.Warning, PC_Warning},
        {TimelineItemType.Error, PC_Error},
    };

    public static readonly StyledProperty<IBrush> IconForegroundProperty =
        AvaloniaProperty.Register<TimelineItem, IBrush>(nameof(IconForeground));

    public IBrush IconForeground
    {
        get => GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    public static readonly StyledProperty<DateTime> TimeProperty = AvaloniaProperty.Register<TimelineItem, DateTime>(
        nameof(Time));
    public DateTime Time
    {
        get => GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public static readonly StyledProperty<string?> TimeFormatProperty = AvaloniaProperty.Register<TimelineItem, string?>(
        nameof(TimeFormat), defaultValue:CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern);

    public string? TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate> DescriptionTemplateProperty = AvaloniaProperty.Register<TimelineItem, IDataTemplate>(
        nameof(DescriptionTemplate));

    public IDataTemplate DescriptionTemplate
    {
        get => GetValue(DescriptionTemplateProperty);
        set => SetValue(DescriptionTemplateProperty, value);
    }

    public static readonly StyledProperty<TimelineItemType> ItemTypeProperty = AvaloniaProperty.Register<TimelineItem, TimelineItemType>(
        nameof(ItemType));

    public TimelineItemType ItemType
    {
        get => GetValue(ItemTypeProperty);
        set => SetValue(ItemTypeProperty, value);
    }

    internal void SetIndex(bool isFirst, bool isLast)
    {
        PseudoClasses.Set(PC_First, isFirst);
        PseudoClasses.Set(PC_Last, isLast);
    }

    static TimelineItem()
    {
        ItemTypeProperty.Changed.AddClassHandler<TimelineItem>((o, e) => { o.OnItemTypeChanged(e); });
        IconForegroundProperty.Changed.AddClassHandler<TimelineItem>((o, e) => { o.OnIconForegroundChanged(e); });
    }

    private void OnItemTypeChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var oldValue = args.GetOldValue<TimelineItemType>();
        var newValue = args.GetNewValue<TimelineItemType>();
        PseudoClasses.Set(_itemTypeMapping[oldValue], false);
        PseudoClasses.Set(_itemTypeMapping[newValue], true);
    }

    private void OnIconForegroundChanged(AvaloniaPropertyChangedEventArgs args)
    {
        IBrush? newValue = args.GetOldValue<IBrush?>();
        PseudoClasses.Set(PC_None, newValue is null);
    }
}