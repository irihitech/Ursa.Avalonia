using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

public class AutoCompleteBox : Avalonia.Controls.AutoCompleteBox, IClearControl
{
    // ReSharper disable once InconsistentNaming
    private const string PART_TextBox = "PART_TextBox";
    private bool _closeBySelectionFlag;
    
    private TextBox? _textbox;
    private Popup? _popup;
    static AutoCompleteBox()
    {
        MinimumPrefixLengthProperty.OverrideDefaultValue<AutoCompleteBox>(0);
    }

    public AutoCompleteBox()
    {
        AddHandler(PointerReleasedEvent, OnCurrentPointerReleased, RoutingStrategies.Tunnel);
    }

    private void OnCurrentPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var source = (e.Source as Control).FindAncestorOfType<ListBoxItem>();
        if (source is not null)
        {
            _closeBySelectionFlag = true;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _textbox?.RemoveHandler(PointerPressedEvent, OnBoxPointerPressed);
        _textbox = e.NameScope.Find<TextBox>(PART_TextBox);
        _popup = e.NameScope.Find<Popup>(PartNames.PART_Popup);
        _textbox?.AddHandler(PointerPressedEvent, OnBoxPointerPressed, handledEventsToo: true);
        PseudoClasses.Set(PseudoClassName.PC_Empty, string.IsNullOrEmpty(Text));
    }

    public void Clear()
    {
        // Note: this method only resets Text to null. 
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

    protected override void OnGotFocus(FocusChangedEventArgs e)
    {
        base.OnGotFocus(e);
        // If the focus is set by pointer navigation, it is handled by PointerPressed, do not open the dropdown.
        if (e.NavigationMethod == NavigationMethod.Pointer) return;
        if (!this.GetTemplateDescendants().Contains(e.Source)) return;
        // If the focus is set by keyboard navigation, open the dropdown.
        if (!_closeBySelectionFlag && IsDropDownOpen == false)
        {
            SetCurrentValue(IsDropDownOpenProperty, true);
        }
        _closeBySelectionFlag = false;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == TextProperty)
        {
            var value = change.GetNewValue<string?>();
            PseudoClasses.Set(PseudoClassName.PC_Empty, string.IsNullOrEmpty(value));
        }

        if (change.Property == IsDropDownOpenProperty && change.GetNewValue<bool>() == false)
        {
            
        }
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        var newElement = e.NewFocusedElement;
        if (newElement is Visual v && _popup?.IsInsidePopup(v) == true)
        {
            return;
        }
        if (Equals(newElement, _textbox))
        {
            return;
        }
        SetCurrentValue(IsDropDownOpenProperty, false);
    }
}
