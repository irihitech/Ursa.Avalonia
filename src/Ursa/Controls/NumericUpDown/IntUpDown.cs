namespace Ursa.Controls;

public class IntUpDown: NumericUpDownBase<int>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static IntUpDown()
    {
        MaximumProperty.OverrideDefaultValue<IntUpDown>(100);
    }
    
    protected override void Increase()
    {
        //throw new NotImplementedException();
        Value += Maximum;
    }

    protected override void Decrease()
    {
        Value -= Maximum;
    }
}