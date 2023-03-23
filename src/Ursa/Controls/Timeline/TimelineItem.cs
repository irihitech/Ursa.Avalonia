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

    public static readonly StyledProperty<object?> DescriptionProperty =
        AvaloniaProperty.Register<TimelineItem, object?>(nameof(Description));

    public object? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly StyledProperty<DateTime> TimeProperty = AvaloniaProperty.Register<TimelineItem, DateTime>(
        nameof(Time));
    public DateTime Time
    {
        get => GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public static readonly StyledProperty<string?> TimeFormatProperty = AvaloniaProperty.Register<TimelineItem, string?>(
        nameof(TimeFormat), defaultValue:CultureInfo.CurrentUICulture.DateTimeFormat.SortableDateTimePattern);

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

public class TimelineItemLayoutProperties: AvaloniaObject
{
    private double _dimensionDelta = 0.01;
    
    public static readonly StyledProperty<double> TimeSlotWidthProperty = AvaloniaProperty.Register<TimelineItemLayoutProperties, double>(
        nameof(TimeSlotWidth));

    public double TimeSlotWidth
    {
        get => GetValue(TimeSlotWidthProperty);
        set
        {
            if (Math.Abs(GetValue(TimeSlotWidthProperty) - value) < _dimensionDelta) return; 
            SetValue(TimeSlotWidthProperty, value);
        }
    }

    public static readonly StyledProperty<double> TimeSlotHeightProperty = AvaloniaProperty.Register<TimelineItemLayoutProperties, double>(
        nameof(TimeSlotHeight));

    public double TimeSlotHeight
    {
        get => GetValue(TimeSlotHeightProperty);
        set
        {
            if (Math.Abs(GetValue(TimeSlotHeightProperty) - value) < _dimensionDelta) return;
            SetValue(TimeSlotHeightProperty, value);
        }
    }

    public static readonly StyledProperty<double> ContentSlotWidthProperty = AvaloniaProperty.Register<TimelineItemLayoutProperties, double>(
        nameof(ContentSlotWidth));

    public double ContentSlotWidth
    {
        get => GetValue(ContentSlotWidthProperty);
        set => SetValue(ContentSlotWidthProperty, value);
    }

    public static readonly StyledProperty<double> ContentSlotHeightProperty = AvaloniaProperty.Register<TimelineItemLayoutProperties, double>(
        nameof(ContentSlotHeight));

    public double ContentSlotHeight
    {
        get => GetValue(ContentSlotHeightProperty);
        set => SetValue(ContentSlotHeightProperty, value);
    }
}