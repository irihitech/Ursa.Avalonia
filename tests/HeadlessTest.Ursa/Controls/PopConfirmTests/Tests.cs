using Avalonia.Controls;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Headless.XUnit;
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
}
