using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Semi.Avalonia;
using Ursa.Demo.Localizations;
using Ursa.Demo.ViewModels.Controls;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace Ursa.Demo.ViewModels;

public partial class MainViewViewModel : ViewModelBase
{
    public WindowNotificationManager? NotificationManager { get; set; }
    public MenuViewModel Menus { get; set; } = new MenuViewModel();
    
    [ObservableProperty] public partial IReadOnlyList<BreadcrumbItemData>? NavigationKeys { get; set; }
    [ObservableProperty] public partial PageMetadataViewModel? PageMetadata { get; set; }

    [ObservableProperty] public partial object? Content { get; set; }

    public MainViewViewModel()
    {
        WeakReferenceMessenger.Default.Register<MainViewViewModel, string, string>(this, "JumpTo", OnNavigation);
        OnNavigation(this, MenuKeys.MenuKeyIntroduction);
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
            MenuKeys.MenuKeyDateOnlyPicker => new DateOnlyPickerDemoViewModel(),
            MenuKeys.MenuKeyDateRangePicker => new DateRangePickerDemoViewModel(),
            MenuKeys.MenuKeyDateOnlyRangePicker => new DateOnlyRangePickerDemoViewModel(),
            MenuKeys.MenuKeyDateTimePicker => new DateTimePickerDemoViewModel(),
            MenuKeys.MenuKeyDateOffsetPicker => new DateOffsetPickerDemoViewModel(),
            MenuKeys.MenuKeyDateOffsetRangePicker => new DateOffsetRangePickerDemoViewModel(),
            MenuKeys.MenuKeyDateTimeOffsetPicker => new DateTimeOffsetPickerDemoViewModel(),
            MenuKeys.MenuKeyDescriptions => new DescriptionsDemoViewModel(),
            MenuKeys.MenuKeyWindowDialog => new WindowDialogDemoViewModel(),
            MenuKeys.MenuKeyOverlayDialog => new OverlayDialogDemoViewModel(),
            MenuKeys.MenuKeyDisableContainer => new DisableContainerDemoViewModel(),
            MenuKeys.MenuKeyDivider => new DividerDemoViewModel(),
            MenuKeys.MenuKeyDrawer => new DrawerDemoViewModel(),
            MenuKeys.MenuKeyDualBadge => new DualBadgeDemoViewModel(),
            MenuKeys.MenuKeyElasticWrapPanel => new ElasticWrapPanelDemoViewModel(),
            MenuKeys.MenuKeyVirtualizingUniformGrid => new VirtualizingUniformGridDemoViewModel(),
            MenuKeys.MenuKeyEnumSelector => new EnumSelectorDemoViewModel(),
            MenuKeys.MenuKeyForm => new FormDemoViewModel(),
            MenuKeys.MenuKeyGroupBox => new GroupBoxDemoViewModel(),
            MenuKeys.MenuKeyIconButton => new IconButtonDemoViewModel(),
            MenuKeys.MenuKeyImageViewer => new ImageViewerDemoViewModel(),
            MenuKeys.MenuKeyIpBox => new IPv4BoxDemoViewModel(),
            MenuKeys.MenuKeyKeyGestureInput => new KeyGestureInputDemoViewModel(),
            MenuKeys.MenuKeyLoading => new LoadingDemoViewModel(),
            MenuKeys.MenuKeyMarquee => new MarqueeDemoViewModel(),
            MenuKeys.MenuKeyMarkdownLine => new MarkdownLineDemoViewModel(),
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
            MenuKeys.MenuKeyQrCode => new QrCodeDemoViewModel(),
            MenuKeys.MenuKeyRangeSlider => new RangeSliderDemoViewModel(),
            MenuKeys.MenuKeyRating => new RatingDemoViewModel(),
            MenuKeys.MenuKeyScrollToButton => new ScrollToButtonDemoViewModel(),
            MenuKeys.MenuKeySelectionList => new SelectionListDemoViewModel(),
            MenuKeys.MenuKeySkeleton => new SkeletonDemoViewModel(),
            MenuKeys.MenuKeyTagInput => new TagInputDemoViewModel(),
            MenuKeys.MenuKeyThemeToggler => new ThemeTogglerDemoViewModel(),
            MenuKeys.MenuKeyThemeVariantMapper => new ThemeVariantMapperDemoViewModel(),
            MenuKeys.MenuKeyTimeBox => new TimeBoxDemoViewModel(),
            MenuKeys.MenuKeyTimeline => new TimelineDemoViewModel(),
            MenuKeys.MenuKeyTimePicker => new TimePickerDemoViewModel(),
            MenuKeys.MenuKeyTimeOnlyPicker => new TimeOnlyPickerDemoViewModel(),
            MenuKeys.MenuKeyTimeRangePicker => new TimeRangePickerDemoViewModel(),
            MenuKeys.MenuKeyTimeOnlyRangePicker => new TimeOnlyRangePickerDemoViewModel(),
            MenuKeys.MenuKeyToast => new ToastDemoViewModel(),
            MenuKeys.MenuKeyToolBar => new ToolBarDemoViewModel(),
            MenuKeys.MenuKeyTreeComboBox => new TreeComboBoxDemoViewModel(),
            MenuKeys.MenuKeyTwoTonePathIcon => new TwoTonePathIconDemoViewModel(),
            MenuKeys.MenuKeyAspectRatioLayout => new AspectRatioLayoutDemoViewModel(),
            MenuKeys.MenuKeyPathPicker => new PathPickerDemoViewModel(),
            MenuKeys.MenuKeyAnchor => new AnchorDemoViewModel(),
            MenuKeys.MenuKeyMultiAutoCompleteBox => new MultiAutoCompleteBoxDemoViewModel(),
            MenuKeys.MenuKeyProportionalCanvas => new ProportionalCanvasDemoViewModel(),
            _ => throw new ArgumentOutOfRangeException(nameof(s), s, null)
        };
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
