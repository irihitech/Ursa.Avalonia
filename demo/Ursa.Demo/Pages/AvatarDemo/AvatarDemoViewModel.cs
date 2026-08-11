using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.AvatarDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(AvatarDemo))]
public partial class AvatarDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Avatar";
    public const string Menu_Header = "Menu_Header_Avatar";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Avatar,
        Description = LanguageManager.Instance.Page_Description_Avatar,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Avatar)],
        Tags = ["Avatar", "Profile", "Image"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/AvatarDemo/AvatarDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/AvatarDemo/AvatarDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    [ObservableProperty] public partial string Content { get; set; } = "AS";
    [ObservableProperty] public partial bool CanClick { get; set; } = true;

    [RelayCommand(CanExecute = nameof(CanClick))]
    private void Click()
    {
        Content = "BM";
    }
}