using System.Diagnostics;
using System.Windows.Input;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public class PaginationDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Pagination,
        Description = LanguageManager.Instance.Page_Description_Pagination,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_NavigationAndMenus), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Pagination)],
        Tags = ["Pagination", "Navigation", "Page"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PaginationDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/PaginationDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public AvaloniaList<int> PageSizes { get; set; } = new() { 10, 20, 50, 100 };

    public ICommand LoadPageCommand { get; }
    public PaginationDemoViewModel()
    {
        this.LoadPageCommand = new RelayCommand<int?>(LoadPage);
    }

    private void LoadPage(int? pageIndex)
    {
        Debug.WriteLine($"Loading page {pageIndex}");
    }
}