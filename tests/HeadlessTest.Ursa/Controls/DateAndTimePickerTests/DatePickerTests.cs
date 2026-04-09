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

namespace HeadlessTest.Ursa.Controls.DateAndTimePickerTests;

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
        //Assert.True(picker.IsDropdownOpen);
        
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

        var button = picker.GetTemplateChildOfType<PathIcon>("PART_Button");
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
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateTime(2025, 2, 17))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent }); 
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
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateTime(2025, 2, 17))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent }); 
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
        textBox?.Focus();
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedDate);
        textBox?.SetValue(TextBox.TextProperty, "2025-02-18");
        window.KeyPressQwerty(PhysicalKey.Enter, RawInputModifiers.None);
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(new DateTime(2025, 2, 18), picker.SelectedDate);
    }

    [AvaloniaFact]
    public void DefaultDateKind_Utc_Applied_When_Calendar_Date_Selected()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        var popup = picker.GetTemplateChildOfType<Popup>(DatePicker.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateTime(2025, 2, 17))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedDate!.Value.Kind);
        Assert.Equal(new DateTime(2025, 2, 17, 0, 0, 0, DateTimeKind.Utc), picker.SelectedDate!.Value);
    }

    [AvaloniaFact]
    public void DefaultDateKind_Local_Applied_When_Calendar_Date_Selected()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            DefaultDateKind = DateTimeKind.Local,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        var popup = picker.GetTemplateChildOfType<Popup>(DatePicker.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateTime(2025, 3, 10))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Local, picker.SelectedDate!.Value.Kind);
    }

    [AvaloniaFact]
    public void DefaultDateKind_Utc_Applied_When_Text_Parsed()
    {
        var window = new Window();
        var picker = new DatePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePicker.PART_TextBox);
        textBox?.Focus();
        Dispatcher.UIThread.RunJobs();
        textBox?.SetValue(TextBox.TextProperty, "2025-06-15");
        window.KeyPressQwerty(PhysicalKey.Enter, RawInputModifiers.None);
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedDate!.Value.Kind);
        Assert.Equal(new DateTime(2025, 6, 15, 0, 0, 0, DateTimeKind.Utc), picker.SelectedDate!.Value);
    }

    [AvaloniaFact]
    public void DefaultDateKind_Unspecified_Is_Default()
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
        textBox?.Focus();
        Dispatcher.UIThread.RunJobs();
        textBox?.SetValue(TextBox.TextProperty, "2025-06-15");
        window.KeyPressQwerty(PhysicalKey.Enter, RawInputModifiers.None);
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Unspecified, picker.SelectedDate!.Value.Kind);
    }
    /*
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
    */

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
        var button = new Button(){Width = 100};
        window.Content = new StackPanel()
        {
            Children =
            {
                picker,
                button,
            }
        };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePicker.PART_TextBox);
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedDate);
        textBox?.Focus();
        textBox?.SetValue(TextBox.TextProperty, "2025-02-18");
        button.Focus();
        
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(new DateTime(2025, 2, 18), picker.SelectedDate);

        textBox?.Focus();
        textBox?.SetValue(TextBox.TextProperty, "2025-02-19");
        button.Focus();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(new DateTime(2025, 2, 19), picker.SelectedDate);
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
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        Assert.NotNull(calendar);
        var nextButton = calendar.GetTemplateChildOfType<Button>(DatePickerCalendarView.PART_NextButton);
        Assert.NotNull(nextButton);
        var position = nextButton.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        
        Dispatcher.UIThread.RunJobs();
        //Assert.True(picker.IsDropdownOpen);
    }
    
}