using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.SelectionListDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(SelectionListDemo))]
public partial class SelectionListDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "SelectionList";
    public const string Menu_Header = "Menu_Header_SelectionList";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_SelectionList,
        Description = LanguageManager.Instance.Page_Description_SelectionList,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_SelectionList)],
        Tags = ["SelectionList", "List", "Selection"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/SelectionListDemo/SelectionListDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/SelectionListDemo/SelectionListDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public ObservableCollection<string> Items { get; set; }
    [ObservableProperty] public partial string? SelectedItem { get; set; }

    public SelectionListDemoViewModel()
    {
        Items = new ObservableCollection<string>()
        {
            "Ding", "Otter", "Husky", "Mr. 17", "Cass"
        };
        SelectedItem = Items[0];
    }

    public void Clear()
    {
        SelectedItem = null;
    }
}