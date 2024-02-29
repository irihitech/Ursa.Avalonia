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
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is BreadcrumbItem breadcrumbItem)
        {
            if (!breadcrumbItem.IsSet(BreadcrumbItem.SeparatorProperty))
            {
                SeparatorProperty.Changed.AddClassHandler<Breadcrumb, object?>((o, e) =>
                {
                    breadcrumbItem.Separator = e.NewValue.Value switch
                    {
                        string s => s,
                        ITemplate<Control> t => t.Build(),
                        _ => e.NewValue.Value?.ToString()
                    };
                });
            }
        }
    }
}