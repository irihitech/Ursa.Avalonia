using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Ursa.Common;
using Ursa.EventArgs;

namespace Ursa.Controls;

[TemplatePart(PART_YesButton, typeof(Button))]
[TemplatePart(PART_NoButton, typeof(Button))]
[TemplatePart(PART_OKButton, typeof(Button))]
[TemplatePart(PART_CancelButton, typeof(Button))]
public class DefaultDrawerControl: DrawerControlBase
{
    public const string PART_YesButton = "PART_YesButton";
    public const string PART_NoButton = "PART_NoButton";
    public const string PART_OKButton = "PART_OKButton";
    public const string PART_CancelButton = "PART_CancelButton";
    
    private Button? _yesButton;
    private Button? _noButton;
    private Button? _okButton;
    private Button? _cancelButton;

    public static readonly StyledProperty<DialogButton> ButtonsProperty = AvaloniaProperty.Register<DefaultDrawerControl, DialogButton>(
        nameof(Buttons), DialogButton.OKCancel);

    public DialogButton Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    public static readonly StyledProperty<DialogMode> ModeProperty = AvaloniaProperty.Register<DefaultDrawerControl, DialogMode>(
        nameof(Mode), DialogMode.None);

    public DialogMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public static readonly StyledProperty<string?> TitleProperty = AvaloniaProperty.Register<DefaultDrawerControl, string?>(
        nameof(Title));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        EventHelper.UnregisterClickEvent(OnDefaultButtonClick, _yesButton, _noButton, _okButton, _cancelButton);
        _yesButton = e.NameScope.Find<Button>(PART_YesButton);
        _noButton = e.NameScope.Find<Button>(PART_NoButton);
        _okButton = e.NameScope.Find<Button>(PART_OKButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        EventHelper.RegisterClickEvent(OnDefaultButtonClick, _yesButton, _noButton, _okButton, _cancelButton);
        SetButtonVisibility();
    }
    
    private void SetButtonVisibility()
    {
        bool isCloseButtonVisible = DataContext is IDialogContext || Buttons != DialogButton.YesNo;
        SetVisibility(_closeButton, isCloseButtonVisible);
        switch (Buttons)
        {
            case DialogButton.None:
                SetVisibility(_okButton, false);
                SetVisibility(_cancelButton, false);
                SetVisibility(_yesButton, false);
                SetVisibility(_noButton, false);
                break;
            case DialogButton.OK:
                SetVisibility(_okButton, true);
                SetVisibility(_cancelButton, false);
                SetVisibility(_yesButton, false);
                SetVisibility(_noButton, false);
                break;
            case DialogButton.OKCancel:
                SetVisibility(_okButton, true);
                SetVisibility(_cancelButton, true);
                SetVisibility(_yesButton, false);
                SetVisibility(_noButton, false);
                break;
            case DialogButton.YesNo:
                SetVisibility(_okButton, false);
                SetVisibility(_cancelButton, false);
                SetVisibility(_yesButton, true);
                SetVisibility(_noButton, true);
                break;
            case DialogButton.YesNoCancel:
                SetVisibility(_okButton, false);
                SetVisibility(_cancelButton, true);
                SetVisibility(_yesButton, true);
                SetVisibility(_noButton, true);
                break;
        }
    }
    
    private void OnDefaultButtonClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            if (button == _okButton)
            {
                OnElementClosing(this, DialogResult.OK);
            }
            else if (button == _cancelButton)
            {
                OnElementClosing(this, DialogResult.Cancel);
            }
            else if (button == _yesButton)
            {
                OnElementClosing(this, DialogResult.Yes);
            }
            else if (button == _noButton)
            {
                OnElementClosing(this, DialogResult.No);
            }
        }
    }
    
    public override void Close()
    {
        if (DataContext is IDialogContext context)
        {
            context.Close();
        }
        else
        {
            DialogResult result = Buttons switch
            {
                DialogButton.None => DialogResult.None,
                DialogButton.OK => DialogResult.OK,
                DialogButton.OKCancel => DialogResult.Cancel,
                DialogButton.YesNo => DialogResult.No,
                DialogButton.YesNoCancel => DialogResult.Cancel,
                _ => DialogResult.None
            };
            RaiseEvent(new ResultEventArgs(ClosedEvent, result));
        }
    }
}