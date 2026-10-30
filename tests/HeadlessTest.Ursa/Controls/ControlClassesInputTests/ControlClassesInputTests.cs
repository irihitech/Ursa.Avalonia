using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.ControlClassesInputTests;

public class ControlClassesInputTests
{
    [AvaloniaFact]
    public void Template_Binds_TargetClasses_To_TagInput()
    {
        var window = new Window();
        var input = new ControlClassesInput();
        window.Content = input;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var tagInput = input.GetVisualDescendants().OfType<TagInput>().FirstOrDefault();
        Assert.NotNull(tagInput);
        Assert.NotNull(tagInput.Tags);
        Assert.Same(input.TargetClasses, tagInput.Tags);
    }
}
