using System.Collections.ObjectModel;

namespace Ursa.Demo.ViewModels;

public class MenuViewModel: ViewModelBase
{
    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

    public MenuViewModel()
    {
        MenuItems = new ObservableCollection<MenuItemViewModel>()
        {
            new() { MenuHeader = "Introduction", Key = MenuKeys.MenuKeyIntroduction, IsSeparator = false },
            new() { MenuHeader = "Controls", IsSeparator = true },
            new() { MenuHeader = "Badge", Key = MenuKeys.MenuKeyBadge },
            new() { MenuHeader = "Banner", Key = MenuKeys.MenuKeyBanner },
            new() { MenuHeader = "Breadcrumb", Key = MenuKeys.MenuKeyBreadcrumb },
            new() { MenuHeader = "Button Group", Key = MenuKeys.MenuKeyButtonGroup },
            new() { MenuHeader = "Class Input", Key = MenuKeys.MenuKeyClassInput },
            new() { MenuHeader = "Clock", Key = MenuKeys.MenuKeyClock, Status = "New" },
            new() { MenuHeader = "Dialog", Key = MenuKeys.MenuKeyDialog },
            new() { MenuHeader = "Disable Container", Key = MenuKeys.MenuKeyDisableContainer },
            new() { MenuHeader = "Divider", Key = MenuKeys.MenuKeyDivider },
            new() { MenuHeader = "Drawer", Key = MenuKeys.MenuKeyDrawer },
            new() { MenuHeader = "DualBadge", Key = MenuKeys.MenuKeyDualBadge },
            new() { MenuHeader = "Enum Selector", Key = MenuKeys.MenuKeyEnumSelector },
            new() { MenuHeader = "Form", Key = MenuKeys.MenuKeyForm },
            new() { MenuHeader = "Icon Button", Key = MenuKeys.MenuKeyIconButton },
            new() { MenuHeader = "ImageViewer", Key = MenuKeys.MenuKeyImageViewer, Status = "WIP" },
            new() { MenuHeader = "IPv4Box", Key = MenuKeys.MenuKeyIpBox },
            new() { MenuHeader = "KeyGestureInput", Key = MenuKeys.MenuKeyKeyGestureInput },
            new() { MenuHeader = "Loading", Key = MenuKeys.MenuKeyLoading },
            new() { MenuHeader = "Message Box", Key = MenuKeys.MenuKeyMessageBox },
            new() { MenuHeader = "MultiComboBox", Key = MenuKeys.MenuKeyMultiComboBox, Status = "New" },
            new() { MenuHeader = "Nav Menu", Key = MenuKeys.MenuKeyNavMenu },
            // new() { MenuHeader = "Number Displayer", Key = MenuKeys.MenuKeyNumberDisplayer, Status = "New" }, 
            new() { MenuHeader = "Numeric UpDown", Key = MenuKeys.MenuKeyNumericUpDown },
            new() { MenuHeader = "NumPad", Key = MenuKeys.MenuKeyNumPad },
            new() { MenuHeader = "Pagination", Key = MenuKeys.MenuKeyPagination },
            new() { MenuHeader = "RangeSlider", Key = MenuKeys.MenuKeyRangeSlider },
            new() { MenuHeader = "Rating", Key = MenuKeys.MenuKeyRating, Status = "New"},
            new() { MenuHeader = "Scroll To", Key = MenuKeys.MenuKeyScrollToButton },
            new() { MenuHeader = "Selection List", Key = MenuKeys.MenuKeySelectionList },
            new() { MenuHeader = "Skeleton", Key = MenuKeys.MenuKeySkeleton },
            new() { MenuHeader = "TagInput", Key = MenuKeys.MenuKeyTagInput },
            new() { MenuHeader = "Theme Toggler", Key = MenuKeys.MenuKeyThemeToggler },
            new() { MenuHeader = "TimePicker", Key = MenuKeys.MenuKeyTimePicker, Status = "New" },
            new() { MenuHeader = "Timeline", Key = MenuKeys.MenuKeyTimeline },
            new() { MenuHeader = "TreeComboBox", Key = MenuKeys.MenuKeyTreeComboBox, Status = "New" },
            new() { MenuHeader = "TwoTonePathIcon", Key = MenuKeys.MenuKeyTwoTonePathIcon},
            new() { MenuHeader = "ToolBar", Key = MenuKeys.MenuKeyToolBar },
            new() { MenuHeader = "Time Box", Key = MenuKeys.MenuKeyTimeBox, Status = "New" },
            new() { MenuHeader = "Verification Code", Key = MenuKeys.MenuKeyVerificationCode},
        };
    }
}

public static class MenuKeys
{
    public const string MenuKeyIntroduction = "Introduction";
    public const string MenuKeyBadge = "Badge";
    public const string MenuKeyBanner = "Banner";
    public const string MenuKeyButtonGroup = "ButtonGroup";
    public const string MenuKeyBreadcrumb= "Breadcrumb";
    public const string MenuKeyClassInput = "Class Input";
    public const string MenuKeyClock = "Clock";
    public const string MenuKeyDialog = "Dialog";
    public const string MenuKeyDivider = "Divider";
    public const string MenuKeyDisableContainer = "DisableContainer";
    public const string MenuKeyDrawer = "Drawer";
    public const string MenuKeyDualBadge = "DualBadge";
    public const string MenuKeyEnumSelector = "EnumSelector";
    public const string MenuKeyForm = "Form";
    public const string MenuKeyImageViewer = "ImageViewer";
    public const string MenuKeyIpBox = "IPv4Box";
    public const string MenuKeyIconButton = "IconButton";
    public const string MenuKeyKeyGestureInput = "KeyGestureInput";
    public const string MenuKeyLoading = "Loading";
    public const string MenuKeyMessageBox = "MessageBox";
    public const string MenuKeyMultiComboBox = "MultiComboBox";
    public const string MenuKeyNavMenu = "NavMenu";
    public const string MenuKeyNumberDisplayer = "NumberDisplayer";
    public const string MenuKeyNumericUpDown = "NumericUpDown";
    public const string MenuKeyNumPad = "NumPad";
    public const string MenuKeyPagination = "Pagination";
    public const string MenuKeyRangeSlider = "RangeSlider";
    public const string MenuKeyRating = "Rating";
    public const string MenuKeyScrollToButton = "ScrollToButton";
    public const string MenuKeySelectionList = "SelectionList";
    public const string MenuKeyTagInput = "TagInput";
    public const string MenuKeySkeleton = "Skeleton"; 
    public const string MenuKeyTimePicker = "TimePicker";
    public const string MenuKeyTimeline = "Timeline";
    public const string MenuKeyTwoTonePathIcon = "TwoTonePathIcon";
    public const string MenuKeyThemeToggler = "ThemeToggler";
    public const string MenuKeyTreeComboBox = "TreeComboBox";
    public const string MenuKeyToolBar = "ToolBar";
    public const string MenuKeyVerificationCode = "VerificationCode";
    public const string MenuKeyTimeBox = "TimeBox";

}