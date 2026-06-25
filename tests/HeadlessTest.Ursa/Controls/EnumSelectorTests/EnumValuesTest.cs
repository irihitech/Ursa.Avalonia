using System.Collections;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.EnumSelectorTests;

public class EnumValuesTest
{
    [AvaloniaFact]
    public void EnumValues_NonEmpty_FiltersEnumMembers()
    {
        var window = new Window();
        var selector = new EnumSelector
        {
            EnumType = typeof(DayOfWeek),
            EnumValues = new List<object>
            {
                DayOfWeek.Monday,
                DayOfWeek.Friday,
            },
        };
        window.Content = selector;
        window.Show();
        
        Assert.NotNull(selector.Values);
        Assert.Equal(2, selector.Values!.Count);
        Assert.Equal(DayOfWeek.Monday, selector.Values[0].Value);
        Assert.Equal(DayOfWeek.Friday, selector.Values[1].Value);
    }

    [AvaloniaFact]
    public void EnumValues_Empty_ReturnsAllEnumMembers()
    {
        var window = new Window();
        var selector = new EnumSelector
        {
            EnumType = typeof(DayOfWeek),
            EnumValues = new List<object>(),
        };
        window.Content = selector;
        window.Show();
        
        Assert.NotNull(selector.Values);
        Assert.Equal(7, selector.Values!.Count);
    }

    [AvaloniaFact]
    public void EnumValues_Null_ReturnsAllEnumMembers()
    {
        var window = new Window();
        var selector = new EnumSelector
        {
            EnumType = typeof(DayOfWeek),
            EnumValues = null,
        };
        window.Content = selector;
        window.Show();
        
        Assert.NotNull(selector.Values);
        Assert.Equal(7, selector.Values!.Count);
    }

    [AvaloniaFact]
    public void EnumValues_ValueCoerce_StillWorks()
    {
        var window = new Window();
        var selector = new EnumSelector
        {
            EnumType = typeof(DayOfWeek),
            EnumValues = new List<object>
            {
                DayOfWeek.Monday,
                DayOfWeek.Friday,
            },
            Value = DayOfWeek.Friday,
        };
        window.Content = selector;
        window.Show();
        
        Assert.Equal(DayOfWeek.Friday, selector.Value);
        Assert.NotNull(selector.SelectedValue);
        Assert.Equal(DayOfWeek.Friday, selector.SelectedValue!.Value);
    }
}
