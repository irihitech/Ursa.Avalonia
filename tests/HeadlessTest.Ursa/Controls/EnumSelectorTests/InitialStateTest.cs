using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.EnumSelectorTests;

public class InitialStateTest
{
    [AvaloniaFact]
    public void Initial_State_Respected()
    {
        var window = new Window();
        var selector = new EnumSelector()
        {
            Value = HorizontalPosition.Right,
            EnumType = typeof(HorizontalPosition),
        };
        window.Content = selector;
        window.Show();
        Assert.Equal(HorizontalPosition.Right, selector.Value);
        Assert.NotNull(selector.SelectedValue);
        Assert.Equal(HorizontalPosition.Right, selector.SelectedValue?.Value);
    }
}