using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.VisualTree;

namespace Ursa.Controls;

[PseudoClasses(PC_First, PC_Last, PC_EmptyIcon, PC_AllLeft, PC_AllRight, PC_Separate)]
[TemplatePart(PART_Header, typeof(ContentPresenter))]
[TemplatePart(PART_Icon, typeof(Panel))]
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
    private Panel? _iconPresenter;
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

    public static readonly StyledProperty<TimelineItemPosition> PositionProperty = AvaloniaProperty.Register<TimelineItem, TimelineItemPosition>(
        nameof(Position), defaultValue: TimelineItemPosition.Right);

    public TimelineItemPosition Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
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
        PositionProperty.Changed.AddClassHandler<TimelineItem, TimelineItemPosition>((item, args) => { item.OnModeChanged(args); });
        AffectsMeasure<TimelineItem>(LeftWidthProperty, RightWidthProperty, IconWidthProperty, PositionProperty);
        AffectsArrange<TimelineItem>(LeftWidthProperty, RightWidthProperty, IconWidthProperty, PositionProperty);
    }

    private void OnModeChanged(AvaloniaPropertyChangedEventArgs<TimelineItemPosition> args)
    {
        SetMode(args.NewValue.Value);
    }

    private void SetMode(TimelineItemPosition mode)
    {
        PseudoClasses.Set(PC_AllLeft, mode == TimelineItemPosition.Left);
        PseudoClasses.Set(PC_AllRight, mode == TimelineItemPosition.Right);
        PseudoClasses.Set(PC_Separate, mode == TimelineItemPosition.Separate);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _rootGrid = e.NameScope.Find<Grid>(PART_RootGrid);
        _headerPresenter = e.NameScope.Find<ContentPresenter>(PART_Header);
        _iconPresenter = e.NameScope.Find<Panel>(PART_Icon);
        _contentPresenter = e.NameScope.Find<ContentPresenter>(PART_Content);
        _timePresenter = e.NameScope.Find<TextBlock>(PART_Time);
        PseudoClasses.Set(PC_EmptyIcon, Icon is null);
        SetMode(Position);
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
    
    internal void SetIfUnset<T>(AvaloniaProperty<T> property, T value)
    {
        if (!IsSet(property))
        {
            SetCurrentValue(property, value);
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var parent = this.Parent as Timeline;
        parent?.InvalidateContainers();
    }
}