using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class AvatarDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Avatar,
        Description = LanguageManager.Instance.Page_Description_Avatar,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Avatar)],
        Tags = ["Avatar", "Profile", "Image"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/AvatarDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/AvatarDemoViewModel.cs",
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