using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using HeadlessTest.Ursa.TestHelpers;
using Ursa.Controls;
using DatePicker = Ursa.Controls.DatePicker;

namespace HeadlessTest.Ursa.Controls.DateTimePicker;

public class DatePickerTests
{
    [AvaloniaFact]
    public void Click_Opens_Popup()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Assert.False(picker.IsDropdownOpen);
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
        
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
        
    }

    [AvaloniaFact]
    public void Click_Button_Toggles_Popup()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();

        var button = picker.GetTemplateChildOfType<Button>(DatePicker.PART_Button);
        var position = button?.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);
        
        Assert.False(picker.IsDropdownOpen);
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(position.Value, MouseButton.Left);
        window.MouseUp(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
        
        window.MouseDown(position.Value, MouseButton.Left);
        window.MouseUp(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.False(picker.IsDropdownOpen);
    }
    
    [AvaloniaFact]
    public void Clear_Set_SelectedDate_To_Null()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        picker.SelectedDate = DateTime.Now;
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);
        picker.Clear();
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedDate);
    }
    
    [AvaloniaFact]
    public void Press_Escape_Closes_Popup()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Assert.False(picker.IsDropdownOpen);
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
        window.KeyPressQwerty(PhysicalKey.Escape, RawInputModifiers.None);
        Assert.False(picker.IsDropdownOpen);
    }
    
    [AvaloniaFact]
    public void Press_Down_Opens_Popup()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Assert.False(picker.IsDropdownOpen);
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        window.KeyPressQwerty(PhysicalKey.ArrowDown, RawInputModifiers.None);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
    }
    
    [AvaloniaFact]
    public void Press_Tab_Closes_Popup()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = new StackPanel()
        {
            Children =
            {
                picker,
                new TextBox(),
            }
        };
        window.Show();
        Assert.False(picker.IsDropdownOpen);
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
        window.KeyPressQwerty(PhysicalKey.Tab, RawInputModifiers.None);
        Assert.False(picker.IsDropdownOpen);
    }
    
    [AvaloniaFact]
    public void SelectedDate_Set_TextBox_Text()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        var popup = picker.GetTemplateChildOfType<Popup>(DatePicker.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<CalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new CalendarDayButtonEventArgs(new DateTime(2025, 2, 17))
            { RoutedEvent = CalendarView.DateSelectedEvent }); 
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePicker.PART_TextBox);
        Assert.NotNull(textBox);
        Assert.Equal("2025-02-17", textBox.Text);
        Assert.False(picker.IsDropdownOpen);
    }
    
    [AvaloniaFact]
    public void Set_SelectedDate_To_Null_Clears_TextBox()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        var popup = picker.GetTemplateChildOfType<Popup>(DatePicker.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<CalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new CalendarDayButtonEventArgs(new DateTime(2025, 2, 17))
            { RoutedEvent = CalendarView.DateSelectedEvent }); 
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePicker.PART_TextBox);
        Assert.NotNull(textBox);
        Assert.Equal("2025-02-17", textBox.Text);
        Assert.False(picker.IsDropdownOpen);
        picker.SelectedDate = null;
        Dispatcher.UIThread.RunJobs();
        Assert.Null(textBox.Text);
    }
    
    [AvaloniaFact]
    public void Set_SelectedDate_Updates_TextBox()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePicker.PART_TextBox);
        picker.SelectedDate = new DateTime(2025, 2, 18);
        Dispatcher.UIThread.RunJobs();
        Assert.Equal("2025-02-18", textBox?.Text);
    }
    
    [AvaloniaFact]
    public void Set_Valid_TextBox_Text_Updates_SelectedDate()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePicker.PART_TextBox);
        textBox?.RaiseEvent(new TextChangedEventArgs(TextBox.TextChangedEvent));
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedDate);
        textBox?.SetValue(TextBox.TextProperty, "2025-02-18");
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(new DateTime(2025, 2, 18), picker.SelectedDate);
    }
    
    [AvaloniaFact]
    public void Set_Invalid_TextBox_Text_Clears_SelectedDate()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        var focusTextBox = new TextBox();
        window.Content = new StackPanel()
        {
            Children =
            {
                picker,
                focusTextBox,
            }
        };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        picker.Focus();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePicker.PART_TextBox);
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedDate);
        textBox?.SetValue(TextBox.TextProperty, "2025-02-18");
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(new DateTime(2025, 2, 18), picker.SelectedDate);
        textBox?.SetValue(TextBox.TextProperty, "2025-02-18-");
        focusTextBox.Focus();
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedDate);
    }

    [AvaloniaFact]
    public void Empty_DisplayFormat_Works()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            DisplayFormat = null,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePicker.PART_TextBox);
        textBox?.RaiseEvent(new TextChangedEventArgs(TextBox.TextChangedEvent));
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedDate);
        textBox?.SetValue(TextBox.TextProperty, "2025-02-18");
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(new DateTime(2025, 2, 18), picker.SelectedDate);

        textBox?.SetValue(TextBox.TextProperty, "2025/02/19");
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(new DateTime(2025, 2, 19), picker.SelectedDate);
    }

    [AvaloniaFact]
    public void Ensure_Focusable()
    {
        var picker = new DatePicker();
        Assert.True(picker.Focusable);
    }

    [AvaloniaFact]
    public void Click_On_Popup_Will_Not_Close_Popup()
    {
        var window = new Window()
        {
            Width = 800, Height = 800
        };
        var picker = new DatePicker()
        {
            Width = 300,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Assert.False(picker.IsDropdownOpen);
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
        var popup = picker.GetTemplateChildOfType<Popup>(DatePicker.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<CalendarView>().FirstOrDefault();
        Assert.NotNull(calendar);
        var nextButton = calendar.GetTemplateChildOfType<Button>(CalendarView.PART_NextButton);
        Assert.NotNull(nextButton);
        var position = nextButton.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
    }
    
}