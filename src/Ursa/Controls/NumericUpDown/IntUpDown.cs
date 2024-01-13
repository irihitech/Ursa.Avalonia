using Avalonia.Utilities;

namespace Ursa.Controls;

public class IntUpDown: NumericUpDownBase<int>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static IntUpDown()
    {
        MaximumProperty.OverrideDefaultValue<IntUpDown>(int.MaxValue);
        StepProperty.OverrideDefaultValue<IntUpDown>(1);
    }
    
    protected override void Increase()
    {
        Value += Step;
    }

    protected override void Decrease()
    {
        Value -= Step;
    }
    
    protected override void UpdateTextToValue(string x)
    {
        if (int.TryParse(x, out var value))
        {
            Value = value;
        }
    }

    protected override bool CommitInput()
    {
        // throw new NotImplementedException();
        return true;
    }

    protected override void SyncTextAndValue()
    {
        // throw new NotImplementedException();
    }

    protected override int Clamp()
    {
        return MathUtilities.Clamp(Value, Maximum, Minimum);
    }
}