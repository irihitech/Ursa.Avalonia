using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.CaptionButtonsTests;

public class Test
{
    [AvaloniaTheory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void UrsaWindow_IsRestoreButtonVisible_Should_Control_RestoreButton_Visibility(bool canMaximize, bool expectedVisibility)
    {
        var window = new UrsaWindow();
        var caption = new CaptionButtons();
        window.Content = caption;
        caption.Attach(window);
        window.Show();
        window.IsRestoreButtonVisible = canMaximize;
        var restoreButton = caption.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Name == "PART_RestoreButton");
        Assert.NotNull(restoreButton);
        Assert.Equal(expectedVisibility, restoreButton.IsVisible);
        window.IsRestoreButtonVisible = !canMaximize;
        Assert.Equal(!expectedVisibility, restoreButton.IsVisible);
    }
    
    [AvaloniaTheory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void UrsaWindow_IsMinimizeButtonVisible_Should_Control_MinimizeButton_Visibility(bool canMinimize, bool expectedVisibility)
    {
        var window = new UrsaWindow();
        var caption = new CaptionButtons();
        window.Content = caption;
        caption.Attach(window);
        window.Show();
        window.IsMinimizeButtonVisible = canMinimize;
        var minimizeButton = caption.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Name == "PART_MinimizeButton");
        Assert.NotNull(minimizeButton);
        Assert.Equal(expectedVisibility, minimizeButton.IsVisible);
        window.IsMinimizeButtonVisible = !canMinimize;
        Assert.Equal(!expectedVisibility, minimizeButton.IsVisible);
    }

    [AvaloniaTheory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void UrsaWindow_IsCloseButtonVisible_Should_Control_CloseButton_Visibility(bool isVisible,
        bool expectedVisibility)
    {
        var window = new UrsaWindow();
        var caption = new CaptionButtons();
        window.Content = caption;
        caption.Attach(window);
        window.Show();
        window.IsCloseButtonVisible = isVisible;
        var closeButton = caption.GetVisualDescendants().OfType<Button>()
                                 .FirstOrDefault(b => b.Name == "PART_CloseButton");
        Assert.NotNull(closeButton);
        Assert.Equal(expectedVisibility, closeButton.IsVisible);
        window.IsCloseButtonVisible = !isVisible;
        Assert.Equal(!expectedVisibility, closeButton.IsVisible);
    }
    
    [AvaloniaTheory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void UrsaWindow_IsFullScreenButtonVisible_Should_Control_FullScreenButton_Visibility
        (bool isVisible,
        bool expectedVisibility)
    {
        var window = new UrsaWindow();
        var caption = new CaptionButtons();
        window.Content = caption;
        caption.Attach(window);
        window.Show();
        window.IsFullScreenButtonVisible = isVisible;
        var fullScreenButton = caption.GetVisualDescendants().OfType<Button>()
                                 .FirstOrDefault(b => b.Name == "PART_FullScreenButton");
        Assert.NotNull(fullScreenButton);
        Assert.Equal(expectedVisibility, fullScreenButton.IsVisible);
        window.IsFullScreenButtonVisible = !isVisible;
        Assert.Equal(!expectedVisibility, fullScreenButton.IsVisible);
    }

    [AvaloniaTheory]
    [InlineData(WindowState.Normal)]
    [InlineData(WindowState.Maximized)]
    public void CaptionButtons_ToggleFullScreen_Should_Set_WindowState(WindowState initialState)
    {
        var window = new UrsaWindow();
        var caption = new CaptionButtons();
        window.Content = caption;
        caption.Attach(window);
        window.Show();
        window.WindowState = initialState;
        var fullScreenButton = caption.GetVisualDescendants().OfType<Button>()
                                      .FirstOrDefault(b => b.Name == "PART_FullScreenButton");
        Assert.NotNull(fullScreenButton);
        fullScreenButton.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Button.ClickEvent));
        Assert.Equal(WindowState.FullScreen, window.WindowState);
        fullScreenButton.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Button.ClickEvent));
        Assert.Equal(initialState, window.WindowState);
    }
    
    [AvaloniaTheory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void Window_CanMaximize_Should_Update_RestoreButton_Visibility(bool canMaximize, bool expectedVisibility)
    {
        var window = new UrsaWindow();
        var caption = new CaptionButtons();
        window.Content = caption;
        caption.Attach(window);
        window.Show();
        window.CanMaximize = canMaximize;
        var restoreButton = caption.GetVisualDescendants().OfType<Button>()
                                   .FirstOrDefault(b => b.Name == "PART_RestoreButton");
        Assert.NotNull(restoreButton);
        Assert.Equal(expectedVisibility, restoreButton.IsVisible);
    }
    
    [AvaloniaTheory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void Window_CanMinimize_Should_Update_MinimizeButton_Visibility(bool canMinimize , bool expectedVisibility)
    {
        var window = new UrsaWindow();
        var caption = new CaptionButtons();
        window.Content = caption;
        caption.Attach(window);
        window.Show();
        window.CanMinimize = canMinimize;
        var minimizeButton = caption.GetVisualDescendants().OfType<Button>()
                                   .FirstOrDefault(b => b.Name == "PART_MinimizeButton");
        Assert.NotNull(minimizeButton);
        Assert.Equal(expectedVisibility, minimizeButton.IsVisible);
    }
}