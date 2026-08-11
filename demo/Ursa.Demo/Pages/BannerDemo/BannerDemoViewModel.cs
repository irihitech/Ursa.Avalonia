using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.BannerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(BannerDemo))]
public partial class BannerDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Banner";
    public const string Menu_Header = "Menu_Header_Banner";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Banner,
        Description = LanguageManager.Instance.Page_Description_Banner,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Banner)],
        Tags = ["Banner", "Alert", "Message"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/BannerDemo/BannerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/BannerDemo/BannerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    private string? _oldTitle = string.Empty;
    private string? _oldContent = string.Empty;
    [ObservableProperty] public partial string? Title { get; set; } = "Welcome to Ursa";
    [ObservableProperty] public partial string? Content { get; set; } = "This is the Demo of Ursa Banner.";
    [ObservableProperty] public partial bool Bordered { get; set; }

    [ObservableProperty] public partial bool SetTitleNull { get; set; } = true;
    [ObservableProperty] public partial bool SetContentNull { get; set; } = true;

    partial void OnSetTitleNullChanged(bool value)
    {
        if (value)
        {
            Title = _oldTitle;
        }
        else
        {
            _oldTitle = Title;
            Title = null;
        }
    }

    partial void OnSetContentNullChanged(bool value)
    {
        if (value)
        {
            Content = _oldContent;
        }
        else
        {
            _oldContent = Content;
            Content = null;
        }
    }
}