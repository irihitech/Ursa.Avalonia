using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Demo.Localizations;
using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.MarkdownLineDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(MarkdownLineDemo))]
public partial class MarkdownLineDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "MarkdownLine";
    public const string Menu_Header = "Menu_Header_MarkdownLine";
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

    [ObservableProperty] public partial string? Markdown { get; set; } = "**Try** *editing* `this` ~~text~~!";
}
