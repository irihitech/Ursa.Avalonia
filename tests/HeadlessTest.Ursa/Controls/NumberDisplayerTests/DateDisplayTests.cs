using System.Reflection;
using Avalonia.Animation;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.NumberDisplayerTests;

public class DateDisplayTests
{
    [Fact]
    public void DateAnimator_Should_Clamp_Progress_At_DateTime_MaxValue()
    {
        var animatorType = typeof(DateDisplay).GetNestedType("DateAnimator", BindingFlags.NonPublic);
        Assert.NotNull(animatorType);
        var animator = Activator.CreateInstance(animatorType);
        Assert.NotNull(animator);

        var interpolate = animatorType.GetMethod(nameof(InterpolatingAnimator<DateTime>.Interpolate));
        Assert.NotNull(interpolate);

        var oldValue = DateTime.MaxValue.AddDays(-1);
        var result = (DateTime)interpolate.Invoke(animator, [1.0000000000000002, oldValue, DateTime.MaxValue])!;

        Assert.Equal(DateTime.MaxValue, result);
    }
}
