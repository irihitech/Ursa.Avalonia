using System.Collections.ObjectModel;

namespace Ursa.Demo.ViewModels;

public class MenuViewModel : ViewModelBase
{
    public MenuViewModel()
    {
        MenuItems = new ObservableCollection<MenuItemViewModel>
        {
            new() { MenuHeader = "Introduction", Key = MenuKeys.MenuKeyIntroduction, IsSeparator = false },
            new() { MenuHeader = "About Us", Key = MenuKeys.MenuKeyAboutUs, IsSeparator = false },
            new() { MenuHeader = "Controls", IsSeparator = true },
            new()
            {
                MenuHeader = "Buttons & Inputs", Children = new ObservableCollection<MenuItemViewModel>
                {
                    new() { MenuHeader = "Button Group", Key = MenuKeys.MenuKeyButtonGroup },
                    new() { MenuHeader = "Icon Button", Key = MenuKeys.MenuKeyIconButton, Status = "Redesigned" },
                    new() { MenuHeader = "AutoCompleteBox", Key = MenuKeys.MenuKeyAutoCompleteBox },
                    new() { MenuHeader = "Class Input", Key = MenuKeys.MenuKeyClassInput },
                    new() { MenuHeader = "Enum Selector", Key = MenuKeys.MenuKeyEnumSelector },
                    new() { MenuHeader = "Form", Key = MenuKeys.MenuKeyForm },
                    new() { MenuHeader = "KeyGestureInput", Key = MenuKeys.MenuKeyKeyGestureInput },
                    new() { MenuHeader = "IPv4Box", Key = MenuKeys.MenuKeyIpBox },
                    new() { MenuHeader = "MultiComboBox", Key = MenuKeys.MenuKeyMultiComboBox },
                    new() { MenuHeader = "Numeric UpDown", Key = MenuKeys.MenuKeyNumericUpDown },
                    new() { MenuHeader = "NumPad", Key = MenuKeys.MenuKeyNumPad },
                    new() { MenuHeader = "PathPicker", Key = MenuKeys.MenuKeyPathPicker, Status = "New" },
                    new() { MenuHeader = "PinCode", Key = MenuKeys.MenuKeyPinCode },
                    new() { MenuHeader = "RangeSlider", Key = MenuKeys.MenuKeyRangeSlider },
                    new() { MenuHeader = "Rating", Key = MenuKeys.MenuKeyRating },
                    new() { MenuHeader = "Selection List", Key = MenuKeys.MenuKeySelectionList },
                    new() { MenuHeader = "TagInput", Key = MenuKeys.MenuKeyTagInput },
                    new() { MenuHeader = "Theme Toggler", Key = MenuKeys.MenuKeyThemeToggler },
                    new() { MenuHeader = "TreeComboBox", Key = MenuKeys.MenuKeyTreeComboBox },
                }
            },
            new()
            {
                MenuHeader = "Dialog & Feedbacks", Children = new ObservableCollection<MenuItemViewModel>()
                {
                    new() { MenuHeader = "Dialog", Key = MenuKeys.MenuKeyDialog },
                    new() { MenuHeader = "Drawer", Key = MenuKeys.MenuKeyDrawer, Status = "Updated" },
                    new() { MenuHeader = "Loading", Key = MenuKeys.MenuKeyLoading, Status = "Updated" },
                    new() { MenuHeader = "Message Box", Key = MenuKeys.MenuKeyMessageBox },
                    new() { MenuHeader = "Notification", Key = MenuKeys.MenuKeyNotification },
                    new() { MenuHeader = "PopConfirm", Key = MenuKeys.MenuKeyPopConfirm },
                    new() { MenuHeader = "Toast", Key = MenuKeys.MenuKeyToast },
                    new() { MenuHeader = "Skeleton", Key = MenuKeys.MenuKeySkeleton },
                }
            },
            new()
            {
                MenuHeader = "Date & Time", Children = new ObservableCollection<MenuItemViewModel>
                {
                    new() { MenuHeader = "Date Picker", Key = MenuKeys.MenuKeyDatePicker, Status = "Updated" },
                    new() { MenuHeader = "Date Range Picker", Key = MenuKeys.MenuKeyDateRangePicker, Status = "Updated" },
                    new() { MenuHeader = "Date Time Picker", Key = MenuKeys.MenuKeyDateTimePicker, Status = "Updated" },
                    new() { MenuHeader = "Time Box", Key = MenuKeys.MenuKeyTimeBox },
                    new() { MenuHeader = "Time Picker", Key = MenuKeys.MenuKeyTimePicker, Status = "Updated" },
                    new() { MenuHeader = "Time Range Picker", Key = MenuKeys.MenuKeyTimeRangePicker, Status = "Updated" },
                    new() { MenuHeader = "Clock", Key = MenuKeys.MenuKeyClock }
                }
            },
            new()
            {
                MenuHeader = "Navigation & Menus", Children = new ObservableCollection<MenuItemViewModel>
                {
                    new() { MenuHeader = "Anchor", Key = MenuKeys.MenuKeyAnchor, Status = "New" },
                    new() { MenuHeader = "Breadcrumb", Key = MenuKeys.MenuKeyBreadcrumb, Status = "Updated" },
                    new() { MenuHeader = "Nav Menu", Key = MenuKeys.MenuKeyNavMenu, Status = "Updated" },
                    new() { MenuHeader = "Pagination", Key = MenuKeys.MenuKeyPagination },
                    new() { MenuHeader = "ToolBar", Key = MenuKeys.MenuKeyToolBar },
                }
            },
            new()
            {
                MenuHeader = "Layout & Display",
                Children = new ObservableCollection<MenuItemViewModel>
                {
                    new() { MenuHeader = "AspectRatioLayout", Key = MenuKeys.MenuKeyAspectRatioLayout },
                    new() { MenuHeader = "Avatar", Key = MenuKeys.MenuKeyAvatar, Status = "WIP" },
                    new() { MenuHeader = "Badge", Key = MenuKeys.MenuKeyBadge },
                    new() { MenuHeader = "Banner", Key = MenuKeys.MenuKeyBanner, Status = "Updated" },
                    new() { MenuHeader = "Disable Container", Key = MenuKeys.MenuKeyDisableContainer },
                    new() { MenuHeader = "Divider", Key = MenuKeys.MenuKeyDivider },
                    new() { MenuHeader = "DualBadge", Key = MenuKeys.MenuKeyDualBadge },
                    new() { MenuHeader = "ImageViewer", Key = MenuKeys.MenuKeyImageViewer },
                    new() { MenuHeader = "ElasticWrapPanel", Key = MenuKeys.MenuKeyElasticWrapPanel },
                    new() { MenuHeader = "Marquee", Key = MenuKeys.MenuKeyMarquee, Status = "New" },
                    new() { MenuHeader = "Number Displayer", Key = MenuKeys.MenuKeyNumberDisplayer },
                    new() { MenuHeader = "Scroll To", Key = MenuKeys.MenuKeyScrollToButton },
                    new() { MenuHeader = "Timeline", Key = MenuKeys.MenuKeyTimeline },
                    new() { MenuHeader = "TwoTonePathIcon", Key = MenuKeys.MenuKeyTwoTonePathIcon }
                }
            },
        };
    }

    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }
}

