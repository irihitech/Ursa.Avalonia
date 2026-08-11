using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Irihi.Dogma.Docs;
using Irihi.Lingua;
using Ursa.Demo.Common;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public class MenuViewModel : ViewModelBase
{
    public MenuViewModel()
    {
        MenuItems = BuildTreeItems(UrsaDocSite.Instance.Roots);
    }
    
    private static ObservableCollection<MenuItemViewModel> BuildTreeItems(IReadOnlyList<DocCategoryNode> node)
    {
        var children = node
            .OrderBy(a => a.Metadata.Order)
            .Select(BuildTreeItem);
        return new ObservableCollection<MenuItemViewModel>(children);
    }


    private static MenuItemViewModel BuildTreeItem(DocCategoryNode node)
    {
        var item = new MenuItemViewModel(node);
        var children = BuildTreeItems(node.Children);
        item.Children = new ObservableCollection<MenuItemViewModel>(children);
        return item;
    }

    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

    public void FilterMenuItems(string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            SetAllVisible(MenuItems);
            return;
        }
        ApplyFilter(MenuItems, searchText);
    }

    private static void SetAllVisible(IEnumerable<MenuItemViewModel> items)
    {
        foreach (var item in items)
        {
            item.IsVisible = true;
            if (item.Children.Count > 0)
                SetAllVisible(item.Children);
        }
    }

    private static bool ApplyFilter(IEnumerable<MenuItemViewModel> items, string searchText)
    {
        var anyVisible = false;
        foreach (var item in items)
        {
            if (item.IsSeparator)
            {
                item.IsVisible = false;
                continue;
            }

            var headerText = (item.MenuHeader as LinguaObservableString)?.CurrentValue;
            var selfMatches = headerText?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;
            var childrenVisible = item.Children.Count > 0 && ApplyFilter(item.Children, searchText);

            item.IsVisible = selfMatches || childrenVisible;
            if (item.IsVisible) anyVisible = true;
        }
        return anyVisible;
    }
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
    public const string MenuKeyMarkdownLine = "MarkdownLine";
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
    public const string MenuKeyShimmer = "Shimmer";
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
