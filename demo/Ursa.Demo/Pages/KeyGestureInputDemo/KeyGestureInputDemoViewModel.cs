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
    private const string StatesAnchorId = "key-gesture-input-states";
    private const string ModifierBehaviorAnchorId = "key-gesture-input-modifier-behavior";

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
                                             Gesture="Ctrl+Shift+A" />
                          <u:KeyGestureInput Width="300"
                                             InnerLeftContent="Left"
                                             InnerRightContent="Right" />
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
                          <u:KeyGestureInput Width="300" Gesture="Ctrl+Shift+A" />
                          <u:KeyGestureInput Width="300" Classes="ClearButton" Gesture="Ctrl+K" />
                          <u:KeyGestureInput Width="300" AllowClear="False" Gesture="Ctrl+Shift+A" />
                          """
        });

        StatesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_KeyGestureInput_Section_States_Header,
            SectionTag = DemoSectionTag.Style,
            Descriptions = { LanguageManager.Instance.Page_KeyGestureInput_Section_States_Description },
            AnchorId = StatesAnchorId
        };
        StatesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:KeyGestureInput Width="300" Gesture="Ctrl+Shift+A" IsReadOnly="True" />
                          <u:KeyGestureInput Width="300" Gesture="Ctrl+K" IsEnabled="False" />
                          <u:KeyGestureInput Width="300"
                                             PlaceholderText="Click to set a shortcut"
                                             FocusedPlaceholderText="Press a key combination" />
                          <u:KeyGestureInput Width="300" Gesture="Ctrl+Shift+A" />
                          <u:KeyGestureInput Width="300" Classes="Small" Gesture="Ctrl+Shift+A" />
                          <u:KeyGestureInput Width="300" Classes="Large" Gesture="Ctrl+Shift+A" />
                          """
        });

        ModifierBehaviorSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_KeyGestureInput_Section_Modifier_Behavior_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_KeyGestureInput_Section_Modifier_Behavior_Description },
            AnchorId = ModifierBehaviorAnchorId
        };
        ModifierBehaviorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:KeyGestureInput Width="300" RequireModifier="True" />
                          <u:KeyGestureInput Width="300" ClearOnModifierRelease="False" />
                          <u:KeyGestureInput Width="300" Gesture="Ctrl+Shift+A" />
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
    public DemoSectionViewModel StatesSection { get; }
    public DemoSectionViewModel ModifierBehaviorSection { get; }

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
        new()
        {
            Header = LanguageManager.Instance.Page_KeyGestureInput_Section_States_Header,
            AnchorId = StatesAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_KeyGestureInput_Section_Modifier_Behavior_Header,
            AnchorId = ModifierBehaviorAnchorId
        },
    ];
}
