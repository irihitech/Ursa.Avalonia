using System.Collections.ObjectModel;
using System.Globalization;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public class MenuViewModel : ViewModelBase
{
    public MenuViewModel()
    {
        var lang = LanguageManager.Instance;
        MenuItems = new ObservableCollection<MenuItemViewModel>
        {
            new() { MenuHeader = lang.Menu_Header_Introduction, Key = MenuKeys.MenuKeyIntroduction, IsSeparator = false },
            new() { MenuHeader = lang.Menu_Header_AboutUs, Key = MenuKeys.MenuKeyAboutUs, IsSeparator = false },
            new() { MenuHeader = lang.Menu_Category_Controls, IsSeparator = true },
            new()
            {
                MenuHeader = lang.Menu_Category_ButtonsAndInputs, Children = new ObservableCollection<MenuItemViewModel>
                {
                    new() { MenuHeader = lang.Menu_Header_ButtonGroup, Key = MenuKeys.MenuKeyButtonGroup },
                    new() { MenuHeader = lang.Menu_Header_IconButton, Key = MenuKeys.MenuKeyIconButton },
                    new() { MenuHeader = lang.Menu_Header_AutoCompleteBox, Key = MenuKeys.MenuKeyAutoCompleteBox },
                    new() { MenuHeader = lang.Menu_Header_ClassInput, Key = MenuKeys.MenuKeyClassInput },
                    new() { MenuHeader = lang.Menu_Header_EnumSelector, Key = MenuKeys.MenuKeyEnumSelector },
                    new() { MenuHeader = lang.Menu_Header_Form, Key = MenuKeys.MenuKeyForm },
                    new() { MenuHeader = lang.Menu_Header_KeyGestureInput, Key = MenuKeys.MenuKeyKeyGestureInput },
                    new() { MenuHeader = lang.Menu_Header_IPv4Box, Key = MenuKeys.MenuKeyIpBox },
                    new() { MenuHeader = lang.Menu_Header_MultiComboBox, Key = MenuKeys.MenuKeyMultiComboBox },
                    new() { MenuHeader = lang.Menu_Header_MultiAutoCompleteBox, Key = MenuKeys.MenuKeyMultiAutoCompleteBox },
                    new() { MenuHeader = lang.Menu_Header_NumericUpDown, Key = MenuKeys.MenuKeyNumericUpDown },
                    new() { MenuHeader = lang.Menu_Header_NumPad, Key = MenuKeys.MenuKeyNumPad },
                    new() { MenuHeader = lang.Menu_Header_PathPicker, Key = MenuKeys.MenuKeyPathPicker, Status = "Updated" },
                    new() { MenuHeader = lang.Menu_Header_PinCode, Key = MenuKeys.MenuKeyPinCode },
                    new() { MenuHeader = lang.Menu_Header_RangeSlider, Key = MenuKeys.MenuKeyRangeSlider },
                    new() { MenuHeader = lang.Menu_Header_Rating, Key = MenuKeys.MenuKeyRating },
                    new() { MenuHeader = lang.Menu_Header_SelectionList, Key = MenuKeys.MenuKeySelectionList },
                    new() { MenuHeader = lang.Menu_Header_TagInput, Key = MenuKeys.MenuKeyTagInput, Status = "Updated" },
                    new() { MenuHeader = lang.Menu_Header_ThemeToggler, Key = MenuKeys.MenuKeyThemeToggler },
                    new() { MenuHeader = lang.Menu_Header_TreeComboBox, Key = MenuKeys.MenuKeyTreeComboBox },
                }
            },
            new()
            {
                MenuHeader = lang.Menu_Category_DialogAndFeedbacks, Children = new ObservableCollection<MenuItemViewModel>()
                {
                    new() { MenuHeader = lang.Menu_Header_WindowDialog, Key = MenuKeys.MenuKeyWindowDialog },
                    new() { MenuHeader = lang.Menu_Header_OverlayDialog, Key = MenuKeys.MenuKeyOverlayDialog },
                    new() { MenuHeader = lang.Menu_Header_Drawer, Key = MenuKeys.MenuKeyDrawer, Status = "Updated" },
                    new() { MenuHeader = lang.Menu_Header_Loading, Key = MenuKeys.MenuKeyLoading },
                    new() { MenuHeader = lang.Menu_Header_MessageBox, Key = MenuKeys.MenuKeyMessageBox, Status = "Updated" },
                    new() { MenuHeader = lang.Menu_Header_Notification, Key = MenuKeys.MenuKeyNotification, Status = "Updated" },
                    new() { MenuHeader = lang.Menu_Header_PopConfirm, Key = MenuKeys.MenuKeyPopConfirm },
                    new() { MenuHeader = lang.Menu_Header_Toast, Key = MenuKeys.MenuKeyToast, Status = "Updated" },
                    new() { MenuHeader = lang.Menu_Header_Skeleton, Key = MenuKeys.MenuKeySkeleton },
                }
            },
            new()
            {
                MenuHeader = lang.Menu_Category_DateAndTime, Children = new ObservableCollection<MenuItemViewModel>
                {
                    new() { MenuHeader = lang.Menu_Header_DatePicker, Key = MenuKeys.MenuKeyDatePicker, Status = "Updated" },
                    new() { MenuHeader = lang.Menu_Header_DateOnlyPicker, Key = MenuKeys.MenuKeyDateOnlyPicker, Status = "New" },
                    new() { MenuHeader = lang.Menu_Header_DateRangePicker, Key = MenuKeys.MenuKeyDateRangePicker, Status = "Updated" },
                    new() { MenuHeader = lang.Menu_Header_DateOnlyRangePicker, Key = MenuKeys.MenuKeyDateOnlyRangePicker, Status = "New" },
                    new() { MenuHeader = lang.Menu_Header_DateTimePicker, Key = MenuKeys.MenuKeyDateTimePicker, Status = "Updated" },
                    new() { MenuHeader = lang.Menu_Header_DateOffsetPicker, Key = MenuKeys.MenuKeyDateOffsetPicker, Status = "New" },
                    new() { MenuHeader = lang.Menu_Header_DateOffsetRangePicker, Key = MenuKeys.MenuKeyDateOffsetRangePicker, Status = "New" },
                    new() { MenuHeader = lang.Menu_Header_DateTimeOffsetPicker, Key = MenuKeys.MenuKeyDateTimeOffsetPicker, Status = "New" },
                    new() { MenuHeader = lang.Menu_Header_TimeBox, Key = MenuKeys.MenuKeyTimeBox },
                    new() { MenuHeader = lang.Menu_Header_TimePicker, Key = MenuKeys.MenuKeyTimePicker, Status = "Updated" },
                    new() { MenuHeader = lang.Menu_Header_TimeOnlyPicker, Key = MenuKeys.MenuKeyTimeOnlyPicker, Status = "New" },
                    new() { MenuHeader = lang.Menu_Header_TimeRangePicker, Key = MenuKeys.MenuKeyTimeRangePicker, Status = "New" },
                    new() { MenuHeader = lang.Menu_Header_TimeOnlyRangePicker, Key = MenuKeys.MenuKeyTimeOnlyRangePicker, Status = "New" },
                    new() { MenuHeader = lang.Menu_Header_Clock, Key = MenuKeys.MenuKeyClock }
                }
            },
            new()
            {
                MenuHeader = lang.Menu_Category_NavigationAndMenus, Children = new ObservableCollection<MenuItemViewModel>
                {
                    new() { MenuHeader = lang.Menu_Header_Anchor, Key = MenuKeys.MenuKeyAnchor },
                    new() { MenuHeader = lang.Menu_Header_Breadcrumb, Key = MenuKeys.MenuKeyBreadcrumb },
                    new() { MenuHeader = lang.Menu_Header_NavMenu, Key = MenuKeys.MenuKeyNavMenu, Status = "Updated" },
                    new() { MenuHeader = lang.Menu_Header_Pagination, Key = MenuKeys.MenuKeyPagination },
                    new() { MenuHeader = lang.Menu_Header_ToolBar, Key = MenuKeys.MenuKeyToolBar },
                }
            },
            new()
            {
                MenuHeader = lang.Menu_Category_LayoutAndDisplay,
                Children = new ObservableCollection<MenuItemViewModel>
                {
                    new() { MenuHeader = lang.Menu_Header_AspectRatioLayout, Key = MenuKeys.MenuKeyAspectRatioLayout },
                    new() { MenuHeader = lang.Menu_Header_Avatar, Key = MenuKeys.MenuKeyAvatar, Status = "WIP" },
                    new() { MenuHeader = lang.Menu_Header_Badge, Key = MenuKeys.MenuKeyBadge },
                    new() { MenuHeader = lang.Menu_Header_Banner, Key = MenuKeys.MenuKeyBanner },
                    new() { MenuHeader = lang.Menu_Header_Descriptions, Key = MenuKeys.MenuKeyDescriptions },
                    new() { MenuHeader = lang.Menu_Header_DisableContainer, Key = MenuKeys.MenuKeyDisableContainer },
                    new() { MenuHeader = lang.Menu_Header_Divider, Key = MenuKeys.MenuKeyDivider },
                    new() { MenuHeader = lang.Menu_Header_GroupBox, Key = MenuKeys.MenuKeyGroupBox, Status = "New" },
                    new() { MenuHeader = lang.Menu_Header_DualBadge, Key = MenuKeys.MenuKeyDualBadge },
                    new() { MenuHeader = lang.Menu_Header_ImageViewer, Key = MenuKeys.MenuKeyImageViewer, Status = "WIP" },
                    new() { MenuHeader = lang.Menu_Header_ElasticWrapPanel, Key = MenuKeys.MenuKeyElasticWrapPanel },
                    new() { MenuHeader = lang.Menu_Header_VirtualizingUniformGrid, Key = MenuKeys.MenuKeyVirtualizingUniformGrid, Status = "New" },
                    new() { MenuHeader = lang.Menu_Header_ProportionalCanvas, Key = MenuKeys.MenuKeyProportionalCanvas, Status = "New" },
                    new() { MenuHeader = lang.Menu_Header_Marquee, Key = MenuKeys.MenuKeyMarquee },
                    new() { MenuHeader = lang.Menu_Header_NumberDisplayer, Key = MenuKeys.MenuKeyNumberDisplayer },
                    new() { MenuHeader = lang.Menu_Header_QrCode, Key = MenuKeys.MenuKeyQrCode },
                    new() { MenuHeader = lang.Menu_Header_ScrollTo, Key = MenuKeys.MenuKeyScrollToButton },
                    new() { MenuHeader = lang.Menu_Header_ThemeVariantMapper, Key = MenuKeys.MenuKeyThemeVariantMapper, Status = "New" },
                    new() { MenuHeader = lang.Menu_Header_Timeline, Key = MenuKeys.MenuKeyTimeline },
                    new() { MenuHeader = lang.Menu_Header_TwoTonePathIcon, Key = MenuKeys.MenuKeyTwoTonePathIcon }
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
    public const string MenuKeyDateOnlyPicker = "DateOnlyPicker";
    public const string MenuKeyDateOnlyRangePicker = "DateOnlyRangePicker";
    public const string MenuKeyDatePicker = "DatePicker";
    public const string MenuKeyDateRangePicker = "DateRangePicker";
    public const string MenuKeyDateTimePicker = "DateTimePicker";
    public const string MenuKeyDateOffsetPicker = "DateOffsetPicker";
    public const string MenuKeyDateOffsetRangePicker = "DateOffsetRangePicker";
    public const string MenuKeyDateTimeOffsetPicker = "DateTimeOffsetPicker";
    public const string MenuKeyDescriptions = "Descriptions";
    public const string MenuKeyWindowDialog = "WindowDialog";
    public const string MenuKeyOverlayDialog = "OverlayDialog";
    public const string MenuKeyProportionalCanvas = "Proportional Canvas";
    public const string MenuKeyDisableContainer = "DisableContainer";
    public const string MenuKeyDivider = "Divider";
    public const string MenuKeyDrawer = "Drawer";
    public const string MenuKeyDualBadge = "DualBadge";
    public const string MenuKeyElasticWrapPanel = "ElasticWrapPanel";
    public const string MenuKeyEnumSelector = "EnumSelector";
    public const string MenuKeyForm = "Form";
    public const string MenuKeyGroupBox = "GroupBox";
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
    public const string MenuKeyQrCode = "QrCode";
    public const string MenuKeyRangeSlider = "RangeSlider";
    public const string MenuKeyRating = "Rating";
    public const string MenuKeyScrollToButton = "ScrollToButton";
    public const string MenuKeySelectionList = "SelectionList";
    public const string MenuKeySkeleton = "Skeleton";
    public const string MenuKeyTagInput = "TagInput";
    public const string MenuKeyThemeToggler = "ThemeToggler";
    public const string MenuKeyThemeVariantMapper = "ThemeVariantMapper";
    public const string MenuKeyTimeBox = "TimeBox";
    public const string MenuKeyTimeline = "Timeline";
    public const string MenuKeyTimeOnlyPicker = "TimeOnlyPicker";
    public const string MenuKeyTimeOnlyRangePicker = "TimeOnlyRangePicker";
    public const string MenuKeyTimePicker = "TimePicker";
    public const string MenuKeyTimeRangePicker = "TimeRangePicker";
    public const string MenuKeyToast = "Toast";
    public const string MenuKeyToolBar = "ToolBar";
    public const string MenuKeyTreeComboBox = "TreeComboBox";
    public const string MenuKeyTwoTonePathIcon = "TwoTonePathIcon";
    public const string MenuKeyVirtualizingUniformGrid = "VirtualizingUniformGrid";
    public const string MenuKeyAspectRatioLayout = "AspectRatioLayout";
    public const string MenuKeyPathPicker = "PathPicker";
    public const string MenuKeyAnchor = "Anchor";
    public const string MenuKeyMultiAutoCompleteBox = "MultiAutoCompleteBox";
}
