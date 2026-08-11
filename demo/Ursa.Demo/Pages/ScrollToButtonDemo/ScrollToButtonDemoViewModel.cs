using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.ScrollToButtonDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(ScrollToButtonDemo))]
public class ScrollToButtonDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "ScrollToButton";
    public const string Menu_Header = "Menu_Header_ScrollTo";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ScrollToButton,
        Description = LanguageManager.Instance.Page_Description_ScrollToButton,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ScrollTo)],
        Tags = ["ScrollToButton", "Scroll", "Button"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ScrollToButtonDemo/ScrollToButtonDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ScrollToButtonDemo/ScrollToButtonDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public ObservableCollection<string> Items { get; set; }

    public ScrollToButtonDemoViewModel()
    {
        Items = new ObservableCollection<string>(Enumerable.Range(0, 1000).Select(a => "Item " + a));
    }
}