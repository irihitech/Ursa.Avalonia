using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;
using Xunit;

namespace Ursa.Themes.Fluent2.Tests;

public class DatePickerChromeTests
{
    [AvaloniaTheory]
    [InlineData("DatePicker", false)]
    [InlineData("DatePicker", true)]
    [InlineData("DateOnlyPicker", false)]
    [InlineData("DateOnlyPicker", true)]
    [InlineData("DateOffsetPicker", false)]
    [InlineData("DateOffsetPicker", true)]
    public void Date_editor_reserves_space_for_calendar_and_clear_buttons(string name, bool dark)
    {
        TemplatedControl picker = name switch
        {
            "DatePicker" => new Ursa.Controls.DatePicker { SelectedDate = new DateTime(2026, 10, 29) },
            "DateOnlyPicker" => new DateOnlyPicker { SelectedDate = new DateOnly(2026, 10, 29) },
            _ => new DateOffsetPicker { SelectedDate = new DateTimeOffset(2026, 10, 29, 0, 0, 0, TimeSpan.Zero) }
        };
        picker.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        var window = new Window
        {
            Content = picker,
            RequestedThemeVariant = dark ? Avalonia.Styling.ThemeVariant.Dark : Avalonia.Styling.ThemeVariant.Light
        };
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            var input = picker.GetVisualDescendants().OfType<TextBox>().Single(control => control.Name == "PART_TextBox");
            var icon = picker.GetVisualDescendants().OfType<PathIcon>().Single(control => control.Name == "PART_Button");
            var clear = picker.GetVisualDescendants().OfType<Button>().Single(control => control.Name == "ClearButton");
            Assert.Equal("2026-10-29", input.Text);

            foreach (var width in new[] { double.NaN, name == "DateOffsetPicker" ? 300d : 140d })
            {
                picker.Width = width;
                window.UpdateLayout();
                Assert.True(input.Bounds.Right <= icon.Bounds.Left);
                if (double.IsNaN(width))
                    Assert.True(input.Bounds.Width >= input.DesiredSize.Width);

                picker.Classes.Add("clearButton");
                window.MouseMove(input.TranslatePoint(new Point(4, 4), window)!.Value);
                Dispatcher.UIThread.RunJobs();
                window.UpdateLayout();
                Assert.True(clear.IsVisible);
                Assert.False(icon.IsVisible);
                Assert.True(input.Bounds.Right <= clear.Bounds.Left);
                picker.Classes.Remove("clearButton");
                window.UpdateLayout();
            }
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaTheory]
    [InlineData("DateRangePicker", false)]
    [InlineData("DateRangePicker", true)]
    [InlineData("DateOnlyRangePicker", false)]
    [InlineData("DateOnlyRangePicker", true)]
    [InlineData("DateOffsetRangePicker", false)]
    [InlineData("DateOffsetRangePicker", true)]
    public void Range_picker_has_one_hover_and_focus_surface(string name, bool dark)
    {
        TemplatedControl picker = name switch
        {
            "DateRangePicker" => new DateRangePicker(),
            "DateOnlyRangePicker" => new DateOnlyRangePicker(),
            _ => new DateOffsetRangePicker()
        };
        picker.Width = 440;
        var window = new Window
        {
            Content = picker,
            SizeToContent = SizeToContent.WidthAndHeight,
            RequestedThemeVariant = dark ? Avalonia.Styling.ThemeVariant.Dark : Avalonia.Styling.ThemeVariant.Light
        };
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            var inputs = picker.GetVisualDescendants().OfType<TextBox>()
                .Where(input => input.Name is "PART_StartTextBox" or "PART_EndTextBox").ToArray();
            Assert.Equal(2, inputs.Length);
            var surface = picker.GetVisualDescendants().OfType<Border>()
                .Single(border => border.Name == "Background" && ReferenceEquals(border.TemplatedParent, picker));
            foreach (var input in inputs)
            {
                var chrome = input.GetVisualDescendants().OfType<Border>().Single(border => border.Name == "PART_BorderElement");
                Assert.False(chrome.IsVisible);
                var point = input.TranslatePoint(new Point(input.Bounds.Width / 2, input.Bounds.Height / 2), window)!.Value;
                window.MouseMove(point);
                Dispatcher.UIThread.RunJobs();
                Assert.True(input.IsPointerOver);
                Assert.False(chrome.IsVisible);
                AssertResource("CalendarDatePickerPointeroverBackground", surface.Background, window);
                input.Focus();
                Dispatcher.UIThread.RunJobs();
                Assert.True(input.IsFocused);
                Assert.False(chrome.IsVisible);
                AssertResource("CalendarDatePickerFocusBorderBrush", surface.BorderBrush, window);
                input.Text = "2026-10-29";
                Assert.Equal("2026-10-29", input.Text);
                picker.SetCurrentValue(DateRangePickerBase.IsDropdownOpenProperty, false);
                Dispatcher.UIThread.RunJobs();
                window.UpdateLayout();
            }

            var icon = picker.GetVisualDescendants().OfType<PathIcon>().Single(control => control.Name == "PART_Button");
            window.MouseMove(icon.TranslatePoint(new Point(4, 4), window)!.Value);
            Dispatcher.UIThread.RunJobs();
            AssertResource("CalendarDatePickerPointeroverBackground", surface.Background, window);
            picker.IsEnabled = false;
            AssertResource("CalendarDatePickerDisabledBackground", surface.Background, window);

            if (picker is DateOffsetRangePicker offset)
            {
                Assert.Equal(0, surface.CornerRadius.TopLeft);
                Assert.Equal(offset.CornerRadius.TopRight, surface.CornerRadius.TopRight);
                offset.ShowOffsetSelection = false;
                Assert.Equal(offset.CornerRadius, surface.CornerRadius);
            }
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaTheory]
    [InlineData("DatePicker")]
    [InlineData("DateOnlyPicker")]
    [InlineData("DateOffsetPicker")]
    [InlineData("DateTimePicker")]
    [InlineData("DateTimeOffsetPicker")]
    [InlineData("TimePicker")]
    [InlineData("TimeOnlyPicker")]
    [InlineData("TimeRangePicker")]
    [InlineData("TimeOnlyRangePicker")]
    public void Embedded_picker_textboxes_do_not_draw_independent_chrome(string name)
    {
        Control picker = name switch
        {
            "DatePicker" => new Ursa.Controls.DatePicker(),
            "DateOnlyPicker" => new DateOnlyPicker(),
            "DateOffsetPicker" => new DateOffsetPicker(),
            "DateTimePicker" => new Ursa.Controls.DateTimePicker(),
            "DateTimeOffsetPicker" => new DateTimeOffsetPicker(),
            "TimePicker" => new Ursa.Controls.TimePicker(),
            "TimeOnlyPicker" => new TimeOnlyPicker(),
            "TimeRangePicker" => new TimeRangePicker(),
            _ => new TimeOnlyRangePicker()
        };
        var window = new Window { Content = picker };
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            var inputs = picker.GetVisualDescendants().OfType<TextBox>()
                .Where(input => input.Name is "PART_TextBox" or "PART_StartTextBox" or "PART_EndTextBox").ToArray();
            Assert.NotEmpty(inputs);
            foreach (var input in inputs)
            {
                input.Focus();
                Dispatcher.UIThread.RunJobs();
                var chrome = input.GetVisualDescendants().OfType<Border>().Single(border => border.Name == "PART_BorderElement");
                Assert.False(chrome.IsVisible);
            }
        }
        finally
        {
            window.Close();
        }
    }

    private static void AssertResource(string key, object? actual, Window window)
    {
        Assert.True(Application.Current!.TryGetResource(key, window.ActualThemeVariant, out var expected));
        var expectedBrush = expected is Color color ? new SolidColorBrush(color) : Assert.IsAssignableFrom<IBrush>(expected);
        var actualBrush = Assert.IsAssignableFrom<IBrush>(actual);
        Assert.Equal(expectedBrush.Opacity, actualBrush.Opacity);
        if (expectedBrush is ILinearGradientBrush gradient)
        {
            var actualGradient = Assert.IsAssignableFrom<ILinearGradientBrush>(actualBrush);
            Assert.Equal(gradient.StartPoint, actualGradient.StartPoint);
            Assert.Equal(gradient.EndPoint, actualGradient.EndPoint);
            Assert.Equal(gradient.SpreadMethod, actualGradient.SpreadMethod);
            Assert.Equal(gradient.GradientStops.Select(stop => (stop.Color, stop.Offset)),
                actualGradient.GradientStops.Select(stop => (stop.Color, stop.Offset)));
        }
        else
        {
            Assert.Equal(Assert.IsAssignableFrom<ISolidColorBrush>(expectedBrush).Color,
                Assert.IsAssignableFrom<ISolidColorBrush>(actualBrush).Color);
        }
    }
}
