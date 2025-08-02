using System.Windows.Input;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.MultiComboBoxSelectedItemListTests;

public class MultiComboBoxSelectedItemListTests
{
    [AvaloniaFact]
    public void MultiComboBoxSelectedItemList_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var list = new UrsaControls.MultiComboBoxSelectedItemList();

        // Assert
        Assert.Null(list.RemoveCommand);
        Assert.NotNull(list.Items);
    }

    [AvaloniaFact]
    public void MultiComboBoxSelectedItemList_Should_Set_RemoveCommand_Property()
    {
        // Arrange
        var window = new Window();
        var list = new UrsaControls.MultiComboBoxSelectedItemList();
        var command = new TestCommand();
        window.Content = list;
        window.Show();

        // Act
        list.RemoveCommand = command;

        // Assert
        Assert.Equal(command, list.RemoveCommand);
    }

    [AvaloniaFact]
    public void MultiComboBoxSelectedItemList_Should_Add_Items()
    {
        // Arrange
        var window = new Window();
        var list = new UrsaControls.MultiComboBoxSelectedItemList();
        window.Content = list;
        window.Show();

        // Act
        list.Items.Add("Item1");
        list.Items.Add("Item2");
        list.Items.Add("Item3");

        // Assert
        Assert.Equal(3, list.Items.Count);
        Assert.Contains("Item1", list.Items);
        Assert.Contains("Item2", list.Items);
        Assert.Contains("Item3", list.Items);
    }

    [AvaloniaFact]
    public void MultiComboBoxSelectedItemList_Should_Be_Visible_When_Added_To_Window()
    {
        // Arrange
        var window = new Window();
        var list = new UrsaControls.MultiComboBoxSelectedItemList();

        // Act
        window.Content = list;
        window.Show();

        // Assert
        Assert.True(list.IsVisible);
    }

    [AvaloniaFact]
    public void MultiComboBoxSelectedItemList_Should_Inherit_From_ItemsControl()
    {
        // Arrange & Act
        var list = new UrsaControls.MultiComboBoxSelectedItemList();

        // Assert
        Assert.IsAssignableFrom<ItemsControl>(list);
    }

    [AvaloniaFact]
    public void MultiComboBoxSelectedItemList_Should_Handle_Empty_Items()
    {
        // Arrange
        var window = new Window();
        var list = new UrsaControls.MultiComboBoxSelectedItemList();
        window.Content = list;
        window.Show();

        // Act & Assert - Should not throw
        Assert.Empty(list.Items);
    }

    [AvaloniaFact]
    public void MultiComboBoxSelectedItemList_Should_Handle_Null_Command()
    {
        // Arrange
        var window = new Window();
        var list = new UrsaControls.MultiComboBoxSelectedItemList();
        var command = new TestCommand();
        window.Content = list;
        window.Show();

        // Act
        list.RemoveCommand = command;
        list.RemoveCommand = null;

        // Assert
        Assert.Null(list.RemoveCommand);
    }

    [AvaloniaFact]
    public void MultiComboBoxSelectedItemList_Should_Work_With_Mixed_Item_Types()
    {
        // Arrange
        var window = new Window();
        var list = new UrsaControls.MultiComboBoxSelectedItemList();
        window.Content = list;
        window.Show();

        // Act
        list.Items.Add("String");
        list.Items.Add(42);
        list.Items.Add(new Button());

        // Assert
        Assert.Equal(3, list.Items.Count);
        Assert.Contains("String", list.Items);
        Assert.Contains(42, list.Items);
    }

    private class TestCommand : ICommand
    {
        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter) { }
        public event EventHandler? CanExecuteChanged;
    }
}