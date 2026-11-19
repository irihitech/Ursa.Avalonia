using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
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

    /// <summary>
    ///     The caption inset is reserved only while the client area extends under the buttons,
    ///     tracks the panel width, and clears while the panel is hidden.
    /// </summary>
    [AvaloniaFact]
    public void CaptionInset_Should_Be_Reserved_Only_When_Client_Area_Is_Extended()
    {
        var buttonArea = new Border { Width = 120, Height = 32 };
        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Children = { buttonArea, new CaptionInsetReporter() },
        };
        var window = new UrsaWindow { Width = 800, Height = 600, Content = panel };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var titleBar = window.GetVisualDescendants().OfType<TitleBar>().FirstOrDefault();
        Assert.NotNull(titleBar);
        Assert.Equal(120, panel.Bounds.Width);
        Assert.False(window.IsExtendedIntoWindowDecorations);
        Assert.Equal(0, window.CaptionButtonsWidth);
        Assert.Equal(0, titleBar.RightInset);

        ReportClientAreaExtension(window, true);
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(120, window.CaptionButtonsWidth);
        Assert.Equal(120, titleBar.RightInset);

        buttonArea.Width = 200;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(200, panel.Bounds.Width);
        Assert.Equal(200, window.CaptionButtonsWidth);
        Assert.Equal(200, titleBar.RightInset);

        panel.IsVisible = false;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(0, window.CaptionButtonsWidth);
        Assert.Equal(0, titleBar.RightInset);

        panel.IsVisible = true;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(200, window.CaptionButtonsWidth);

        ReportClientAreaExtension(window, false);
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(0, window.CaptionButtonsWidth);
        Assert.Equal(0, titleBar.RightInset);

        window.Close();
    }

    /// <summary>
    ///     A closed popup detaches its child from the visual tree while it stays
    ///     logically attached: the inset clears and reporting resumes on reopen.
    /// </summary>
    [AvaloniaFact]
    public void CaptionInset_Should_Clear_When_Panel_Is_Detached_From_Visual_Tree()
    {
        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Children = { new Border { Width = 120, Height = 32 }, new CaptionInsetReporter() },
        };
        var anchor = new Border();
        var popup = new Popup { Child = panel, PlacementTarget = anchor };
        var window = new UrsaWindow
        {
            Width = 800,
            Height = 600,
            Content = new StackPanel { Children = { anchor, popup } },
        };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        ReportClientAreaExtension(window, true);
        Dispatcher.UIThread.RunJobs();

        popup.IsOpen = true;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(120, window.CaptionButtonsWidth);

        popup.IsOpen = false;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(0, window.CaptionButtonsWidth);

        popup.IsOpen = true;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(120, window.CaptionButtonsWidth);

        popup.IsOpen = false;
        window.Close();
    }

    // Headless never extends the client area, so replay the platform's notification.
    // The callback is hidden from Avalonia's reference assemblies, hence reflection.
    private static void ReportClientAreaExtension(Window window, bool isExtended)
    {
        var impl = window.PlatformImpl!;
        var callback = impl.GetType().GetProperty("ExtendClientAreaToDecorationsChanged")?.GetValue(impl);
        Assert.IsType<Action<bool>>(callback).Invoke(isExtended);
    }
}
