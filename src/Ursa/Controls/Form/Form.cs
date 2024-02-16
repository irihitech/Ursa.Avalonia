using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;

namespace Ursa.Controls;

public class Form: ItemsControl
{

    public static readonly StyledProperty<AvaloniaList<FormItem>> ItemsProperty = AvaloniaProperty.Register<Form, AvaloniaList<FormItem>>(
        "Items");
    
    public AvaloniaList<FormItem> Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public void AddChild(object child)
    {
        throw new NotImplementedException();
    }
}