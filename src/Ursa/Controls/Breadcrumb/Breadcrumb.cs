using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Metadata;

namespace Ursa.Controls;

public class Breadcrumb: ItemsControl
{
    private static readonly ITemplate<Panel?> _defaultPanel =
        new FuncTemplate<Panel?>(() => new StackPanel() { Orientation = Orientation.Horizontal });
    
    
    public static readonly StyledProperty<IBinding?> IconBindingProperty = AvaloniaProperty.Register<Breadcrumb, IBinding?>(
        nameof(IconBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? IconBinding
    {
        get => GetValue(IconBindingProperty);
        set => SetValue(IconBindingProperty, value);
    }

    public static readonly StyledProperty<IBinding?> CommandBindingProperty = AvaloniaProperty.Register<Breadcrumb, IBinding?>(
        nameof(CommandBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? CommandBinding
    {
        get => GetValue(CommandBindingProperty);
        set => SetValue(CommandBindingProperty, value);
    }

    public static readonly StyledProperty<object?> SeparatorProperty = AvaloniaProperty.Register<Breadcrumb, object?>(
        nameof(Separator));

    /// <summary>
    /// Separator between items.
    /// Usage: Separator can only be raw string or ITemplate&lt;Control&gt;.
    /// </summary>
    public object? Separator
    {
        get => GetValue(SeparatorProperty);
        set => SetValue(SeparatorProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty = AvaloniaProperty.Register<Breadcrumb, IDataTemplate?>(
        nameof(IconTemplate));

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> IsReadOnlyProperty = AvaloniaProperty.Register<Breadcrumb, bool>(
        nameof(IsReadOnly));

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }
    
    static Breadcrumb()
    {
        ItemsPanelProperty.OverrideDefaultValue<Breadcrumb>(_defaultPanel);
    }
    
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<BreadcrumbItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new BreadcrumbItem();
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        // base.PrepareContainerForItemOverride(container, item, index);
        if (container is not BreadcrumbItem breadcrumbItem) return;
        if (!breadcrumbItem.IsSet(BreadcrumbItem.SeparatorProperty))
        {
            if (GetSeparatorInstance(Separator) is { } a)
            {
                breadcrumbItem.Separator = a;
            }
            SeparatorProperty.Changed.AddClassHandler<Breadcrumb, object?>((_, args) =>
            {
                if (GetSeparatorInstance(args.NewValue.Value) is { } b)
                    breadcrumbItem.Separator = b;
            });
        }

        PseudolassesExtensions.Set(container.Classes, BreadcrumbItem.PC_Last, index == ItemCount - 1);

        if (container == item) return;
        if(!breadcrumbItem.IsSet(ContentControl.ContentProperty))
        {
            breadcrumbItem.SetCurrentValue(ContentControl.ContentProperty, item);
            if (DisplayMemberBinding is not null)
            {
                breadcrumbItem[!ContentControl.ContentProperty] = DisplayMemberBinding;
            }
        }
        if (!breadcrumbItem.IsSet(ContentControl.ContentTemplateProperty) && this.ItemTemplate != null)
        {
            breadcrumbItem.SetCurrentValue(ContentControl.ContentTemplateProperty, this.ItemTemplate);
        }
        if (!breadcrumbItem.IsSet(BreadcrumbItem.IconProperty) && IconBinding != null)
        {
            breadcrumbItem[!BreadcrumbItem.IconProperty] = IconBinding;
        }
        if (!breadcrumbItem.IsSet(BreadcrumbItem.CommandProperty) && CommandBinding != null)
        {
            breadcrumbItem[!BreadcrumbItem.CommandProperty] = CommandBinding;
        }
        if (!breadcrumbItem.IsSet(BreadcrumbItem.IconTemplateProperty) && IconTemplate != null)
        {
            breadcrumbItem.IconTemplate = IconTemplate;
        }
    }

    private static object? GetSeparatorInstance(object? separator) => separator switch
    {
        null => null,
        string s => s,
        ITemplate<Control?> t => t.Build(),
        _ => separator.ToString()
    };
}