using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HeadlessTest.Ursa.Controls.NavMenuTests.CanSelectTests;

public partial class TestView5 : UserControl
{
    public TestView5()
    {
        InitializeComponent();
        DataContext = new TestView5ViewModel();
    }
}

public partial class TestView5ViewModel
{
    public ObservableCollection<TestView5MenuItemViewModel> MenuItems { get; } = new()
    {
        new() { Text = "Menu Item 1" },
    };
}

public partial class TestView5MenuItemViewModel : ObservableObject
{
    [ObservableProperty] public partial string? Text { get; set; }
}
