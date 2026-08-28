using System.Collections.ObjectModel;

using Irihi.Dogma.Controls;
using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.TagInputDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(TagInputDemo))]
public class TagInputDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "TagInput";
    public const string Menu_Header = "Menu_Header_TagInput";
    private const string BasicUsageAnchorId = "tag-input-basic-usage";
    private const string SeparatorBehaviorAnchorId = "tag-input-separator-behavior";
    private const string DistinctTagsAnchorId = "tag-input-distinct-tags";
    private const string MultilineInputAnchorId = "tag-input-multiline-input";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TagInput,
        Description = LanguageManager.Instance.Page_Description_TagInput,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TagInput)],
        Tags = ["TagInput", "Input", "Tag"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TagInputDemo/TagInputDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TagInputDemo/TagInputDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public DemoSectionViewModel BasicUsageSection { get; }
    public DemoSectionViewModel SeparatorBehaviorSection { get; }
    public DemoSectionViewModel DistinctSection { get; }
    public DemoSectionViewModel MultilineSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_TagInput_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_TagInput_Section_Separator_Behavior_Header,
            AnchorId = SeparatorBehaviorAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_TagInput_Section_Distinct_Tags_Header,
            AnchorId = DistinctTagsAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_TagInput_Section_Multiline_Input_Header,
            AnchorId = MultilineInputAnchorId
        }
    ];

    public TagInputDemoViewModel()
    {
        BasicUsageSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TagInput_Section_Basic_Usage_Header,
            Descriptions = { LanguageManager.Instance.Page_TagInput_Section_Basic_Usage_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = BasicUsageAnchorId
        };
        BasicUsageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TagInput
                              AllowDuplicates="True"
                              Separator="-"
                              PlaceholderText="Enter tags separated with -"
                              Tags="{Binding Tags}" />
                          """
        });
        BasicUsageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          // Collection must be initialized
                          public ObservableCollection<string> Tags { get; set; } = [];
                          """
        });

        SeparatorBehaviorSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TagInput_Section_Separator_Behavior_Header,
            Descriptions = { LanguageManager.Instance.Page_TagInput_Section_Separator_Behavior_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = SeparatorBehaviorAnchorId
        };
        SeparatorBehaviorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TagInput Separator="," Tags="{Binding SeparatorTags}" />
                          <u:TagInput Tags="{Binding NoSeparatorTags}" />
                          """
        });
        SeparatorBehaviorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ObservableCollection<string> SeparatorTags { get; set; } = [];
                          public ObservableCollection<string> NoSeparatorTags { get; set; } = [];
                          """
        });

        DistinctSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TagInput_Section_Distinct_Tags_Header,
            Descriptions = { LanguageManager.Instance.Page_TagInput_Section_Distinct_Tags_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DistinctTagsAnchorId
        };
        DistinctSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TagInput
                              AllowDuplicates="False"
                              LostFocusBehavior="Clear"
                              Separator="-"
                              Tags="{Binding DistinctTags}" />
                          """
        });
        DistinctSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ObservableCollection<string> DistinctTags { get; set; } = [];
                          """
        });

        MultilineSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TagInput_Section_Multiline_Input_Header,
            Descriptions = { LanguageManager.Instance.Page_TagInput_Section_Multiline_Input_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = MultilineInputAnchorId
        };
        MultilineSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TagInput
                              AllowDuplicates="False"
                              AcceptsReturn="True"
                              LostFocusBehavior="Clear"
                              Separator="-"
                              Tags="{Binding MultilineTags}" />
                          """
        });
        MultilineSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ObservableCollection<string> MultilineTags { get; set; } = [];
                          """
        });
    }


    public ObservableCollection<string> Tags { get; set; } = [];

    public ObservableCollection<string> SeparatorTags { get; set; } = [];

    public ObservableCollection<string> NoSeparatorTags { get; set; } = [];

    public ObservableCollection<string> DistinctTags { get; set; } = [];

    public ObservableCollection<string> MultilineTags { get; set; } = [];
}
