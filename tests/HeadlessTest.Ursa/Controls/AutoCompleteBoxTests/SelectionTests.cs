using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using HeadlessTest.Ursa.TestHelpers;
using UrsaControl = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.AutoCompleteBoxTests;

public class SelectionTests
{
    private static UrsaControl.AutoCompleteBox CreateBox(params string[] items) => new()
    {
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Top,
        Width = 200,
        Height = 32,
        ItemsSource = items,
    };

    private static ListBox? GetDropDownList(UrsaControl.AutoCompleteBox box)
    {
        var popup = box.GetTemplateChildOfType<Popup>("PART_Popup");
        return popup?.GetLogicalDescendants().OfType<ListBox>().FirstOrDefault();
    }

    private static void RaisePointerReleased(Control target, Visual root, Point position)
    {
        var pointer = new Pointer(Pointer.GetNextFreeId(), PointerType.Mouse, true);
        var properties = new PointerPointProperties(RawInputModifiers.None, PointerUpdateKind.LeftButtonReleased);
        target.RaiseEvent(new PointerReleasedEventArgs(
            target, pointer, root, position, 0, properties, KeyModifiers.None, MouseButton.Left));
    }

    [AvaloniaFact]
    public void Selecting_Item_In_Dropdown_Should_Keep_Selection_And_Close_Dropdown()
    {
        var items = new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo" };
        var window = new Window();
        var box = CreateBox(items);
        window.Content = box;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        box.Focus();
        Dispatcher.UIThread.RunJobs();
        Assert.True(box.IsDropDownOpen);

        var list = GetDropDownList(box);
        Assert.NotNull(list);
        window.UpdateLayout();
        Dispatcher.UIThread.RunJobs();

        // Simulate a mouse click on the third item, mirroring the built-in selection
        // adapter: the press moves focus into the popup and updates the ListBox
        // selection, the release commits the selection (closes the dropdown and
        // refocuses the textbox).
        // Note: with Avalonia >= 12.1.6 the dropdown stays open while focus is inside
        // it and the click is committed on release; older versions close it as soon as
        // focus leaves the textbox and commit the selection at press time. Either way
        // the final state below must hold.
        var container = list.GetLogicalDescendants().OfType<ListBoxItem>()
            .FirstOrDefault(c => Equals(c.Content, "Charlie"));
        Assert.NotNull(container);
        // Compute the click position while the popup is still attached: on Avalonia < 12.1.6
        // the dropdown closes as soon as focus moves into it.
        var position = container.TranslatePoint(
            new Point(container.Bounds.Width / 2, container.Bounds.Height / 2), window);
        Assert.NotNull(position);

        container.Focus();
        Dispatcher.UIThread.RunJobs();

        list.SelectedItem = "Charlie";
        Dispatcher.UIThread.RunJobs();
        Assert.Equal("Charlie", box.SelectedItem);
        Assert.Equal("Charlie", box.Text);

        RaisePointerReleased(container, window, position.Value);
        Dispatcher.UIThread.RunJobs();

        Assert.False(box.IsDropDownOpen);
        Assert.Equal("Charlie", box.SelectedItem);
        Assert.Equal("Charlie", box.Text);
    }
}
