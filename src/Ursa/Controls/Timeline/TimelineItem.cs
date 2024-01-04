using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media;

namespace Ursa.Controls;

[PseudoClasses(PC_First, PC_Last, PC_EmptyIcon)]
[TemplatePart(PART_Header, typeof(ContentPresenter))]
[TemplatePart(PART_Icon, typeof(ContentPresenter))]
[TemplatePart(PART_Content, typeof(ContentPresenter))]
[TemplatePart(PART_Time, typeof(TextBlock))]
[TemplatePart(PART_RootGrid, typeof(Grid))]
public class TimelineItem: HeaderedContentControl
{
    public const string PC_First = ":first";
    public const string PC_Last = ":last";
    public const string PC_EmptyIcon = ":empty-icon";
    public const string PC_AllLeft=":all-left"; 
    public const string PC_AllRight=":all-right";
    public const string PC_Separate = ":separate";
    public const string PART_Header = "PART_Header";
    public const string PART_Icon = "PART_Icon";
    public const string PART_Content = "PART_Content";
    public const string PART_Time = "PART_Time";
    public const string PART_RootGrid = "PART_RootGrid";
    
    private ContentPresenter? _headerPresenter;
    private ContentPresenter? _iconPresenter;
    private ContentPresenter? _contentPresenter;
    private TextBlock? _timePresenter;
    private Grid? _rootGrid;
    
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

    public static readonly StyledProperty<TimelineItemDisplayMode> ModeProperty = AvaloniaProperty.Register<TimelineItem, TimelineItemDisplayMode>(
        nameof(Mode), defaultValue: TimelineItemDisplayMode.Right);

    public TimelineItemDisplayMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
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

    public static readonly StyledProperty<DateTime> TimeProperty = AvaloniaProperty.Register<TimelineItem, DateTime>(
        nameof(Time));

    public DateTime Time
    {
        get => GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public static readonly StyledProperty<string?> TimeFormatProperty = AvaloniaProperty.Register<TimelineItem, string?>(
        nameof(TimeFormat));

    public string? TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    static TimelineItem()
    {
        IconProperty.Changed.AddClassHandler<TimelineItem, object?>((item, args) => { item.OnIconChanged(args); });
        ModeProperty.Changed.AddClassHandler<TimelineItem, TimelineItemDisplayMode>((item, args) => { item.OnModeChanged(args); });
        AffectsMeasure<TimelineItem>(LeftWidthProperty, RightWidthProperty, IconWidthProperty);
    }

    private void OnModeChanged(AvaloniaPropertyChangedEventArgs<TimelineItemDisplayMode> args)
    {
        SetMode(args.NewValue.Value);
    }

    private void SetMode(TimelineItemDisplayMode mode)
    {
        PseudoClasses.Set(PC_AllLeft, mode == TimelineItemDisplayMode.Left);
        PseudoClasses.Set(PC_AllRight, mode == TimelineItemDisplayMode.Right);
        PseudoClasses.Set(PC_Separate, mode == TimelineItemDisplayMode.Separate);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        PseudoClasses.Set(PC_EmptyIcon, Icon is null);
        _headerPresenter = e.NameScope.Find<ContentPresenter>(PART_Header);
        _iconPresenter = e.NameScope.Find<ContentPresenter>(PART_Icon);
        _contentPresenter = e.NameScope.Find<ContentPresenter>(PART_Content);
        _timePresenter = e.NameScope.Find<TextBlock>(PART_Time);
        _rootGrid = e.NameScope.Find<Grid>(PART_RootGrid);
        SetMode(Mode);
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

    internal (double left, double mid, double right) GetWidth()
    {
        if (_headerPresenter is null) return new ValueTuple<double, double, double>(0, 0, 0);
        double header = _headerPresenter?.DesiredSize.Width ?? 0;
        double icon = _iconPresenter?.DesiredSize.Width ?? 0;
        double content = _contentPresenter?.DesiredSize.Width ?? 0;
        double time = _timePresenter?.DesiredSize.Width ?? 0;
        double max = Math.Max(header, content);
        if (Mode == TimelineItemDisplayMode.Left)
        {
            max = Math.Max(max, time);
            return (max, icon, 0);
        }
        if (Mode == TimelineItemDisplayMode.Right)
        {
            max = Math.Max(max, time);
            return (0, icon, max);
        }
        if (Mode == TimelineItemDisplayMode.Separate)
        {
            return (time, icon, max);
        }
        return new ValueTuple<double, double, double>(0, 0, 0);
    }

    internal void SetWidth(double? left, double? mid, double? right)
    {
        _rootGrid.ColumnDefinitions[0].Width = new GridLength(left??0);
        _rootGrid.ColumnDefinitions[1].Width = new GridLength(mid??0);
        _rootGrid.ColumnDefinitions[2].Width = new GridLength(right??0);
    }
}