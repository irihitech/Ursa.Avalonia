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
            MenuKeys.MenuKeyBadge => new BadgeDemoViewModel(),
            MenuKeys.MenuKeyBanner => new BannerDemoViewModel(),
            MenuKeys.MenuKeyButtonGroup => new ButtonGroupDemoViewModel(),
            MenuKeys.MenuKeyBreadcrumb => new BreadcrumbDemoViewModel(),
            MenuKeys.MenuKeyClassInput => new ClassInputDemoViewModel(),
            MenuKeys.MenuKeyDialog => new DialogDemoViewModel(),
            MenuKeys.MenuKeyDivider => new DividerDemoViewModel(),
            MenuKeys.MenuKeyDisableContainer => new DisableContainerDemoViewModel(),
            MenuKeys.MenuKeyDrawer => new DrawerDemoViewModel(),
            MenuKeys.MenuKeyDualBadge => new DualBadgeDemoViewModel(),
            MenuKeys.MenuKeyEnumSelector => new EnumSelectorDemoViewModel(),
            MenuKeys.MenuKeyForm => new FormDemoViewModel(),
            MenuKeys.MenuKeyImageViewer => new ImageViewerDemoViewModel(),
            MenuKeys.MenuKeyIconButton => new IconButtonDemoViewModel(),
            MenuKeys.MenuKeyIpBox => new IPv4BoxDemoViewModel(),
            MenuKeys.MenuKeyKeyGestureInput => new KeyGestureInputDemoViewModel(),
            MenuKeys.MenuKeyLoading => new LoadingDemoViewModel(),
            MenuKeys.MenuKeyMessageBox => new MessageBoxDemoViewModel(),
            MenuKeys.MenuKeyNavMenu => new NavMenuDemoViewModel(),
            MenuKeys.MenuKeyNumberDisplayer => new NumberDisplayerDemoViewModel(),
            MenuKeys.MenuKeyNumericUpDown => new NumericUpDownDemoViewModel(),
            MenuKeys.MenuKeyPagination => new PaginationDemoViewModel(),
            MenuKeys.MenuKeyRangeSlider => new RangeSliderDemoViewModel(),
            MenuKeys.MenuKeyScrollToButton => new ScrollToButtonDemoViewModel(),
            MenuKeys.MenuKeySelectionList => new SelectionListDemoViewModel(),
            MenuKeys.MenuKeySkeleton => new SkeletonDemoViewModel(),
            MenuKeys.MenuKeyTagInput => new TagInputDemoViewModel(),
            MenuKeys.MenuKeyTimeline => new TimelineDemoViewModel(),
            MenuKeys.MenuKeyTwoTonePathIcon => new TwoTonePathIconDemoViewModel(),
            MenuKeys.MenuKeyThemeToggler => new ThemeTogglerDemoViewModel(),
            MenuKeys.MenuKeyToolBar => new ToolBarDemoViewModel(),
        };
    }
}