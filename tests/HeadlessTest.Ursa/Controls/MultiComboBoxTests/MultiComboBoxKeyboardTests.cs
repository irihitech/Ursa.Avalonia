using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Threading;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.MultiComboBoxTests;

public class MultiComboBoxKeyboardTests
{
    [AvaloniaFact]
    public void MultiComboBox_Should_Toggle_Dropdown_With_F4_Key()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();
        comboBox.Focus();

        // Act - Press F4 to open
        comboBox.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.F4
        });
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.True(comboBox.IsDropDownOpen);

        // Act - Press F4 again to close
        comboBox.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.F4
        });
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.False(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Toggle_Dropdown_With_AltDown_Key()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();
        comboBox.Focus();

        // Act - Press Alt+Down to open
        comboBox.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.Down,
            KeyModifiers = KeyModifiers.Alt
        });
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.True(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Toggle_Dropdown_With_AltUp_Key()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();
        comboBox.Focus();

        // Act - Press Alt+Up to open
        comboBox.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.Up,
            KeyModifiers = KeyModifiers.Alt
        });
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.True(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Close_Dropdown_With_Escape_Key()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();
        comboBox.Focus();
        comboBox.IsDropDownOpen = true;

        // Act - Press Escape to close
        comboBox.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.Escape
        });
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.False(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Not_Close_Dropdown_With_Escape_When_Already_Closed()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();
        comboBox.Focus();
        comboBox.IsDropDownOpen = false;

        // Act - Press Escape when already closed
        comboBox.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.Escape
        });
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.False(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Open_Dropdown_With_Enter_Key()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();
        comboBox.Focus();

        // Act - Press Enter to open
        comboBox.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.Return
        });
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.True(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Open_Dropdown_With_Space_Key()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();
        comboBox.Focus();

        // Act - Press Space to open
        comboBox.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.Space
        });
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.True(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Close_Dropdown_With_Tab_Key()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();
        comboBox.Focus();
        comboBox.IsDropDownOpen = true;

        // Act - Press Tab to close
        comboBox.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.Tab
        });
        Dispatcher.UIThread.RunJobs();

        // Assert
        Assert.False(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Not_Toggle_With_F4_And_Alt()
    {
        // Arrange
        var window = new Window();
        var comboBox = new UrsaControls.MultiComboBox();
        window.Content = comboBox;
        window.Show();
        comboBox.Focus();

        // Act - Press F4 with Alt modifier (should not toggle)
        comboBox.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.F4,
            KeyModifiers = KeyModifiers.Alt
        });
        Dispatcher.UIThread.RunJobs();

        // Assert - Should remain closed
        Assert.False(comboBox.IsDropDownOpen);
    }

    [AvaloniaFact]
    public void MultiComboBox_Should_Be_Focusable()
    {
        // Arrange & Act
        var comboBox = new UrsaControls.MultiComboBox();

        // Assert
        Assert.True(comboBox.Focusable);
    }
}
