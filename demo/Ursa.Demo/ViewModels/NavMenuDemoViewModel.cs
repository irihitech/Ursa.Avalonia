using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;
namespace Ursa.Demo.ViewModels;

public class NavMenuDemoViewModel: ObservableObject
{
    public ObservableCollection<MenuItem> MenuItems { get; set; } = new ObservableCollection<MenuItem>
    {
        new MenuItem { Header = "Introduction" , Children =
        {
            new MenuItem() { Header = "Getting Started", Children =
            {
                new MenuItem() { Header = "Code of Conduct" },
                new MenuItem() { Header = "How to Contribute" },
                new MenuItem() { Header = "Development Workflow" },
            }},
            new MenuItem() { Header = "Design Principles" },
            new MenuItem() { Header = "Contributing", Children =
            {
                new MenuItem() { Header = "Code of Conduct" },
                new MenuItem() { Header = "How to Contribute" },
                new MenuItem() { Header = "Development Workflow" },
            }},
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
    public int IconIndex { get; set; }
    public ICommand NavigationCommand { get; set; }
    static Random r = new Random();
    public MenuItem()
    {
        NavigationCommand = new AsyncRelayCommand(OnNavigate);
        IconIndex = r.Next(100);
    }

    private async Task OnNavigate()
    {
        await MessageBox.ShowOverlayAsync(Header??string.Empty, "Navigation Result");
    }

    public ObservableCollection<MenuItem> Children { get; set; } = new ObservableCollection<MenuItem>();
}