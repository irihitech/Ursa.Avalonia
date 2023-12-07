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
            new() { MenuHeader = "ButtonGroup", Key = MenuKeys.MenuKeyButtonGroup },
            new() { MenuHeader = "Divider", Key = MenuKeys.MenuKeyDivider },
            new() { MenuHeader = "DualBadge", Key = MenuKeys.MenuKeyDualBadge },
            new() { MenuHeader = "ImageViewer", Key = MenuKeys.MenuKeyImageViewer },
            new() { MenuHeader = "IPv4Box", Key = MenuKeys.MenuKeyIpBox },
            new() { MenuHeader = "Loading", Key = MenuKeys.MenuKeyLoading },
            new() { MenuHeader = "Navigation", Key = MenuKeys.MenuKeyNavigation },
            new() { MenuHeader = "Pagination", Key = MenuKeys.MenuKeyPagination },
            new() { MenuHeader = "TagInput", Key = MenuKeys.MenuKeyTagInput },
            new() { MenuHeader = "Timeline", Key = MenuKeys.MenuKeyTimeline },
        };
    }
}