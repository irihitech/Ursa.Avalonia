using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class AnchorDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Anchor,
        Description = LanguageManager.Instance.Page_Description_Anchor,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_NavigationAndMenus), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Anchor)],
        Tags = ["Anchor", "Navigation", "Scroll"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/AnchorDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/AnchorDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public List<AnchorItemViewModel> AnchorItems { get; } = new()
    {
        new AnchorItemViewModel { AnchorId = "anchor1", Header = "Anchor 1" },
        new AnchorItemViewModel { AnchorId = "anchor2", Header = "Anchor 2" },
        new AnchorItemViewModel
        {
            AnchorId = "anchor3", Header = "Anchor 3",
            Children =
            [
                new AnchorItemViewModel() { AnchorId = "anchor3-1", Header = "Anchor 3.1" },
                new AnchorItemViewModel()
                {
                    AnchorId = "anchor3-2", Header = "Anchor 3.2",
                    Children =
                    [
                        new AnchorItemViewModel() { AnchorId = "anchor3-2-1", Header = "Anchor 3.2.1" },
                        new AnchorItemViewModel() { AnchorId = "anchor3-2-2", Header = "Anchor 3.2.2" },
                        new AnchorItemViewModel() { AnchorId = "anchor3-2-3", Header = "Anchor 3.2.3" }
                    ]
                },
                new AnchorItemViewModel() { AnchorId = "anchor3-3", Header = "Anchor 3.3" }
            ]
        },
        new AnchorItemViewModel { AnchorId = "anchor4", Header = "Anchor 4" },
        new AnchorItemViewModel { AnchorId = "anchor5", Header = "Anchor 5" },
        new AnchorItemViewModel { AnchorId = "anchor6", Header = "Anchor 6" },
        new AnchorItemViewModel { AnchorId = "anchor7", Header = "Anchor 7" },
    };
}

public partial class AnchorItemViewModel : ObservableObject
{
    [ObservableProperty] private string? _anchorId;
    [ObservableProperty] private string? _header;
    public ObservableCollection<AnchorItemViewModel>? Children { get; set; }
}