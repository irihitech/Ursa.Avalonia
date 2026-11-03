using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Ursa.Demo.Common;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.ViewModels;

public partial class MainViewViewModel : ViewModelBase
{
    public MenuViewModel Menus { get; set; } = new MenuViewModel();
    [ObservableProperty] public partial MenuItemViewModel? SelectedMenuItem { get; set; }
    [ObservableProperty] public partial IReadOnlyList<BreadcrumbItemData>? NavigationKeys { get; set; }
    [ObservableProperty] public partial PageMetadataViewModel? PageMetadata { get; set; }
    [ObservableProperty] public partial string? SearchText { get; set; }
    [ObservableProperty] public partial object? Content { get; set; }
    [ObservableProperty] public partial bool IsSettingsSelected { get; set; }

    public MainViewViewModel()
    {
        WeakReferenceMessenger.Default.Register<MainViewViewModel, string, string>(this, "JumpTo", OnNavigation);
        //OnNavigation(this, MenuKeys.MenuKeyIntroduction);
        SelectedMenuItem = Menus.MenuItems.FirstOrDefault();
    }

    partial void OnSearchTextChanged(string? value)
    {
        Menus.FilterMenuItems(value);
    }

    partial void OnSelectedMenuItemChanged(MenuItemViewModel? value)
    {
        IsSettingsSelected = value == Menus.SettingsMenuItem;
        if (value is null) return;
        var content = value.Node.Page?.Metadata.ViewModelFactory();
        this.Content = content;
        if (Content is IPageMetadataProvider provider)
        {
            PageMetadata = provider.PageMetadata;
            NavigationKeys = provider.PageMetadata.Breadcrumbs;
        }
        else
        {
            PageMetadata = null;
            NavigationKeys = null;
        }
    }

    [RelayCommand]
    private void NavigateToSettings()
    {
        if (Menus.SettingsMenuItem is null) return;
        SelectedMenuItem = Menus.SettingsMenuItem;
    }

    private void OnNavigation(MainViewViewModel vm, string s)
    {
        var item = UrsaDocSite.Instance.FindPage(s);
        Content = item?.Metadata.ViewModelFactory();
        if (Content is IPageMetadataProvider provider)
        {
            PageMetadata = provider.PageMetadata;
            NavigationKeys = provider.PageMetadata.Breadcrumbs;
        }
        else
        {
            PageMetadata = null;
            NavigationKeys = null;
        }
    }
}
