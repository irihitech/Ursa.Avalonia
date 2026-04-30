using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class ScrollToButtonDemoViewModel: ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "ScrollToButton",
        Description = "ScrollToButton is a floating button that scrolls the viewport to a target element.",
        Breadcrumbs = ["Layout & Display", "Scroll To"],
        Tags = ["ScrollToButton", "Scroll", "Button"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ScrollToButtonDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/ScrollToButtonDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public ObservableCollection<string> Items { get; set; }

    public ScrollToButtonDemoViewModel()
    {
        Items = new ObservableCollection<string>(Enumerable.Range(0, 1000).Select(a => "Item " + a));
    }
}