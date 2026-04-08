using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.NavMenuTests.InitiallyCollapsedTests;

public class Tests
{
    /// <summary>
    /// Verifies that when a NavMenu starts with IsHorizontalCollapsed=true,
    /// top-level items with sub-menus are accessible and report the correct item counts.
    /// </summary>
    [AvaloniaFact]
    public void NavMenu_InitiallyCollapsed_TopLevel_Items_Are_Accessible()
    {
        var window = new Window { Width = 400, Height = 400 };
        var view = new TestView();
        window.Content = view;
        window.Show();

        var menu = view.FindControl<NavMenu>("Menu");
        Assert.NotNull(menu);
        Assert.True(menu.IsHorizontalCollapsed);

        // Top-level items should be accessible directly from the menu
        var topItem1 = menu.ContainerFromIndex(0) as NavMenuItem;
        var topItem2 = menu.ContainerFromIndex(1) as NavMenuItem;
        var topItem3 = menu.ContainerFromIndex(2) as NavMenuItem;

        Assert.NotNull(topItem1);
        Assert.NotNull(topItem2);
        Assert.NotNull(topItem3);

        Assert.Equal(2, topItem1.ItemCount);
        Assert.Equal(1, topItem2.ItemCount);
        Assert.Equal(0, topItem3.ItemCount);
    }

    /// <summary>
    /// Verifies that sub-menu items of an initially-collapsed NavMenu are realized
    /// and accessible via ContainerFromIndex.
    /// </summary>
    [AvaloniaFact]
    public void NavMenu_InitiallyCollapsed_SubMenu_Items_Are_Realized()
    {
        var window = new Window { Width = 400, Height = 400 };
        var view = new TestView();
        window.Content = view;
        window.Show();

        var menu = view.FindControl<NavMenu>("Menu");
        Assert.NotNull(menu);
        Assert.True(menu.IsHorizontalCollapsed);

        var topItem1 = menu.ContainerFromIndex(0) as NavMenuItem;
        Assert.NotNull(topItem1);
        Assert.Equal(2, topItem1.ItemCount);

        // Sub-items should be realized even when the menu starts collapsed
        var subItem1 = topItem1.ContainerFromIndex(0) as NavMenuItem;
        var subItem2 = topItem1.ContainerFromIndex(1) as NavMenuItem;

        Assert.NotNull(subItem1);
        Assert.NotNull(subItem2);
    }

    /// <summary>
    /// Verifies that expanding a NavMenu that was initially collapsed makes
    /// sub-menu items visible in the normal items panel.
    /// </summary>
    [AvaloniaFact]
    public void NavMenu_InitiallyCollapsed_ExpandMenu_SubItems_Become_Available()
    {
        var window = new Window { Width = 400, Height = 400 };
        var view = new TestView();
        window.Content = view;
        window.Show();

        var menu = view.FindControl<NavMenu>("Menu");
        Assert.NotNull(menu);
        Assert.True(menu.IsHorizontalCollapsed);

        var topItem1 = menu.ContainerFromIndex(0) as NavMenuItem;
        Assert.NotNull(topItem1);

        // Expand the menu
        menu.SetCurrentValue(NavMenu.IsHorizontalCollapsedProperty, false);

        // After expanding, sub-items should be accessible via ContainerFromIndex
        var subItem1 = topItem1.ContainerFromIndex(0) as NavMenuItem;
        var subItem2 = topItem1.ContainerFromIndex(1) as NavMenuItem;

        Assert.NotNull(subItem1);
        Assert.NotNull(subItem2);
    }

