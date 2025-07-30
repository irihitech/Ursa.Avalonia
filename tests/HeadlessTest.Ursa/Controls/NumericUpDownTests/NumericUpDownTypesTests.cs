using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.NumericUpDownTests;

/// <summary>
/// Tests for all specific numeric type implementations of NumericUpDown
/// </summary>
public class NumericUpDownTypesTests
{
    #region UInt Tests
    [AvaloniaFact]
    public void NumericUIntUpDown_Should_Initialize_With_Correct_Defaults()
    {
        var window = new Window();
        var numericUpDown = new NumericUIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(uint.MaxValue, numericUpDown.Maximum);
        Assert.Equal(uint.MinValue, numericUpDown.Minimum);
        Assert.Equal(1u, numericUpDown.Step);
    }

    [AvaloniaFact]
    public void NumericUIntUpDown_Should_Handle_Value_Operations()
    {
        var window = new Window();
        var numericUpDown = new NumericUIntUpDown
        {
            Value = 10u,
            Step = 5u,
            Minimum = 0u,
            Maximum = 100u
        };
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Equal(10u, numericUpDown.Value);
        
        // Test setting value within range
        numericUpDown.Value = 50u;
        Assert.Equal(50u, numericUpDown.Value);
        
        // Test clamping above maximum
        numericUpDown.Value = 150u;
        Assert.Equal(100u, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericUIntUpDown_Should_Parse_Text_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericUIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericUIntUpDown).GetMethod("ParseText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(parseMethod);
        
        var parameters = new object[] { "123", (uint)0 };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters);
        var parsedValue = (uint)parameters[1];
        
        Assert.True(result);
        Assert.Equal(123u, parsedValue);
    }
    #endregion

    #region Double Tests
    [AvaloniaFact]
    public void NumericDoubleUpDown_Should_Initialize_With_Correct_Defaults()
    {
        var window = new Window();
        var numericUpDown = new NumericDoubleUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(double.MaxValue, numericUpDown.Maximum);
        Assert.Equal(double.MinValue, numericUpDown.Minimum);
        Assert.Equal(1.0, numericUpDown.Step);
    }

