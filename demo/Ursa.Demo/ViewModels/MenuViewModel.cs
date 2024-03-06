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
            new() { MenuHeader = "Badge", Key = MenuKeys.MenuKeyBadge, Status = "Updated"},
            new() { MenuHeader = "Banner", Key = MenuKeys.MenuKeyBanner },
            new() { MenuHeader = "Breadcrumb", Key = MenuKeys.MenuKeyBreadcrumb, Status = "New" },
            new() { MenuHeader = "Button Group", Key = MenuKeys.MenuKeyButtonGroup},
            new() { MenuHeader = "Class Input", Key = MenuKeys.MenuKeyClassInput, Status = "New" },
            new() { MenuHeader = "Dialog", Key = MenuKeys.MenuKeyDialog, Status = "Updated"},
            new() { MenuHeader = "Disable Container", Key = MenuKeys.MenuKeyDisableContainer, Status = "New"},
            new() { MenuHeader = "Divider", Key = MenuKeys.MenuKeyDivider },
            new() { MenuHeader = "Drawer", Key = MenuKeys.MenuKeyDrawer, Status = "New"},
            new() { MenuHeader = "DualBadge", Key = MenuKeys.MenuKeyDualBadge },
            new() { MenuHeader = "Enum Selector", Key = MenuKeys.MenuKeyEnumSelector },
            new() { MenuHeader = "Form", Key = MenuKeys.MenuKeyForm, Status = "New" },
            new() { MenuHeader = "Icon Button", Key = MenuKeys.MenuKeyIconButton },
            new() { MenuHeader = "ImageViewer", Key = MenuKeys.MenuKeyImageViewer, Status = "WIP" },
            new() { MenuHeader = "IPv4Box", Key = MenuKeys.MenuKeyIpBox, Status = "Updated" },
            new() { MenuHeader = "KeyGestureInput", Key = MenuKeys.MenuKeyKeyGestureInput },
            new() { MenuHeader = "Loading", Key = MenuKeys.MenuKeyLoading, Status = "Updated" },
            new() { MenuHeader = "Message Box", Key = MenuKeys.MenuKeyMessageBox },
            new() { MenuHeader = "Nav Menu", Key = MenuKeys.MenuKeyNavMenu, Status = "New"},
            // new() { MenuHeader = "Number Displayer", Key = MenuKeys.MenuKeyNumberDisplayer, Status = "New" }, 
            new() { MenuHeader = "Numeric UpDown", Key = MenuKeys.MenuKeyNumericUpDown },
            new() { MenuHeader = "Pagination", Key = MenuKeys.MenuKeyPagination },
            new() { MenuHeader = "RangeSlider", Key = MenuKeys.MenuKeyRangeSlider },
            new() { MenuHeader = "Selection List", Key = MenuKeys.MenuKeySelectionList, Status = "New" },
            new() { MenuHeader = "Skeleton", Key = MenuKeys.MenuKeySkeleton, Status = "New" },
            new() { MenuHeader = "TagInput", Key = MenuKeys.MenuKeyTagInput },
            new() { MenuHeader = "Theme Toggler", Key = MenuKeys.MenuKeyThemeToggler, Status = "New" },
            new() { MenuHeader = "Timeline", Key = MenuKeys.MenuKeyTimeline, Status = "WIP" },
            new() { MenuHeader = "TwoTonePathIcon", Key = MenuKeys.MenuKeyTwoTonePathIcon},
            new() { MenuHeader = "ToolBar", Key = MenuKeys.MenuKeyToolBar, Status = "New" }
        };
    }
}