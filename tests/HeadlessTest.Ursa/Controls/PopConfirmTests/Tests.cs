using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using HeadlessTest.Ursa.TestHelpers;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.PopConfirmTests;

public class Tests
{
    [AvaloniaFact]
    public void Setting_Placement_To_Custom_Should_Coerce_To_Center()
    {
        var popConfirm = new UrsaControls.PopConfirm
        {
            Placement = PlacementMode.Custom
        };

        Assert.Equal(PlacementMode.Center, popConfirm.Placement);
    }

    [AvaloniaFact]
    public void Setting_Placement_To_Non_Custom_Should_Be_Preserved()
    {
        var popConfirm = new UrsaControls.PopConfirm
        {
            Placement = PlacementMode.Top
        };

        Assert.Equal(PlacementMode.Top, popConfirm.Placement);
    }

    [AvaloniaFact]
    public void Popup_Should_Be_Placed_Relative_To_Trigger_Control()
    {
        var trigger = new Button { Content = "Open" };
        var popConfirm = new UrsaControls.PopConfirm
        {
            Width = 500,
            Content = trigger
        };
        var window = new Window { Content = popConfirm };

        window.Show();
        Dispatcher.UIThread.RunJobs();

        var popup = popConfirm.GetTemplateChildOfType<Popup>(UrsaControls.PopConfirm.PART_Popup);

        Assert.NotNull(popup);
        Assert.Same(trigger, popup.PlacementTarget);

        window.Close();
    }
}
