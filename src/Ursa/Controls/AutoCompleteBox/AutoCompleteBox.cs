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
        _popup?.Child?.RemoveHandler(PointerReleasedEvent, OnPopupChildPointerReleased);
        _textbox = e.NameScope.Find<TextBox>(PART_TextBox);
        _popup = e.NameScope.Find<Popup>(PartNames.PART_Popup);
        _textbox?.AddHandler(PointerPressedEvent, OnBoxPointerPressed, handledEventsToo: true);
        // The dropdown is hosted in its own popup, so pointer releases on dropdown items never
        // reach this control's own handlers. Track them on the popup content so that the
        // GotFocus raised by the selection commit (which refocuses the textbox) does not
        // reopen the dropdown.
        _popup?.Child?.AddHandler(PointerReleasedEvent, OnPopupChildPointerReleased, RoutingStrategies.Tunnel);
        PseudoClasses.Set(PseudoClassName.PC_Empty, string.IsNullOrEmpty(Text));
    }

    private void OnPopupChildPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _closeBySelectionFlag = true;
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
        // When focus moves into the dropdown (the user pressed an item), leave it there:
        // the base OnGotFocus would redirect focus back to the textbox, which closes the
        // dropdown before the click can be committed on release (Avalonia >= 12.1.6 keeps
        // the dropdown open while focus is inside it).
        if (e.Source is Visual v && _popup is not null &&
            (_popup.IsInsidePopup(v) || (_popup.Child is { } child && child.IsVisualAncestorOf(v))))
        {
            return;
        }
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
        if (newElement is Visual v && _popup is not null &&
            (_popup.IsInsidePopup(v) || (_popup.Child is { } child && child.IsVisualAncestorOf(v))))
        {
            // On Avalonia < 12.1.6 the base closes the dropdown here; a later GotFocus
            // (focus falling back to the textbox when the popup goes away) must not
            // reopen it during the click.
            _closeBySelectionFlag = true;
            return;
        }
        if (Equals(newElement, _textbox))
        {
            return;
        }
        SetCurrentValue(IsDropDownOpenProperty, false);
    }
}
