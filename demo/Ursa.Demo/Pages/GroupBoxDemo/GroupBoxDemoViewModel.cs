using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.GroupBoxDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(GroupBoxDemo))]
public class GroupBoxDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "GroupBox";
    public const string Menu_Header = "Menu_Header_GroupBox";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_GroupBox,
        Description = LanguageManager.Instance.Page_Description_GroupBox,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_GroupBox)],
        Tags = ["GroupBox", "Container", "Layout"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/GroupBoxDemo/GroupBoxDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/GroupBoxDemo/GroupBoxDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

}

