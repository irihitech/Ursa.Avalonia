using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.LoadingTests;

public class LoadingContainerTests
{
    [AvaloniaFact]
    public void LoadingContainer_Should_Use_Stretch_ContentAlignment_By_Default()
    {
        var window = new Window();
        var container = new LoadingContainer();
        window.Content = container;
        window.Show();

        Assert.Equal(HorizontalAlignment.Stretch, container.HorizontalContentAlignment);
        Assert.Equal(VerticalAlignment.Stretch, container.VerticalContentAlignment);
    }

    [AvaloniaFact]
    public void LoadingContainer_Should_Allow_Overriding_ContentAlignment()
    {
        var window = new Window();
        var container = new LoadingContainer();
        window.Content = container;
        window.Show();

        container.HorizontalContentAlignment = HorizontalAlignment.Center;
        container.VerticalContentAlignment = VerticalAlignment.Center;

        Assert.Equal(HorizontalAlignment.Center, container.HorizontalContentAlignment);
        Assert.Equal(VerticalAlignment.Center, container.VerticalContentAlignment);
    }
}
