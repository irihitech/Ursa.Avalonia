using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Avalonia.Threading;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.TreeComboBoxTests;

public class TreeComboBoxTests
{
    [AvaloniaFact]
    public void TreeComboBox_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var comboBox = new UrsaControls.TreeComboBox();

        // Assert
        Assert.True(comboBox.MaxDropDownHeight >= 0); // Has a default value
        Assert.Null(comboBox.Watermark);
        Assert.False(comboBox.IsDropDownOpen);
        Assert.Equal(HorizontalAlignment.Stretch, comboBox.HorizontalContentAlignment);
        Assert.Equal(VerticalAlignment.Stretch, comboBox.VerticalContentAlignment);
        Assert.Null(comboBox.SelectedItemTemplate);
        Assert.Null(comboBox.SelectionBoxItem);
        Assert.Null(comboBox.SelectedItem);
        Assert.Null(comboBox.InnerLeftContent);
        Assert.Null(comboBox.InnerRightContent);
        Assert.Null(comboBox.PopupInnerTopContent);
        Assert.Null(comboBox.PopupInnerBottomContent);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Set_MaxDropDownHeight_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.MaxDropDownHeight = 200.0;

        // Assert
        Assert.Equal(200.0, comboBox.MaxDropDownHeight);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Set_Watermark_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var watermark = "Select an item...";
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.Watermark = watermark;

        // Assert
        Assert.Equal(watermark, comboBox.Watermark);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Set_IsDropDownOpen_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.IsDropDownOpen = true;

