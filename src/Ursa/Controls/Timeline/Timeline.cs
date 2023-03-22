using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Templates;

namespace Ursa.Controls;

public class Timeline: ItemsControl
{
    
    public static readonly StyledProperty<IDataTemplate?> ItemDescriptionTemplateProperty = AvaloniaProperty.Register<Timeline, IDataTemplate?>(
        nameof(ItemDescriptionTemplate));

    public IDataTemplate? ItemDescriptionTemplate
    {
        get => GetValue(ItemDescriptionTemplateProperty);
        set => SetValue(ItemDescriptionTemplateProperty, value);
    }

    protected override bool IsItemItsOwnContainerOverride(Control item)
    {
        return item is TimelineItem;
    }

    protected override Control CreateContainerForItemOverride()
    {
        return new TimelineItem();
    }
    
    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is TimelineItem c )
        {
            if (item is ITimelineItemData data)
            {
                c[TimelineItem.TimeProperty] = data;
                c[ContentControl.ContentProperty] = data.Content;
                c[TimelineItem.DescriptionProperty] = data.Description;
                if(ItemTemplate is {}) c[ContentControl.ContentTemplateProperty] = this.ItemTemplate;
                if(ItemDescriptionTemplate is {}) c[TimelineItem.DescriptionTemplateProperty] = this.ItemDescriptionTemplate;
            }
            else
            {
                c.Content = item;
                if (ItemTemplate is { }) c[ContentControl.ContentTemplateProperty] = this.ItemTemplate;
            }
        }
    }
}