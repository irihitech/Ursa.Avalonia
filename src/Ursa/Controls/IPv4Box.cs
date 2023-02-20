using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Ursa.Controls;

[TemplatePart(PART_FirstTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_SecondTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_ThirdTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_FourthTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_PortNumericInput, typeof(NumericUpDown))]
public class IPv4Box: TemplatedControl
{
    public const string PART_FirstTextPresenter = "PART_FirstTextPresenter";
    public const string PART_SecondTextPresenter = "PART_SecondTextPresenter";
    public const string PART_ThirdTextPresenter = "PART_ThirdTextPresenter";
    public const string PART_FourthTextPresenter = "PART_FourthTextPresenter";
    public const string PART_PortNumericInput = "PART_PortNumericInput";
    private TextPresenter? _firstTextPresenter;
    private TextPresenter? _secondTextPresenter;
    private TextPresenter? _thirdTextPresenter;
    private TextPresenter? _fourthTextPresenter;
    private NumericUpDown? _portNumericInput;

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
        _firstTextPresenter = e.NameScope.Find<TextPresenter>(PART_FirstTextPresenter);
        _secondTextPresenter = e.NameScope.Find<TextPresenter>(PART_SecondTextPresenter);
        _thirdTextPresenter = e.NameScope.Find<TextPresenter>(PART_ThirdTextPresenter);
        _fourthTextPresenter = e.NameScope.Find<TextPresenter>(PART_FourthTextPresenter);
        _portNumericInput = e.NameScope.Find<NumericUpDown>(PART_PortNumericInput);
        var box = new TextBox();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Left)
        {
            if ((_firstTextPresenter?.IsFocused??false) && _firstTextPresenter.SelectionStart == 0)
            {
                _secondTextPresenter?.Focus();
            }
        }
        else if (e.Key == Key.Right)
        {
            if ((_firstTextPresenter?.IsFocused ?? false) && _firstTextPresenter?.SelectionEnd == 2) 
            {
                _firstTextPresenter.Focus();
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
}