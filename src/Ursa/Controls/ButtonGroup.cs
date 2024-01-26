using System.Net.Http.Headers;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Metadata;

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
            if ( CommandBinding is not null)
            {
                button[!Button.CommandProperty] = CommandBinding;
            }
            if ( CommandParameterBinding is not null)
            {
                button[!Button.CommandParameterProperty] = CommandParameterBinding;
            }
            if ( ContentBinding is not null)
            {
                button[!Button.ContentProperty] = ContentBinding;
            }
            if (ItemTemplate is not null)
            {
                button.ContentTemplate = ItemTemplate;
            }
        }
    }
}