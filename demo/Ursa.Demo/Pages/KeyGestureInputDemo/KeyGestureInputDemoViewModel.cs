using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Irihi.Dogma.Controls;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.KeyGestureInputDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(KeyGestureInputDemo))]
public class KeyGestureInputDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "KeyGestureInput";
    public const string Menu_Header = "Menu_Header_KeyGestureInput";
    private const string BasicUsageAnchorId = "key-gesture-input-basic-usage";
    private const string AcceptableKeysAnchorId = "key-gesture-input-acceptable-keys";
    private const string ClearButtonStyleAnchorId = "key-gesture-input-clear-button-style-class";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_KeyGestureInput,
        Description = LanguageManager.Instance.Page_Description_KeyGestureInput,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_KeyGestureInput)],
        Tags = ["KeyGestureInput", "Input", "HotKey"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/KeyGestureInputDemo/KeyGestureInputDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/KeyGestureInputDemo/KeyGestureInputDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public KeyGestureInputDemoViewModel()
    {
        BasicUsageSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_KeyGestureInput_Section_Basic_Usage_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_KeyGestureInput_Section_Basic_Usage_Description },
            AnchorId = BasicUsageAnchorId
        };
        BasicUsageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:KeyGestureInput Width="300"
                                             HorizontalAlignment="Center" />
                          """
        });

        AcceptableKeysSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_KeyGestureInput_Section_Acceptable_Keys_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_KeyGestureInput_Section_Acceptable_Keys_Description },
            AnchorId = AcceptableKeysAnchorId
        };
        AcceptableKeysSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:KeyGestureInput Width="300"
                                             HorizontalAlignment="Center"
                                             AcceptableKeys="{Binding AcceptableKeys}" />
                          """
        });
        AcceptableKeysSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public List<Key> AcceptableKeys { get; set; } = new()
                          {
                              Key.A,
                              Key.B,
                              Key.C,
                          };
                          """
        });

        ClearButtonStyleSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_KeyGestureInput_Section_ClearButton_Style_Class_Header,
            SectionTag = DemoSectionTag.Style,
            Descriptions = { LanguageManager.Instance.Page_KeyGestureInput_Section_ClearButton_Style_Class_Description },
            AnchorId = ClearButtonStyleAnchorId
        };
        ClearButtonStyleSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:KeyGestureInput Width="300"
                                             HorizontalAlignment="Center"
                                             Classes="ClearButton"
                                             InnerLeftContent="Left"
                                             InnerRightContent="Right" />
                          """
        });

    }

    public List<Key> AcceptableKeys { get; set; } = new List<Key>()
    {
        Key.A, Key.B, Key.C,
    };

    public DemoSectionViewModel BasicUsageSection { get; }
    public DemoSectionViewModel AcceptableKeysSection { get; }
    public DemoSectionViewModel ClearButtonStyleSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; set; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_KeyGestureInput_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_KeyGestureInput_Section_Acceptable_Keys_Header,
            AnchorId = AcceptableKeysAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_KeyGestureInput_Section_ClearButton_Style_Class_Header,
            AnchorId = ClearButtonStyleAnchorId
        },
    ];
}