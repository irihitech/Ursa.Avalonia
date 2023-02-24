using System.Diagnostics;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Ursa.Controls;

[TemplatePart(PART_FirstTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_SecondTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_ThirdTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_FourthTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_PortTextPresenter, typeof(TextPresenter))]
public class IPv4Box: TemplatedControl
{
    public const string PART_FirstTextPresenter = "PART_FirstTextPresenter";
    public const string PART_SecondTextPresenter = "PART_SecondTextPresenter";
    public const string PART_ThirdTextPresenter = "PART_ThirdTextPresenter";
    public const string PART_FourthTextPresenter = "PART_FourthTextPresenter";
    public const string PART_PortTextPresenter = "PART_PortTextPresenter";
    private TextPresenter? _firstText;
    private TextPresenter? _secondText;
    private TextPresenter? _thirdText;
    private TextPresenter? _fourthText;
    private TextPresenter? _portText;
    private byte _firstByte;
    private byte _secondByte;
    private byte _thridByte;
    private byte _fourthByte;
    private int _port;
    private TextPresenter?[] _presenters = new TextPresenter?[5];
    private TextPresenter? _currentActivePresenter;

    public static readonly StyledProperty<bool> ShowPortProperty = AvaloniaProperty.Register<IPv4Box, bool>(
        nameof(ShowPort));

    public bool ShowPort
    {
        get => GetValue(ShowPortProperty);
        set => SetValue(ShowPortProperty, value);
    }
    
    public static readonly StyledProperty<IPAddress?> IPAddressProperty = AvaloniaProperty.Register<IPv4Box, IPAddress?>(
        nameof(IPAddress));

    public IPAddress? IPAddress
    {
        get => GetValue(IPAddressProperty);
        set => SetValue(IPAddressProperty, value);
    }
    
    public static readonly StyledProperty<int> PortProperty = AvaloniaProperty.Register<IPv4Box, int>(
        nameof(Port));

    public int Port
    {
        get => GetValue(PortProperty);
        set => SetValue(PortProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        ClearTextPresenterEvents(_firstText);
        ClearTextPresenterEvents(_secondText);
        ClearTextPresenterEvents(_thirdText);
        ClearTextPresenterEvents(_fourthText);
        ClearTextPresenterEvents(_portText);
        _firstText = e.NameScope.Find<TextPresenter>(PART_FirstTextPresenter);
        _secondText = e.NameScope.Find<TextPresenter>(PART_SecondTextPresenter);
        _thirdText = e.NameScope.Find<TextPresenter>(PART_ThirdTextPresenter);
        _fourthText = e.NameScope.Find<TextPresenter>(PART_FourthTextPresenter);
        _portText = e.NameScope.Find<TextPresenter>(PART_PortTextPresenter);
        _presenters[0] = _portText;
        _presenters[1] = _firstText;
        _presenters[2] = _secondText;
        _presenters[3] = _thirdText;
        _presenters[4] = _fourthText;
        RegisterTextPresenterEvents(_firstText);
        RegisterTextPresenterEvents(_secondText);
        RegisterTextPresenterEvents(_thirdText);
        RegisterTextPresenterEvents(_fourthText);
        RegisterTextPresenterEvents(_portText);
    }

    private void ClearTextPresenterEvents(TextPresenter? presenter)
    {
        if (presenter is null) return;
        presenter.LostFocus -= OnTextPresenterLostFocus;
    }

    private void RegisterTextPresenterEvents(TextPresenter? presenter)
    {
        if(presenter  is null) return;
        presenter.LostFocus += OnTextPresenterLostFocus;
    }

    private void OnTextPresenterLostFocus(object? sender, RoutedEventArgs args)
    {
        if (sender is TextPresenter p)
        {
            p.HideCaret();
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        var source = e.Source;
        Point position = e.GetPosition(_firstText);
        foreach (var presenter in _presenters)
        {
            if (presenter?.Bounds.Contains(position) ?? false)
            {
                presenter?.ShowCaret();
                _currentActivePresenter = presenter;
            }
            else
            {
                presenter?.HideCaret();
            }
        }
        Debug.WriteLine(_currentActivePresenter?.Name);
        base.OnPointerPressed(e);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        _firstText?.HideCaret();
        _secondText?.HideCaret();
        _thirdText?.HideCaret();
        _fourthText?.HideCaret();
        _portText?.HideCaret();
        _currentActivePresenter = null;
    }
}