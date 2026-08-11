using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Demo.Localizations;
using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.ShimmerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(ShimmerDemo))]
public partial class ShimmerDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Shimmer";
    public const string Menu_Header = "Menu_Header_Shimmer";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Shimmer,
        Description = LanguageManager.Instance.Page_Description_Shimmer,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Shimmer)],
        Tags = ["Shimmer", "Loading", "Text"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ShimmerDemo/ShimmerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ShimmerDemo/ShimmerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    [ObservableProperty] public partial bool IsActive { get; set; } = true;

    [RelayCommand]
    private void Pause() => IsActive = false;

    [RelayCommand]
    private void Resume() => IsActive = true;
}
