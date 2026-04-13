using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Metadata;
using Avalonia.VisualTree;

namespace Ursa.Controls;

public class Timeline: ItemsControl
{
    private static readonly FuncTemplate<Panel?> DefaultPanel = new((Func<Panel>)(() => new StackPanel()));
    
    public static readonly StyledProperty<BindingBase?> IconMemberBindingProperty = AvaloniaProperty.Register<Timeline, BindingBase?>(
        nameof(IconMemberBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public BindingBase? IconMemberBinding
    {
        get => GetValue(IconMemberBindingProperty);
        set => SetValue(IconMemberBindingProperty, value);
    }

    public static readonly StyledProperty<BindingBase?> HeaderMemberBindingProperty = AvaloniaProperty.Register<Timeline, BindingBase?>(
        nameof(HeaderMemberBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public BindingBase? HeaderMemberBinding
    {
        get => GetValue(HeaderMemberBindingProperty);
        set => SetValue(HeaderMemberBindingProperty, value);
    }

    public static readonly StyledProperty<BindingBase?> ContentMemberBindingProperty = AvaloniaProperty.Register<Timeline, BindingBase?>(
        nameof(ContentMemberBinding));
    
    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public BindingBase? ContentMemberBinding
    {
        get => GetValue(ContentMemberBindingProperty);
        set => SetValue(ContentMemberBindingProperty, value);
    }
    

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty = AvaloniaProperty.Register<Timeline, IDataTemplate?>(
        nameof(IconTemplate));

    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> DescriptionTemplateProperty = AvaloniaProperty.Register<Timeline, IDataTemplate?>(
        nameof(DescriptionTemplate));

    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IDataTemplate? DescriptionTemplate
    {
        get => GetValue(DescriptionTemplateProperty);
        set => SetValue(DescriptionTemplateProperty, value);
    }

    public static readonly StyledProperty<BindingBase?> TimeMemberBindingProperty = AvaloniaProperty.Register<Timeline, BindingBase?>(
        nameof(TimeMemberBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public BindingBase? TimeMemberBinding
    {
        get => GetValue(TimeMemberBindingProperty);
        set => SetValue(TimeMemberBindingProperty, value);
    }

    public static readonly StyledProperty<string?> TimeFormatProperty = AvaloniaProperty.Register<Timeline, string?>(
        nameof(TimeFormat), defaultValue:"yyyy-MM-dd HH:mm:ss");

    public string? TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }
    

    public static readonly StyledProperty<TimelineDisplayMode> ModeProperty = AvaloniaProperty.Register<Timeline, TimelineDisplayMode>(
        nameof(Mode));

    public TimelineDisplayMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    static Timeline()
    {
        ItemsPanelProperty.OverrideDefaultValue<Timeline>(DefaultPanel);
        ModeProperty.Changed.AddClassHandler<Timeline>((s, e) => s.InvalidateContainers());
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is not TimelineItem;
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        if (item is TimelineItem t) return t;
        return new TimelineItem();
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is TimelineItem t)
        {
            bool start = index == 0;
            SetTimelineItemPosition(t, Mode, index);
            bool end = index == ItemCount - 1;
            t.SetEnd(start, end);
            if (IconMemberBinding is not null)
            {
                t.Bind(TimelineItem.IconProperty, IconMemberBinding);
            }
            if (HeaderMemberBinding != null)
            {
                t.Bind(HeaderedContentControl.HeaderProperty, HeaderMemberBinding);
            }
            if (ContentMemberBinding != null)
            {
                t.Bind(ContentControl.ContentProperty, ContentMemberBinding);
            }
            if (TimeMemberBinding != null)
            {
                t.Bind(TimelineItem.TimeProperty, TimeMemberBinding);
            }
            t.SetIfUnset(TimelineItem.TimeFormatProperty, TimeFormat);
            t.SetIfUnset(TimelineItem.IconTemplateProperty, IconTemplate);
            t.SetIfUnset(HeaderedContentControl.HeaderTemplateProperty, ItemTemplate);
            t.SetIfUnset(ContentControl.ContentTemplateProperty, DescriptionTemplate);
        }
    }
    
    internal void InvalidateContainers()
    {
        var timelineItems = this.GetVisualDescendants().OfType<TimelineItem>().ToList();
        for (var i = 0; i < timelineItems.Count; i++)
        {
            bool isFirst = i == 0;
            bool isLast = i == timelineItems.Count - 1;
            timelineItems[i].SetEnd(isFirst, isLast);
            SetTimelineItemPosition(timelineItems[i], Mode, i);
        }
        InvalidateMeasure();
        InvalidateArrange();
    }

    private void SetTimelineItemPosition(TimelineItem item, TimelineDisplayMode mode, int index)
    {
        switch (mode)
        {
            case TimelineDisplayMode.Left:
                item.Position = TimelineItemPosition.Left;
                break;
            case  TimelineDisplayMode.Right:
                item.Position =  TimelineItemPosition.Right;
                break;
            case TimelineDisplayMode.Center:
                item.Position = TimelineItemPosition.Separate;
                break;
            case TimelineDisplayMode.Alternate:
                item.Position = index % 2 == 0 ? TimelineItemPosition.Left : TimelineItemPosition.Right;
                break;
        }
    }
}