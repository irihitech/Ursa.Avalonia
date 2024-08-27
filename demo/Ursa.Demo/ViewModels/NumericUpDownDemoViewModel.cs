using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Globalization;

namespace Ursa.Demo.ViewModels;

public partial class NumericUpDownDemoViewModel : ObservableObject
{


    private double _oldWidth = 300;
    [ObservableProperty] private bool _autoWidth = true;
    [ObservableProperty] private double _width = double.NaN;
    [ObservableProperty] private uint _value;
    [ObservableProperty] private string _fontFamily = "Consolas";
    [ObservableProperty] private bool _allowDrag;
    [ObservableProperty] private bool _isReadOnly;

    [ObservableProperty] private Array _arrayHorizontalAlignment;
    [ObservableProperty] private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Center;

    [ObservableProperty] private Array _arrayHorizontalContentAlignment;
    [ObservableProperty] private HorizontalAlignment _horizontalContentAlignment = HorizontalAlignment.Center;
    [ObservableProperty] private object? _innerLeftContent = "obj:0x";
    [ObservableProperty] private object? _innerRightContent = "%";
    [ObservableProperty] private string _watermark = "Water mark showed";
    [ObservableProperty] private string _formatString = "X8";
    [ObservableProperty] private Array _arrayParsingNumberStyle;
    [ObservableProperty] private NumberStyles _parsingNumberStyle = NumberStyles.AllowHexSpecifier;
    [ObservableProperty] private bool _allowSpin = true;
    [ObservableProperty] private bool _showButtonSpinner = true;

    [ObservableProperty] private UInt32 _maximum = UInt32.MaxValue;
    [ObservableProperty] private UInt32 _minimum = UInt32.MinValue;
    [ObservableProperty] private UInt32 _step = 1;

    [ObservableProperty] private bool _isEnable = true;

    [ObservableProperty] private string _commandUpdateText = "Command not Execute";
    
    [RelayCommand]
    void Trythis(uint v)
    {
        CommandUpdateText = $"Command Exe,CommandParameter={v}";
    }


    public NumericUpDownDemoViewModel()
    {
        ArrayHorizontalContentAlignment = Enum.GetValues(typeof(HorizontalAlignment));
        ArrayHorizontalAlignment = Enum.GetValues(typeof(HorizontalAlignment));
        ArrayParsingNumberStyle = Enum.GetValues(typeof(NumberStyles));
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
}