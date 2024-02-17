using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public class Form: ItemsControl
{
    public static readonly AttachedProperty<string> LabelProperty =
        AvaloniaProperty.RegisterAttached<Form, Control, string>("Label");
    public static void SetLabel(Control obj, string value) => obj.SetValue(LabelProperty, value);
    public static string GetLabel(Control obj) => obj.GetValue(LabelProperty);
    

    public static readonly AttachedProperty<bool> IsRequiredProperty =
        AvaloniaProperty.RegisterAttached<Form, Control, bool>("IsRequired");
    public static void SetIsRequired(Control obj, bool value) => obj.SetValue(IsRequiredProperty, value);
    public static bool GetIsRequired(Control obj) => obj.GetValue(IsRequiredProperty);

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is FormItem or FormGroup;
    }
    
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        if (item is not Control control) return new FormItem();
        string label = GetLabel(control);
        bool isRequired = GetIsRequired(control);
        return new FormItem() { Label = label, IsRequired = isRequired, Content = control };
    }
}