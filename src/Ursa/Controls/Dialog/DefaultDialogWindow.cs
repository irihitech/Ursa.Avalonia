using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;
using Ursa.Common;

namespace Ursa.Controls;

[TemplatePart(PART_YesButton, typeof(Button))]
[TemplatePart(PART_NoButton, typeof(Button))]
[TemplatePart(PART_OKButton, typeof(Button))]
[TemplatePart(PART_CancelButton, typeof(Button))]
public class DefaultDialogWindow: DialogWindow
{
    protected override Type StyleKeyOverride { get; } = typeof(DefaultDialogWindow);
    
    public const string PART_YesButton = "PART_YesButton";
    public const string PART_NoButton = "PART_NoButton";
    public const string PART_OKButton = "PART_OKButton";
    public const string PART_CancelButton = "PART_CancelButton";
    
    private Button? _yesButton;
    private Button? _noButton;
    private Button? _okButton;
    private Button? _cancelButton;

    public static readonly StyledProperty<DialogButton> ButtonsProperty = AvaloniaProperty.Register<DefaultDialogWindow, DialogButton>(
        nameof(Buttons));

    public DialogButton Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    public static readonly StyledProperty<DialogMode> ModeProperty = AvaloniaProperty.Register<DefaultDialogWindow, DialogMode>(
        nameof(Mode));

    public DialogMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnDefaultClose, _okButton, _cancelButton, _yesButton, _noButton);
        _okButton = e.NameScope.Find<Button>(PART_OKButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        _yesButton = e.NameScope.Find<Button>(PART_YesButton);
        _noButton = e.NameScope.Find<Button>(PART_NoButton);
        Button.ClickEvent.AddHandler(OnDefaultClose, _okButton, _cancelButton, _yesButton, _noButton);
        SetButtonVisibility();
    }

    private void OnDefaultClose(object sender, RoutedEventArgs e)
    {
        if (sender == _yesButton)
        {
            Close(DialogResult.Yes);
            return;
        }
        if(sender == _noButton)
        {
            Close(DialogResult.No);
            return;
        }
        if(sender == _okButton)
        {
            Close(DialogResult.OK);
            return;
        }
        if(sender == _cancelButton)
        {
            Close(DialogResult.Cancel);
            return;
        }
    }

    private void SetButtonVisibility()
    {
        bool closeButtonVisible = DataContext is IDialogContext || Buttons != DialogButton.YesNo;
        SetVisibility(_closeButton, closeButtonVisible);
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
    
    private void SetVisibility(Button? button, bool visible)
    {
        if (button is not null) button.IsVisible = visible;
    }

    protected internal override void OnCloseButtonClicked(object sender, RoutedEventArgs args)
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
            Close(result);
        }
    }
}