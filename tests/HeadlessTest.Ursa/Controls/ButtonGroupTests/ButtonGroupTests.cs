using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Headless.XUnit;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.ButtonGroupTests;

public class ButtonGroupTests
{
    [AvaloniaFact]
    public void ButtonGroup_Should_Create_Button_Containers_For_Non_Button_Items()
    {
        // Arrange
        var window = new Window();
        var buttonGroup = new ButtonGroup();
        window.Content = buttonGroup;
        window.Show();

        var items = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
        buttonGroup.ItemsSource = items;

        // Act & Assert
        var generatedButtons = buttonGroup.GetVisualDescendants().OfType<Button>().ToList();
        Assert.Equal(3, generatedButtons.Count);
        
        // Verify each button has the correct content
        for (int i = 0; i < items.Count; i++)
        {
            Assert.Equal(items[i], generatedButtons[i].Content);
        }
    }

    [AvaloniaFact]
    public void ButtonGroup_Should_Not_Wrap_Existing_Button_Items()
    {
        // Arrange
        var window = new Window();
        var buttonGroup = new ButtonGroup();
        window.Content = buttonGroup;
        window.Show();

        var button1 = new Button { Content = "Button1" };
        var button2 = new Button { Content = "Button2" };
        buttonGroup.Items.Add(button1);
        buttonGroup.Items.Add(button2);

        // Act & Assert
        var visualButtons = buttonGroup.GetVisualDescendants().OfType<Button>().ToList();
        Assert.Equal(2, visualButtons.Count);
        Assert.Contains(button1, visualButtons);
        Assert.Contains(button2, visualButtons);
    }

    [AvaloniaFact]
    public void ButtonGroup_Should_Apply_CommandBinding_To_Generated_Buttons()
    {
        // Arrange
        var window = new Window();
        var buttonGroup = new ButtonGroup();
        window.Content = buttonGroup;
        window.Show();

        var commandExecuted = false;
        var testCommand = new RelayCommand(() => commandExecuted = true);
        var testItem = new TestItem { Command = testCommand };

        buttonGroup.CommandBinding = new Binding("Command");
        buttonGroup.Items.Add(testItem);

        // Act
        var generatedButton = buttonGroup.GetVisualDescendants().OfType<Button>().FirstOrDefault();
        Assert.NotNull(generatedButton);
        
        generatedButton.Command?.Execute(null);

        // Assert
        Assert.True(commandExecuted);
    }

    [AvaloniaFact]
    public void ButtonGroup_Should_Apply_CommandParameterBinding_To_Generated_Buttons()
    {
        // Arrange
        var window = new Window();
        var buttonGroup = new ButtonGroup();
        window.Content = buttonGroup;
        window.Show();

        object? receivedParameter = null;
        var testCommand = new RelayCommand<object>(param => receivedParameter = param);
        var testItem = new TestItem { Command = testCommand, Parameter = "TestParam" };

        buttonGroup.CommandBinding = new Binding("Command");
        buttonGroup.CommandParameterBinding = new Binding("Parameter");
        buttonGroup.Items.Add(testItem);

        // Act
        var generatedButton = buttonGroup.GetVisualDescendants().OfType<Button>().FirstOrDefault();
        Assert.NotNull(generatedButton);
        
        generatedButton.Command?.Execute(generatedButton.CommandParameter);

        // Assert
        Assert.Equal("TestParam", receivedParameter);
    }

    [AvaloniaFact]
    public void ButtonGroup_Should_Apply_ContentBinding_To_Generated_Buttons()
    {
        // Arrange
        var window = new Window();
        var buttonGroup = new ButtonGroup();
        window.Content = buttonGroup;
        window.Show();

        var testItem = new TestItem { DisplayName = "Display Content" };
        buttonGroup.ContentBinding = new Binding("DisplayName");
        buttonGroup.Items.Add(testItem);

        // Act
        var generatedButton = buttonGroup.GetVisualDescendants().OfType<Button>().FirstOrDefault();

        // Assert
        Assert.NotNull(generatedButton);
        Assert.Equal("Display Content", generatedButton.Content);
    }

    [AvaloniaFact]
    public void ButtonGroup_Should_Apply_ItemTemplate_To_Generated_Buttons()
    {
        // Arrange
        var window = new Window();
        var buttonGroup = new ButtonGroup();
        window.Content = buttonGroup;
        window.Show();

        var template = new FuncDataTemplate<TestItem>((item, _) => 
            new TextBlock { Text = $"Template: {item?.DisplayName}" });
        
        buttonGroup.ItemTemplate = template;
        var testItem = new TestItem { DisplayName = "Test Item" };
        buttonGroup.Items.Add(testItem);

        // Act
        var generatedButton = buttonGroup.GetVisualDescendants().OfType<Button>().FirstOrDefault();

        // Assert
        Assert.NotNull(generatedButton);
        Assert.Equal(template, generatedButton.ContentTemplate);
        
        // Since templates are applied to the content, let's verify the data context is correct
        Assert.Equal(testItem, generatedButton.DataContext);
    }

    [AvaloniaFact]
    public void ButtonGroup_Should_Handle_Mixed_Button_And_Non_Button_Items()
    {
        // Arrange
        var window = new Window();
        var buttonGroup = new ButtonGroup();
        window.Content = buttonGroup;
        window.Show();

        var existingButton = new Button { Content = "Existing Button" };
        var stringItem = "String Item";
        
        buttonGroup.Items.Add(existingButton);
        buttonGroup.Items.Add(stringItem);

        // Act & Assert
        var allButtons = buttonGroup.GetVisualDescendants().OfType<Button>().ToList();
        Assert.Equal(2, allButtons.Count);
        
        // One should be the existing button, one should be generated
        Assert.Contains(existingButton, allButtons);
        
        var generatedButton = allButtons.First(b => b != existingButton);
        Assert.Equal(stringItem, generatedButton.Content);
    }

    [AvaloniaFact]
    public void ButtonGroup_Should_Update_When_ItemsSource_Changes()
    {
        // Arrange
        var window = new Window();
        var buttonGroup = new ButtonGroup();
        window.Content = buttonGroup;
        window.Show();

        var items = new ObservableCollection<string> { "Item1", "Item2" };
        buttonGroup.ItemsSource = items;

        // Initially should have 2 buttons
        var initialButtons = buttonGroup.GetVisualDescendants().OfType<Button>().ToList();
        Assert.Equal(2, initialButtons.Count);

        // Act - Add an item
        items.Add("Item3");

        // Assert - Should now have 3 buttons
        var updatedButtons = buttonGroup.GetVisualDescendants().OfType<Button>().ToList();
        Assert.Equal(3, updatedButtons.Count);

        // Act - Remove an item
        items.RemoveAt(0);

        // Assert - Should now have 2 buttons
        var finalButtons = buttonGroup.GetVisualDescendants().OfType<Button>().ToList();
        Assert.Equal(2, finalButtons.Count);
    }
}

// Helper class for testing
public class TestItem
{
    public string? DisplayName { get; set; }
    public ICommand? Command { get; set; }
    public object? Parameter { get; set; }
}