using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_NoButton, typeof(Button))]
[TemplatePart(PART_OKButton, typeof(Button))]
[TemplatePart(PART_CancelButton, typeof(Button))]
[TemplatePart(PART_YesButton, typeof(Button))]
public class MessageBoxWindow(MessageBoxButton buttons) : Window
{
    public const string PART_CloseButton = "PART_CloseButton";
    public const string PART_YesButton = "PART_YesButton";
    public const string PART_NoButton = "PART_NoButton";
    public const string PART_OKButton = "PART_OKButton";
    public const string PART_CancelButton = "PART_CancelButton";

    private Button? _closeButton;
    private Button? _yesButton;
    private Button? _noButton;
    private Button? _okButton;
    private Button? _cancelButton;

    protected override Type StyleKeyOverride => typeof(MessageBoxWindow);

    public static readonly StyledProperty<MessageBoxIcon> MessageIconProperty =
        AvaloniaProperty.Register<MessageBoxWindow, MessageBoxIcon>(
            nameof(MessageIcon));

    public MessageBoxIcon MessageIcon
    {
        get => GetValue(MessageIconProperty);
        set => SetValue(MessageIconProperty, value);
    }

    public MessageBoxWindow() : this(MessageBoxButton.OK)
    {
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnDefaultButtonClick, _yesButton, _noButton, _okButton, _cancelButton);
        Button.ClickEvent.RemoveHandler(OnCloseButtonClick, _closeButton);
        _yesButton = e.NameScope.Find<Button>(PART_YesButton);
        _noButton = e.NameScope.Find<Button>(PART_NoButton);
        _okButton = e.NameScope.Find<Button>(PART_OKButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        Button.ClickEvent.AddHandler(OnDefaultButtonClick, _yesButton, _noButton, _okButton, _cancelButton);
        Button.ClickEvent.AddHandler(OnCloseButtonClick, _closeButton);
        SetButtonVisibility();
    }

    private void SetButtonVisibility()
    {
        switch (buttons)
        {
            case MessageBoxButton.OK:
                Button.IsVisibleProperty.SetValue(true, _okButton);
                Button.IsVisibleProperty.SetValue(false, _cancelButton, _yesButton, _noButton);
                break;
            case MessageBoxButton.OKCancel:
                Button.IsVisibleProperty.SetValue(true, _okButton, _cancelButton);
                Button.IsVisibleProperty.SetValue(false, _yesButton, _noButton);
                break;
            case MessageBoxButton.YesNo:
                Button.IsVisibleProperty.SetValue(false, _okButton, _cancelButton);
                Button.IsVisibleProperty.SetValue(true, _yesButton, _noButton);
                break;
            case MessageBoxButton.YesNoCancel:
                Button.IsVisibleProperty.SetValue(false, _okButton);
                Button.IsVisibleProperty.SetValue(true, _cancelButton, _yesButton, _noButton);
                break;
        }
    }

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e)
    {
        if (buttons == MessageBoxButton.OK)
        {
            Close(MessageBoxResult.OK);
        }

        Close(MessageBoxResult.Cancel);
    }

    private void OnDefaultButtonClick(object? sender, RoutedEventArgs e)
    {
        if (Equals(sender, _okButton))
        {
            Close(MessageBoxResult.OK);
        }
        else if (Equals(sender, _cancelButton))
        {
            Close(MessageBoxResult.Cancel);
        }
        else if (Equals(sender, _yesButton))
        {
            Close(MessageBoxResult.Yes);
        }
        else if (Equals(sender, _noButton))
        {
            Close(MessageBoxResult.No);
        }
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        if (e.Key == Key.Escape && buttons == MessageBoxButton.OK)
        {
            Close(MessageBoxResult.OK);
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }
}