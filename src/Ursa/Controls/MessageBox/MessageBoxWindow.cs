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
    
    private Button? _cancelButton;
    private Button? _noButton;
    private Button? _okButton;
    private Button? _yesButton;

    public MessageBoxWindow() : this(MessageBoxButton.OK)
    {
    }

    protected override Type StyleKeyOverride => typeof(MessageBoxWindow);

    public static readonly StyledProperty<MessageBoxIcon> MessageIconProperty = 
        AvaloniaProperty.Register<MessageBoxWindow, MessageBoxIcon>(nameof(MessageIcon));

    public MessageBoxIcon MessageIcon
    {
        get => GetValue(MessageIconProperty);
        set => SetValue(MessageIconProperty, value);
    }

    /// <summary>
    /// Observable source for the message title. When set, the control subscribes to this
    /// observable and forwards each value to the <see cref="Window.Title"/> property.
    /// The previous subscription is automatically disposed.
    /// </summary>
    public static readonly StyledProperty<IObservable<string>?> TitleSourceProperty =
        AvaloniaProperty.Register<MessageBoxWindow, IObservable<string>?>(nameof(TitleSource));

    /// <summary>
    /// Observable source for the message content. When set, the control subscribes to this
    /// observable and forwards each value to the <see cref="ContentControl.Content"/> property.
    /// The previous subscription is automatically disposed.
    /// </summary>
    public static readonly StyledProperty<IObservable<string>?> ContentSourceProperty =
        AvaloniaProperty.Register<MessageBoxWindow, IObservable<string>?>(nameof(ContentSource));

    private IDisposable? _titleSourceSubscription;
    private IDisposable? _contentSourceSubscription;

    static MessageBoxWindow()
    {
        TitleSourceProperty.Changed.AddClassHandler<MessageBoxWindow>((o, e) =>
        {
            o._titleSourceSubscription?.Dispose();
            o._titleSourceSubscription = null;
            if (e.NewValue is IObservable<string> observable)
            {
                o._titleSourceSubscription = observable.Subscribe(s => o.Title = s);
            }
        });
        ContentSourceProperty.Changed.AddClassHandler<MessageBoxWindow>((o, e) =>
        {
            o._contentSourceSubscription?.Dispose();
            o._contentSourceSubscription = null;
            if (e.NewValue is IObservable<string> observable)
            {
                o._contentSourceSubscription = observable.Subscribe(s => o.Content = s);
            }
        });
    }

    public IObservable<string>? TitleSource
    {
        get => GetValue(TitleSourceProperty);
        set => SetValue(TitleSourceProperty, value);
    }

    public IObservable<string>? ContentSource
    {
        get => GetValue(ContentSourceProperty);
        set => SetValue(ContentSourceProperty, value);
    }

    protected override void OnClosed(System.EventArgs e)
    {
        _titleSourceSubscription?.Dispose();
        _contentSourceSubscription?.Dispose();
        base.OnClosed(e);
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
        var closeButtonVisible = buttons != MessageBoxButton.YesNo;
        IsVisibleProperty.SetValue(closeButtonVisible, _closeButton);
        switch (buttons)
        {
            case MessageBoxButton.OK:
                IsVisibleProperty.SetValue(true, _okButton);
                IsVisibleProperty.SetValue(false, _cancelButton, _yesButton, _noButton);
                break;
            case MessageBoxButton.OKCancel:
                IsVisibleProperty.SetValue(true, _okButton, _cancelButton);
                IsVisibleProperty.SetValue(false, _yesButton, _noButton);
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

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e)
    {
        if (buttons == MessageBoxButton.OK) Close(MessageBoxResult.OK);

        Close(MessageBoxResult.Cancel);
    }

    private void OnDefaultButtonClick(object? sender, RoutedEventArgs e)
    {
        if (Equals(sender, _okButton))
            Close(MessageBoxResult.OK);
        else if (Equals(sender, _cancelButton))
            Close(MessageBoxResult.Cancel);
        else if (Equals(sender, _yesButton))
            Close(MessageBoxResult.Yes);
        else if (Equals(sender, _noButton)) Close(MessageBoxResult.No);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        if (e.Key != Key.Escape) return;
        switch (buttons)
        {
            case MessageBoxButton.OK:
                Close(MessageBoxResult.OK);
                break;
            case MessageBoxButton.OKCancel:
                Close(MessageBoxResult.Cancel);
                break;
            case MessageBoxButton.YesNoCancel:
                Close(MessageBoxResult.Cancel);
                break;
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var defaultButton = buttons switch
        {
            MessageBoxButton.OK => _okButton,
            MessageBoxButton.OKCancel => _cancelButton,
            MessageBoxButton.YesNo => _yesButton,
            MessageBoxButton.YesNoCancel => _cancelButton,
            _ => null
        };
        Button.IsDefaultProperty.SetValue(true, defaultButton);
        defaultButton?.Focus();
    }
}
