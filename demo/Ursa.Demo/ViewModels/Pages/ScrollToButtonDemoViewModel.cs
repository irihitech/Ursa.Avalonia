using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public class ScrollToButtonDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ScrollToButton,
        Description = LanguageManager.Instance.Page_Description_ScrollToButton,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ScrollTo)],
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