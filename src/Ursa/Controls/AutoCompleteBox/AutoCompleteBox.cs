using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

public class AutoCompleteBox : Avalonia.Controls.AutoCompleteBox, IClearControl
{
    // ReSharper disable once InconsistentNaming
    private const string PART_TextBox = "PART_TextBox";
    
    private TextBox? _textbox;
    static AutoCompleteBox()
    {
        MinimumPrefixLengthProperty.OverrideDefaultValue<AutoCompleteBox>(0);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _textbox = e.NameScope.Find<TextBox>(PART_TextBox);
        _textbox?.AddHandler(PointerPressedEvent, OnBoxPointerPressed, handledEventsToo: true);
        PseudoClasses.Set(PseudoClassName.PC_Empty, string.IsNullOrEmpty(Text));
    }

    public void Clear()
    {
        // Note: this method only reset Text to null. 
        // By default, AutoCompleteBox will clear the SelectedItem when Text is set to null.
        // But user can use custom Predicate to control the behavior when Text is set to null.
        SetCurrentValue(TextProperty, null);
    }

    private void OnBoxPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (Equals(sender, _textbox)
            && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed
            && IsDropDownOpen == false)
        {
            SetCurrentValue(IsDropDownOpenProperty, true);
        }
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

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == TextProperty)
        {
            var value = change.GetNewValue<string?>();
            PseudoClasses.Set(PseudoClassName.PC_Empty, string.IsNullOrEmpty(value));
        }
    }
}