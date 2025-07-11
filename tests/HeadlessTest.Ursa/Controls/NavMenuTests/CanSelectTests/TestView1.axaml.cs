using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.NavMenuTests.CanSelectTests;

public partial class TestView1 : UserControl
{
    public TestView1()
    {
        InitializeComponent();
    }

    private void Menu_OnSelectionChanging(object? sender, SelectionChangingEventArgs e)
    {
        var newItem = e.NewItems;
        if (newItem is [NavMenuItem { Name: "MenuItem2" }])
        {
            e.CanSelect = false; // Prevent selection change for MenuItem2
        }
    }
}