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

[TemplatePart(PART_FirstTextBox, typeof(TextBox))]
[TemplatePart(PART_SecondTextBox, typeof(TextBox))]
[TemplatePart(PART_ThirdTextBox, typeof(TextBox))]
[TemplatePart(PART_FourthTextBox, typeof(TextBox))]
[TemplatePart(PART_PortTextBox, typeof(TextBox))]
public class IPv4Box: TemplatedControl
{
    public const string PART_FirstTextBox = "PART_FirstTextPresenter";
    public const string PART_SecondTextBox = "PART_SecondTextPresenter";
    public const string PART_ThirdTextBox = "PART_ThirdTextPresenter";
    public const string PART_FourthTextBox = "PART_FourthTextPresenter";
    public const string PART_PortTextBox = "PART_PortNumericInput";
    private TextBox? _firstText;
    private TextBox? _secondText;
    private TextBox? _thirdText;
    private TextBox? _fourthText;
    private TextBox? _portText;

    public static readonly StyledProperty<bool> ShowPortProperty = AvaloniaProperty.Register<IPv4Box, bool>(
        nameof(ShowPort));

    public bool ShowPort
    {
        get => GetValue(ShowPortProperty);
        set => SetValue(ShowPortProperty, value);
    }
    
    public static readonly StyledProperty<IPAddress> IPAddressProperty = AvaloniaProperty.Register<IPv4Box, IPAddress>(
        nameof(IPAddress));

    public IPAddress IPAddress
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
        _firstText = e.NameScope.Find<TextBox>(PART_FirstTextBox);
        _secondText = e.NameScope.Find<TextBox>(PART_SecondTextBox);
        _thirdText = e.NameScope.Find<TextBox>(PART_ThirdTextBox);
        _fourthText = e.NameScope.Find<TextBox>(PART_FourthTextBox);
        _portText = e.NameScope.Find<TextBox>(PART_PortTextBox);

        _firstText.LostFocus += OnTextLostFocus;
    }

    private void OnTextKeyDown(object sender, KeyEventArgs args)
    {
        if (args.Key == Key.Right)
        {
            _secondText?.Focus();
        }
    }

    private void OnTextLostFocus(object? sender, RoutedEventArgs args)
    {
        if (sender?.Equals(_firstText)??false)
        {
            Debug.WriteLine("FIRST");
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Left)
        {
            if ((_firstText?.IsFocused??false) && _firstText.SelectionStart == 0)
            {
                _secondText?.Focus();
            }
        }
        else if (e.Key == Key.Right)
        {
            if ((_firstText?.IsFocused ?? false) && _firstText?.SelectionEnd == 2) 
            {
                _firstText.Focus();
            }
        }
        else if (e.Key == Key.Tab)
        {
            
        }
        else if (!(e.Key is >= Key.D0 and <= Key.D9 || e.Key is >= Key.NumPad0 and <= Key.NumPad9))
        {
            return;
        }
        base.OnKeyDown(e);
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        if (_firstText != null)
        {
            _firstText.Text = e.Text;
        }
        base.OnTextInput(e);
    }
}