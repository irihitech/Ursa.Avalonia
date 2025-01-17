using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Headless.XUnit;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.BadgeTests;

public class BadgeTests
{
    [AvaloniaFact]
    public void Badge_Container_Should_DisAppear_If_Header_Is_Null_And_Dot_Equals_False()
    {
        // Arrange
        var window = new Window();
        var badge = new UrsaControls.Badge
        {
            Header = string.Empty,
            Dot = true
        };
        window.Content = badge;
        window.Show();

        Assert.True(badge.IsVisible);

        var badgeContainer = badge.GetTemplateChildren().OfType<Border>()
            .FirstOrDefault(a => a.Name == UrsaControls.Badge.PART_BadgeContainer);

        Assert.NotNull(badgeContainer);

        Assert.True(badgeContainer.IsVisible);

        // Act
        badge.Header = null;
        // Assert
        Assert.True(badgeContainer.IsVisible);

        // Act
        badge.Dot = false;
        // Assert
        Assert.False(badgeContainer.IsVisible);
    }

    [AvaloniaFact]
    public void Badge_Container_Should_Overflow_If_Number_Larger_Than_OverflowCount()
    {
        // Arrange
        var window = new Window();
        var badge = new UrsaControls.Badge
        {
            Header = 0,
            OverflowCount = 10
        };
        window.Content = badge;
        window.Show();

        Assert.True(badge.IsVisible);

        var header = badge.GetTemplateChildren().OfType<ContentPresenter>()
            .FirstOrDefault(a => a.Name == UrsaControls.Badge.PART_HeaderPresenter);

        Assert.NotNull(header);
        Assert.Equal(header.Content, 0);

        // Act
        badge.Header = 10;

        // Assert
        Assert.Equal(header.Content, 10);

        // Act
        badge.Header = 11;
        Assert.Equal(header.Content, "10+");

        // Act
        badge.OverflowCount = 100;
        Assert.Equal(header.Content, 11);
    }
}