using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media;

namespace Ursa.Controls;

[PseudoClasses(PC_First, PC_Last)]
public class TimelineItem: ContentControl
{
    public const string PC_First = ":first";
    public const string PC_Last = ":last";

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

    internal void SetIndex(bool isFirst, bool isLast)
    {
        PseudoClasses.Set(PC_First, isFirst);
        PseudoClasses.Set(PC_Last, isLast);
    }
}