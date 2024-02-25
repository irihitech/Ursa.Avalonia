using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Irihi.Avalonia.Shared.Helpers;
using Ursa.Common;

namespace Ursa.Controls;

/// <summary>
/// The messageBox used to display in OverlayDialogHost. 
/// </summary>
[TemplatePart(PART_NoButton, typeof(Button))]
[TemplatePart(PART_OKButton, typeof(Button))]
[TemplatePart(PART_CancelButton, typeof(Button))]
[TemplatePart(PART_YesButton, typeof(Button))]
public class MessageBoxControl: DialogControlBase
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
        Button.ClickEvent.RemoveHandler(DefaultButtonsClose, _okButton, _cancelButton, _yesButton, _noButton);
        _okButton = e.NameScope.Find<Button>(PART_OKButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        _yesButton = e.NameScope.Find<Button>(PART_YesButton);
        _noButton = e.NameScope.Find<Button>(PART_NoButton);
        Button.ClickEvent.AddHandler(DefaultButtonsClose, _okButton, _cancelButton, _yesButton, _noButton);
        SetButtonVisibility();
    }

    private void DefaultButtonsClose(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            if (button == _okButton)
            {
                OnElementClosing(this, MessageBoxResult.OK);
            }
            else if (button == _cancelButton)
            {
                OnElementClosing(this, MessageBoxResult.Cancel);
            }
            else if (button == _yesButton)
            {
                OnElementClosing(this, MessageBoxResult.Yes);
            }
            else if (button == _noButton)
            {
                OnElementClosing(this, MessageBoxResult.No);
            }
        }
    }
    
    private void SetButtonVisibility()
    {
        switch (Buttons)
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

    public override void Close()
    {
        MessageBoxResult result = Buttons switch
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