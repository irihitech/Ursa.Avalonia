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
    private const string DistinctAndMultilineAnchorId = "tag-input-distinct-and-multiline";
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
    public DemoSectionViewModel DistinctAndMultilineSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_TagInput_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_TagInput_Section_Distinct_And_Multiline_Header,
            AnchorId = DistinctAndMultilineAnchorId
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
                          private ObservableCollection<string> _tags = new();
                          public ObservableCollection<string> Tags
                          {
                              get => _tags;
                              set => SetProperty(ref _tags, value);
                          }
                          """
        });

        DistinctAndMultilineSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TagInput_Section_Distinct_And_Multiline_Header,
            Descriptions = { LanguageManager.Instance.Page_TagInput_Section_Distinct_And_Multiline_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = DistinctAndMultilineAnchorId
        };
        DistinctAndMultilineSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TagInput
                              AllowDuplicates="False"
                              LostFocusBehavior="Clear"
                              Separator="-"
                              Tags="{Binding DistinctTags}" />

                          <u:TagInput
                              AllowDuplicates="False"
                              AcceptsReturn="True"
                              LostFocusBehavior="Clear"
                              Separator="-"
                              Tags="{Binding DistinctTags}" />
                          """
        });
        DistinctAndMultilineSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          private ObservableCollection<string> _distinctTags = new();
                          public ObservableCollection<string> DistinctTags
                          {
                              get => _distinctTags;
                              set => SetProperty(ref _distinctTags, value);
                          }
                          """
        });
    }

    private ObservableCollection<string> _tags = new () ;
    public ObservableCollection<string> Tags
    {
        get => _tags;
        set => SetProperty(ref _tags, value);
    }

    private ObservableCollection<string> _distinctTags = new();
    public ObservableCollection<string> DistinctTags
    {
        get => _distinctTags;
        set => SetProperty(ref _distinctTags, value);
    }
}