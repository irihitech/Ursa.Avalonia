using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace Ursa.Controls;

public class VerificationCodeItem: TemplatedControl
{
    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<VerificationCodeItem, string>(
        nameof(Text), defaultBindingMode: BindingMode.TwoWay);

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<char> PasswordCharProperty = AvaloniaProperty.Register<VerificationCodeItem, char>(
        nameof(PasswordChar), defaultBindingMode: BindingMode.TwoWay);

    public char PasswordChar
    {
        get => GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }
}