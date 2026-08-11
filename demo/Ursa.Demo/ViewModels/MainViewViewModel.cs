using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Semi.Avalonia;
using Ursa.Demo.Common;
using Ursa.Demo.Localizations;
using Ursa.Demo.ViewModels.Controls;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace Ursa.Demo.ViewModels;

public partial class MainViewViewModel : ViewModelBase
{
    public WindowNotificationManager? NotificationManager { get; set; }
    public MenuViewModel Menus { get; set; } = new MenuViewModel();
    [ObservableProperty] public partial MenuItemViewModel? SelectedMenuItem { get; set; }
    
    [ObservableProperty] public partial IReadOnlyList<BreadcrumbItemData>? NavigationKeys { get; set; }
    [ObservableProperty] public partial PageMetadataViewModel? PageMetadata { get; set; }
    [ObservableProperty] public partial string? SearchText { get; set; }

    [ObservableProperty] public partial object? Content { get; set; }

    partial void OnSearchTextChanged(string? value)
    {
        Menus.FilterMenuItems(value);
    }

    public MainViewViewModel()
    {
        WeakReferenceMessenger.Default.Register<MainViewViewModel, string, string>(this, "JumpTo", OnNavigation);
        //OnNavigation(this, MenuKeys.MenuKeyIntroduction);
        SelectedMenuItem = Menus.MenuItems.FirstOrDefault();
    }

    partial void OnSelectedMenuItemChanged(MenuItemViewModel? value)
    {
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

    private void OnNavigation(MainViewViewModel vm, string s)
    {
        var item = UrsaDocSite.Instance.FindPage(s);
        Content = item.Metadata.ViewModelFactory();
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

    public ObservableCollection<ThemeItem> Themes { get; } =
    [
        new("Default", ThemeVariant.Default),
        new("Light", ThemeVariant.Light),
        new("Dark", ThemeVariant.Dark),
        new("Aquatic", SemiTheme.Aquatic),
        new("Desert", SemiTheme.Desert),
        new("Dusk", SemiTheme.Dusk),
        new("NightSky", SemiTheme.NightSky)
    ];

    [ObservableProperty] public partial ThemeItem? SelectedTheme { get; set; }

    partial void OnSelectedThemeChanged(ThemeItem? oldValue, ThemeItem? newValue)
    {
        if (newValue is null) return;
        var app = Application.Current;
        if (app is not null)
        {
            app.RequestedThemeVariant = newValue.Theme;
            NotificationManager?.Show(
                new Notification("Theme changed", $"Theme changed to {newValue.Name}"),
                type: NotificationType.Success,
                classes: ["Light"]);
        }
    }

    [ObservableProperty] public partial IObservable<string?>? FooterText { get; set; } = LanguageManager.Instance.Menu_Header_Settings;

    [ObservableProperty] public partial bool IsCollapsed { get; set; }

    partial void OnIsCollapsedChanged(bool value)
    {
        FooterText = value ? null : LanguageManager.Instance.Menu_Header_Settings;
    }
}

public class ThemeItem(string name, ThemeVariant theme)
{
    public string Name { get; set; } = name;
    public ThemeVariant Theme { get; set; } = theme;
}
