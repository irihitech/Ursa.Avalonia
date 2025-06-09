using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

public class AutoCompleteBox : Avalonia.Controls.AutoCompleteBox, IClearControl
{
    static AutoCompleteBox()
    {
        MinimumPrefixLengthProperty.OverrideDefaultValue<AutoCompleteBox>(0);
    }

    public AutoCompleteBox()
    {
        AddHandler(PointerPressedEvent, OnBoxPointerPressed, RoutingStrategies.Tunnel);
    }

    public void Clear()
    {
        SetCurrentValue(SelectedItemProperty, null);
    }

    private void OnBoxPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (Equals(sender, this) && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed && IsDropDownOpen == false)
            SetCurrentValue(IsDropDownOpenProperty, true);
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        // If the focus is set by pointer navigation, it is handled by PointerPressed, do not open the dropdown.
        if (e.NavigationMethod == NavigationMethod.Pointer) return;
        if (!this.GetTemplateChildren().Contains(e.Source)) return;
        // If the focus is set by keyboard navigation, open the dropdown.
        if (IsDropDownOpen == false) SetCurrentValue(IsDropDownOpenProperty, true);
    }
}