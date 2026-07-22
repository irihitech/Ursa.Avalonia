using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class BadgeDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Badge,
        Description = LanguageManager.Instance.Page_Description_Badge,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Badge)],
        Tags = ["Badge", "Label", "Status"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/BadgeDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/BadgeDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    [ObservableProperty] public partial string? Text { get; set; } = null;

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
