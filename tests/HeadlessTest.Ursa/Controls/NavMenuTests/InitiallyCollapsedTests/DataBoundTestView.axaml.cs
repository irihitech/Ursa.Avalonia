using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HeadlessTest.Ursa.Controls.NavMenuTests.InitiallyCollapsedTests;

public partial class DataBoundTestView : UserControl
{
    public DataBoundTestView()
    {
        DataContext = new DataBoundTestViewModel();
        InitializeComponent();
    }
}

public partial class DataBoundTestViewModel
{
    public ObservableCollection<NavMenuItemViewModel> MenuItems { get; } = new()
    {
        new NavMenuItemViewModel
        {
            Header = "Item 1",
            Children = new ObservableCollection<NavMenuItemViewModel>
            {
                new() { Header = "Sub Item 1" },
                new() { Header = "Sub Item 2" },
            }
        },
        new NavMenuItemViewModel
        {
            Header = "Item 2",
            Children = new ObservableCollection<NavMenuItemViewModel>
            {
                new() { Header = "Sub Item 3" },
            }
        },
        new NavMenuItemViewModel { Header = "Item 3" },
    };
}

public partial class NavMenuItemViewModel : ObservableObject
{
    [ObservableProperty] private string? _header;
    public ObservableCollection<NavMenuItemViewModel> Children { get; init; } = new();
}
