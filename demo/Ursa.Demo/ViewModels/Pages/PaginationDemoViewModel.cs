using System.Diagnostics;
using System.Windows.Input;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class PaginationDemoViewModel : ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Pagination",
        Description = "Pagination provides controls to navigate between pages of content.",
        Breadcrumbs = ["Navigation & Menus", "Pagination"],
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