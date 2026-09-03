using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.MarkdownLineDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(MarkdownLineDemo))]
public partial class MarkdownLineDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "MarkdownLine";
    public const string Menu_Header = "Menu_Header_MarkdownLine";
    private const string BasicFormattingAnchorId = "markdown-line-basic-formatting";
    private const string MixedFormattingAnchorId = "markdown-line-mixed-formatting";
    private const string CodeStylingAnchorId = "markdown-line-code-styling";
    private const string LivePreviewAnchorId = "markdown-line-live-preview";

    public PageMetadataViewModel PageMetadata { get; set; } = new()
    {
        Title = LanguageManager.Instance.Page_Title_MarkdownLine,
        Description = LanguageManager.Instance.Page_Description_MarkdownLine,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_MarkdownLine)],
        Tags = ["Markdown", "Text", "Inline", "Formatting"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/MarkdownLineDemo/MarkdownLineDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/MarkdownLineDemo/MarkdownLineDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public MarkdownLineDemoViewModel()
    {
        BasicFormattingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_MarkdownLine_Section_Basic_Formatting_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_MarkdownLine_Section_Basic_Formatting_Description },
            AnchorId = BasicFormattingAnchorId,
        };
        BasicFormattingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:MarkdownLine Markdown="**Bold text** and *italic text* and ~~strikethrough~~" />
                          <u:MarkdownLine Markdown="__Bold (underscore)__ and _italic (underscore)_" />
                          <u:MarkdownLine Markdown="`inline code` with monospace font" />
                          """
        });

        MixedFormattingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_MarkdownLine_Section_Mixed_Formatting_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_MarkdownLine_Section_Mixed_Formatting_Description },
            AnchorId = MixedFormattingAnchorId,
        };
        MixedFormattingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:MarkdownLine Markdown="**Bold *and italic* together**" />
                          <u:MarkdownLine Markdown="~~Strikethrough **and bold**~~" />
                          <u:MarkdownLine Markdown="`code **not parsed** inside code`" />
                          <u:MarkdownLine Markdown="**Bold with `inline code` inside**" />
                          """
        });

        CodeStylingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_MarkdownLine_Section_Code_Styling_Header,
            SectionTag = DemoSectionTag.Others,
            Descriptions = { LanguageManager.Instance.Page_MarkdownLine_Section_Code_Styling_Description },
            AnchorId = CodeStylingAnchorId,
        };
        CodeStylingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:MarkdownLine MaxWidth="420"
                                          Markdown="This is a **very long** line of text that demonstrates `code` wrapping behavior. ~~Old pricing~~ *New pricing* is now available." />
                          <u:MarkdownLine CodeBackground="{DynamicResource SemiColorPrimaryLight}"
                                          Markdown="`Highlighted code` with a custom background color" />
                          <u:MarkdownLine CodeBackground="Transparent"
                                          CodeFontFamily="Cascadia Code"
                                          Markdown="`Plain code` with a custom font family" />
                          """
        });

        LivePreviewSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_MarkdownLine_Section_Live_Preview_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_MarkdownLine_Section_Live_Preview_Description },
            AnchorId = LivePreviewAnchorId,
        };
        LivePreviewSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBox AcceptsReturn="True"
                                   MinHeight="96"
                                   PlaceholderText="Type your Markdown here..."
                                   Text="{Binding Markdown}" />

                          <u:MarkdownLine Markdown="{Binding Markdown}" />
                          """
        });
        LivePreviewSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial string? Markdown { get; set; } = "**Try** *editing* `this` ~~text~~!";
                          """
        });
    }

    public DemoSectionViewModel BasicFormattingSection { get; }
    public DemoSectionViewModel MixedFormattingSection { get; }
    public DemoSectionViewModel CodeStylingSection { get; }
    public DemoSectionViewModel LivePreviewSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; set; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_MarkdownLine_Section_Basic_Formatting_Header,
            AnchorId = BasicFormattingAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_MarkdownLine_Section_Mixed_Formatting_Header,
            AnchorId = MixedFormattingAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_MarkdownLine_Section_Code_Styling_Header,
            AnchorId = CodeStylingAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_MarkdownLine_Section_Live_Preview_Header,
            AnchorId = LivePreviewAnchorId,
        },
    ];

    [ObservableProperty] public partial string? Markdown { get; set; } = "**Try** *editing* `this` ~~text~~!";
}
