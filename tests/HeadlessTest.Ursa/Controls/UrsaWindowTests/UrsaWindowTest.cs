using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.UrsaWindowTests;

public class UrsaWindowTest
{
    [AvaloniaFact]
    public void Default_UrsaWindow_Closing_Is_Called_Once()
    {
        var ursaWindow = new UrsaWindow();
        ursaWindow.Show();
        int count = 0;
        ursaWindow.Closing += (_, _) => count++;
        ursaWindow.Close();
        Assert.Equal(1, count);
    }

    [AvaloniaFact]
    public void Inferenced_Window_Closing_Called_Once_When_Yes()
    {
        var window = new UrsaWindowWithCloseInference();
        window.Show();
        int count = 0;
        window.Closing += (_, _) => count++;
        window.Close();
        window.DialogViewModel.CloseYes();
        Assert.Equal(1, count);
    }

    [AvaloniaFact]
    public void Inferenced_Window_Closing_Called_Once_When_No()
    {
        var window = new UrsaWindowWithCloseInference();
        window.Show();
        int count = 0;
        window.Closing += (_, _) => count++;
        window.Close();
        window.DialogViewModel.Close();
        Assert.Equal(1, count);
    }

    /// <summary>
    ///     Headless has no drawn decorations: CaptionButtonsWidth stays 0 and the
    ///     title bar keeps its authored margin across window state changes.
    /// </summary>
    [AvaloniaFact]
    public void FullScreen_Toggle_Should_Keep_TitleBar_Properties_Without_DrawnDecorations()
    {
        var window = new UrsaWindow
        {
            Width = 800,
            Height = 600,
            TitleBarMargin = new Thickness(0, 0, 7, 0),
        };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var titleBar = window.GetVisualDescendants().OfType<TitleBar>().FirstOrDefault();
        Assert.NotNull(titleBar);
        Assert.Equal(0, window.CaptionButtonsWidth);
        Assert.Equal(0, titleBar.RightInset);
        Assert.Equal(7, titleBar.Margin.Right);

        window.WindowState = WindowState.FullScreen;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(0, window.CaptionButtonsWidth);
        Assert.Equal(0, titleBar.RightInset);
        Assert.Equal(7, titleBar.Margin.Right);

        window.WindowState = WindowState.Normal;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(0, window.CaptionButtonsWidth);
        Assert.Equal(0, titleBar.RightInset);
        Assert.Equal(7, titleBar.Margin.Right);

        window.Close();
    }
}
