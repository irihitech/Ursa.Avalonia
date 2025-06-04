using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Metadata;
using Avalonia.VisualTree;

namespace Ursa.Controls;

public class Breadcrumb : ItemsControl
{
    private static readonly ITemplate<Panel?> _defaultPanel =
        new FuncTemplate<Panel?>(() => new StackPanel { Orientation = Orientation.Horizontal });

    public static readonly StyledProperty<BindingBase?> IconBindingProperty = AvaloniaProperty.Register<Breadcrumb, BindingBase?>(
        nameof(IconBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public BindingBase? IconBinding
    {
        get => GetValue(IconBindingProperty);
        set => SetValue(IconBindingProperty, value);
    }

    public static readonly StyledProperty<BindingBase?> CommandBindingProperty = AvaloniaProperty.Register<Breadcrumb, BindingBase?>(
        nameof(CommandBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public BindingBase? CommandBinding
    {
        get => GetValue(CommandBindingProperty);
        set => SetValue(CommandBindingProperty, value);
    }

    public static readonly StyledProperty<BindingBase?> CommandParameterBindingProperty = AvaloniaProperty.Register<Breadcrumb, BindingBase?>(
        nameof(CommandParameterBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public BindingBase? CommandParameterBinding
    {
        get => GetValue(CommandParameterBindingProperty);
        set => SetValue(CommandParameterBindingProperty, value);
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

    static Breadcrumb()
    {
        ItemsPanelProperty.OverrideDefaultValue<Breadcrumb>(_defaultPanel);
        SeparatorProperty.Changed.AddClassHandler<Breadcrumb, object?>((b, args) => b.OnSeparatorChanged(args));
    }

    private void OnSeparatorChanged(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (GetSeparatorInstance(Separator) is { } a)
        {
            var breadcrumbItems = this.GetVisualDescendants().OfType<BreadcrumbItem>().ToList();
            foreach (var item in breadcrumbItems)
            {
                item.Separator = a;
            }
        }
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
        if (container is not BreadcrumbItem breadcrumbItem) return;
        if (!breadcrumbItem.IsSet(BreadcrumbItem.SeparatorProperty))
        {
            if (GetSeparatorInstance(Separator) is { } a)
            {
                breadcrumbItem.Separator = a;
            }
        }

        if (container == item) return;
        if (!breadcrumbItem.IsSet(ContentControl.ContentProperty))
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

        if (!breadcrumbItem.IsSet(IconButton.IconProperty) && IconBinding != null)
        {
            breadcrumbItem[!IconButton.IconProperty] = IconBinding;
        }

        if (!breadcrumbItem.IsSet(Button.CommandProperty) && CommandBinding != null)
        {
            breadcrumbItem[!Button.CommandProperty] = CommandBinding;
        }

        if (!breadcrumbItem.IsSet(Button.CommandParameterProperty) && CommandParameterBinding != null)
        {
            breadcrumbItem[!Button.CommandParameterProperty] = CommandParameterBinding;
        }

        if (!breadcrumbItem.IsSet(IconButton.IconTemplateProperty) && IconTemplate != null)
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

    internal void InvalidateContainers()
    {
        var breadcrumbItems = this.GetVisualDescendants().OfType<BreadcrumbItem>().ToList();
        for (var i = 0; i < breadcrumbItems.Count; i++)
        {
            breadcrumbItems[i].SetPseudoClassLast(i == breadcrumbItems.Count - 1);
        }
    }
}
