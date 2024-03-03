using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public class BreadcrumbDemoViewModel: ObservableObject
{
    public ObservableCollection<BreadcrumbDemoItem> Items1 { get; set; }

    public BreadcrumbDemoViewModel()
    {
        Items1 = new ObservableCollection<BreadcrumbDemoItem>()
        {
            new BreadcrumbDemoItem() { Section = "Home", Icon = "Home" },
            new BreadcrumbDemoItem() { Section = "Page 1", Icon = "Page" },
            new BreadcrumbDemoItem() { Section = "Page 2", Icon = "Page" },
            new BreadcrumbDemoItem() { Section = "Page 3", Icon = "Page" },
        };
    }
}

public class BreadcrumbDemoItem: ObservableObject
{
    public string Section { get; set; }
    public string Icon { get; set; }
}