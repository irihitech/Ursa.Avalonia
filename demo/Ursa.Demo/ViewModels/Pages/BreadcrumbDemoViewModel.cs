using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class BreadcrumbDemoViewModel: ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Breadcrumb",
        Description = "Breadcrumb displays a navigation trail showing the current page location.",
        Breadcrumbs = ["Navigation & Menus", "Breadcrumb"],
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
    [ObservableProperty] private bool _isReadOnly;
    
    public ICommand Command { get; set; }

    public BreadcrumbDemoItem()
    {
        Command = new AsyncRelayCommand(async () =>
        {
            await OverlayMessageBox.ShowAsync(Section ?? string.Empty);
        });
    }
}