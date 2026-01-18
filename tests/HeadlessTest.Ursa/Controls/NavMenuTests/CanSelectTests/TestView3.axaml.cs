using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.NavMenuTests.CanSelectTests;

public partial class TestView3 : UserControl
{
    public int CommandExecutionCount { get; private set; }
    
    public ICommand OnMenuItemClicked { get; }
    
    public TestView3()
    {
        OnMenuItemClicked = new RelayCommand(IncrementCounter);
        InitializeComponent();
    }

    private void IncrementCounter()
    {
        CommandExecutionCount++;
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
