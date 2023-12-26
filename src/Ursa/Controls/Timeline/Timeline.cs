using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Metadata;

namespace Ursa.Controls;

public class Timeline: ItemsControl
{
    public static readonly StyledProperty<IBinding?> IconMemberBindingProperty = AvaloniaProperty.Register<Timeline, IBinding?>(
        nameof(IconMemberBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? IconMemberBinding
    {
        get => GetValue(IconMemberBindingProperty);
        set => SetValue(IconMemberBindingProperty, value);
    }

    public static readonly StyledProperty<IBinding?> HeaderMemberBindingProperty = AvaloniaProperty.Register<Timeline, IBinding?>(
        nameof(HeaderMemberBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? HeaderMemberBinding
    {
        get => GetValue(HeaderMemberBindingProperty);
        set => SetValue(HeaderMemberBindingProperty, value);
    }

    public static readonly StyledProperty<IBinding?> DescriptionMemberBindingProperty = AvaloniaProperty.Register<Timeline, IBinding?>(
        nameof(DescriptionMemberBinding));
    
    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? DescriptionMemberBinding
    {
        get => GetValue(DescriptionMemberBindingProperty);
        set => SetValue(DescriptionMemberBindingProperty, value);
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
            bool end = index == ItemCount - 1;
            t.SetEnd(start, end);
            if (IconMemberBinding != null)
            {
                t.Bind(TimelineItem.IconProperty, IconMemberBinding);
            }
            if (HeaderMemberBinding != null)
            {
                t.Bind(HeaderedContentControl.HeaderProperty, HeaderMemberBinding);
            }
            if (DescriptionMemberBinding != null)
            {
                t.Bind(ContentControl.ContentProperty, DescriptionMemberBinding);
            }
            t.SetCurrentValue(TimelineItem.IconTemplateProperty, IconTemplate);
            t.SetCurrentValue(HeaderedContentControl.HeaderTemplateProperty, ItemTemplate);
            t.SetCurrentValue(ContentControl.ContentTemplateProperty, DescriptionTemplate);
        }

    }
}