using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace Ursa.Controls;

public class PinCodeItem: TemplatedControl
{
    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<PinCodeItem, string>(
        nameof(Text), defaultBindingMode: BindingMode.TwoWay);

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<char> PasswordCharProperty = AvaloniaProperty.Register<PinCodeItem, char>(
        nameof(PasswordChar), defaultBindingMode: BindingMode.TwoWay);

    public char PasswordChar
    {
        get => GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }
}