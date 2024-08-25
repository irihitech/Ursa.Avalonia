using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

/// <summary>
///     The messageBox used to display in OverlayDialogHost.
/// </summary>
[TemplatePart(PART_NoButton, typeof(Button))]
[TemplatePart(PART_OKButton, typeof(Button))]
[TemplatePart(PART_CancelButton, typeof(Button))]
[TemplatePart(PART_YesButton, typeof(Button))]
public class MessageBoxControl : DialogControlBase
{
    public const string PART_YesButton = "PART_YesButton";
    public const string PART_NoButton = "PART_NoButton";
    public const string PART_OKButton = "PART_OKButton";
    public const string PART_CancelButton = "PART_CancelButton";

    public static readonly StyledProperty<MessageBoxIcon> MessageIconProperty =
        AvaloniaProperty.Register<MessageBoxWindow, MessageBoxIcon>(
            nameof(MessageIcon));

    public static readonly StyledProperty<MessageBoxButton> ButtonsProperty =
        AvaloniaProperty.Register<MessageBoxControl, MessageBoxButton>(
            nameof(Buttons));

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<MessageBoxControl, string?>(
            nameof(Title));

    private Button? _cancelButton;
    private Button? _noButton;
    private Button? _okButton;

    private Button? _yesButton;

    static MessageBoxControl()
    {
        ButtonsProperty.Changed.AddClassHandler<MessageBoxControl>((o, _) => { o.SetButtonVisibility(); });
    }

    public MessageBoxIcon MessageIcon
    {
        get => GetValue(MessageIconProperty);
        set => SetValue(MessageIconProperty, value);
    }

    public MessageBoxButton Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
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

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var defaultButton = Buttons switch
        {
            MessageBoxButton.OK => _okButton,
            MessageBoxButton.OKCancel => _cancelButton,
            MessageBoxButton.YesNo => _yesButton,
            MessageBoxButton.YesNoCancel => _cancelButton,
            _ => null
        };
        defaultButton?.Focus();
    }

    private void DefaultButtonsClose(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button) return;
        var result = button switch
        {
            _ when button == _okButton => MessageBoxResult.OK,
            _ when button == _cancelButton => MessageBoxResult.Cancel,
            _ when button == _yesButton => MessageBoxResult.Yes,
            _ when button == _noButton => MessageBoxResult.No,
            _ => MessageBoxResult.None
        };
        OnElementClosing(this, result);
    }

    private void SetButtonVisibility()
    {
        var closeButtonVisible = Buttons != MessageBoxButton.YesNo;
        IsVisibleProperty.SetValue(closeButtonVisible, _closeButton);
        switch (Buttons)
        {
            case MessageBoxButton.OK:
                IsVisibleProperty.SetValue(true, _okButton);
                IsVisibleProperty.SetValue(false, _cancelButton, _yesButton, _noButton);
                Button.IsDefaultProperty.SetValue(true, _okButton);
                Button.IsDefaultProperty.SetValue(false, _cancelButton, _yesButton, _noButton);
                break;
            case MessageBoxButton.OKCancel:
                IsVisibleProperty.SetValue(true, _okButton, _cancelButton);
                IsVisibleProperty.SetValue(false, _yesButton, _noButton);
                Button.IsDefaultProperty.SetValue(true, _okButton);
                Button.IsDefaultProperty.SetValue(false, _cancelButton, _yesButton, _noButton);
                break;
            case MessageBoxButton.YesNo:
                IsVisibleProperty.SetValue(false, _okButton, _cancelButton);
                IsVisibleProperty.SetValue(true, _yesButton, _noButton);
                break;
            case MessageBoxButton.YesNoCancel:
                IsVisibleProperty.SetValue(false, _okButton);
                IsVisibleProperty.SetValue(true, _cancelButton, _yesButton, _noButton);
                break;
        }
    }

    public override void Close()
    {
        var result = Buttons switch
        {
            MessageBoxButton.OK => MessageBoxResult.OK,
            MessageBoxButton.OKCancel => MessageBoxResult.Cancel,
            MessageBoxButton.YesNo => MessageBoxResult.No,
            MessageBoxButton.YesNoCancel => MessageBoxResult.Cancel,
            _ => MessageBoxResult.None
        };
        OnElementClosing(this, result);
    }
}