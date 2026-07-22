using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public class BreadcrumbDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Breadcrumb,
        Description = LanguageManager.Instance.Page_Description_Breadcrumb,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_NavigationAndMenus), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Breadcrumb)],
        Tags = ["Breadcrumb", "Navigation", "Path"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/BreadcrumbDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/BreadcrumbDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public ObservableCollection<BreadcrumbDemoItem> Items1 { get; set; } =
    [
        new BreadcrumbDemoItem { Section = "Home", Icon = "Home" },
        new BreadcrumbDemoItem { Section = "Page 1", Icon = "Page" },
        new BreadcrumbDemoItem { Section = "Page 2", Icon = "Page" },
        new BreadcrumbDemoItem { Section = "Page 3", Icon = "Page" },
        new BreadcrumbDemoItem { Section = "Page 4", Icon = "Page", IsReadOnly = true }
    ];
}

public partial class BreadcrumbDemoItem: ObservableObject
{
    public string? Section { get; set; }
    public string? Icon { get; set; }
    [ObservableProperty] public partial bool IsReadOnly { get; set; }
    
    public ICommand Command { get; set; }

    public BreadcrumbDemoItem()
    {
        Command = new AsyncRelayCommand(async () =>
        {
            await OverlayMessageBox.ShowAsync(Section ?? string.Empty);
        });
    }
}