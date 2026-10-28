using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using HeadlessTest.Ursa.TestHelpers;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.SettingsCardTests;

public class SettingsCardTest
{
    [AvaloniaFact]
    public void SettingsCard_DefaultValues()
    {
        var card = new SettingsCard();
        Assert.False(card.IsExpanded);
        Assert.False(card.ShowChevron);
        Assert.Null(card.Detail);
        Assert.False(card.Classes.Contains(":interactive"));
        Assert.False(card.Focusable);
    }

    [AvaloniaFact]
    public void SettingsCard_DetailSet_BecomesInteractiveWithPseudoClasses()
    {
        var card = new SettingsCard();
        card.Detail = new StackPanel();

        Assert.Contains(":has-detail", card.Classes);
        Assert.Contains(":interactive", card.Classes);
        Assert.True(card.Focusable);
    }

    [AvaloniaFact]
    public void SettingsCard_DetailCleared_CollapsesAndLosesInteractivity()
    {
        var card = new SettingsCard { Detail = new StackPanel(), IsExpanded = true };

        card.Detail = null;

        Assert.False(card.IsExpanded);
        Assert.DoesNotContain(":has-detail", card.Classes);
        Assert.DoesNotContain(":interactive", card.Classes);
        Assert.False(card.Focusable);
    }

    [AvaloniaFact]
    public void SettingsCard_CommandOrChevron_MakesInteractive()
    {
        var card = new SettingsCard();
        Assert.DoesNotContain(":interactive", card.Classes);

        card.ShowChevron = true;
        Assert.Contains(":interactive", card.Classes);

        card.ShowChevron = false;
        Assert.DoesNotContain(":interactive", card.Classes);
    }

    [AvaloniaFact]
    public void SettingsCard_Click_TogglesExpanded()
    {
        var card = new SettingsCard { Detail = new StackPanel() };
        var window = new Window { Content = card };
        window.Show();

        card.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Assert.True(card.IsExpanded);
        Assert.Contains(":expanded", card.Classes);

        card.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Assert.False(card.IsExpanded);
        Assert.DoesNotContain(":expanded", card.Classes);
    }

    [AvaloniaFact]
    public void SettingsCard_ClickWithoutDetail_DoesNothing()
    {
        var card = new SettingsCard();
        var window = new Window { Content = card };
        window.Show();

        card.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Assert.False(card.IsExpanded);
    }

    [AvaloniaFact]
    public void SettingsCard_TemplateApplies_DetailHostWired()
    {
        var card = new SettingsCard { Detail = new StackPanel() };
        var window = new Window { Content = card };
        window.Show();

        var host = card.GetTemplateChildOfType<Border>("PART_DetailHost");
        Assert.NotNull(host);
        Assert.False(host!.IsVisible);

        card.IsExpanded = true;
        Assert.True(host.IsVisible);
    }

    [AvaloniaFact]
    public void SettingsCard_IsExpandedBindsTwoWayByDefault()
    {
        var mode = SettingsCard.IsExpandedProperty.GetMetadata<SettingsCard>().DefaultBindingMode;
        Assert.Equal(Avalonia.Data.BindingMode.TwoWay, mode);
    }

    [AvaloniaFact]
    public void SettingsCard_NestedCardGroupInDetail_Works()
    {
        var innerCheckBox = new CheckBox();
        var innerRow = new SettingsCard { Content = "Inner row" };
        innerRow.RightContent = innerCheckBox;
        var innerGroup = new SettingsCardGroup();
        innerGroup.Items.Add(innerRow);
        var outer = new SettingsCard { Content = "Outer", Detail = innerGroup };
        var outerGroup = new SettingsCardGroup();
        outerGroup.Items.Add(outer);
        var window = new Window { Content = outerGroup };
        window.Show();

        outer.IsExpanded = true;
        Assert.True(outer.IsExpanded);
        Assert.Contains(":expanded", outer.Classes);
        // Realize the now-visible detail subtree (layout is not driven automatically in tests).
        window.UpdateLayout();

        var host = outer.GetTemplateChildOfType<Border>(SettingsCard.PART_DetailHost);
        Assert.NotNull(host);
        Assert.True(host!.IsVisible);

        // The nested card renders inside the detail host, and interacting with the
        // nested content leaves the outer row expanded.
        Assert.Contains(innerRow, host.GetVisualDescendants());
        innerCheckBox.IsChecked = true;
        Assert.True(innerCheckBox.IsChecked);
        Assert.True(outer.IsExpanded);

        // Toggling again still collapses normally.
        outer.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Assert.False(outer.IsExpanded);
        Assert.False(host.IsVisible);
    }