public static class MenuKeys
{
    public const string MenuKeyIntroduction = "Introduction";
    public const string MenuKeyAboutUs = "AboutUs";
    public const string MenuKeyAutoCompleteBox = "AutoCompleteBox";
    public const string MenuKeyAvatar = "Avatar";
    public const string MenuKeyBadge = "Badge";
    public const string MenuKeyBanner = "Banner";
    public const string MenuKeyBreadcrumb = "Breadcrumb";
    public const string MenuKeyButtonGroup = "ButtonGroup";
    public const string MenuKeyClassInput = "Class Input";
    public const string MenuKeyClock = "Clock";
    public const string MenuKeyDatePicker = "DatePicker";
    public const string MenuKeyDateRangePicker = "DateRangePicker";
    public const string MenuKeyDateTimePicker = "DateTimePicker";
    public const string MenuKeyDialog = "Dialog";
    public const string MenuKeyDisableContainer = "DisableContainer";
    public const string MenuKeyDivider = "Divider";
    public const string MenuKeyDrawer = "Drawer";
    public const string MenuKeyDualBadge = "DualBadge";
    public const string MenuKeyElasticWrapPanel = "ElasticWrapPanel";
    public const string MenuKeyEnumSelector = "EnumSelector";
    public const string MenuKeyForm = "Form";
    public const string MenuKeyIconButton = "IconButton";
    public const string MenuKeyImageViewer = "ImageViewer";
    public const string MenuKeyIpBox = "IPv4Box";
    public const string MenuKeyKeyGestureInput = "KeyGestureInput";
    public const string MenuKeyLoading = "Loading";
    public const string MenuKeyMarquee = "Marquee";
    public const string MenuKeyMessageBox = "MessageBox";
    public const string MenuKeyMultiComboBox = "MultiComboBox";
    public const string MenuKeyNavMenu = "NavMenu";
    public const string MenuKeyNotification = "Notification";
    public const string MenuKeyNumberDisplayer = "NumberDisplayer";
    public const string MenuKeyNumericUpDown = "NumericUpDown";
    public const string MenuKeyNumPad = "NumPad";
    public const string MenuKeyPagination = "Pagination";
    public const string MenuKeyPinCode = "PinCode";
    public const string MenuKeyPopConfirm = "PopConfirm";
    public const string MenuKeyRangeSlider = "RangeSlider";
    public const string MenuKeyRating = "Rating";
    public const string MenuKeyScrollToButton = "ScrollToButton";
    public const string MenuKeySelectionList = "SelectionList";
    public const string MenuKeySkeleton = "Skeleton";
    public const string MenuKeyTagInput = "TagInput";
    public const string MenuKeyThemeToggler = "ThemeToggler";
    public const string MenuKeyTimeBox = "TimeBox";
    public const string MenuKeyTimeline = "Timeline";
    public const string MenuKeyTimePicker = "TimePicker";
    public const string MenuKeyTimeRangePicker = "TimeRangePicker";
    public const string MenuKeyToast = "Toast";
    public const string MenuKeyToolBar = "ToolBar";
    public const string MenuKeyTreeComboBox = "TreeComboBox";
    public const string MenuKeyTwoTonePathIcon = "TwoTonePathIcon";
    public const string MenuKeyAspectRatioLayout = "AspectRatioLayout";
    public const string MenuKeyPathPicker = "PathPicker";
    public const string MenuKeyAnchor = "Anchor";
}