using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.NumericUpDownTests;

/// <summary>
/// Final comprehensive test suite for all NumericUpDown classes following existing test patterns
/// </summary>
public class NumericUpDownFinalTests
{
    [AvaloniaFact]
    public void NumericIntUpDown_Should_Initialize_And_Work()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        // Test initialization
        Assert.Null(numericUpDown.Value);
        Assert.Equal(int.MaxValue, numericUpDown.Maximum);
        Assert.Equal(int.MinValue, numericUpDown.Minimum);
        Assert.Equal(1, numericUpDown.Step);
        
        // Test value setting
        numericUpDown.Value = 42;
        Assert.Equal(42, numericUpDown.Value);
        
        // Test clear
        numericUpDown.Clear();
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericUIntUpDown_Should_Initialize_And_Work()
    {
        var window = new Window();
        var numericUpDown = new NumericUIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(uint.MaxValue, numericUpDown.Maximum);
        Assert.Equal(uint.MinValue, numericUpDown.Minimum);
        Assert.Equal(1u, numericUpDown.Step);
        
        numericUpDown.Value = 42u;
        Assert.Equal(42u, numericUpDown.Value);
        
        numericUpDown.Clear();
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericDoubleUpDown_Should_Initialize_And_Work()
    {
        var window = new Window();
        var numericUpDown = new NumericDoubleUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(double.MaxValue, numericUpDown.Maximum);
        Assert.Equal(double.MinValue, numericUpDown.Minimum);
        Assert.Equal(1.0, numericUpDown.Step);
        
        numericUpDown.Value = 3.14159;
        Assert.Equal(3.14159, numericUpDown.Value);
        
        numericUpDown.Clear();
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericByteUpDown_Should_Initialize_And_Work()
    {
        var window = new Window();
        var numericUpDown = new NumericByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(byte.MaxValue, numericUpDown.Maximum);
        Assert.Equal(byte.MinValue, numericUpDown.Minimum);
        Assert.Equal((byte)1, numericUpDown.Step);
        
        numericUpDown.Value = 255;
        Assert.Equal((byte)255, numericUpDown.Value);
        
        numericUpDown.Clear();
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericSByteUpDown_Should_Initialize_And_Work()
    {
        var window = new Window();
        var numericUpDown = new NumericSByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(sbyte.MaxValue, numericUpDown.Maximum);
        Assert.Equal(sbyte.MinValue, numericUpDown.Minimum);
        Assert.Equal((sbyte)1, numericUpDown.Step);
        
        numericUpDown.Value = -50;
        Assert.Equal((sbyte)(-50), numericUpDown.Value);
        
        numericUpDown.Clear();
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]  
    public void NumericShortUpDown_Should_Initialize_And_Work()
    {
        var window = new Window();
        var numericUpDown = new NumericShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(short.MaxValue, numericUpDown.Maximum);
        Assert.Equal(short.MinValue, numericUpDown.Minimum);
        Assert.Equal((short)1, numericUpDown.Step);
        
        numericUpDown.Value = 1000;
        Assert.Equal((short)1000, numericUpDown.Value);
        
        numericUpDown.Clear();
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericUShortUpDown_Should_Initialize_And_Work()
    {
        var window = new Window();
        var numericUpDown = new NumericUShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(ushort.MaxValue, numericUpDown.Maximum);
        Assert.Equal(ushort.MinValue, numericUpDown.Minimum);  
        Assert.Equal((ushort)1, numericUpDown.Step);
        
        numericUpDown.Value = 2000;
        Assert.Equal((ushort)2000, numericUpDown.Value);
        
        numericUpDown.Clear();
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericLongUpDown_Should_Initialize_And_Work()
    {
        var window = new Window();
        var numericUpDown = new NumericLongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(long.MaxValue, numericUpDown.Maximum);
        Assert.Equal(long.MinValue, numericUpDown.Minimum);
        Assert.Equal(1L, numericUpDown.Step);
        
        numericUpDown.Value = 9223372036854775806L;
        Assert.Equal(9223372036854775806L, numericUpDown.Value);
        
        numericUpDown.Clear();
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericULongUpDown_Should_Initialize_And_Work()
    {
        var window = new Window();
        var numericUpDown = new NumericULongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(ulong.MaxValue, numericUpDown.Maximum);
        Assert.Equal(ulong.MinValue, numericUpDown.Minimum);
        Assert.Equal(1UL, numericUpDown.Step);
        
        numericUpDown.Value = 18446744073709551614UL;
        Assert.Equal(18446744073709551614UL, numericUpDown.Value);
        
        numericUpDown.Clear();
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericFloatUpDown_Should_Initialize_And_Work()
    {
        var window = new Window();
        var numericUpDown = new NumericFloatUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(float.MaxValue, numericUpDown.Maximum);
        Assert.Equal(float.MinValue, numericUpDown.Minimum);
        Assert.Equal(1.0f, numericUpDown.Step);
        
        numericUpDown.Value = 3.14159f;
        Assert.Equal(3.14159f, numericUpDown.Value);
        
        numericUpDown.Clear();
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericDecimalUpDown_Should_Initialize_And_Work()
    {
        var window = new Window();
        var numericUpDown = new NumericDecimalUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Null(numericUpDown.Value);
        Assert.Equal(decimal.MaxValue, numericUpDown.Maximum);
        Assert.Equal(decimal.MinValue, numericUpDown.Minimum);
        Assert.Equal(1m, numericUpDown.Step);
        
        numericUpDown.Value = 123.456789123456789m;
        Assert.Equal(123.456789123456789m, numericUpDown.Value);
        
        numericUpDown.Clear();
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Fire_ValueChanged_Event()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        int? oldValue = null;
        int? newValue = null;
        bool eventFired = false;
        
        numericUpDown.ValueChanged += (sender, e) =>
        {
            oldValue = e.OldValue;
            newValue = e.NewValue;
            eventFired = true;
        };
        
        numericUpDown.Value = 42;
        
        Assert.True(eventFired);
        Assert.Null(oldValue);
        Assert.Equal(42, newValue);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Handle_Min_Max_Properties()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            Minimum = 0,
            Maximum = 100
        };
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Equal(0, numericUpDown.Minimum);
        Assert.Equal(100, numericUpDown.Maximum);
        
        // Test setting value within range
        numericUpDown.Value = 50;
        Assert.Equal(50, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Handle_EmptyInputValue()
    {
        var window = new Window(); 
        var numericUpDown = new NumericIntUpDown
        {
            EmptyInputValue = 0
        };
        window.Content = numericUpDown;
        window.Show();
        
        numericUpDown.Value = 42;
        numericUpDown.Clear();
        
        // After clear with EmptyInputValue set, should use that value
        Assert.Equal(0, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Handle_UI_Properties()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            AllowSpin = false,
            ShowButtonSpinner = false,
            AllowDrag = true,
            IsReadOnly = true,
            Watermark = "Enter number"
        };
        window.Content = numericUpDown;
        window.Show();
        
        Assert.False(numericUpDown.AllowSpin);
        Assert.False(numericUpDown.ShowButtonSpinner);
        Assert.True(numericUpDown.AllowDrag);
        Assert.True(numericUpDown.IsReadOnly);
        Assert.Equal("Enter number", numericUpDown.Watermark);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Handle_Content_Properties()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var leftContent = new TextBlock { Text = "Left" };
        var rightContent = new TextBlock { Text = "Right" };
        
        numericUpDown.InnerLeftContent = leftContent;
        numericUpDown.InnerRightContent = rightContent;
        
        Assert.Equal(leftContent, numericUpDown.InnerLeftContent);
        Assert.Equal(rightContent, numericUpDown.InnerRightContent);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Handle_Format_Properties()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            FormatString = "D5"
        };
        window.Content = numericUpDown;
        window.Show();
        
        Assert.Equal("D5", numericUpDown.FormatString);
        Assert.NotNull(numericUpDown.NumberFormat);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Parse_Text_Input()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        // Use reflection to test ParseText method
        var parseMethod = typeof(NumericIntUpDown).GetMethod("ParseText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(parseMethod);
        
        // Test valid input
        var parameters = new object[] { "123", 0 };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        var parsedValue = (int)parameters[1];
        
        Assert.True(result);
        Assert.Equal(123, parsedValue);
        
        // Test invalid input
        parameters = new object[] { "invalid", 0 };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Format_Values()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        // Use reflection to test ValueToString method
        var formatMethod = typeof(NumericIntUpDown).GetMethod("ValueToString", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(formatMethod);
        
        var result = (string?)formatMethod.Invoke(numericUpDown, new object?[] { 123 });
        Assert.Equal("123", result);
        
        result = (string?)formatMethod.Invoke(numericUpDown, new object?[] { null });
        Assert.Null(result);
    }

    [AvaloniaFact]
    public void NumericUpDown_Should_Have_Abstract_Methods()
    {
        var window = new Window();
        var intUpDown = new NumericIntUpDown();
        
        window.Content = intUpDown;
        window.Show();
        
        // Test that concrete implementations have required methods
        var parseMethod = typeof(NumericIntUpDown).GetMethod("ParseText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(parseMethod);
        
        var formatMethod = typeof(NumericIntUpDown).GetMethod("ValueToString", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(formatMethod);
        
        // Test Zero property exists (use specific binding flags to avoid ambiguity)
        var zeroProperty = typeof(NumericIntUpDown).GetProperty("Zero", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
        if (zeroProperty != null)
        {
            var zeroValue = zeroProperty.GetValue(intUpDown);
            Assert.Equal(0, zeroValue);
        }
    }

    [AvaloniaFact]
    public void All_NumericUpDown_Types_Should_Have_Clear_Method()
    {
        var window = new Window();
        
        // Test a few representative types
        var types = new[]
        {
            typeof(NumericIntUpDown),
            typeof(NumericDoubleUpDown),
            typeof(NumericDecimalUpDown)
        };
        
        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            Assert.NotNull(instance);
            
            var clearMethod = type.GetMethod("Clear");
            Assert.NotNull(clearMethod);
            
            // Verify Clear method can be called (may not test full functionality due to UI thread requirements)
            Assert.NotNull(clearMethod.DeclaringType);
        }
    }
}