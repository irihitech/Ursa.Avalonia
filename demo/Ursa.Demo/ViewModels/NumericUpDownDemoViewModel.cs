using Avalonia.Controls;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Globalization;
using Ursa.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ursa.Demo.ViewModels;

public partial class NumericUpDownDemoViewModel : ObservableObject
{


    private double _oldWidth = 300;
    [ObservableProperty] private bool _AutoWidth = true;
    [ObservableProperty] private double _Width = Double.NaN;
    [ObservableProperty] private uint _Value;
    [ObservableProperty] private string _FontFamily = "Consolas";
    [ObservableProperty] private bool _AllowDrag = false;
    [ObservableProperty] private bool _IsReadOnly = false;

    [ObservableProperty] private Array _Array_HorizontalAlignment;
    [ObservableProperty] private HorizontalAlignment _HorizontalAlignment = HorizontalAlignment.Center;

    [ObservableProperty] private Array _Array_HorizontalContentAlignment;
    [ObservableProperty] private HorizontalAlignment _HorizontalContentAlignment = HorizontalAlignment.Center;
    [ObservableProperty] private object? _InnerLeftContent = "obj:0x";
    [ObservableProperty] private string _Watermark = "Water mark showed";
    [ObservableProperty] private string _FormatString = "X8";
    [ObservableProperty] private Array _Array_ParsingNumberStyle;
    [ObservableProperty] private NumberStyles _ParsingNumberStyle = NumberStyles.AllowHexSpecifier;
    [ObservableProperty] private bool _AllowSpin = true;
    [ObservableProperty] private bool _ShowButtonSpinner = true;

    [ObservableProperty] private UInt32 _Maximum = UInt32.MaxValue;
    [ObservableProperty] private UInt32 _Minimum = UInt32.MinValue;
    [ObservableProperty] private UInt32 _Step = 1;

    [ObservableProperty] private bool _IsEnable = true;

    [ObservableProperty] private string _CommandUpdateText = "Command not Execute";
    [ObservableProperty] private string _ValueChangedUpdateText = "Command not Execute";

    [ObservableProperty] private bool _IsUpdateValueWhenLostFocus = false;

    uint v = 0;
    [RelayCommand]
    // void Trythis()
    void Trythis(uint v)
    // void Trythis(object v)
    {
        CommandUpdateText = $"Command Exe,Parameter={v}";
    }


    public NumericUpDownDemoViewModel()
    {
        Array_HorizontalContentAlignment = Enum.GetValues(typeof(HorizontalAlignment));
        Array_HorizontalAlignment = Enum.GetValues(typeof(HorizontalAlignment));
        Array_ParsingNumberStyle = Enum.GetValues(typeof(NumberStyles));
        NumericUIntUpDown numericUIntUpDown;
        TextBox textBox;

    }

    partial void OnAutoWidthChanged(bool value)
    {
        if (value)
        {
            _oldWidth = Width;
            Width = double.NaN;
        }
        else
        {
            Width = _oldWidth;
        }
    }

    partial void OnValueChanging(uint oldValue, uint newValue)
    {
        // Console.WriteLine(oldValue);
    }

}