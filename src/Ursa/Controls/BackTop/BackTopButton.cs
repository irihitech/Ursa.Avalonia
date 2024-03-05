using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public class BackTopButton: Button
{
    public static readonly StyledProperty<Control> TargetProperty = AvaloniaProperty.Register<BackTopButton, Control>(
        nameof(Target));

    public Control Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }
    
    protected override void OnClick()
    {
        base.OnClick();
        
    }
}