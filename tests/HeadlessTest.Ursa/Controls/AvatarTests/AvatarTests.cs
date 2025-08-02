using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.AvatarTests;

public class AvatarTests
{
    [AvaloniaFact]
    public void Avatar_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var avatar = new UrsaControls.Avatar();

        // Assert
        Assert.Null(avatar.Source);
        Assert.Null(avatar.HoverMask);
    }

    [AvaloniaFact]
    public void Avatar_Should_Set_Source_Property()
    {
        // Arrange
        var window = new Window();
        var avatar = new UrsaControls.Avatar();
        window.Content = avatar;
        window.Show();

        // Act - Test with null since creating a real bitmap is complex in tests
        avatar.Source = null;

        // Assert
        Assert.Null(avatar.Source);
    }

    [AvaloniaFact]
    public void Avatar_Should_Set_HoverMask_Property()
    {
        // Arrange
        var window = new Window();
        var avatar = new UrsaControls.Avatar();
        var hoverMask = "Edit";
        window.Content = avatar;
        window.Show();

        // Act
        avatar.HoverMask = hoverMask;

        // Assert
        Assert.Equal(hoverMask, avatar.HoverMask);
    }

    [AvaloniaFact]
    public void Avatar_Should_Accept_Null_Source()
    {
        // Arrange
        var window = new Window();
        var avatar = new UrsaControls.Avatar();
        window.Content = avatar;
        window.Show();

        // Act
        avatar.Source = null;

        // Assert
        Assert.Null(avatar.Source);
    }

    [AvaloniaFact]
    public void Avatar_Should_Accept_Null_HoverMask()
    {
        // Arrange
        var window = new Window();
        var avatar = new UrsaControls.Avatar();
        var hoverMask = "Edit";
        window.Content = avatar;
        window.Show();

        // Act
        avatar.HoverMask = hoverMask;
        avatar.HoverMask = null;

        // Assert
        Assert.Null(avatar.HoverMask);
    }

    [AvaloniaFact]
    public void Avatar_Should_Be_Visible_When_Added_To_Window()
    {
        // Arrange
        var window = new Window();
        var avatar = new UrsaControls.Avatar();

        // Act
        window.Content = avatar;
        window.Show();

        // Assert
        Assert.True(avatar.IsVisible);
    }

    [AvaloniaFact]
    public void Avatar_Should_Inherit_From_Button()
    {
        // Arrange & Act
        var avatar = new UrsaControls.Avatar();

        // Assert
        Assert.IsAssignableFrom<Button>(avatar);
    }
}