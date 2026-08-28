using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
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
    private const string BasicUsageAnchorId = "numeric-up-down-basic-usage";
    private const string AvailableTypesAnchorId = "numeric-up-down-available-types";
    private const string DragAdjustmentAnchorId = "numeric-up-down-drag-adjustment";
    private const string EmptyInputValueAnchorId = "numeric-up-down-empty-input-value";
    private const string StyleClassesAnchorId = "numeric-up-down-style-classes";
    private const string ValidationAnchorId = "numeric-up-down-validation";
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

    public DemoSectionViewModel BasicUsageSection { get; }
    public DemoSectionViewModel AvailableTypesSection { get; }
    public DemoSectionViewModel DragAdjustmentSection { get; }
    public DemoSectionViewModel EmptyInputValueSection { get; }
    public DemoSectionViewModel StyleClassesSection { get; }
    public DemoSectionViewModel ValidationSection { get; }
    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Available_Types_Header,
            AnchorId = AvailableTypesAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Drag_Adjustment_Header,
            AnchorId = DragAdjustmentAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Empty_Input_Value_Header,
            AnchorId = EmptyInputValueAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Style_Classes_Header,
            AnchorId = StyleClassesAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Validation_Header,
            AnchorId = ValidationAnchorId
        }
    ];

    [ObservableProperty] public partial uint Value { get; set; } = 30;
    
    public NumericUpDownDemoViewModel()
    {
        BasicUsageSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Basic_Usage_Header,
            Descriptions = { LanguageManager.Instance.Page_NumericUpDown_Section_Basic_Usage_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = BasicUsageAnchorId
        };
        BasicUsageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:NumericIntUpDown Value="18" Minimum="0" Maximum="120" />
                          <u:NumericUIntUpDown Value="{Binding Value}" Minimum="0" Maximum="100" />
                          <u:NumericDoubleUpDown Value="3.5" Minimum="0" Maximum="10" />
                          """
        });
        BasicUsageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial uint Value { get; set; } = 30;
                          """
        });

        AvailableTypesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Available_Types_Header,
            Descriptions = { LanguageManager.Instance.Page_NumericUpDown_Section_Available_Types_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = AvailableTypesAnchorId
        };
        AvailableTypesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:NumericIntUpDown Value="1" />
                          <u:NumericUIntUpDown Value="1" />
                          <u:NumericDoubleUpDown Value="1.5" />
                          <u:NumericByteUpDown Value="1" />
                          <u:NumericSByteUpDown Value="1" />
                          <u:NumericShortUpDown Value="1" />
                          <u:NumericUShortUpDown Value="1" />
                          <u:NumericLongUpDown Value="1" />
                          <u:NumericULongUpDown Value="1" />
                          <u:NumericFloatUpDown Value="1.5" />
                          <u:NumericDecimalUpDown Value="1.5" />
                          """
        });

        DragAdjustmentSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Drag_Adjustment_Header,
            Descriptions = { LanguageManager.Instance.Page_NumericUpDown_Section_Drag_Adjustment_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DragAdjustmentAnchorId
        };
        DragAdjustmentSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:NumericDoubleUpDown
                              AllowDrag="True"
                              Value="0"
                              Minimum="-10"
                              Maximum="10"
                              Step="0.5" />
                          """
        });

        EmptyInputValueSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Empty_Input_Value_Header,
            Descriptions = { LanguageManager.Instance.Page_NumericUpDown_Section_Empty_Input_Value_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = EmptyInputValueAnchorId
        };
        EmptyInputValueSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:NumericIntUpDown EmptyInputValue="18" Value="18" />
                          <u:NumericDoubleUpDown EmptyInputValue="3.14" Value="3.14" />
                          """
        });

        StyleClassesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Style_Classes_Header,
            Descriptions = { LanguageManager.Instance.Page_NumericUpDown_Section_Style_Classes_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = StyleClassesAnchorId
        };
        StyleClassesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
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

        ValidationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_NumericUpDown_Section_Validation_Header,
            Descriptions = { LanguageManager.Instance.Page_NumericUpDown_Section_Validation_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ValidationAnchorId
        };
        ValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:NumericUIntUpDown PlaceholderText="Validation Error">
                              <DataValidationErrors.Error>
                                  <system:Exception />
                              </DataValidationErrors.Error>
                          </u:NumericUIntUpDown>
                          """
        });
    }
}
