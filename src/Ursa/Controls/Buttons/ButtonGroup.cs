using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Metadata;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class ButtonGroup: ItemsControl
{
    public static readonly StyledProperty<IBinding?> CommandBindingProperty = AvaloniaProperty.Register<ButtonGroup, IBinding?>(
        nameof(CommandBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? CommandBinding
    {
        get => GetValue(CommandBindingProperty);
        set => SetValue(CommandBindingProperty, value);
    }

    public static readonly StyledProperty<IBinding?> CommandParameterBindingProperty = AvaloniaProperty.Register<ButtonGroup, IBinding?>(
        nameof(CommandParameterBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? CommandParameterBinding
    {
        get => GetValue(CommandParameterBindingProperty);
        set => SetValue(CommandParameterBindingProperty, value);
    }
    
    public static readonly StyledProperty<IBinding?> ContentBindingProperty = AvaloniaProperty.Register<ButtonGroup, IBinding?>(
        nameof(ContentBinding));
    
    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? ContentBinding
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