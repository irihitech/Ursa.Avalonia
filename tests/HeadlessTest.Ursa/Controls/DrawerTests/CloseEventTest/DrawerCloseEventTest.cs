using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls;

public class DrawerCloseEventTest
{
    [AvaloniaFact]
    public async void Test()
    {
        UrsaWindow testWindow = new()
        {
            Content = new OverlayDialogHost() { HostId = "root" }
        };
        testWindow.Show();
        DrawerCloseTestPopupControl level1 = new();
        _ = OverlayDialog.ShowCustomModal<object>(level1, new DrawerCloseTestPopupControlVM(), "root");
        level1.OpenPopup();
        var level2 = level1.Popup;
        Assert.NotNull(level2);
        level2.OpenPopup();
        var level3 = level2.Popup;
        Assert.NotNull(level3);
        level2.ClosePopup();
        await Task.Delay(TimeSpan.FromSeconds(1));
        Dispatcher.UIThread.RunJobs();
        Assert.True(level1.IsAttachedToVisualTree()
                    && level2.IsAttachedToVisualTree()
                    && level3.IsAttachedToVisualTree() is false);

        level1.ClosePopup();
        await Task.Delay(TimeSpan.FromSeconds(1));
        Dispatcher.UIThread.RunJobs();
        Assert.True(level1.IsAttachedToVisualTree()
                    && level2.IsAttachedToVisualTree() is false
                    && level3.IsAttachedToVisualTree() is false);

        level1.Close();
        await Task.Delay(TimeSpan.FromSeconds(1));
        Dispatcher.UIThread.RunJobs();
        Assert.False(level1.IsAttachedToVisualTree()
                     && level2.IsAttachedToVisualTree()
                     && level3.IsAttachedToVisualTree());

        Assert.Equal(level1.LResult, level1.RResult);
        Assert.Equal(level2.LResult, level2.RResult);
        Assert.Equal(level3.LResult, level3.RResult);

        _ = OverlayDialog.ShowCustomModal<object>(level1, new DrawerCloseTestPopupControlVM(), "root");
        level1.OpenPopup();
        level2 = level1.Popup;
        Assert.NotNull(level2);
        level2.OpenPopup();
        level3 = level2.Popup;
        Assert.NotNull(level3);
        level3.OpenPopup();
        level1.Close();
        await Task.Delay(TimeSpan.FromSeconds(1));
        Dispatcher.UIThread.RunJobs();
        Assert.False(level1.IsAttachedToVisualTree()
                     && level2.IsAttachedToVisualTree()
                     && level3.IsAttachedToVisualTree());
    }
}