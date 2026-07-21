using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Demo.Localizations;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.ViewModels;

public partial class MarkdownLineDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new()
    {
        Title = LanguageManager.Instance.Page_Title_MarkdownLine,
        Description = LanguageManager.Instance.Page_Description_MarkdownLine,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_MarkdownLine)],
        Tags = ["Markdown", "Text", "Inline", "Formatting"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/MarkdownLineDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/MarkdownLineDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    [ObservableProperty] private string? _markdown = "**Try** *editing* `this` ~~text~~!";
}
