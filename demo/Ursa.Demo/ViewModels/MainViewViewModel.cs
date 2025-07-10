using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Ursa.Themes.Semi;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace Ursa.Demo.ViewModels;

public partial class MainViewViewModel : ViewModelBase
{
    public WindowNotificationManager? NotificationManager { get; set; }
    public MenuViewModel Menus { get; set; } = new MenuViewModel();

    private object? _content;

    public object? Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    public MainViewViewModel()
    {
        WeakReferenceMessenger.Default.Register<MainViewViewModel, string>(this, OnNavigation);
    }


    private void OnNavigation(MainViewViewModel vm, string s)
    {
        Content = s switch
        {
            MenuKeys.MenuKeyIntroduction => new IntroductionDemoViewModel(),
            MenuKeys.MenuKeyAboutUs => new AboutUsDemoViewModel(),
            MenuKeys.MenuKeyAutoCompleteBox => new AutoCompleteBoxDemoViewModel(),
            MenuKeys.MenuKeyAvatar => new AvatarDemoViewModel(),
            MenuKeys.MenuKeyBadge => new BadgeDemoViewModel(),
            MenuKeys.MenuKeyBanner => new BannerDemoViewModel(),
            MenuKeys.MenuKeyBreadcrumb => new BreadcrumbDemoViewModel(),
            MenuKeys.MenuKeyButtonGroup => new ButtonGroupDemoViewModel(),
            MenuKeys.MenuKeyClassInput => new ClassInputDemoViewModel(),
            MenuKeys.MenuKeyClock => new ClockDemoViewModel(),
            MenuKeys.MenuKeyDatePicker => new DatePickerDemoViewModel(),
            MenuKeys.MenuKeyDateRangePicker => new DateRangePickerDemoViewModel(),
            MenuKeys.MenuKeyDateTimePicker => new DateTimePickerDemoViewModel(),
            MenuKeys.MenuKeyDialog => new DialogDemoViewModel(),
            MenuKeys.MenuKeyDisableContainer => new DisableContainerDemoViewModel(),
            MenuKeys.MenuKeyDivider => new DividerDemoViewModel(),
            MenuKeys.MenuKeyDrawer => new DrawerDemoViewModel(),
            MenuKeys.MenuKeyDualBadge => new DualBadgeDemoViewModel(),
            MenuKeys.MenuKeyElasticWrapPanel => new ElasticWrapPanelDemoViewModel(),
            MenuKeys.MenuKeyEnumSelector => new EnumSelectorDemoViewModel(),
            MenuKeys.MenuKeyForm => new FormDemoViewModel(),
            MenuKeys.MenuKeyIconButton => new IconButtonDemoViewModel(),
            MenuKeys.MenuKeyImageViewer => new ImageViewerDemoViewModel(),
            MenuKeys.MenuKeyIpBox => new IPv4BoxDemoViewModel(),
            MenuKeys.MenuKeyKeyGestureInput => new KeyGestureInputDemoViewModel(),
            MenuKeys.MenuKeyLoading => new LoadingDemoViewModel(),
            MenuKeys.MenuKeyMarquee => new MarqueeDemoViewModel(),
            MenuKeys.MenuKeyMessageBox => new MessageBoxDemoViewModel(),
            MenuKeys.MenuKeyMultiComboBox => new MultiComboBoxDemoViewModel(),
            MenuKeys.MenuKeyNavMenu => new NavMenuDemoViewModel(),
            MenuKeys.MenuKeyNotification => new NotificationDemoViewModel(),
            MenuKeys.MenuKeyNumberDisplayer => new NumberDisplayerDemoViewModel(),
            MenuKeys.MenuKeyNumericUpDown => new NumericUpDownDemoViewModel(),
            MenuKeys.MenuKeyNumPad => new NumPadDemoViewModel(),
            MenuKeys.MenuKeyPagination => new PaginationDemoViewModel(),
            MenuKeys.MenuKeyPinCode => new PinCodeDemoViewModel(),
            MenuKeys.MenuKeyPopConfirm => new PopConfirmDemoViewModel(),
            MenuKeys.MenuKeyRangeSlider => new RangeSliderDemoViewModel(),
            MenuKeys.MenuKeyRating => new RatingDemoViewModel(),
            MenuKeys.MenuKeyScrollToButton => new ScrollToButtonDemoViewModel(),
            MenuKeys.MenuKeySelectionList => new SelectionListDemoViewModel(),
            MenuKeys.MenuKeySkeleton => new SkeletonDemoViewModel(),
            MenuKeys.MenuKeyTagInput => new TagInputDemoViewModel(),
            MenuKeys.MenuKeyThemeToggler => new ThemeTogglerDemoViewModel(),
            MenuKeys.MenuKeyTimeBox => new TimeBoxDemoViewModel(),
            MenuKeys.MenuKeyTimeline => new TimelineDemoViewModel(),
            MenuKeys.MenuKeyTimePicker => new TimePickerDemoViewModel(),
            MenuKeys.MenuKeyTimeRangePicker => new TimeRangePickerDemoViewModel(),
            MenuKeys.MenuKeyToast => new ToastDemoViewModel(),
            MenuKeys.MenuKeyToolBar => new ToolBarDemoViewModel(),
            MenuKeys.MenuKeyTreeComboBox => new TreeComboBoxDemoViewModel(),
            MenuKeys.MenuKeyTwoTonePathIcon => new TwoTonePathIconDemoViewModel(),
            MenuKeys.MenuKeyAspectRatioLayout => new AspectRatioLayoutDemoViewModel(),
            MenuKeys.MenuKeyPathPicker => new PathPickerDemoViewModel(),
            MenuKeys.MenuKeyAnchor => new AnchorDemoViewModel(),
            _ => throw new ArgumentOutOfRangeException(nameof(s), s, null)
        };
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

    [ObservableProperty] private ThemeItem? _selectedTheme;

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

    [ObservableProperty] private string? _footerText = "Settings";

    [ObservableProperty] private bool _isCollapsed;

    partial void OnIsCollapsedChanged(bool value)
    {
        FooterText = value ? null : "Settings";
    }
}

public class ThemeItem(string name, ThemeVariant theme)
{
    public string Name { get; set; } = name;
    public ThemeVariant Theme { get; set; } = theme;
}