using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.TreeComboBoxItemTests;

public class TreeComboBoxItemTests
{
    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var item = new UrsaControls.TreeComboBoxItem();

        // Assert
        Assert.False(item.IsSelected);
        Assert.False(item.IsExpanded);
        Assert.True(item.IsSelectable);
        Assert.Equal(0, item.Level);
        Assert.Null(item.Header);
        Assert.Null(item.Owner);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Set_IsSelected_Property()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.TreeComboBoxItem();
        window.Content = item;
        window.Show();

        // Act
        item.IsSelected = true;

        // Assert
        Assert.True(item.IsSelected);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Set_IsExpanded_Property()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.TreeComboBoxItem();
        window.Content = item;
        window.Show();

        // Act
        item.IsExpanded = true;

        // Assert
        Assert.True(item.IsExpanded);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Set_IsSelectable_Property()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.TreeComboBoxItem();
        window.Content = item;
        window.Show();

        // Act
        item.IsSelectable = false;

        // Assert
        Assert.False(item.IsSelectable);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Set_Header_Property()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.TreeComboBoxItem();
        var header = "Test Header";
        window.Content = item;
        window.Show();

        // Act
        item.Header = header;

        // Assert
        Assert.Equal(header, item.Header);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Set_Header_As_Control()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.TreeComboBoxItem();
        var header = new Button { Content = "Header Button" };
        window.Content = item;
        window.Show();

        // Act
        item.Header = header;

        // Assert
        Assert.Equal(header, item.Header);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Add_Child_Items()
    {
        // Arrange
        var window = new Window();
        var parentItem = new UrsaControls.TreeComboBoxItem { Header = "Parent" };
        var childItem1 = new UrsaControls.TreeComboBoxItem { Header = "Child 1" };
        var childItem2 = new UrsaControls.TreeComboBoxItem { Header = "Child 2" };
        window.Content = parentItem;
        window.Show();

        // Act
        parentItem.Items.Add(childItem1);
        parentItem.Items.Add(childItem2);

        // Assert
        Assert.Equal(2, parentItem.Items.Count);
        Assert.Equal(2, parentItem.ItemCount);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Be_Visible_When_Added_To_Window()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.TreeComboBoxItem();

        // Act
        window.Content = item;
        window.Show();

        // Assert
        Assert.True(item.IsVisible);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Inherit_From_ItemsControl()
    {
        // Arrange & Act
        var item = new UrsaControls.TreeComboBoxItem();

        // Assert
        Assert.IsAssignableFrom<ItemsControl>(item);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Toggle_Selection()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.TreeComboBoxItem();
        window.Content = item;
        window.Show();

        // Act
        item.IsSelected = true;
        item.IsSelected = false;

        // Assert
        Assert.False(item.IsSelected);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Toggle_Expansion()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.TreeComboBoxItem();
        window.Content = item;
        window.Show();

        // Act
        item.IsExpanded = true;
        item.IsExpanded = false;

        // Assert
        Assert.False(item.IsExpanded);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Handle_Null_Header()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.TreeComboBoxItem();
        window.Content = item;
        window.Show();

        // Act
        item.Header = "Test";
        item.Header = null;

        // Assert
        Assert.Null(item.Header);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Support_DataContext()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.TreeComboBoxItem();
        var dataContext = new { Name = "Test", IsEnabled = true };
        window.Content = item;
        window.Show();

        // Act
        item.DataContext = dataContext;

        // Assert
        Assert.Equal(dataContext, item.DataContext);
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Handle_Selection_When_Not_Selectable()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.TreeComboBoxItem();
        window.Content = item;
        window.Show();

        // Act
        item.IsSelectable = false;
        item.IsSelected = true; // Should still work programmatically

        // Assert
        Assert.False(item.IsSelectable);
        Assert.True(item.IsSelected); // Still can be selected programmatically
    }

    [AvaloniaFact]
    public void TreeComboBoxItem_Should_Handle_Nested_Items()
    {
        // Arrange
        var window = new Window();
        var rootItem = new UrsaControls.TreeComboBoxItem { Header = "Root" };
        var childItem = new UrsaControls.TreeComboBoxItem { Header = "Child" };
        var grandChildItem = new UrsaControls.TreeComboBoxItem { Header = "GrandChild" };
        window.Content = rootItem;
        window.Show();

        // Act
        childItem.Items.Add(grandChildItem);
        rootItem.Items.Add(childItem);

        // Assert
        Assert.Single(rootItem.Items);
        Assert.Single(childItem.Items);
        Assert.Empty(grandChildItem.Items);
    }
}