    [AvaloniaFact]
    public void SettingsCard_NestedGroupInDetail_RendersFlat()
    {
        var first = new SettingsCard { Content = "First nested" };
        var last = new SettingsCard { Content = "Last nested" };
        var innerGroup = new SettingsCardGroup();
        innerGroup.Items.Add(first);
        innerGroup.Items.Add(last);
        var outer = new SettingsCard { Content = "Outer", Detail = innerGroup, IsExpanded = true };
        var outerGroup = new SettingsCardGroup();
        outerGroup.Items.Add(outer);
        var window = new Window { Content = outerGroup };
        window.Show();
        window.UpdateLayout();

        // The nested group cancels the detail presenter margins so rows span the card edge to edge.
        Assert.Equal(new Avalonia.Thickness(-48, -12, -16, -12), innerGroup.Margin);

        // Card chrome removed: transparent background, no border.
        var cardBorder = innerGroup.GetTemplateChildOfType<Border>("PART_Root");
        Assert.NotNull(cardBorder);
        Assert.Equal(Avalonia.Media.Brushes.Transparent, cardBorder!.Background);
        Assert.Equal(new Avalonia.Thickness(0), cardBorder.BorderThickness);

        // Nested rows keep square top corners (a header row sits above); the last nested row
        // closes the card with the outer bottom radius.
        Assert.Equal(new Avalonia.CornerRadius(0), first.CornerRadius);
        Assert.Equal(0, last.CornerRadius.TopLeft);
        Assert.Equal(0, last.CornerRadius.TopRight);
        Assert.Equal(outerGroup.CornerRadius.BottomLeft, last.CornerRadius.BottomLeft);
        Assert.Equal(outerGroup.CornerRadius.BottomRight, last.CornerRadius.BottomRight);
    }

    [AvaloniaFact]
    public void SettingsCard_NestedRadioButtons_NeedGroupNameToBeMutuallyExclusive()
    {
        // RadioButtons in DIFFERENT rows have different parents, so Avalonia's default
        // same-parent grouping does not apply: a shared GroupName is required for
        // mutual exclusion across rows.
        var radioA = new RadioButton { GroupName = "theme" };
        var radioB = new RadioButton { GroupName = "theme" };
        var loneA = new RadioButton();
        var loneB = new RadioButton();

        var innerGroup = new SettingsCardGroup();
        innerGroup.Items.Add(new SettingsCard { Content = "Light", RightContent = radioA });
        innerGroup.Items.Add(new SettingsCard { Content = "Dark", RightContent = radioB });
        var outer = new SettingsCard { Content = "Theme", Detail = innerGroup, IsExpanded = true };
        var group = new SettingsCardGroup();
        group.Items.Add(outer);
        group.Items.Add(new SettingsCard { Content = "Lone A", RightContent = loneA });
        group.Items.Add(new SettingsCard { Content = "Lone B", RightContent = loneB });
        var window = new Window { Content = group };
        window.Show();
        window.UpdateLayout();

        // Same GroupName: checking B unchecks A.
        radioA.IsChecked = true;
        Assert.True(radioA.IsChecked);
        radioB.IsChecked = true;
        Assert.False(radioA.IsChecked);
        Assert.True(radioB.IsChecked);

        // Without GroupName (different parents): no mutual exclusion — both stay checked.
        loneA.IsChecked = true;
        loneB.IsChecked = true;
        Assert.True(loneA.IsChecked);
        Assert.True(loneB.IsChecked);

        // The outer row stays expanded throughout.
        Assert.True(outer.IsExpanded);
    }
}
