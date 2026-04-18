using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.PathPickerTests;

public class PathPickerButtonContentTests
{
    [AvaloniaFact]
    public void ButtonContent_IsNull_ByDefault()
    {
        var picker = new PathPicker();
        var window = new Window { Content = picker };
        window.Show();

        Assert.Null(picker.ButtonContent);
    }

    [AvaloniaFact]
    public void ButtonContent_SyncsWithTitle_WhenNotExplicitlySet()
    {
        var picker = new PathPicker();
        var window = new Window { Content = picker };
        window.Show();

        picker.Title = "Select File";

        Assert.Equal("Select File", picker.ButtonContent);
    }

    [AvaloniaFact]
    public void ButtonContent_UpdatesWith_Title_OnSubsequentChanges()
    {
        var picker = new PathPicker();
        var window = new Window { Content = picker };
        window.Show();

        picker.Title = "Select File";
        Assert.Equal("Select File", picker.ButtonContent);

        picker.Title = "Open Folder";
        Assert.Equal("Open Folder", picker.ButtonContent);
    }

    [AvaloniaFact]
    public void ButtonContent_IsNotOverwritten_WhenExplicitlySet()
    {
        var picker = new PathPicker();
        var window = new Window { Content = picker };
        window.Show();

        picker.Title = "Select File";
        picker.ButtonContent = "Custom Icon Content";

        picker.Title = "Open Folder";

        Assert.Equal("Custom Icon Content", picker.ButtonContent);
    }

    [AvaloniaFact]
    public void ButtonContent_CanBeSetIndependentlyOfTitle()
    {
        var picker = new PathPicker();
        var window = new Window { Content = picker };
        window.Show();

        picker.Title = "Dialog Title";
        picker.ButtonContent = "Browse...";

        Assert.Equal("Dialog Title", picker.Title);
        Assert.Equal("Browse...", picker.ButtonContent);
    }
}
