using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Ursa.Common;

namespace Ursa.Controls;

/// <summary>
/// The messageBox used to display in OverlayDialogHost. 
/// </summary>
[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_NoButton, typeof(Button))]
[TemplatePart(PART_OKButton, typeof(Button))]
[TemplatePart(PART_CancelButton, typeof(Button))]
[TemplatePart(PART_YesButton, typeof(Button))]
public class MessageBoxControl: DialogControl
{
    public const string PART_YesButton = "PART_YesButton";
    public const string PART_NoButton = "PART_NoButton";
    public const string PART_OKButton = "PART_OKButton";
    public const string PART_CancelButton = "PART_CancelButton";
    
    private Button? _yesButton;
    private Button? _noButton;
    private Button? _okButton;
    private Button? _cancelButton;
    
    public static readonly StyledProperty<MessageBoxIcon> MessageIconProperty =
        AvaloniaProperty.Register<MessageBoxWindow, MessageBoxIcon>(
            nameof(MessageIcon));

    public MessageBoxIcon MessageIcon
    {
        get => GetValue(MessageIconProperty);
        set => SetValue(MessageIconProperty, value);
    }

    public static readonly StyledProperty<MessageBoxButton> ButtonsProperty = AvaloniaProperty.Register<MessageBoxControl, MessageBoxButton>(
        nameof(Buttons), MessageBoxButton.OK);

    public MessageBoxButton Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    public static readonly StyledProperty<string?> TitleProperty = AvaloniaProperty.Register<MessageBoxControl, string?>(
        nameof(Title));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    static MessageBoxControl()
    {
        ButtonsProperty.Changed.AddClassHandler<MessageBoxControl>((o, e) => { o.SetButtonVisibility(); });
    }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        EventHelper.UnregisterClickEvent(DefaultButtonsClose, _okButton, _cancelButton, _yesButton, _noButton);
        _okButton = e.NameScope.Find<Button>(PART_OKButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        _yesButton = e.NameScope.Find<Button>(PART_YesButton);
        _noButton = e.NameScope.Find<Button>(PART_NoButton);
        EventHelper.RegisterClickEvent(DefaultButtonsClose, _okButton, _cancelButton, _yesButton, _noButton);
        SetButtonVisibility();
    }

    private void DefaultButtonsClose(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            if (button == _okButton)
            {
                OnDialogControlClosing(this, MessageBoxResult.OK);
            }
            else if (button == _cancelButton)
            {
                OnDialogControlClosing(this, MessageBoxResult.Cancel);
            }
            else if (button == _yesButton)
            {
                OnDialogControlClosing(this, MessageBoxResult.Yes);
            }
            else if (button == _noButton)
            {
                OnDialogControlClosing(this, MessageBoxResult.No);
            }
        }
    }
    
    private void SetButtonVisibility()
    {
        switch (Buttons)
        {
            case MessageBoxButton.OK:
                SetVisibility(_okButton, true);
                SetVisibility(_cancelButton, false);
                SetVisibility(_yesButton, false);
                SetVisibility(_noButton, false);
                break;
            case MessageBoxButton.OKCancel:
                SetVisibility(_okButton, true);
                SetVisibility(_cancelButton, true);
                SetVisibility(_yesButton, false);
                SetVisibility(_noButton, false);
                break;
            case MessageBoxButton.YesNo:
                SetVisibility(_okButton, false);
                SetVisibility(_cancelButton, false);
                SetVisibility(_yesButton, true);
                SetVisibility(_noButton, true);
                break;
            case MessageBoxButton.YesNoCancel:
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
}