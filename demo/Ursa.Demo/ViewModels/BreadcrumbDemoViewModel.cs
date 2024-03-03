using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

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
            new BreadcrumbDemoItem() { Section = "Page 4", Icon = "Page" },
        };
    }
}

public class BreadcrumbDemoItem: ObservableObject
{
    public string Section { get; set; }
    public string Icon { get; set; }
    
    public ICommand Command { get; set; }

    public BreadcrumbDemoItem()
    {
        Command = new RelayCommand(() =>
        {
            MessageBox.ShowOverlayAsync(Section);
        });
    }
}