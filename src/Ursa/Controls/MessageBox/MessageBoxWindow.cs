using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_NoButton, typeof(Button))]
[TemplatePart(PART_OKButton, typeof(Button))]
[TemplatePart(PART_CancelButton, typeof(Button))]
[TemplatePart(PART_YesButton, typeof(Button))]
public class MessageBoxWindow: Window
{
    public const string PART_CloseButton = "PART_CloseButton";
    public const string PART_YesButton = "PART_YesButton";
    public const string PART_NoButton = "PART_NoButton";
    public const string PART_OKButton = "PART_OKButton";
    public const string PART_CancelButton = "PART_CancelButton";

    private MessageBoxButton _buttonConfigs;
    
    private Button? _closeButton;
    private Button? _yesButton;
    private Button? _noButton;
    private Button? _okButton;
    private Button? _cancelButton;
    
    protected override Type StyleKeyOverride => typeof(MessageBoxWindow);

    static MessageBoxWindow()
    {
        
    }

    public MessageBoxWindow()
    {
        _buttonConfigs = MessageBoxButton.OK;
    }

    public MessageBoxWindow(MessageBoxButton buttons)
    {
        _buttonConfigs = buttons;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_closeButton != null)
        {
            _closeButton.Click -= OnCloseButtonClick;
        }
        if (_yesButton != null)
        {
            _yesButton.Click -= OnYesButtonClick;
        }
        if (_noButton != null)
        {
            _noButton.Click -= OnNoButtonClick;
        }
        if (_okButton != null)
        {
            _okButton.Click -= OnOKButtonClick;
        }
        if (_cancelButton != null)
        {
            _cancelButton.Click -= OnCancelButtonClick;
        }
        _yesButton = e.NameScope.Find<Button>(PART_YesButton);
        _noButton = e.NameScope.Find<Button>(PART_NoButton);
        _okButton = e.NameScope.Find<Button>(PART_OKButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        if (_closeButton is not null)
        {
            _closeButton.Click += OnCloseButtonClick;
        }
        if (_yesButton is not null)
        {
            _yesButton.Click += OnYesButtonClick;
        }
        if (_noButton is not null)
        {
            _noButton.Click += OnNoButtonClick;
        }
        if (_okButton is not null)
        {
            _okButton.Click += OnOKButtonClick;
        }
        if (_cancelButton is not null)
        {
            _cancelButton.Click += OnCancelButtonClick;
        }
        SetButtonVisibility();
    }

    private void SetButtonVisibility()
    {
        if (_buttonConfigs == MessageBoxButton.OK)
        {
            if (_closeButton != null) _closeButton.IsVisible = true;
            if (_yesButton != null) _yesButton.IsVisible = false;
            if (_noButton != null) _noButton.IsVisible = false;
            if (_okButton != null) _okButton.IsVisible = true;
            if (_cancelButton != null) _cancelButton.IsVisible = false;
        }
        else if (_buttonConfigs == MessageBoxButton.OKCancel)
        {
            if (_closeButton != null) _closeButton.IsVisible = true;
            if (_yesButton != null) _yesButton.IsVisible = false;
            if (_noButton != null) _noButton.IsVisible = false;
            if (_okButton != null) _okButton.IsVisible = true;
            if (_cancelButton != null) _cancelButton.IsVisible = true;
        }
        else if (_buttonConfigs == MessageBoxButton.YesNo)
        {
            if (_closeButton != null) _closeButton.IsVisible = false;
            if (_yesButton != null) _yesButton.IsVisible = true;
            if (_noButton != null) _noButton.IsVisible = true;
            if (_okButton != null) _okButton.IsVisible = false;
            if (_cancelButton != null) _cancelButton.IsVisible = false;
        }
        else if (_buttonConfigs == MessageBoxButton.YesNoCancel)
        {
            if (_closeButton != null) _closeButton.IsVisible = true;
            if (_yesButton != null) _yesButton.IsVisible = true;
            if (_noButton != null) _noButton.IsVisible = true;
            if (_okButton != null) _okButton.IsVisible = false;
            if (_cancelButton != null) _cancelButton.IsVisible = true;
        }
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        if (_buttonConfigs == MessageBoxButton.OK)
        {
            Close(MessageBoxResult.OK);
        }
        Close(MessageBoxResult.Cancel);
    }
    
    private void OnYesButtonClick(object sender, RoutedEventArgs e)
    {
        Close(MessageBoxResult.Yes);
    }
    
    private void OnNoButtonClick(object sender, RoutedEventArgs e)
    {
        Close(MessageBoxResult.No);
    }
    
    private void OnOKButtonClick(object sender, RoutedEventArgs e)
    {
        Close(MessageBoxResult.OK);
    }
    
    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        Close(MessageBoxResult.Cancel);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        if (e.Key == Key.Escape && _buttonConfigs == MessageBoxButton.OK)
        {
            Close(MessageBoxResult.OK);
        }
    }
}