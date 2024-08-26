using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_YesButton, typeof(Button))]
[TemplatePart(PART_NoButton, typeof(Button))]
[TemplatePart(PART_OKButton, typeof(Button))]
[TemplatePart(PART_CancelButton, typeof(Button))]
public class DefaultDialogWindow : DialogWindow
{
    public const string PART_YesButton = "PART_YesButton";
    public const string PART_NoButton = "PART_NoButton";
    public const string PART_OKButton = "PART_OKButton";
    public const string PART_CancelButton = "PART_CancelButton";

    public static readonly StyledProperty<DialogButton> ButtonsProperty =
        AvaloniaProperty.Register<DefaultDialogWindow, DialogButton>(
            nameof(Buttons));

    public static readonly StyledProperty<DialogMode> ModeProperty =
        AvaloniaProperty.Register<DefaultDialogWindow, DialogMode>(
            nameof(Mode));

    private Button? _cancelButton;
    private Button? _noButton;
    private Button? _okButton;

    private Button? _yesButton;
    protected override Type StyleKeyOverride { get; } = typeof(DefaultDialogWindow);

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
        Button.ClickEvent.RemoveHandler(OnDefaultClose, _okButton, _cancelButton, _yesButton, _noButton);
        _okButton = e.NameScope.Find<Button>(PART_OKButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        _yesButton = e.NameScope.Find<Button>(PART_YesButton);
        _noButton = e.NameScope.Find<Button>(PART_NoButton);
        Button.ClickEvent.AddHandler(OnDefaultClose, _okButton, _cancelButton, _yesButton, _noButton);
        SetButtonVisibility();
    }

    private void OnDefaultClose(object? sender, RoutedEventArgs e)
    {
        if (Equals(sender, _yesButton))
            Close(DialogResult.Yes);
        else if (Equals(sender, _noButton))
            Close(DialogResult.No);
        else if (Equals(sender, _okButton))
            Close(DialogResult.OK);
        else if (Equals(sender, _cancelButton))
            Close(DialogResult.Cancel);
    }

    private void SetButtonVisibility()
    {
        // Close button should be hidden instead if invisible to retain layout. 
        IsVisibleProperty.SetValue(true, _closeButton);
        var closeButtonVisible =
            IsCloseButtonVisible ?? (DataContext is IDialogContext || Buttons != DialogButton.YesNo);
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

    protected override void OnCloseButtonClicked(object? sender, RoutedEventArgs args)
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
            Close(result);
        }
    }
}