    [AvaloniaFact]
    public void NumericDoubleUpDown_Should_Handle_Decimal_Values()
    {
        var window = new Window();
        var numericUpDown = new NumericDoubleUpDown
        {
            Value = 10.5,
            Step = 0.1,
            Minimum = 0.0,
            Maximum = 100.0
        };
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Equal(10.5, numericUpDown.Value);
        
        numericUpDown.Value = 3.14159;
        Assert.Equal(3.14159, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericDoubleUpDown_Should_Parse_Decimal_Text()
    {
        var window = new Window();
        var numericUpDown = new NumericDoubleUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericDoubleUpDown).GetMethod("ParseText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(parseMethod);
        
        var parameters = new object[] { "123.456", 0.0 };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters);
        var parsedValue = (double)parameters[1];
        
        Assert.True(result);
        Assert.Equal(123.456, parsedValue, precision: 10);
    }
    #endregion

    #region Byte Tests
    [AvaloniaFact]
    public void NumericByteUpDown_Should_Initialize_With_Correct_Defaults()
    {
        var window = new Window();
        var numericUpDown = new NumericByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(byte.MaxValue, numericUpDown.Maximum);
        Assert.Equal(byte.MinValue, numericUpDown.Minimum);
        Assert.Equal((byte)1, numericUpDown.Step);
    }

    [AvaloniaFact] 
    public void NumericByteUpDown_Should_Respect_Byte_Range()
    {
        var window = new Window();
        var numericUpDown = new NumericByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        // Test maximum value
        numericUpDown.Value = 255;
        Assert.Equal((byte)255, numericUpDown.Value);
        
        // Test minimum value
        numericUpDown.Value = 0;
        Assert.Equal((byte)0, numericUpDown.Value);
        
        // Test overflow should clamp to max - test using the Maximum property constraint
        numericUpDown.Maximum = 200;
        numericUpDown.Value = 250; // This should be clamped to Maximum
        Assert.Equal((byte)200, numericUpDown.Value);
    }
    #endregion

    #region SByte Tests
    [AvaloniaFact]
    public void NumericSByteUpDown_Should_Initialize_With_Correct_Defaults()
    {
        var window = new Window();
        var numericUpDown = new NumericSByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(sbyte.MaxValue, numericUpDown.Maximum);
        Assert.Equal(sbyte.MinValue, numericUpDown.Minimum);
        Assert.Equal((sbyte)1, numericUpDown.Step);
    }

    [AvaloniaFact]
    public void NumericSByteUpDown_Should_Handle_Negative_Values()
    {
        var window = new Window();
        var numericUpDown = new NumericSByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        numericUpDown.Value = -50;
        Assert.Equal((sbyte)(-50), numericUpDown.Value);
        
        numericUpDown.Value = 127;
        Assert.Equal((sbyte)127, numericUpDown.Value);
        
        numericUpDown.Value = -128;
        Assert.Equal((sbyte)(-128), numericUpDown.Value);
    }
    #endregion

    #region Short Tests
    [AvaloniaFact]
    public void NumericShortUpDown_Should_Initialize_With_Correct_Defaults()
    {
        var window = new Window();
        var numericUpDown = new NumericShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(short.MaxValue, numericUpDown.Maximum);
        Assert.Equal(short.MinValue, numericUpDown.Minimum);
        Assert.Equal((short)1, numericUpDown.Step);
    }

    [AvaloniaFact]
    public void NumericShortUpDown_Should_Handle_Short_Range()
    {
        var window = new Window();
        var numericUpDown = new NumericShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        numericUpDown.Value = 32767;
        Assert.Equal((short)32767, numericUpDown.Value);
        
        numericUpDown.Value = -32768;
        Assert.Equal((short)(-32768), numericUpDown.Value);
    }
    #endregion

    #region UShort Tests
    [AvaloniaFact]
    public void NumericUShortUpDown_Should_Initialize_With_Correct_Defaults()
    {
        var window = new Window();
        var numericUpDown = new NumericUShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(ushort.MaxValue, numericUpDown.Maximum);
        Assert.Equal(ushort.MinValue, numericUpDown.Minimum);
        Assert.Equal((ushort)1, numericUpDown.Step);
    }

    [AvaloniaFact]
    public void NumericUShortUpDown_Should_Handle_UShort_Range()
    {
        var window = new Window();
        var numericUpDown = new NumericUShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        numericUpDown.Value = 65535;
        Assert.Equal((ushort)65535, numericUpDown.Value);
        
        numericUpDown.Value = 0;
        Assert.Equal((ushort)0, numericUpDown.Value);
    }
    #endregion

    #region Long Tests
    [AvaloniaFact]
    public void NumericLongUpDown_Should_Initialize_With_Correct_Defaults()
    {
        var window = new Window();
        var numericUpDown = new NumericLongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(long.MaxValue, numericUpDown.Maximum);
        Assert.Equal(long.MinValue, numericUpDown.Minimum);
        Assert.Equal(1L, numericUpDown.Step);
    }

    [AvaloniaFact]
    public void NumericLongUpDown_Should_Handle_Large_Values()
    {
        var window = new Window();
        var numericUpDown = new NumericLongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var largeValue = 9223372036854775807L; // long.MaxValue
        numericUpDown.Value = largeValue;
        Assert.Equal(largeValue, numericUpDown.Value);
        
        var smallValue = -9223372036854775808L; // long.MinValue
        numericUpDown.Value = smallValue;
        Assert.Equal(smallValue, numericUpDown.Value);
    }
    #endregion

    #region ULong Tests
    [AvaloniaFact]
    public void NumericULongUpDown_Should_Initialize_With_Correct_Defaults()
    {
        var window = new Window();
        var numericUpDown = new NumericULongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(ulong.MaxValue, numericUpDown.Maximum);
        Assert.Equal(ulong.MinValue, numericUpDown.Minimum);
        Assert.Equal(1UL, numericUpDown.Step);
    }

    [AvaloniaFact]
    public void NumericULongUpDown_Should_Handle_Large_Unsigned_Values()
    {
        var window = new Window();
        var numericUpDown = new NumericULongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var largeValue = 18446744073709551615UL; // ulong.MaxValue
        numericUpDown.Value = largeValue;
        Assert.Equal(largeValue, numericUpDown.Value);
        
        numericUpDown.Value = 0UL;
        Assert.Equal(0UL, numericUpDown.Value);
    }
    #endregion

    #region Float Tests
    [AvaloniaFact]
    public void NumericFloatUpDown_Should_Initialize_With_Correct_Defaults()
    {
        var window = new Window();
        var numericUpDown = new NumericFloatUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(float.MaxValue, numericUpDown.Maximum);
        Assert.Equal(float.MinValue, numericUpDown.Minimum);
        Assert.Equal(1.0f, numericUpDown.Step);
    }

    [AvaloniaFact]
    public void NumericFloatUpDown_Should_Handle_Float_Precision()
    {
        var window = new Window();
        var numericUpDown = new NumericFloatUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var preciseValue = 3.14159f;
        numericUpDown.Value = preciseValue;
        Assert.Equal(preciseValue, numericUpDown.Value);
        
        // Test very small values
        var smallValue = 0.0001f;
        numericUpDown.Value = smallValue;
        Assert.Equal((double)smallValue, (double)numericUpDown.Value!, precision: 4);
    }
    #endregion

    #region Decimal Tests
    [AvaloniaFact]
    public void NumericDecimalUpDown_Should_Initialize_With_Correct_Defaults()
    {
        var window = new Window();
        var numericUpDown = new NumericDecimalUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(decimal.MaxValue, numericUpDown.Maximum);
        Assert.Equal(decimal.MinValue, numericUpDown.Minimum);
        Assert.Equal(1m, numericUpDown.Step);
    }

    [AvaloniaFact]
    public void NumericDecimalUpDown_Should_Handle_High_Precision_Decimals()
    {
        var window = new Window();
        var numericUpDown = new NumericDecimalUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var preciseValue = 123.456789123456789m;
        numericUpDown.Value = preciseValue;
        Assert.Equal(preciseValue, numericUpDown.Value);
        
        // Test currency-like values
        var currencyValue = 1234.56m;
        numericUpDown.Value = currencyValue;
        Assert.Equal(currencyValue, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericDecimalUpDown_Should_Parse_Decimal_Text()
    {
        var window = new Window();
        var numericUpDown = new NumericDecimalUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericDecimalUpDown).GetMethod("ParseText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(parseMethod);
        
        var parameters = new object[] { "123.456", 0m };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters);
        var parsedValue = (decimal)parameters[1];
        
        Assert.True(result);
        Assert.Equal(123.456m, parsedValue);
    }
    #endregion

    #region Cross-Type Validation Tests
    [AvaloniaTheory]
    [InlineData(typeof(NumericIntUpDown))]
    [InlineData(typeof(NumericUIntUpDown))]
    [InlineData(typeof(NumericDoubleUpDown))]
    [InlineData(typeof(NumericByteUpDown))]
    [InlineData(typeof(NumericSByteUpDown))]
    [InlineData(typeof(NumericShortUpDown))]
    [InlineData(typeof(NumericUShortUpDown))]
    [InlineData(typeof(NumericLongUpDown))]
    [InlineData(typeof(NumericULongUpDown))]
    [InlineData(typeof(NumericFloatUpDown))]
    [InlineData(typeof(NumericDecimalUpDown))]
    public void All_NumericUpDown_Types_Should_Support_Clear_Operation(Type numericUpDownType)
    {
        var window = new Window();
        var numericUpDown = Activator.CreateInstance(numericUpDownType)!;
        window.Content = (Control)numericUpDown;
        window.Show();
        
        // Set a value using reflection (since each type has different value types)
        var valueProperty = numericUpDownType.GetProperty("Value");
        Assert.NotNull(valueProperty);
        
        // Set some non-null value (using appropriate type for each numeric type)
        object testValue = numericUpDownType.Name switch
        {
            nameof(NumericIntUpDown) => 42,
            nameof(NumericUIntUpDown) => 42u,
            nameof(NumericDoubleUpDown) => 42.0,
            nameof(NumericByteUpDown) => (byte)42,
            nameof(NumericSByteUpDown) => (sbyte)42,
            nameof(NumericShortUpDown) => (short)42,
            nameof(NumericUShortUpDown) => (ushort)42,
            nameof(NumericLongUpDown) => 42L,
            nameof(NumericULongUpDown) => 42UL,
            nameof(NumericFloatUpDown) => 42.0f,
            nameof(NumericDecimalUpDown) => 42m,
            _ => throw new ArgumentException($"Unknown type: {numericUpDownType.Name}")
        };
        
        valueProperty.SetValue(numericUpDown, testValue);
        Assert.NotNull(valueProperty.GetValue(numericUpDown));
        
        // Test Clear operation
        var clearMethod = numericUpDownType.GetMethod("Clear");
        Assert.NotNull(clearMethod);
        clearMethod.Invoke(numericUpDown, null);
        
        // Value should be null after clear (or EmptyInputValue if set)
        var clearedValue = valueProperty.GetValue(numericUpDown);
        Assert.True(clearedValue == null || clearedValue.Equals(GetEmptyInputValue(numericUpDown)));
    }

    private static object? GetEmptyInputValue(object numericUpDown)
    {
        var emptyInputValueProperty = numericUpDown.GetType().GetProperty("EmptyInputValue");
        return emptyInputValueProperty?.GetValue(numericUpDown);
    }

    [AvaloniaTheory]
    [InlineData(typeof(NumericIntUpDown))]
    [InlineData(typeof(NumericUIntUpDown))]
    [InlineData(typeof(NumericDoubleUpDown))]
    [InlineData(typeof(NumericByteUpDown))]
    [InlineData(typeof(NumericSByteUpDown))]
    [InlineData(typeof(NumericShortUpDown))]
    [InlineData(typeof(NumericUShortUpDown))]
    [InlineData(typeof(NumericLongUpDown))]
    [InlineData(typeof(NumericULongUpDown))]
    [InlineData(typeof(NumericFloatUpDown))]
    [InlineData(typeof(NumericDecimalUpDown))]
    public void All_NumericUpDown_Types_Should_Have_Correct_StyleKeyOverride(Type numericUpDownType)
    {
        var window = new Window();
        var numericUpDown = Activator.CreateInstance(numericUpDownType)!;
        window.Content = (Control)numericUpDown;
        window.Show();
        
        // All specific implementations should use the base NumericUpDown style
        var styleKeyProperty = numericUpDownType.GetProperty("StyleKeyOverride", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (styleKeyProperty != null)
        {
            var styleKey = styleKeyProperty.GetValue(numericUpDown);
            Assert.Equal(typeof(global::Ursa.Controls.NumericUpDown), styleKey);
        }
    }
    #endregion
}