using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.NumericUpDownDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(NumericUpDownDemo))]
public partial class NumericUpDownDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "NumericUpDown";
    public const string Menu_Header = "Menu_Header_NumericUpDown";
    private const string ConfigurablePlaygroundAnchorId = "numeric-up-down-configurable-playground";
    private const string NumericTypesAndHexAnchorId = "numeric-up-down-numeric-types-and-hex";
    private const string StyleAndValidationAnchorId = "numeric-up-down-style-and-validation";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_NumericUpDown,
        Description = LanguageManager.Instance.Page_Description_NumericUpDown,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_NumericUpDown)],
        Tags = ["NumericUpDown", "Input", "Number"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/NumericUpDownDemo/NumericUpDownDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/NumericUpDownDemo/NumericUpDownDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public DemoSectionViewModel ConfigurablePlaygroundSection { get; }
    public DemoSectionViewModel NumericTypesAndHexSection { get; }
    public DemoSectionViewModel StyleAndValidationSection { get; }
    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Configurable_Playground_Header,
            AnchorId = ConfigurablePlaygroundAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Numeric_Types_And_Hex_Header,
            AnchorId = NumericTypesAndHexAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Style_And_Validation_Header,
            AnchorId = StyleAndValidationAnchorId
        }
    ];

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
        ConfigurablePlaygroundSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Configurable_Playground_Header,
            Descriptions = { LanguageManager.Instance.Page_NumericUpDown_Section_Configurable_Playground_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ConfigurablePlaygroundAnchorId
        };
        ConfigurablePlaygroundSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:NumericUIntUpDown
                              Width="{Binding Width}"
                              AllowDrag="{Binding AllowDrag}"
                              AllowSpin="{Binding AllowSpin}"
                              Command="{Binding TrythisCommand}"
                              FormatString="{Binding FormatString}"
                              Maximum="{Binding Maximum}"
                              Minimum="{Binding Minimum}"
                              ParsingNumberStyle="{Binding ParsingNumberStyle}"
                              Step="{Binding Step}"
                              Value="{Binding Value}" />
                          """
        });
        ConfigurablePlaygroundSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial uint Value { get; set; }
                          [ObservableProperty] public partial uint Maximum { get; set; } = UInt32.MaxValue;
                          [ObservableProperty] public partial uint Minimum { get; set; } = UInt32.MinValue;
                          [ObservableProperty] public partial uint Step { get; set; } = 1;
                          """
        });

        NumericTypesAndHexSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Numeric_Types_And_Hex_Header,
            Descriptions = { LanguageManager.Instance.Page_NumericUpDown_Section_Numeric_Types_And_Hex_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = NumericTypesAndHexAnchorId
        };
        NumericTypesAndHexSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:NumericIntUpDown Value="2" />
                          <u:NumericDoubleUpDown Step="0.5" Value="3.1" />
                          <u:NumericUIntUpDown
                              FontFamily="Consolas"
                              FormatString="X8"
                              ParsingNumberStyle="AllowHexSpecifier"
                              Value="2" />
                          """
        });

        StyleAndValidationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Style_And_Validation_Header,
            Descriptions = { LanguageManager.Instance.Page_NumericUpDown_Section_Style_And_Validation_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = StyleAndValidationAnchorId
        };
        StyleAndValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:NumericIntUpDown Classes="ClearButton" />
                          <u:NumericUIntUpDown Classes="Small" />
                          <u:NumericUIntUpDown Classes="Large" />
                          <u:NumericUIntUpDown Classes="Split" />
                          """
        });

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