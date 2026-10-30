using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
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
        Assert.DoesNotContain(":interactive", card.Classes);
        // Plain rows stay focusable so keyboard navigation does not skip them.
        Assert.True(card.Focusable);
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
        Assert.True(card.Focusable);
    }

    [AvaloniaFact]
    public void SettingsCard_PlainRow_CanFocus()
    {
        // A row without Detail, Command or ShowChevron must still be reachable by
        // keyboard navigation.
        var card = new SettingsCard { Content = "Plain" };
        var window = new Window { Content = card };
        window.Show();

        card.Focus();
        Assert.True(card.IsFocused);
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
    public void SettingsCard_CardsInDetail_Works()
    {
        var innerCheckBox = new CheckBox();
        var innerRow = new SettingsCard { Content = "Inner row" };
        innerRow.RightContent = innerCheckBox;
        var detailPanel = new StackPanel();
        detailPanel.Children.Add(innerRow);
        var outer = new SettingsCard { Content = "Outer", Detail = detailPanel };
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
    public void SettingsCard_CardsInDetail_RenderFlat()
    {
        var first = new SettingsCard { Content = "First nested" };
        var last = new SettingsCard { Content = "Last nested" };
        var detailPanel = new StackPanel();
        detailPanel.Children.Add(first);
        detailPanel.Children.Add(last);
        var outer = new SettingsCard { Content = "Outer", Detail = detailPanel, IsExpanded = true };
        var outerGroup = new SettingsCardGroup();
        outerGroup.Items.Add(outer);
        var window = new Window { Content = outerGroup };
        window.Show();
        window.UpdateLayout();

        // The detail presenter has no margin, so nested cards span the card edge to edge.
        Assert.Equal(new Avalonia.Thickness(0), first.Margin);

        // Cards are square-cornered; the group card carries the radius.
        Assert.Equal(new Avalonia.CornerRadius(0), first.CornerRadius);
        Assert.Equal(new Avalonia.CornerRadius(0), last.CornerRadius);

        // Separators between nested rows, none after the last one.
        Assert.True(first.GetTemplateChildOfType<Border>("PART_Separator")!.IsVisible);
        Assert.False(last.GetTemplateChildOfType<Border>("PART_Separator")!.IsVisible);
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

        var detailPanel = new StackPanel();
        detailPanel.Children.Add(new SettingsCard { Content = "Light", RightContent = radioA });
        detailPanel.Children.Add(new SettingsCard { Content = "Dark", RightContent = radioB });
        var outer = new SettingsCard { Content = "Theme", Detail = detailPanel, IsExpanded = true };
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

    [AvaloniaFact]
    public void SettingsCard_ClickFromEmbeddedHeaderControl_DoesNotToggle()
    {
        // Button.ClickEvent bubbles, so a click on a button placed in RightContent of an
        // expandable row must belong to that button, not toggle the row.
        var embedded = new Button { Content = "Edit" };
        var card = new SettingsCard { Content = "Row", Detail = new StackPanel(), RightContent = embedded };
        var window = new Window { Content = card };
        window.Show();

        embedded.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Assert.False(card.IsExpanded);

        card.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Assert.True(card.IsExpanded);
    }

    [AvaloniaFact]
    public void SettingsCard_EnterInDetailEditor_DoesNotCollapseRow()
    {
        var box = new TextBox();
        var card = new SettingsCard { Content = "Row", Detail = box, IsExpanded = true };
        var window = new Window { Content = card };
        window.Show();
        window.UpdateLayout();

        box.Focus();
        box.RaiseEvent(new KeyEventArgs { RoutedEvent = InputElement.KeyDownEvent, Key = Key.Enter });
        box.RaiseEvent(new KeyEventArgs { RoutedEvent = InputElement.KeyUpEvent, Key = Key.Enter });
        Assert.True(card.IsExpanded);

        box.RaiseEvent(new KeyEventArgs { RoutedEvent = InputElement.KeyDownEvent, Key = Key.Space });
        box.RaiseEvent(new KeyEventArgs { RoutedEvent = InputElement.KeyUpEvent, Key = Key.Space });
        Assert.True(card.IsExpanded);
    }

    [AvaloniaFact]
    public void SettingsCard_EnterOnFocusedCard_StillToggles()
    {
        var card = new SettingsCard { Content = "Row", Detail = new StackPanel() };
        var window = new Window { Content = card };
        window.Show();

        card.Focus();
        card.RaiseEvent(new KeyEventArgs { RoutedEvent = InputElement.KeyDownEvent, Key = Key.Enter });
        card.RaiseEvent(new KeyEventArgs { RoutedEvent = InputElement.KeyUpEvent, Key = Key.Enter });
        Assert.True(card.IsExpanded);
    }

    [AvaloniaFact]
    public void SettingsCard_MouseClickOnHeader_TogglesExpansion()
    {
        var card = new SettingsCard { Content = "Row", Detail = new StackPanel() };
        var window = new Window { Content = card };
        window.Show();
        window.UpdateLayout();

        // Click the header border (the card itself can stretch far beyond it).
        var header = card.GetTemplateChildOfType<Border>("PART_Root");
        Assert.NotNull(header);
        var pos = header!.TranslatePoint(new Point(header.Bounds.Width / 2, header.Bounds.Height / 2), window);
        Assert.NotNull(pos);
        window.MouseDown(pos.Value, MouseButton.Left);
        window.MouseUp(pos.Value, MouseButton.Left);
        Assert.True(card.IsExpanded);
    }

    [AvaloniaFact]
    public void SettingsCard_DetailEditorPointerPress_GetsFocusAndKeepsExpansion()
    {
        // Models an editor that leaves pointer presses unhandled (like KeyGestureInput):
        // pressing it must focus it, and neither the press nor the release may collapse
        // the row. A press on plain detail content behaves the same.
        var editor = new Border
        {
            Focusable = true,
            Background = Brushes.Transparent,
            Width = 100,
            Height = 24,
        };
        var plain = new TextBlock { Text = "Plain detail content" };
        var detail = new StackPanel();
        detail.Children.Add(editor);
        detail.Children.Add(plain);
        var card = new SettingsCard { Content = "Row", Detail = detail, IsExpanded = true };
        var window = new Window { Content = card };
        window.Show();
        window.UpdateLayout();

        var editorPos = editor.TranslatePoint(new Point(10, 12), window);
        Assert.NotNull(editorPos);
        window.MouseDown(editorPos.Value, MouseButton.Left);
        window.MouseUp(editorPos.Value, MouseButton.Left);
        Assert.True(card.IsExpanded);
        Assert.True(editor.IsFocused);

        var plainPos = plain.TranslatePoint(new Point(4, 4), window);
        Assert.NotNull(plainPos);
        window.MouseDown(plainPos.Value, MouseButton.Left);
        window.MouseUp(plainPos.Value, MouseButton.Left);
        Assert.True(card.IsExpanded);
    }

    [AvaloniaFact]
    public void SettingsCard_HeaderEditorPointerPress_FocusesEditorAndDoesNotToggle()
    {
        // The same unhandled-editor rule applies to the header area: pressing an editor
        // in RightContent focuses it instead of arming the row button.
        var editor = new Border
        {
            Focusable = true,
            Background = Brushes.Transparent,
            Width = 60,
            Height = 20,
        };
        var card = new SettingsCard { Content = "Row", Detail = new StackPanel(), RightContent = editor, IsExpanded = true };
        var window = new Window { Content = card };
        window.Show();
        window.UpdateLayout();

        var pos = editor.TranslatePoint(new Point(5, 10), window);
        Assert.NotNull(pos);
        window.MouseDown(pos.Value, MouseButton.Left);
        window.MouseUp(pos.Value, MouseButton.Left);

        Assert.True(editor.IsFocused);
        Assert.True(card.IsExpanded);
    }
}
