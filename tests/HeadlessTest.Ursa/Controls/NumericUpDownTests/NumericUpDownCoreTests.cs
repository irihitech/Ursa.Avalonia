using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.NumericUpDownTests;

/// <summary>
/// Simplified and focused tests for NumericUpDown controls that cover core functionality
/// </summary>
public class NumericUpDownCoreTests
{
    [AvaloniaFact]
    public void NumericIntUpDown_Should_Initialize_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        // Test default values
        Assert.Null(numericUpDown.Value);
        Assert.Equal(int.MaxValue, numericUpDown.Maximum);
        Assert.Equal(int.MinValue, numericUpDown.Minimum);
        Assert.Equal(1, numericUpDown.Step);
        Assert.True(numericUpDown.AllowSpin);
        Assert.False(numericUpDown.IsReadOnly);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Set_And_Get_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        // Test setting a value
        numericUpDown.Value = 42;
        Assert.Equal(42, numericUpDown.Value);
        
        // Test setting null
        numericUpDown.Value = null;
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Fire_ValueChanged_Event()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        int? oldVal = null;
        int? newVal = null;
        bool eventFired = false;
        
        numericUpDown.ValueChanged += (sender, e) =>
        {
            oldVal = e.OldValue;
            newVal = e.NewValue;
            eventFired = true;
        };
        
        numericUpDown.Value = 42;
        Dispatcher.UIThread.RunJobs();
        
        Assert.True(eventFired);
        Assert.Null(oldVal);
        Assert.Equal(42, newVal);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Clear_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            Value = 42
        };
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        Assert.Equal(42, numericUpDown.Value);
        
        numericUpDown.Clear();
        Dispatcher.UIThread.RunJobs();
        
        Assert.Null(numericUpDown.Value);
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
        
        Dispatcher.UIThread.RunJobs();
        
        numericUpDown.Value = 42;
        numericUpDown.Clear();
        Dispatcher.UIThread.RunJobs();
        
        // After clear with EmptyInputValue set, should use that value
        Assert.Equal(0, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericDoubleUpDown_Should_Handle_Decimal_Values()
    {
        var window = new Window();
        var numericUpDown = new NumericDoubleUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        numericUpDown.Value = 3.14159;
        Assert.Equal(3.14159, numericUpDown.Value);
        
        numericUpDown.Value = -2.5;
        Assert.Equal(-2.5, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericByteUpDown_Should_Handle_Byte_Range()
    {
        var window = new Window();
        var numericUpDown = new NumericByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        // Test valid byte values
        numericUpDown.Value = 255;
        Assert.Equal((byte)255, numericUpDown.Value);
        
        numericUpDown.Value = 0;
        Assert.Equal((byte)0, numericUpDown.Value);
        
        numericUpDown.Value = 128;
        Assert.Equal((byte)128, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericSByteUpDown_Should_Handle_Signed_Range()
    {
        var window = new Window();
        var numericUpDown = new NumericSByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        numericUpDown.Value = -128;
        Assert.Equal((sbyte)(-128), numericUpDown.Value);
        
        numericUpDown.Value = 127;
        Assert.Equal((sbyte)127, numericUpDown.Value);
        
        numericUpDown.Value = 0;
        Assert.Equal((sbyte)0, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericDecimalUpDown_Should_Handle_High_Precision()
    {
        var window = new Window();
        var numericUpDown = new NumericDecimalUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        var preciseValue = 123.456789123456789m;
        numericUpDown.Value = preciseValue;
        Assert.Equal(preciseValue, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericFloatUpDown_Should_Handle_Float_Values()
    {
        var window = new Window();
        var numericUpDown = new NumericFloatUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        var floatValue = 3.14159f;
        numericUpDown.Value = floatValue;
        Assert.Equal(floatValue, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericLongUpDown_Should_Handle_Large_Values()
    {
        var window = new Window();
        var numericUpDown = new NumericLongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        var largeValue = 9223372036854775807L; // long.MaxValue
        numericUpDown.Value = largeValue;
        Assert.Equal(largeValue, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericULongUpDown_Should_Handle_Large_Unsigned_Values()
    {
        var window = new Window();
        var numericUpDown = new NumericULongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        var largeValue = 18446744073709551615UL; // ulong.MaxValue
        numericUpDown.Value = largeValue;
        Assert.Equal(largeValue, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericUpDown_Should_Handle_Min_Max_Properties()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            Minimum = 0,
            Maximum = 100
        };
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        Assert.Equal(0, numericUpDown.Minimum);
        Assert.Equal(100, numericUpDown.Maximum);
        
        // Test within range
        numericUpDown.Value = 50;
        Assert.Equal(50, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericUpDown_Should_Handle_Step_Property()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            Step = 5
        };
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        Assert.Equal(5, numericUpDown.Step);
    }

    [AvaloniaFact]
    public void NumericUpDown_Should_Handle_UI_Properties()
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
        
        Dispatcher.UIThread.RunJobs();
        
        Assert.False(numericUpDown.AllowSpin);
        Assert.False(numericUpDown.ShowButtonSpinner);
        Assert.True(numericUpDown.AllowDrag);
        Assert.True(numericUpDown.IsReadOnly);
        Assert.Equal("Enter number", numericUpDown.Watermark);
    }

    [AvaloniaFact]
    public void NumericUpDown_Should_Handle_Content_Properties()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        var leftContent = new TextBlock { Text = "Left" };
        var rightContent = new TextBlock { Text = "Right" };
        
        numericUpDown.InnerLeftContent = leftContent;
        numericUpDown.InnerRightContent = rightContent;
        
        Assert.Equal(leftContent, numericUpDown.InnerLeftContent);
        Assert.Equal(rightContent, numericUpDown.InnerRightContent);
    }

    [AvaloniaFact]
    public void NumericUpDown_Should_Handle_Format_Properties()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            FormatString = "D5"
        };
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        Assert.Equal("D5", numericUpDown.FormatString);
        Assert.NotNull(numericUpDown.NumberFormat);
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
    public void All_NumericUpDown_Types_Should_Instantiate_Successfully(Type numericUpDownType)
    {
        var window = new Window();
        var numericUpDown = Activator.CreateInstance(numericUpDownType);
        Assert.NotNull(numericUpDown);
        
        window.Content = (Control)numericUpDown!;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        // If we reach here without exception, instantiation was successful
        Assert.True(true);
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
    public void All_NumericUpDown_Types_Should_Support_Clear(Type numericUpDownType)
    {
        var window = new Window();
        var numericUpDown = Activator.CreateInstance(numericUpDownType);
        Assert.NotNull(numericUpDown);
        
        window.Content = (Control)numericUpDown!;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        // Test that Clear method exists and can be called
        var clearMethod = numericUpDownType.GetMethod("Clear");
        Assert.NotNull(clearMethod);
        
        // Should not throw
        clearMethod.Invoke(numericUpDown, null);
        Dispatcher.UIThread.RunJobs();
    }

    [AvaloniaFact]
    public void NumericUpDown_Should_Parse_Text_Input()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        // Test that ParseText method exists and works
        var parseMethod = typeof(NumericIntUpDown).GetMethod("ParseText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(parseMethod);
        
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
    public void NumericUpDown_Should_Format_Values()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        // Test ValueToString method
        var formatMethod = typeof(NumericIntUpDown).GetMethod("ValueToString", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(formatMethod);
        
        var result = (string?)formatMethod.Invoke(numericUpDown, new object?[] { 123 });
        Assert.Equal("123", result);
        
        result = (string?)formatMethod.Invoke(numericUpDown, new object?[] { null });
        Assert.Null(result);
    }
}