        // Assert
        Assert.True(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Set_HorizontalContentAlignment_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.HorizontalContentAlignment = HorizontalAlignment.Center;

        // Assert
        Assert.Equal(HorizontalAlignment.Center, comboBox.HorizontalContentAlignment);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Set_VerticalContentAlignment_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.VerticalContentAlignment = VerticalAlignment.Center;

        // Assert
        Assert.Equal(VerticalAlignment.Center, comboBox.VerticalContentAlignment);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Set_SelectedItem_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var selectedItem = "Test Item";
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.SelectedItem = selectedItem;

        // Assert
        Assert.Equal(selectedItem, comboBox.SelectedItem);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Set_InnerLeftContent_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var content = "Left Content";
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.InnerLeftContent = content;

        // Assert
        Assert.Equal(content, comboBox.InnerLeftContent);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Set_InnerRightContent_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var content = "Right Content";
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.InnerRightContent = content;

        // Assert
        Assert.Equal(content, comboBox.InnerRightContent);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Set_PopupInnerTopContent_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var content = "Top Content";
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.PopupInnerTopContent = content;

        // Assert
        Assert.Equal(content, comboBox.PopupInnerTopContent);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Set_PopupInnerBottomContent_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var content = "Bottom Content";
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.PopupInnerBottomContent = content;

        // Assert
        Assert.Equal(content, comboBox.PopupInnerBottomContent);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Add_Items()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var item1 = new UrsaControls.TreeComboBoxItem { Header = "Item 1" };
        var item2 = new UrsaControls.TreeComboBoxItem { Header = "Item 2" };
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.Items.Add(item1);
        comboBox.Items.Add(item2);

        // Assert
        Assert.Equal(2, comboBox.Items.Count);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Be_Visible_When_Added_To_Window()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();

        // Act
        window.Content = comboBox;
        window.Show();

        // Assert
        Assert.True(comboBox.IsVisible);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Inherit_From_ItemsControl()
    {
        // Arrange & Act
        var comboBox = new UrsaControls.TreeComboBox();

        // Assert
        Assert.IsAssignableFrom<ItemsControl>(comboBox);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Handle_Null_SelectedItem()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.SelectedItem = "Test";
        comboBox.SelectedItem = null;

        // Assert
        Assert.Null(comboBox.SelectedItem);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Toggle_DropDown_State()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.IsDropDownOpen = true;
        comboBox.IsDropDownOpen = false;

        // Assert
        Assert.False(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Handle_Complex_Content()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var innerContent = new Button { Content = "Complex Content" };
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.InnerLeftContent = innerContent;

        // Assert
        Assert.Equal(innerContent, comboBox.InnerLeftContent);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Support_Hierarchical_Items()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var parentItem = new UrsaControls.TreeComboBoxItem { Header = "Parent" };
        var childItem = new UrsaControls.TreeComboBoxItem { Header = "Child" };
        parentItem.Items.Add(childItem);
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.Items.Add(parentItem);

        // Assert
        Assert.Single(comboBox.Items);
        Assert.Single(parentItem.Items);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Handle_SelectionBoxItem_ReadOnly()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        window.Content = comboBox;
        window.Show();

        // Act - SelectionBoxItem is read-only, just verify it can be accessed
        var selectionBoxItem = comboBox.SelectionBoxItem;

        // Assert
        Assert.Null(selectionBoxItem); // Initially null
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Support_Hierarchical_ItemsSource()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var hierarchicalData = new List<HierarchicalItem>
        {
            new HierarchicalItem 
            { 
                Name = "Root1", 
                Children = new List<HierarchicalItem>
                {
                    new HierarchicalItem { Name = "Child1.1" },
                    new HierarchicalItem { Name = "Child1.2" }
                }
            },
            new HierarchicalItem 
            { 
                Name = "Root2", 
                Children = new List<HierarchicalItem>
                {
                    new HierarchicalItem { Name = "Child2.1" },
                    new HierarchicalItem 
                    { 
                        Name = "Child2.2",
                        Children = new List<HierarchicalItem>
                        {
                            new HierarchicalItem { Name = "GrandChild2.2.1" }
                        }
                    }
                }
            }
        };
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.ItemsSource = hierarchicalData;

        // Assert
        Assert.Equal(hierarchicalData, comboBox.ItemsSource);
        Assert.Equal(2, comboBox.Items.Count);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Update_With_Observable_Hierarchical_ItemsSource()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var hierarchicalData = new ObservableCollection<HierarchicalItem>
        {
            new HierarchicalItem 
            { 
                Name = "Root1", 
                Children = new List<HierarchicalItem>
                {
                    new HierarchicalItem { Name = "Child1.1" }
                }
            }
        };
        window.Content = comboBox;
        window.Show();
        comboBox.ItemsSource = hierarchicalData;
        comboBox.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs();

        // Act
        hierarchicalData.Add(new HierarchicalItem { Name = "Root2" });

        // Assert
        Assert.Equal(2, comboBox.Items.Count);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Clear_SelectedItem_Via_Clear_Method()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        window.Content = comboBox;
        window.Show();

        comboBox.SelectedItem = "Test Item";

        // Act
        comboBox.Clear();

        // Assert
        Assert.Null(comboBox.SelectedItem);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Clear_When_SelectedItem_Is_Already_Null()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        window.Content = comboBox;
        window.Show();

        // Act & Assert - Should not throw
        comboBox.Clear();
        Assert.Null(comboBox.SelectedItem);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Handle_Complex_Hierarchical_Objects()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var complexHierarchy = new List<ComplexHierarchicalItem>
        {
            new ComplexHierarchicalItem 
            { 
                Id = 1,
                Title = "Department A", 
                Type = "Department",
                SubItems = new List<ComplexHierarchicalItem>
                {
                    new ComplexHierarchicalItem { Id = 11, Title = "Team 1", Type = "Team" },
                    new ComplexHierarchicalItem { Id = 12, Title = "Team 2", Type = "Team" }
                }
            }
        };
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.ItemsSource = complexHierarchy;

        // Assert
        Assert.Equal(complexHierarchy, comboBox.ItemsSource);
        Assert.Single(comboBox.Items);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Implement_IClearControl()
    {
        // Arrange & Act
        var comboBox = new UrsaControls.TreeComboBox();

        // Assert
        Assert.IsAssignableFrom<Irihi.Avalonia.Shared.Contracts.IClearControl>(comboBox);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Handle_Mixed_ItemsSource_Types()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var mixedData = new List<object>
        {
            "Simple String",
            new HierarchicalItem 
            { 
                Name = "Complex Item",
                Children = new List<HierarchicalItem>
                {
                    new HierarchicalItem { Name = "Child" }
                }
            },
            42
        };
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.ItemsSource = mixedData;

        // Assert
        Assert.Equal(mixedData, comboBox.ItemsSource);
        Assert.Equal(3, comboBox.Items.Count);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Clear_And_Select_New_Item()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        window.Content = comboBox;
        window.Show();

        comboBox.SelectedItem = "First Item";

        // Act
        comboBox.Clear();
        comboBox.SelectedItem = "Second Item";

        // Assert
        Assert.Equal("Second Item", comboBox.SelectedItem);
    }

    [AvaloniaFact]
    public void TreeComboBox_Should_Handle_Empty_Hierarchical_ItemsSource()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.TreeComboBox();
        var emptyHierarchicalData = new List<HierarchicalItem>();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.ItemsSource = emptyHierarchicalData;

        // Assert
        Assert.Equal(emptyHierarchicalData, comboBox.ItemsSource);
        Assert.Empty(comboBox.Items);
    }

    // Helper classes for hierarchical testing
    private class HierarchicalItem
    {
        public string Name { get; set; } = "";
        public List<HierarchicalItem>? Children { get; set; }
    }

    private class ComplexHierarchicalItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Type { get; set; } = "";
        public List<ComplexHierarchicalItem>? SubItems { get; set; }
    }
}