    /// <summary>
    /// Verifies that toggling between collapsed and expanded states keeps
    /// sub-menu item counts consistent and accessible after expand.
    /// </summary>
    [AvaloniaFact]
    public void NavMenu_Toggle_Collapsed_Expanded_ItemCount_Consistent()
    {
        var window = new Window { Width = 400, Height = 400 };
        var view = new TestView();
        window.Content = view;
        window.Show();

        var menu = view.FindControl<NavMenu>("Menu");
        Assert.NotNull(menu);

        var topItem1 = menu.ContainerFromIndex(0) as NavMenuItem;
        Assert.NotNull(topItem1);

        // Initially collapsed - sub-items are realized and have correct item count
        Assert.True(menu.IsHorizontalCollapsed);
        Assert.Equal(2, topItem1.ItemCount);
        Assert.NotNull(topItem1.ContainerFromIndex(0));
        Assert.NotNull(topItem1.ContainerFromIndex(1));

        // Expand - items remain accessible
        menu.SetCurrentValue(NavMenu.IsHorizontalCollapsedProperty, false);
        Assert.False(menu.IsHorizontalCollapsed);
        Assert.Equal(2, topItem1.ItemCount);
        Assert.NotNull(topItem1.ContainerFromIndex(0));
        Assert.NotNull(topItem1.ContainerFromIndex(1));

        // Collapse again - item count remains correct
        menu.SetCurrentValue(NavMenu.IsHorizontalCollapsedProperty, true);
        Assert.True(menu.IsHorizontalCollapsed);
        Assert.Equal(2, topItem1.ItemCount);
    }

    /// <summary>
    /// Verifies that data-bound sub-menu items are populated when the NavMenu
    /// is initially collapsed.
    /// </summary>
    [AvaloniaFact]
    public void NavMenu_InitiallyCollapsed_DataBound_SubMenuItems_Are_Populated()
    {
        var window = new Window { Width = 400, Height = 400 };
        var view = new DataBoundTestView();
        window.Content = view;
        window.Show();

        var menu = view.FindControl<NavMenu>("Menu");
        Assert.NotNull(menu);
        Assert.True(menu.IsHorizontalCollapsed);

        var topItem1 = menu.ContainerFromIndex(0) as NavMenuItem;
        var topItem2 = menu.ContainerFromIndex(1) as NavMenuItem;
        var topItem3 = menu.ContainerFromIndex(2) as NavMenuItem;

        Assert.NotNull(topItem1);
        Assert.NotNull(topItem2);
        Assert.NotNull(topItem3);

        // Sub-items should have been populated even when initially collapsed
        Assert.Equal(2, topItem1.ItemCount);
        Assert.Equal(1, topItem2.ItemCount);
        Assert.Equal(0, topItem3.ItemCount);

        // Sub-item containers should be realized even in the collapsed state
        Assert.NotNull(topItem1.ContainerFromIndex(0));
        Assert.NotNull(topItem1.ContainerFromIndex(1));
        Assert.NotNull(topItem2.ContainerFromIndex(0));
    }

    /// <summary>
    /// Verifies that selection works correctly after expanding a NavMenu that
    /// was initially collapsed.
    /// </summary>
    [AvaloniaFact]
    public void NavMenu_InitiallyCollapsed_ExpandThenSelect_Works()
    {
        var window = new Window { Width = 400, Height = 400 };
        var view = new TestView();
        window.Content = view;
        window.Show();

        var menu = view.FindControl<NavMenu>("Menu");
        Assert.NotNull(menu);

        // Expand the menu
        menu.SetCurrentValue(NavMenu.IsHorizontalCollapsedProperty, false);

        // Expand the first item's sub-menu
        var topItem1 = menu.ContainerFromIndex(0) as NavMenuItem;
        Assert.NotNull(topItem1);
        topItem1.SetCurrentValue(NavMenuItem.IsVerticalCollapsedProperty, false);

        // Select the leaf item "MenuItem3"
        var item3 = view.FindControl<NavMenuItem>("MenuItem3");
        Assert.NotNull(item3);

        var point = item3.TranslatePoint(new Avalonia.Point(0, 0), window);
        Assert.NotNull(point);

        window.MouseDown(new Avalonia.Point(point.Value.X + 10, point.Value.Y + 10), Avalonia.Input.MouseButton.Left);
        window.MouseUp(new Avalonia.Point(point.Value.X + 10, point.Value.Y + 10), Avalonia.Input.MouseButton.Left);

        Assert.Equal(item3, menu.SelectedItem);
    }
}
