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
            new() { MenuHeader = "Dialog", Key = MenuKeys.MenuKeyDialog },
            new() { MenuHeader = "ButtonGroup", Key = MenuKeys.MenuKeyButtonGroup, Status = "Updated"},
            new() { MenuHeader = "Divider", Key = MenuKeys.MenuKeyDivider },
            new() { MenuHeader = "DualBadge", Key = MenuKeys.MenuKeyDualBadge },
            new() { MenuHeader = "Enum Selector", Key = MenuKeys.MenuKeyEnumSelector },
            new() { MenuHeader = "IconButton", Key = MenuKeys.MenuKeyIconButton },
            new() { MenuHeader = "ImageViewer", Key = MenuKeys.MenuKeyImageViewer },
            new() { MenuHeader = "IPv4Box", Key = MenuKeys.MenuKeyIpBox },
            new() { MenuHeader = "KeyGestureInput", Key = MenuKeys.MenuKeyKeyGestureInput },
            new() { MenuHeader = "Loading", Key = MenuKeys.MenuKeyLoading },
            new() { MenuHeader = "Message Box", Key = MenuKeys.MenuKeyMessageBox, Status = "New" },
            new() { MenuHeader = "Navigation", Key = MenuKeys.MenuKeyNavigation, Status = "WIP" },
            new() { MenuHeader = "NumericUpDown", Key = MenuKeys.MenuKeyNumericUpDown, Status = "New" },
            new() { MenuHeader = "Pagination", Key = MenuKeys.MenuKeyPagination },
            new() { MenuHeader = "RangeSlider", Key = MenuKeys.MenuKeyRangeSlider, Status = "New"},
            new() { MenuHeader = "TagInput", Key = MenuKeys.MenuKeyTagInput },
            new() { MenuHeader = "Timeline", Key = MenuKeys.MenuKeyTimeline, Status = "Updated" },
            new() { MenuHeader = "TwoTonePathIcon", Key = MenuKeys.MenuKeyTwoTonePathIcon, Status = "New"},
        };
    }
}