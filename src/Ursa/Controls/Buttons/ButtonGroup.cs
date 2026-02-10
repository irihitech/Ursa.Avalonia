using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Metadata;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class ButtonGroup: ItemsControl
{
    public static readonly StyledProperty<BindingBase?> CommandBindingProperty = AvaloniaProperty.Register<ButtonGroup, BindingBase?>(
        nameof(CommandBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public BindingBase? CommandBinding
    {
        get => GetValue(CommandBindingProperty);
        set => SetValue(CommandBindingProperty, value);
    }

    public static readonly StyledProperty<BindingBase?> CommandParameterBindingProperty = AvaloniaProperty.Register<ButtonGroup, BindingBase?>(
        nameof(CommandParameterBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public BindingBase? CommandParameterBinding
    {
        get => GetValue(CommandParameterBindingProperty);
        set => SetValue(CommandParameterBindingProperty, value);
    }
    
    public static readonly StyledProperty<BindingBase?> ContentBindingProperty = AvaloniaProperty.Register<ButtonGroup, BindingBase?>(
        nameof(ContentBinding));
    
    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public BindingBase? ContentBinding
    {
        get => GetValue(ContentBindingProperty);
        set => SetValue(ContentBindingProperty, value);
    }
    
    
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is not Button;
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new Button();
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if(container is Button button)
        {
            button.TryBind(Button.CommandProperty, CommandBinding);
            button.TryBind(Button.CommandParameterProperty, CommandParameterBinding);
            button.TryBind(ContentControl.ContentProperty, ContentBinding);
            button.TryBind(ContentControl.ContentTemplateProperty, this[!ItemTemplateProperty]);
        }
    }
}