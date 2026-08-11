using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Common;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.IconButtonDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(IconButtonDemo))]
public partial class IconButtonDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "IconButton";
    public const string Menu_Header = "Menu_Header_IconButton";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_IconButton,
        Description = LanguageManager.Instance.Page_Description_IconButton,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_IconButton)],
        Tags = ["IconButton", "Button", "Icon"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/IconButtonDemo/IconButtonDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/IconButtonDemo/IconButtonDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial bool IsLoading { get; set; }
    [ObservableProperty] public partial bool IsLoading2 { get; set; }
    [ObservableProperty] public partial Position SelectedPosition { get; set; }
}