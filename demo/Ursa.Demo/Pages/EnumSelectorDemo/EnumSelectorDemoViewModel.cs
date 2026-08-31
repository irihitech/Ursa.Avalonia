using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;
 
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.EnumSelectorDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(EnumSelectorDemo))]
public partial class EnumSelectorDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "EnumSelector";
    public const string Menu_Header = "Menu_Header_EnumSelector";
    private const string BasicUsageAnchorId = "enum-selector-basic-usage";
    private const string SmallSizeAnchorId = "enum-selector-small-size";
    private const string CustomEnumValuesAnchorId = "enum-selector-custom-enum-values";
    private const string DescriptionAttributeAnchorId = "enum-selector-description-attribute";
    private const string CustomDisplayNamesAnchorId = "enum-selector-custom-display-names";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_EnumSelector,
        Description = LanguageManager.Instance.Page_Description_EnumSelector,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_EnumSelector)],
        Tags = ["EnumSelector", "Enum", "Selector"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/EnumSelectorDemo/EnumSelectorDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/EnumSelectorDemo/EnumSelectorDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public EnumSelectorDemoViewModel()
    {
        BasicSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_EnumSelector_Section_Basic_Usage_Header,
            Descriptions = { LanguageManager.Instance.Page_EnumSelector_Section_Basic_Usage_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = BasicUsageAnchorId
        };
        BasicSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:EnumSelector
                              Width="200"
                              EnumType="{x:Type system:DayOfWeek}"
                              Value="{Binding Value}" />
                          """
        });

        StylingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_EnumSelector_Section_Small_Size_Header,
            Descriptions = { LanguageManager.Instance.Page_EnumSelector_Section_Small_Size_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = SmallSizeAnchorId
        };
        StylingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:EnumSelector
                              Width="200"
                              Classes="Small"
                              EnumType="{x:Type system:DayOfWeek}"
                              Value="{Binding Value}" />
                          """
        });

        CustomEnumValuesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_EnumSelector_Section_Custom_EnumValues_Header,
            Descriptions = { LanguageManager.Instance.Page_EnumSelector_Section_Custom_EnumValues_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = CustomEnumValuesAnchorId
        };
        CustomEnumValuesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:EnumSelector
                              Width="200"
                              EnumType="{x:Type system:DayOfWeek}"
                              EnumValues="{Binding CustomEnumValues}"
                              Value="{Binding Value2}" />

                          <u:EnumSelector
                              Width="200"
                              EnumType="{x:Type system:DayOfWeek}"
                              Value="{Binding Value3}">
                              <u:EnumSelector.EnumValues>
                                  <generic:List x:TypeArguments="system:DayOfWeek">
                                      <system:DayOfWeek>Saturday</system:DayOfWeek>
                                      <system:DayOfWeek>Sunday</system:DayOfWeek>
                                      <system:DayOfWeek>Monday</system:DayOfWeek>
                                  </generic:List>
                              </u:EnumSelector.EnumValues>
                          </u:EnumSelector>
                          """
        });
        CustomEnumValuesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public IList CustomEnumValues { get; set; } = new List<object>
                          {
                              DayOfWeek.Monday,
                              DayOfWeek.Wednesday,
                              DayOfWeek.Friday,
                          };
                          """
        });

        DescriptionAttributeSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_EnumSelector_Section_Description_Attribute_Header,
            Descriptions = { LanguageManager.Instance.Page_EnumSelector_Section_Description_Attribute_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DescriptionAttributeAnchorId
        };
        DescriptionAttributeSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:EnumSelector
                              Width="220"
                              EnumType="{x:Type vm:DescriptionSampleEnum}"
                              DisplayDescription="True"
                              Value="{Binding DescriptionAttributeValue}" />
                          """
        });
        DescriptionAttributeSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public enum DescriptionSampleEnum
                          {
                              [Description("Waiting for review")]
                              Pending,
                              [Description("Approved and ready")]
                              Approved,
                              [Description("Rejected")]
                              Rejected
                          }
                          """
        });

        CustomDisplayNamesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_EnumSelector_Section_Custom_DisplayNames_Header,
            Descriptions = { LanguageManager.Instance.Page_EnumSelector_Section_Custom_DisplayNames_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = CustomDisplayNamesAnchorId
        };
        CustomDisplayNamesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:EnumSelector
                              Width="200"
                              EnumType="{x:Type system:DayOfWeek}"
                              DisplayDescription="True"
                              Value="{Binding Value3}">
                              <u:EnumSelector.EnumValues>
                                  <generic:List x:TypeArguments="u:EnumItemTuple">
                                      <u:EnumItemTuple Value="{x:Static system:DayOfWeek.Saturday}" DisplayName="星期六" />
                                      <u:EnumItemTuple Value="{x:Static system:DayOfWeek.Sunday}" DisplayName="星期日" />
                                      <u:EnumItemTuple Value="{x:Static system:DayOfWeek.Monday}" DisplayName="星期一" />
                                  </generic:List>
                              </u:EnumSelector.EnumValues>
                          </u:EnumSelector>
                          """
        });
    }

    [ObservableProperty] public partial object? Value { get; set; }
    [ObservableProperty] public partial object? Value2 { get; set; }
    [ObservableProperty] public partial object? DescriptionAttributeValue { get; set; }
    [ObservableProperty] public partial object? Value3 { get; set; }

    public DemoSectionViewModel BasicSection { get; }
    public DemoSectionViewModel StylingSection { get; }
    public DemoSectionViewModel CustomEnumValuesSection { get; }
    public DemoSectionViewModel DescriptionAttributeSection { get; }
    public DemoSectionViewModel CustomDisplayNamesSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_EnumSelector_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_EnumSelector_Section_Small_Size_Header,
            AnchorId = SmallSizeAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_EnumSelector_Section_Custom_EnumValues_Header,
            AnchorId = CustomEnumValuesAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_EnumSelector_Section_Description_Attribute_Header,
            AnchorId = DescriptionAttributeAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_EnumSelector_Section_Custom_DisplayNames_Header,
            AnchorId = CustomDisplayNamesAnchorId
        }
    ];

    public IList CustomEnumValues { get; set; } = new List<object>
    {
        DayOfWeek.Monday,
        DayOfWeek.Wednesday,
        DayOfWeek.Friday,
    };
}

public enum DescriptionSampleEnum
{
    [Description("Waiting for review")]
    Pending,
    [Description("Approved and ready")]
    Approved,
    [Description("Rejected")]
    Rejected
}
