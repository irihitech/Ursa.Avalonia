using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.MultiComboBoxTests;

public class MultiComboBoxTests
{
    [AvaloniaFact]
    public void MultiComboBox_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var comboBox = new UrsaControls.MultiComboBox();

        // Assert
        Assert.False(comboBox.IsDropDownOpen);
        Assert.Equal(0.0, comboBox.MaxDropDownHeight);
        Assert.Equal(0.0, comboBox.MaxSelectionBoxHeight);
        Assert.NotNull(comboBox.SelectedItems);
        Assert.Empty(comboBox.SelectedItems);
        Assert.Null(comboBox.SelectedItemTemplate);
        Assert.Null(comboBox.Watermark);
        Assert.Null(comboBox.InnerLeftContent);
        Assert.Null(comboBox.InnerRightContent);
        Assert.Null(comboBox.PopupInnerTopContent);
        Assert.Null(comboBox.PopupInnerBottomContent);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Set_IsDropDownOpen_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.IsDropDownOpen = true;

        // Assert
        Assert.True(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Set_MaxDropDownHeight_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.MaxDropDownHeight = 200.0;

        // Assert
        Assert.Equal(200.0, comboBox.MaxDropDownHeight);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Set_MaxSelectionBoxHeight_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.MaxSelectionBoxHeight = 150.0;

        // Assert
        Assert.Equal(150.0, comboBox.MaxSelectionBoxHeight);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Set_Watermark_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        var watermark = "Select items...";
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.Watermark = watermark;

        // Assert
        Assert.Equal(watermark, comboBox.Watermark);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Set_InnerLeftContent_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        var content = "Left";
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.InnerLeftContent = content;

        // Assert
        Assert.Equal(content, comboBox.InnerLeftContent);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Set_InnerRightContent_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        var content = "Right";
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.InnerRightContent = content;

        // Assert
        Assert.Equal(content, comboBox.InnerRightContent);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Set_PopupInnerTopContent_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        var content = "Top";
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.PopupInnerTopContent = content;

        // Assert
        Assert.Equal(content, comboBox.PopupInnerTopContent);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Set_PopupInnerBottomContent_Property()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        var content = "Bottom";
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.PopupInnerBottomContent = content;

        // Assert
        Assert.Equal(content, comboBox.PopupInnerBottomContent);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Add_Items_To_SelectedItems()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.SelectedItems?.Add("Item1");
        comboBox.SelectedItems?.Add("Item2");

        // Assert
        Assert.Equal(2, comboBox.SelectedItems?.Count);
        Assert.True(comboBox.SelectedItems?.Contains("Item1"));
        Assert.True(comboBox.SelectedItems?.Contains("Item2"));
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Remove_Items_From_SelectedItems()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.SelectedItems?.Add("Item1");
        comboBox.SelectedItems?.Add("Item2");
        comboBox.SelectedItems?.Remove("Item1");

        // Assert
        Assert.Equal(1, comboBox.SelectedItems?.Count);
        Assert.False(comboBox.SelectedItems?.Contains("Item1"));
        Assert.True(comboBox.SelectedItems?.Contains("Item2"));
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Replace_SelectedItems_Collection()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        var newItems = new AvaloniaList<object> { "NewItem1", "NewItem2" };
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.SelectedItems = newItems;

        // Assert
        Assert.Equal(newItems, comboBox.SelectedItems);
        Assert.Equal(2, comboBox.SelectedItems.Count);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Be_Visible_When_Added_To_Window()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();

        // Act
        window.Content = comboBox;
        window.Show();

        // Assert
        Assert.True(comboBox.IsVisible);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Inherit_From_SelectingItemsControl()
    {
        // Arrange & Act
        var comboBox = new UrsaControls.MultiComboBox();

        // Assert
        Assert.IsAssignableFrom<SelectingItemsControl>(comboBox);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Be_Focusable()
    {
        // Arrange & Act
        var comboBox = new UrsaControls.MultiComboBox();

        // Assert
        Assert.True(comboBox.Focusable);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Handle_Null_SelectedItems()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.SelectedItems = null;

        // Assert
        Assert.Null(comboBox.SelectedItems);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Handle_Mixed_Type_SelectedItems()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.SelectedItems?.Add("String");
        comboBox.SelectedItems?.Add(42);
        comboBox.SelectedItems?.Add(new Button());

        // Assert
        Assert.Equal(3, comboBox.SelectedItems?.Count);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Toggle_DropDown_State()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.IsDropDownOpen = true;
        comboBox.IsDropDownOpen = false;

        // Assert
        Assert.False(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Generate_Candidates_From_ItemsSource()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        var itemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3", "Item4" };
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.ItemsSource = itemsSource;

        // Assert
        Assert.Equal(itemsSource, comboBox.ItemsSource);
        Assert.Equal(4, comboBox.Items.Count);
        Assert.Contains("Item1", comboBox.Items);
        Assert.Contains("Item2", comboBox.Items);
        Assert.Contains("Item3", comboBox.Items);
        Assert.Contains("Item4", comboBox.Items);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Update_Candidates_When_ItemsSource_Changes()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        var itemsSource = new ObservableCollection<string> { "Item1", "Item2" };
        window.Content = comboBox;
        window.Show();
        comboBox.ItemsSource = itemsSource;

        // Act
        itemsSource.Add("Item3");
        itemsSource.Remove("Item1");

        // Assert
        Assert.Equal(2, comboBox.Items.Count);
        Assert.DoesNotContain("Item1", comboBox.Items);
        Assert.Contains("Item2", comboBox.Items);
        Assert.Contains("Item3", comboBox.Items);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Support_Complex_Objects_In_ItemsSource()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        var complexItems = new List<ComplexItem>
        {
            new ComplexItem { Id = 1, Name = "First", Category = "A" },
            new ComplexItem { Id = 2, Name = "Second", Category = "B" },
            new ComplexItem { Id = 3, Name = "Third", Category = "A" }
        };
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.ItemsSource = complexItems;

        // Assert
        Assert.Equal(complexItems, comboBox.ItemsSource);
        Assert.Equal(3, comboBox.Items.Count);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Remove_Item_Via_Remove_Method()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        var selectedItem = "SelectedItem";
        window.Content = comboBox;
        window.Show();

        comboBox.SelectedItems?.Add(selectedItem);
        comboBox.SelectedItems?.Add("OtherItem");

        // Create a mock styled element with DataContext
        var mockElement = new Border { DataContext = selectedItem };

        // Act
        comboBox.Remove(mockElement);

        // Assert
        Assert.Equal(1, comboBox.SelectedItems?.Count);
        Assert.DoesNotContain(selectedItem, comboBox.SelectedItems?.Cast<string>());
        Assert.Contains("OtherItem", comboBox.SelectedItems?.Cast<string>());
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Clear_SelectedItems_Via_Clear_Method()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();

        comboBox.SelectedItems?.Add("Item1");
        comboBox.SelectedItems?.Add("Item2");
        comboBox.SelectedItems?.Add("Item3");

        // Act
        comboBox.Clear();

        // Assert
        Assert.NotNull(comboBox.SelectedItems);
        Assert.Empty(comboBox.SelectedItems);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Handle_Remove_With_Null_DataContext()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();

        comboBox.SelectedItems?.Add("Item1");
        var mockElement = new Border { DataContext = null };

        // Act & Assert - Should not throw
        comboBox.Remove(mockElement);
        Assert.Single(comboBox.SelectedItems!);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Handle_Remove_With_Non_StyledElement()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();

        comboBox.SelectedItems?.Add("Item1");

        // Act & Assert - Should not throw
        comboBox.Remove("Not a styled element");
        Assert.Single(comboBox.SelectedItems!);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Clear_Empty_SelectedItems_Without_Error()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();

        // Act & Assert - Should not throw
        comboBox.Clear();
        Assert.NotNull(comboBox.SelectedItems);
        Assert.Empty(comboBox.SelectedItems);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Work_With_Mixed_ItemsSource_And_SelectedItems()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        var itemsSource = new List<string> { "Option1", "Option2", "Option3", "Option4" };
        window.Content = comboBox;
        window.Show();

        // Act
        comboBox.ItemsSource = itemsSource;
        comboBox.SelectedItems?.Add("Option1");
        comboBox.SelectedItems?.Add("Option3");
        comboBox.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.Equal(4, comboBox.Items.Count);
        Assert.Equal(2, comboBox.SelectedItems?.Count);
        Assert.Contains("Option1", comboBox.SelectedItems?.Cast<string>());
        Assert.Contains("Option3", comboBox.SelectedItems?.Cast<string>());
    }

    // Helper class for complex object testing
    private class ComplexItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
    }
}