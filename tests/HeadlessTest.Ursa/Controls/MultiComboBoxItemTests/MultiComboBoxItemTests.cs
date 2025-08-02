using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.MultiComboBoxItemTests;

public class MultiComboBoxItemTests
{
    [AvaloniaFact]
    public void MultiComboBoxItem_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var item = new UrsaControls.MultiComboBoxItem();

        // Assert
        Assert.False(item.IsSelected);
        Assert.Null(item.Content);
    }

    [AvaloniaFact]
    public void MultiComboBoxItem_Should_Set_IsSelected_Property()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.MultiComboBoxItem();
        window.Content = item;
        window.Show();

        // Act
        item.IsSelected = true;

        // Assert
        Assert.True(item.IsSelected);
    }

    [AvaloniaFact]
    public void MultiComboBoxItem_Should_Toggle_IsSelected_Property()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.MultiComboBoxItem();
        window.Content = item;
        window.Show();

        // Act
        item.IsSelected = true;
        item.IsSelected = false;

        // Assert
        Assert.False(item.IsSelected);
    }

    [AvaloniaFact]
    public void MultiComboBoxItem_Should_Set_Content_Property()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.MultiComboBoxItem();
        var content = "Test Content";
        window.Content = item;
        window.Show();

        // Act
        item.Content = content;

        // Assert
        Assert.Equal(content, item.Content);
    }

    [AvaloniaFact]
    public void MultiComboBoxItem_Should_Set_Content_As_Control()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.MultiComboBoxItem();
        var content = new Button { Content = "Button Content" };
        window.Content = item;
        window.Show();

        // Act
        item.Content = content;

        // Assert
        Assert.Equal(content, item.Content);
    }

    [AvaloniaFact]
    public void MultiComboBoxItem_Should_Be_Visible_When_Added_To_Window()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.MultiComboBoxItem();

        // Act
        window.Content = item;
        window.Show();

        // Assert
        Assert.True(item.IsVisible);
    }

    [AvaloniaFact]
    public void MultiComboBoxItem_Should_Inherit_From_ContentControl()
    {
        // Arrange & Act
        var item = new UrsaControls.MultiComboBoxItem();

        // Assert
        Assert.IsAssignableFrom<ContentControl>(item);
    }

    [AvaloniaFact]
    public void MultiComboBoxItem_Should_Be_Focusable()
    {
        // Arrange & Act
        var item = new UrsaControls.MultiComboBoxItem();

        // Assert
        Assert.True(item.Focusable);
    }

    [AvaloniaFact]
    public void MultiComboBoxItem_Should_Handle_Null_Content()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.MultiComboBoxItem();
        window.Content = item;
        window.Show();

        // Act
        item.Content = "Test";
        item.Content = null;

        // Assert
        Assert.Null(item.Content);
    }

    [AvaloniaFact]
    public void MultiComboBoxItem_Should_Handle_DataContext()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.MultiComboBoxItem();
        var dataContext = new { Name = "Test", Value = 42 };
        window.Content = item;
        window.Show();

        // Act
        item.DataContext = dataContext;

        // Assert
        Assert.Equal(dataContext, item.DataContext);
    }

    [AvaloniaFact]
    public void MultiComboBoxItem_Should_Support_Multiple_Selection_States()
    {
        // Arrange
        var window = new Window();
        var item1 = new UrsaControls.MultiComboBoxItem { Content = "Item 1" };
        var item2 = new UrsaControls.MultiComboBoxItem { Content = "Item 2" };
        var stackPanel = new StackPanel();
        stackPanel.Children.Add(item1);
        stackPanel.Children.Add(item2);
        window.Content = stackPanel;
        window.Show();

        // Act
        item1.IsSelected = true;
        item2.IsSelected = true;

        // Assert
        Assert.True(item1.IsSelected);
        Assert.True(item2.IsSelected);
    }

    [AvaloniaFact]
    public void MultiComboBoxItem_Should_Change_Selection_Independently()
    {
        // Arrange
        var window = new Window();
        var item1 = new UrsaControls.MultiComboBoxItem { Content = "Item 1" };
        var item2 = new UrsaControls.MultiComboBoxItem { Content = "Item 2" };
        var stackPanel = new StackPanel();
        stackPanel.Children.Add(item1);
        stackPanel.Children.Add(item2);
        window.Content = stackPanel;
        window.Show();

        // Act
        item1.IsSelected = true;
        item2.IsSelected = true;
        item1.IsSelected = false;

        // Assert
        Assert.False(item1.IsSelected);
        Assert.True(item2.IsSelected);
    }
}