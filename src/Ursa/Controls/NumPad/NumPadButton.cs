using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace Ursa.Controls;

public class NumPadButton: RepeatButton
{
    public static readonly StyledProperty<Key?> NumKeyProperty = AvaloniaProperty.Register<NumPadButton, Key?>(
        nameof(NumKey));

    public Key? NumKey
    {
        get => GetValue(NumKeyProperty);
        set => SetValue(NumKeyProperty, value);
    }

    public static readonly StyledProperty<Key?> FunctionKeyProperty = AvaloniaProperty.Register<NumPadButton, Key?>(
        nameof(FunctionKey));

    public Key? FunctionKey
    {
        get => GetValue(FunctionKeyProperty);
        set => SetValue(FunctionKeyProperty, value);
    }
    
    public static readonly StyledProperty<bool> NumModeProperty = AvaloniaProperty.Register<NumPadButton, bool>(
        nameof(NumMode));
    
    public bool NumMode
    {
        get => GetValue(NumModeProperty);
        set => SetValue(NumModeProperty, value);
    }

    public static readonly StyledProperty<object?> NumContentProperty = AvaloniaProperty.Register<NumPadButton, object?>(
        nameof(NumContent));

    public object? NumContent
    {
        get => GetValue(NumContentProperty);
        set => SetValue(NumContentProperty, value);
    }
    
    public static readonly StyledProperty<object?> FunctionContentProperty = AvaloniaProperty.Register<NumPadButton, object?>(
        nameof(FunctionContent));
    
    public object? FunctionContent
    {
        get => GetValue(FunctionContentProperty);
        set => SetValue(FunctionContentProperty, value);
    }
    
}