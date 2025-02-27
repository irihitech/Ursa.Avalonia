using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Common;
using Ursa.Controls;
using Ursa.Controls.Options;

namespace HeadlessTest.Ursa.Controls.DrawerTests.MeasureTest;

public class DrawerMeasureTest
{
    [AvaloniaTheory]
    [InlineData(Position.Left)]
    [InlineData(Position.Right)]
    [InlineData(Position.Top)]
    [InlineData(Position.Bottom)]
    public async void Default_Drawer_Is_Constrained_When_Content_Is_Large(Position position)
    {
        var window = new UrsaWindow
        {
            Height = 600,
            Width = 800
        };
        var textBlock = new TextBlock
        {
            Width = 1000,
            Height = 1000
        };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        _ = Drawer.ShowModal(textBlock, "hello world", null,
            new DrawerOptions { Position = position, TopLevelHashCode = window.GetHashCode() });
        await Task.Delay(TimeSpan.FromSeconds(0.1));
        var dialogControl = window.GetVisualDescendants().OfType<DefaultDrawerControl>().SingleOrDefault();
        Assert.NotNull(dialogControl);
        Assert.True(dialogControl.Bounds.Width <= window.Bounds.Width);
        Assert.True(dialogControl.Bounds.Height <= window.Bounds.Height);
    }

    [AvaloniaTheory]
    [InlineData(Position.Left)]
    [InlineData(Position.Right)]
    [InlineData(Position.Top)]
    [InlineData(Position.Bottom)]
    public async void Custom_Drawer_Is_Constrained_When_Content_Is_Large(Position position)
    {
        var window = new UrsaWindow
        {
            Height = 600,
            Width = 800
        };
        var textBlock = new TextBlock
        {
            Width = 1000,
            Height = 1000
        };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Drawer.ShowCustom(textBlock, "hello world", null,
            new DrawerOptions { Position = position, TopLevelHashCode = window.GetHashCode() });
        await Task.Delay(TimeSpan.FromSeconds(0.1));
        var dialogControl = window.GetVisualDescendants().OfType<CustomDrawerControl>().SingleOrDefault();
        Assert.NotNull(dialogControl);
        Assert.True(dialogControl.Bounds.Width <= window.Bounds.Width);
        Assert.True(dialogControl.Bounds.Height <= window.Bounds.Height);
    }
}