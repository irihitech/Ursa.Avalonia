using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_OKButton, typeof(Button))]
[TemplatePart(PART_CancelButton, typeof(Button))]
[TemplatePart(PART_YesButton, typeof(Button))]
[TemplatePart(PART_NoButton, typeof(Button))]
public class DefaultDialogControl : DialogControlBase
{
    public const string PART_OKButton = "PART_OKButton";
    public const string PART_CancelButton = "PART_CancelButton";
    public const string PART_YesButton = "PART_YesButton";
    public const string PART_NoButton = "PART_NoButton";

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<DefaultDialogControl, string?>(
            nameof(Title));

    public static readonly StyledProperty<DialogButton> ButtonsProperty =
        AvaloniaProperty.Register<DefaultDialogControl, DialogButton>(
            nameof(Buttons));

    public static readonly StyledProperty<DialogMode> ModeProperty =
        AvaloniaProperty.Register<DefaultDialogControl, DialogMode>(
            nameof(Mode));

    private Button? _cancelButton;
    private Button? _noButton;

    private Button? _okButton;
    private Button? _yesButton;

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public DialogButton Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    public DialogMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(DefaultButtonsClose, _okButton, _cancelButton, _yesButton, _noButton);
        _okButton = e.NameScope.Find<Button>(PART_OKButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        _yesButton = e.NameScope.Find<Button>(PART_YesButton);
        _noButton = e.NameScope.Find<Button>(PART_NoButton);
        Button.ClickEvent.AddHandler(DefaultButtonsClose, _okButton, _cancelButton, _yesButton, _noButton);
        SetButtonVisibility();
    }


    private void SetButtonVisibility()
    {
        var closeButtonVisible =  IsCloseButtonVisible ?? (DataContext is IDialogContext || Buttons != DialogButton.YesNo );
        IsHitTestVisibleProperty.SetValue(closeButtonVisible, _closeButton);
        if (!closeButtonVisible)
        {
            OpacityProperty.SetValue(0, _closeButton);
        }
        switch (Buttons)
        {
            case DialogButton.None:
                IsVisibleProperty.SetValue(false, _okButton, _cancelButton, _yesButton, _noButton);
                break;
            case DialogButton.OK:
                IsVisibleProperty.SetValue(true, _okButton);
                IsVisibleProperty.SetValue(false, _cancelButton, _yesButton, _noButton);
                break;
            case DialogButton.OKCancel:
                IsVisibleProperty.SetValue(true, _okButton, _cancelButton);
                IsVisibleProperty.SetValue(false, _yesButton, _noButton);
                break;
            case DialogButton.YesNo:
                IsVisibleProperty.SetValue(false, _okButton, _cancelButton);
                IsVisibleProperty.SetValue(true, _yesButton, _noButton);
                break;
            case DialogButton.YesNoCancel:
                IsVisibleProperty.SetValue(false, _okButton);
                IsVisibleProperty.SetValue(true, _cancelButton, _yesButton, _noButton);
                break;
        }
    }

    private void DefaultButtonsClose(object? sender, RoutedEventArgs args)
    {
        if (sender is Button button)
        {
            if (button == _okButton)
                OnElementClosing(this, DialogResult.OK);
            else if (button == _cancelButton)
                OnElementClosing(this, DialogResult.Cancel);
            else if (button == _yesButton)
                OnElementClosing(this, DialogResult.Yes);
            else if (button == _noButton) OnElementClosing(this, DialogResult.No);
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
            var result = Buttons switch
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