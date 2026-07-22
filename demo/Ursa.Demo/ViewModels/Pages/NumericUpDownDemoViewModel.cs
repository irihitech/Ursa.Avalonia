using System;
using System.Globalization;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class NumericUpDownDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_NumericUpDown,
        Description = LanguageManager.Instance.Page_Description_NumericUpDown,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_NumericUpDown)],
        Tags = ["NumericUpDown", "Input", "Number"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/NumericUpDownDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/NumericUpDownDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };



    private double _oldWidth = 300;
    [ObservableProperty] public partial bool AutoWidth { get; set; } = true;
    [ObservableProperty] public partial double Width { get; set; } = double.NaN;
    [ObservableProperty] public partial uint Value { get; set; }
    [ObservableProperty] public partial string FontFamily { get; set; } = "Consolas";
    [ObservableProperty] public partial bool AllowDrag { get; set; }
    [ObservableProperty] public partial bool IsReadOnly { get; set; }

    [ObservableProperty] public partial Array ArrayHorizontalAlignment { get; set; }
    [ObservableProperty] public partial HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;

    [ObservableProperty] public partial Array ArrayHorizontalContentAlignment { get; set; }
    [ObservableProperty] public partial HorizontalAlignment HorizontalContentAlignment { get; set; } = HorizontalAlignment.Center;
    [ObservableProperty] public partial object? InnerLeftContent { get; set; } = "obj:0x";
    [ObservableProperty] public partial object? InnerRightContent { get; set; } = "%";
    [ObservableProperty] public partial string PlaceholderText { get; set; } = "Placeholder Text showed";
    [ObservableProperty] public partial string FormatString { get; set; } = "X8";
    [ObservableProperty] public partial Array ArrayParsingNumberStyle { get; set; }
    [ObservableProperty] public partial NumberStyles ParsingNumberStyle { get; set; } = NumberStyles.AllowHexSpecifier;
    [ObservableProperty] public partial bool AllowSpin { get; set; } = true;
    [ObservableProperty] public partial bool ShowButtonSpinner { get; set; } = true;

    [ObservableProperty] public partial UInt32 Maximum { get; set; } = UInt32.MaxValue;
    [ObservableProperty] public partial UInt32 Minimum { get; set; } = UInt32.MinValue;
    [ObservableProperty] public partial UInt32 Step { get; set; } = 1;

    [ObservableProperty] public partial bool IsEnable { get; set; } = true;

    [ObservableProperty] public partial string CommandUpdateText { get; set; } = "Command not Execute";
    
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