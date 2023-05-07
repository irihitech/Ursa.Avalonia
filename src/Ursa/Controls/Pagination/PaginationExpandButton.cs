using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Ursa.Controls;

[TemplatePart(PART_Button, typeof(Button))]
[TemplatePart(PART_Popup, typeof(Popup))]
public class PaginationExpandButton: TemplatedControl
{
    public const string PART_Button = "PART_Button";
    public const string PART_Popup = "PART_Popup";
    private Popup? _popup;
    private Button? _button;
    
    public static readonly StyledProperty<AvaloniaList<int>> PagesProperty = AvaloniaProperty.Register<PaginationExpandButton, AvaloniaList<int>>(
        nameof(Pages));

    public AvaloniaList<int> Pages
    {
        get => GetValue(PagesProperty);
        set => SetValue(PagesProperty, value);
    }

    public static readonly StyledProperty<bool> IsDropdownOpenProperty = AvaloniaProperty.Register<PaginationExpandButton, bool>(
        nameof(IsDropdownOpen));

    public bool IsDropdownOpen
    {
        get => GetValue(IsDropdownOpenProperty);
        set => SetValue(IsDropdownOpenProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _popup = e.NameScope.Find<Popup>(PART_Popup);
        _button = e.NameScope.Find<Button>(PART_Button);
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        if (!e.Handled && e.Source is Visual source )
        {
            SetCurrentValue(IsDropdownOpenProperty, true);
            e.Handled = true;
        }
        base.OnPointerEntered(e);

    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        // IsDropdownOpen = false;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
            {
                SetCurrentValue(IsDropdownOpenProperty, false);
                e.Handled = true;
            }
            else
            {
                //SetCurrentValue(IsDropdownOpenProperty, !IsDropdownOpen);
                SetCurrentValue(IsDropdownOpenProperty, false);
                e.Handled = true;
            }
        }
        base.OnPointerReleased(e);
    }
}