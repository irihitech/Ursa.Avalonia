using Avalonia.Input;
using Avalonia.Interactivity;

namespace Ursa.Controls;

public class AutoCompleteBox: Avalonia.Controls.AutoCompleteBox
{
    protected override Type StyleKeyOverride { get; } = typeof(Avalonia.Controls.AutoCompleteBox);

    static AutoCompleteBox()
    {
        MinimumPrefixLengthProperty.OverrideDefaultValue<AutoCompleteBox>(0);
    }

    public AutoCompleteBox()
    {
        this.AddHandler(PointerPressedEvent, OnBoxPointerPressed, RoutingStrategies.Tunnel);
    }

    private void OnBoxPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (Equals(sender, this) && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            SetCurrentValue(IsDropDownOpenProperty, true);
        }
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        SetCurrentValue(IsDropDownOpenProperty, true);
    }
}