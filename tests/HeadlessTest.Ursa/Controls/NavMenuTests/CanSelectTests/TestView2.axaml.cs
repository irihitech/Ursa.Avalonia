using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.NavMenuTests.CanSelectTests;

public partial class TestView2 : UserControl
{
    public TestView2()
    {
        InitializeComponent();
        this.DataContext = new TestView2ViewModel();
    }

    private void NavMenu_OnSelectionChanging(object? sender, SelectionChangingEventArgs e)
    {
        if (e.NewItems is [MenuItemViewModel item])
        {
            if (item.Text.Contains("2"))
            {
                e.CanSelect = false;
            }
        }
    }
}

public partial class TestView2ViewModel
{
    public ObservableCollection<MenuItemViewModel> MenuItems { get; } = new()
    {
        new MenuItemViewModel { Text = "Menu Item 1" },
        new MenuItemViewModel { Text = "Menu Item 2" },
        new MenuItemViewModel { Text = "Menu Item 3" }
    };
}

public partial class MenuItemViewModel: ObservableObject
{
    [ObservableProperty] private string? _text;
}