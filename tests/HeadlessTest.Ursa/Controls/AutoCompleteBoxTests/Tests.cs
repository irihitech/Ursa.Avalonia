using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Layout;
using UrsaControl = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.AutoCompleteBoxTests;

public class Tests
{
    [AvaloniaFact]
    // Ideally Should not open popup when there is no items, but sub class have no access to this status. 
    public void AutoCompleteBox_Focus_And_LostFocus()
    {
        var window = new Window();
        var autoCompleteBox = new UrsaControl.AutoCompleteBox()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Width = 100,
            Height = 100,
        };
        var textBox = new TextBox()
        {
            Width = 100, Height = 100,
        };
        window.Content = new StackPanel()
        {
            Children = { autoCompleteBox, textBox }
        };
        window.Show();
        
        Assert.False(autoCompleteBox.IsDropDownOpen);
        autoCompleteBox.Focus();
        Assert.True(autoCompleteBox.IsDropDownOpen);
        textBox.Focus();
        Assert.False(autoCompleteBox.IsDropDownOpen);
    }
    
    [AvaloniaFact]
    public void Click_AutoCompleteBox_With_Mouse_Should_Open_Dropdown()
    {
        var window = new Window();
        var autoCompleteBox = new UrsaControl.AutoCompleteBox()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Width = 100,
            Height = 100,
            ItemsSource = new List<string> {"Hello", "World"}
        };
        window.Content = autoCompleteBox;
        window.Show();
        
        Assert.False(autoCompleteBox.IsDropDownOpen);
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Assert.True(autoCompleteBox.IsDropDownOpen);
    }
    
    [AvaloniaFact]
    public void Click_AutoCompleteBox_With_Mouse_Should_NotOpen_Dropdown_When_Empty()
    {
        var window = new Window();
        var autoCompleteBox = new UrsaControl.AutoCompleteBox()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Width = 100,
            Height = 100,
        };
        window.Content = autoCompleteBox;
        window.Show();
        
        Assert.False(autoCompleteBox.IsDropDownOpen);
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Assert.False(autoCompleteBox.IsDropDownOpen);
    }
    
    [AvaloniaFact]
    public void Right_Click_AutoCompleteBox_With_Mouse_Should_NotOpen_Dropdown()
    {
        var window = new Window();
        var autoCompleteBox = new UrsaControl.AutoCompleteBox()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Width = 100,
            Height = 100,
            ItemsSource = new List<string> {"Hello", "World"},
        };
        window.Content = autoCompleteBox;
        window.Show();
        
        Assert.False(autoCompleteBox.IsDropDownOpen);
        window.MouseDown(new Point(10, 10), MouseButton.Right);
        Assert.False(autoCompleteBox.IsDropDownOpen);
    }
}