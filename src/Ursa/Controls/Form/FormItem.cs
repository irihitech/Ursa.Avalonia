using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

public class FormItem: ContentControl
{
    public static readonly StyledProperty<string> LabelProperty = AvaloniaProperty.Register<FormItem, string>(
        nameof(Label));

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly StyledProperty<bool> IsRequiredProperty = AvaloniaProperty.Register<FormItem, bool>(
        nameof(IsRequired));

    public bool IsRequired
    {
        get => GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }
    
}