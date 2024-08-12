using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

public class AutoCompleteBox: Avalonia.Controls.AutoCompleteBox, IClearControl
{
    // protected override Type StyleKeyOverride { get; } = typeof(Avalonia.Controls.AutoCompleteBox);
    private TextBox? _text;
    static AutoCompleteBox()
    {
        MinimumPrefixLengthProperty.OverrideDefaultValue<AutoCompleteBox>(0);
    }

    public AutoCompleteBox()
    {
        this.AddHandler(PointerPressedEvent, OnBoxPointerPressed, RoutingStrategies.Tunnel);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _text = e.NameScope.Get<TextBox>("PART_TextBox");
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

    public void Clear()
    {
        SetCurrentValue(SelectedItemProperty, null);
    }
}