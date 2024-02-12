using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public class NavMenuDemoViewModel: ObservableObject
{
    public ObservableCollection<MenuItem> MenuItems { get; set; } = new ObservableCollection<MenuItem>
    {
        new MenuItem { Header = "Introduction" , Children =
        {
            new MenuItem() { Header = "Getting Started" },
            new MenuItem() { Header = "Design Principles" },
            new MenuItem() { Header = "Contributing" },
        }},
        new MenuItem { Header = "Badge" },
        new MenuItem { Header = "Banner" },
        new MenuItem { Header = "ButtonGroup" },
        new MenuItem { Header = "Class Input" },
        new MenuItem { Header = "Dialog" },
        new MenuItem { Header = "Divider" },
        new MenuItem { Header = "Drawer" },
        new MenuItem { Header = "DualBadge" },
        new MenuItem { Header = "EnumSelector" },
        new MenuItem { Header = "ImageViewer" },
        new MenuItem { Header = "IPv4Box" },
        new MenuItem { Header = "IconButton" },
        new MenuItem { Header = "KeyGestureInput" },
        new MenuItem { Header = "Loading" },
        new MenuItem { Header = "MessageBox" },
        new MenuItem { Header = "Navigation" },
        new MenuItem { Header = "NavMenu" },
        new MenuItem { Header = "NumericUpDown" },
        new MenuItem { Header = "Pagination" },
        new MenuItem { Header = "RangeSlider" },
        new MenuItem { Header = "SelectionList" },
        new MenuItem { Header = "TagInput" },
        new MenuItem { Header = "Timeline" },
        new MenuItem { Header = "TwoTonePathIcon" },
        new MenuItem { Header = "ThemeToggler" }
    };
}

public class MenuItem
{
    public string? Header { get; set; }
    public string? Icon { get; set; }
    public ObservableCollection<MenuItem> Children { get; set; } = new ObservableCollection<MenuItem>();
}