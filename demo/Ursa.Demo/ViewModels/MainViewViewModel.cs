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
using Ursa.Demo.Pages.AboutUsDemo;
using Ursa.Demo.Pages.AnchorDemo;
using Ursa.Demo.Pages.AspectRatioLayoutDemo;
using Ursa.Demo.Pages.AutoCompleteBoxDemo;
using Ursa.Demo.Pages.AvatarDemo;
using Ursa.Demo.Pages.BadgeDemo;
using Ursa.Demo.Pages.BannerDemo;
using Ursa.Demo.Pages.BreadcrumbDemo;
using Ursa.Demo.Pages.ButtonGroupDemo;
using Ursa.Demo.Pages.ClassInputDemo;
using Ursa.Demo.Pages.ClockDemo;
using Ursa.Demo.Pages.DateOffsetPickerDemo;
using Ursa.Demo.Pages.DateOffsetRangePickerDemo;
using Ursa.Demo.Pages.DateOnlyPickerDemo;
using Ursa.Demo.Pages.DateOnlyRangePickerDemo;
using Ursa.Demo.Pages.DatePickerDemo;
using Ursa.Demo.Pages.DateRangePickerDemo;
using Ursa.Demo.Pages.DateTimeOffsetPickerDemo;
using Ursa.Demo.Pages.DateTimePickerDemo;
using Ursa.Demo.Pages.DescriptionsDemo;
using Ursa.Demo.Pages.DisableContainerDemo;
using Ursa.Demo.Pages.DividerDemo;
using Ursa.Demo.Pages.DrawerDemo;
using Ursa.Demo.Pages.DualBadgeDemo;
using Ursa.Demo.Pages.ElasticWrapPanelDemo;
using Ursa.Demo.Pages.EnumSelectorDemo;
using Ursa.Demo.Pages.FormDemo;
using Ursa.Demo.Pages.GroupBoxDemo;
using Ursa.Demo.Pages.IPv4BoxDemo;
using Ursa.Demo.Pages.IconButtonDemo;
using Ursa.Demo.Pages.ImageViewerDemo;
using Ursa.Demo.Pages.IntroductionDemo;
using Ursa.Demo.Pages.KeyGestureInputDemo;
using Ursa.Demo.Pages.LoadingDemo;
using Ursa.Demo.Pages.MarkdownLineDemo;
using Ursa.Demo.Pages.MarqueeDemo;
using Ursa.Demo.Pages.MessageBoxDemo;
using Ursa.Demo.Pages.MultiAutoCompleteBoxDemo;
using Ursa.Demo.Pages.MultiComboBoxDemo;
using Ursa.Demo.Pages.NavMenuDemo;
using Ursa.Demo.Pages.NotificationDemo;
using Ursa.Demo.Pages.NumPadDemo;
using Ursa.Demo.Pages.NumberDisplayerDemo;
using Ursa.Demo.Pages.NumericUpDownDemo;
using Ursa.Demo.Pages.OverlayDialogDemo;
using Ursa.Demo.Pages.PaginationDemo;
using Ursa.Demo.Pages.PathPickerDemo;
using Ursa.Demo.Pages.PinCodeDemo;
using Ursa.Demo.Pages.PopConfirmDemo;
using Ursa.Demo.Pages.ProportionalCanvasDemo;
using Ursa.Demo.Pages.QrCodeDemo;
using Ursa.Demo.Pages.RangeSliderDemo;
using Ursa.Demo.Pages.RatingDemo;
using Ursa.Demo.Pages.ScrollToButtonDemo;
using Ursa.Demo.Pages.SelectionListDemo;
using Ursa.Demo.Pages.ShimmerDemo;
using Ursa.Demo.Pages.SkeletonDemo;
using Ursa.Demo.Pages.TagInputDemo;
using Ursa.Demo.Pages.ThemeTogglerDemo;
using Ursa.Demo.Pages.ThemeVariantMapperDemo;
using Ursa.Demo.Pages.TimeBoxDemo;
using Ursa.Demo.Pages.TimeOnlyPickerDemo;
using Ursa.Demo.Pages.TimeOnlyRangePickerDemo;
using Ursa.Demo.Pages.TimePickerDemo;
using Ursa.Demo.Pages.TimeRangePickerDemo;
using Ursa.Demo.Pages.TimelineDemo;
using Ursa.Demo.Pages.ToastDemo;
using Ursa.Demo.Pages.ToolBarDemo;
using Ursa.Demo.Pages.TreeComboBoxDemo;
using Ursa.Demo.Pages.TwoTonePathIconDemo;
using Ursa.Demo.Pages.VirtualizingUniformGridDemo;
using Ursa.Demo.Pages.WindowDialogDemo;
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
