using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Threading;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.NumericUpDownTests;

public class NumericUpDownBaseTests
{
    [AvaloniaFact]
    public void NumericIntUpDown_Should_Initialize_With_Default_Values()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
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
        
        numericUpDown.Value = 42;
        Assert.Equal(42, numericUpDown.Value);
        
        numericUpDown.Value = null;
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Clamp_Value_To_Range()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            Minimum = 0,
            Maximum = 100
        };
        window.Content = numericUpDown;
        window.Show();
        
        // Test value above maximum
        numericUpDown.Value = 150;
        Assert.Equal(100, numericUpDown.Value);
        
        // Test value below minimum
        numericUpDown.Value = -50;
        Assert.Equal(0, numericUpDown.Value);
        
        // Test value within range
        numericUpDown.Value = 50;
        Assert.Equal(50, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Increase_Value_By_Step()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            Value = 10,
            Step = 5,
            Maximum = 100
        };
        window.Content = numericUpDown;
        window.Show();
        
        // Test increase
        numericUpDown.Value = 10;
        var method = typeof(NumericIntUpDown).GetMethod("Increase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method?.Invoke(numericUpDown, null);
        
        Assert.Equal(15, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Decrease_Value_By_Step()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            Value = 10,
            Step = 5,
            Minimum = 0
        };
        window.Content = numericUpDown;
        window.Show();
        
        // Test decrease
        var method = typeof(NumericIntUpDown).GetMethod("Decrease", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method?.Invoke(numericUpDown, null);
        
        Assert.Equal(5, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Not_Exceed_Maximum_When_Increasing()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            Value = 95,
            Step = 10,
            Maximum = 100
        };
        window.Content = numericUpDown;
        window.Show();
        
        var method = typeof(NumericIntUpDown).GetMethod("Increase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method?.Invoke(numericUpDown, null);
        
        Assert.Equal(100, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Not_Go_Below_Minimum_When_Decreasing()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            Value = 5,
            Step = 10,
            Minimum = 0
        };
        window.Content = numericUpDown;
        window.Show();
        
        var method = typeof(NumericIntUpDown).GetMethod("Decrease", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method?.Invoke(numericUpDown, null);
        
        Assert.Equal(0, numericUpDown.Value);
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
    public void NumericIntUpDown_Should_Handle_Null_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        numericUpDown.Value = null;
        Assert.Null(numericUpDown.Value);
        
        // Test increasing from null should use minimum or zero
        var method = typeof(NumericIntUpDown).GetMethod("Increase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method?.Invoke(numericUpDown, null);
        
        // Should go to minimum value (int.MinValue) or zero depending on implementation
        Assert.NotNull(numericUpDown.Value);
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
        
        numericUpDown.Clear();
        Assert.Null(numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Respect_ReadOnly_Property()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            Value = 10,
            IsReadOnly = true
        };
        window.Content = numericUpDown;
        window.Show();
        
        // When read-only, increase/decrease should not work
        var initialValue = numericUpDown.Value;
        var increaseMethod = typeof(NumericIntUpDown).GetMethod("Increase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        increaseMethod?.Invoke(numericUpDown, null);
        
        Assert.Equal(initialValue, numericUpDown.Value);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Coerce_Maximum_Below_Minimum()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            Minimum = 100
        };
        window.Content = numericUpDown;
        window.Show();
        
        // Setting maximum below minimum should coerce maximum to minimum
        numericUpDown.Maximum = 50;
        Assert.Equal(100, numericUpDown.Maximum);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Coerce_Minimum_Above_Maximum()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            Maximum = 50
        };
        window.Content = numericUpDown;
        window.Show();
        
        // Setting minimum above maximum should coerce minimum to maximum
        numericUpDown.Minimum = 100;
        Assert.Equal(50, numericUpDown.Minimum);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Parse_Text_Input()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        // Test parsing valid integer
        var parseMethod = typeof(NumericIntUpDown).GetMethod("ParseText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(parseMethod);
        
        var parameters = new object[] { "123", 0 };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters);
        var parsedValue = (int)parameters[1];
        
        Assert.True(result);
        Assert.Equal(123, parsedValue);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Handle_Invalid_Text_Input()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        // Test parsing invalid text
        var parseMethod = typeof(NumericIntUpDown).GetMethod("ParseText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(parseMethod);
        
        var parameters = new object[] { "invalid", 0 };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters);
        
        Assert.False(result);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Format_Value_To_String()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        // Test value formatting
        var formatMethod = typeof(NumericIntUpDown).GetMethod("ValueToString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(formatMethod);
        
        var result = (string?)formatMethod.Invoke(numericUpDown, new object?[] { 123 });
        Assert.Equal("123", result);
        
        // Test null value formatting
        result = (string?)formatMethod.Invoke(numericUpDown, new object?[] { null });
        Assert.Null(result);
    }

    [AvaloniaFact]
    public void NumericIntUpDown_Should_Handle_Format_String()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown
        {
            FormatString = "D5", // Format with leading zeros
            Value = 42
        };
        window.Content = numericUpDown;
        window.Show();
        
        var formatMethod = typeof(NumericIntUpDown).GetMethod("ValueToString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(formatMethod);
        
        var result = (string?)formatMethod.Invoke(numericUpDown, new object?[] { 42 });
        Assert.Equal("00042", result);
    }
}