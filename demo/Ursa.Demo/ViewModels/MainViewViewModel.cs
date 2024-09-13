using System;
using CommunityToolkit.Mvvm.Messaging;

namespace Ursa.Demo.ViewModels;

public class MainViewViewModel : ViewModelBase
{
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
            MenuKeys.MenuKeyAutoCompleteBox => new AutoCompleteBoxDemoViewModel(),
            MenuKeys.MenuKeyAvatar => new AvatarDemoViewModel(),
            MenuKeys.MenuKeyBadge => new BadgeDemoViewModel(),
            MenuKeys.MenuKeyBanner => new BannerDemoViewModel(),
            MenuKeys.MenuKeyBreadcrumb => new BreadcrumbDemoViewModel(),
            MenuKeys.MenuKeyButtonGroup => new ButtonGroupDemoViewModel(),
            MenuKeys.MenuKeyClassInput => new ClassInputDemoViewModel(),
            MenuKeys.MenuKeyClock => new ClockDemoViewModel(),
            MenuKeys.MenuKeyDatePicker => new DatePickerDemoViewModel(),
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
            MenuKeys.MenuKeyMessageBox => new MessageBoxDemoViewModel(),
            MenuKeys.MenuKeyMultiComboBox => new MultiComboBoxDemoViewModel(),
            MenuKeys.MenuKeyNavMenu => new NavMenuDemoViewModel(),
            MenuKeys.MenuKeyNotification => new NotificationDemoViewModel(),
            MenuKeys.MenuKeyNumberDisplayer => new NumberDisplayerDemoViewModel(),
            MenuKeys.MenuKeyNumericUpDown => new NumericUpDownDemoViewModel(),
            MenuKeys.MenuKeyNumPad => new NumPadDemoViewModel(),
            MenuKeys.MenuKeyPagination => new PaginationDemoViewModel(),
            MenuKeys.MenuKeyPinCode => new PinCodeDemoViewModel(),
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
            MenuKeys.MenuKeyToast => new ToastDemoViewModel(),
            MenuKeys.MenuKeyToolBar => new ToolBarDemoViewModel(),
            MenuKeys.MenuKeyTreeComboBox => new TreeComboBoxDemoViewModel(),
            MenuKeys.MenuKeyTwoTonePathIcon => new TwoTonePathIconDemoViewModel(),
            _ => throw new ArgumentOutOfRangeException(nameof(s), s, null)
        };
    }
}