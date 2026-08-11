using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Lingua;
using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.BadgeDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(BadgeDemo))]
public partial class BadgeDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Badge";
    public const string Menu_Header = "Menu_Header_Badge";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Badge,
        Description = LanguageManager.Instance.Page_Description_Badge,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Badge)],
        Tags = ["Badge", "Label", "Status"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/BadgeDemo/BadgeDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/BadgeDemo/BadgeDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    [ObservableProperty] public partial string? Text { get; set; } = null;

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; set; } =
    [
        new() { Header = LinguaObservableString.FromLiteral("Item 1"), AnchorId = "Item1" },
        new() { Header = LinguaObservableString.FromLiteral("Item 2"), AnchorId = "Item2" },
        new() { Header = LinguaObservableString.FromLiteral("Item 3"), AnchorId = "Item3" },
    ];

    [RelayCommand]
    public void ChangeText()
    {
        if (Text == null)
        {
            Text = DateTime.Now.ToShortDateString();
        }
        else
        {
            Text = null;
        }
    }
}
