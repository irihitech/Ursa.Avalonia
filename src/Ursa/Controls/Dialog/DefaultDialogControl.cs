using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Ursa.Common;
using Ursa.EventArgs;

namespace Ursa.Controls;

[TemplatePart(PART_OKButton, typeof(Button))]
[TemplatePart(PART_CancelButton, typeof(Button))]
[TemplatePart(PART_YesButton, typeof(Button))]
[TemplatePart(PART_NoButton, typeof(Button))]
public class DefaultDialogControl: DialogControlBase
{
    public const string PART_OKButton = "PART_OKButton";
    public const string PART_CancelButton = "PART_CancelButton";
    public const string PART_YesButton = "PART_YesButton";
    public const string PART_NoButton = "PART_NoButton";
    
    private Button? _okButton;
    private Button? _cancelButton;
    private Button? _yesButton;
    private Button? _noButton;
    
    public static readonly StyledProperty<string?> TitleProperty = AvaloniaProperty.Register<DefaultDialogControl, string?>(
        nameof(Title));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<DialogButton> ButtonsProperty = AvaloniaProperty.Register<DefaultDialogControl, DialogButton>(
        nameof(Buttons));

    public DialogButton Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    public static readonly StyledProperty<DialogMode> ModeProperty = AvaloniaProperty.Register<DefaultDialogControl, DialogMode>(
        nameof(Mode));

    public DialogMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        EventHelper.UnregisterClickEvent(DefaultButtonsClose, _okButton, _cancelButton, _yesButton, _noButton);
        _okButton = e.NameScope.Find<Button>(PART_OKButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        _yesButton = e.NameScope.Find<Button>(PART_YesButton);
        _noButton = e.NameScope.Find<Button>(PART_NoButton);
        EventHelper.RegisterClickEvent(DefaultButtonsClose, _yesButton, _noButton, _okButton, _cancelButton);
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
    
    private void DefaultButtonsClose(object sender, RoutedEventArgs args)
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
            OnElementClosing(this, result);
        }
    }
}