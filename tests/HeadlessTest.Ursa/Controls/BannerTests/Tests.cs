using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.BannerTests;

public class Tests
{
    [AvaloniaFact]
    public void Click_On_Banner_Close_Button_Should_Hide_Banner()
    {
        // Arrange
        var window = new Window();
        var banner = new UrsaControls.Banner()
        {
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
            Width = 300,
            Height = 100,
            CanClose = true
        };
        window.Content = banner;
        window.Show();
        
        Assert.True(banner.IsVisible);

        var closeButton = banner.GetTemplateChildren().OfType<Button>()
                                .FirstOrDefault(a => a.Name == UrsaControls.Banner.PART_CloseButton);

        Assert.NotNull(closeButton);
        
        Assert.True(closeButton.IsVisible);

        var clickPosition = closeButton.Bounds.Center;
        clickPosition = new Point(clickPosition.X + 20, clickPosition.Y + 20);

        // Act
        window.MouseDown(clickPosition, MouseButton.Left);
        window.MouseUp(clickPosition, MouseButton.Left);
        
        // Assert
        Assert.False(banner.IsVisible);
    }
    
    [AvaloniaFact]
    public void Click_On_Banner_Does_Nothing_If_Cannot_Close()
    {
        // Arrange
        var window = new Window();
        var banner = new UrsaControls.Banner()
        {
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
            Width = 300,
            Height = 100,
            CanClose = false
        };
        window.Content = banner;
        window.Show();
        
        Assert.True(banner.IsVisible);

        var closeButton = banner.GetTemplateChildren().OfType<Button>()
                                .FirstOrDefault(a => a.Name == UrsaControls.Banner.PART_CloseButton);

        Assert.NotNull(closeButton);
        
        Assert.False(closeButton.IsVisible);

        var clickPosition = closeButton.Bounds.Center;
        clickPosition = new Point(clickPosition.X + 20, clickPosition.Y + 20);

        // Act
        window.MouseDown(clickPosition, MouseButton.Left);
        window.MouseUp(clickPosition, MouseButton.Left);
        
        // Assert
        Assert.True(banner.IsVisible);
    }
}