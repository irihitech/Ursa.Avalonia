using System.Reflection;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.NumericUpDownTests;

/// <summary>
/// Comprehensive tests for Add, Minus, ParseText, and Zero methods for all NumericUpDown classes
/// </summary>
public class NumericUpDownMethodTests
{
    #region NumericIntUpDown Method Tests
    
    [AvaloniaFact]
    public void NumericIntUpDown_Add_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var addMethod = typeof(NumericIntUpDown).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(addMethod);
        
        // Test basic addition
        var result = addMethod.Invoke(numericUpDown, new object?[] { 10, 5 });
        Assert.Equal(15, result);
        
        // Test addition with negative numbers
        result = addMethod.Invoke(numericUpDown, new object?[] { -5, 3 });
        Assert.Equal(-2, result);
        
        // Test addition with null values
        result = addMethod.Invoke(numericUpDown, new object?[] { null, 5 });
        Assert.Null(result);
        
        result = addMethod.Invoke(numericUpDown, new object?[] { 5, null });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericIntUpDown_Minus_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var minusMethod = typeof(NumericIntUpDown).GetMethod("Minus", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(minusMethod);
        
        // Test basic subtraction
        var result = minusMethod.Invoke(numericUpDown, new object?[] { 10, 5 });
        Assert.Equal(5, result);
        
        // Test subtraction with negative numbers
        result = minusMethod.Invoke(numericUpDown, new object?[] { -5, 3 });
        Assert.Equal(-8, result);
        
        // Test subtraction with null values
        result = minusMethod.Invoke(numericUpDown, new object?[] { null, 5 });
        Assert.Null(result);
        
        result = minusMethod.Invoke(numericUpDown, new object?[] { 5, null });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericIntUpDown_ParseText_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericIntUpDown).GetMethod("ParseText", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(parseMethod);
        
        // Test valid integer parsing
        var parameters = new object?[] { "123", null };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal(123, parameters[1]);
        
        // Test negative integer parsing
        parameters = new object?[] { "-456", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal(-456, parameters[1]);
        
        // Test invalid parsing
        parameters = new object?[] { "abc", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
        
        // Test empty string
        parameters = new object?[] { "", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
        
        // Test null input
        parameters = new object?[] { null, null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
    }
    
    [AvaloniaFact]
    public void NumericIntUpDown_Zero_Should_Return_Correct_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var zeroProperty = typeof(NumericIntUpDown).GetProperty("Zero", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(zeroProperty);
        
        var result = zeroProperty.GetValue(numericUpDown);
        Assert.Equal(0, result);
    }
    
    #endregion
    
    #region NumericUIntUpDown Method Tests
    
    [AvaloniaFact]
    public void NumericUIntUpDown_Add_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericUIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var addMethod = typeof(NumericUIntUpDown).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(addMethod);
        
        // Test basic addition
        var result = addMethod.Invoke(numericUpDown, new object?[] { 10u, 5u });
        Assert.Equal(15u, result);
        
        // Test addition with null values
        result = addMethod.Invoke(numericUpDown, new object?[] { null, 5u });
        Assert.Null(result);
        
        result = addMethod.Invoke(numericUpDown, new object?[] { 5u, null });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericUIntUpDown_Minus_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericUIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var minusMethod = typeof(NumericUIntUpDown).GetMethod("Minus", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(minusMethod);
        
        // Test basic subtraction
        var result = minusMethod.Invoke(numericUpDown, new object?[] { 10u, 5u });
        Assert.Equal(5u, result);
        
        // Test subtraction with null values
        result = minusMethod.Invoke(numericUpDown, new object?[] { null, 5u });
        Assert.Null(result);
        
        result = minusMethod.Invoke(numericUpDown, new object?[] { 5u, null });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericUIntUpDown_ParseText_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericUIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericUIntUpDown).GetMethod("ParseText", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(parseMethod);
        
        // Test valid unsigned integer parsing
        var parameters = new object?[] { "123", null };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal(123u, parameters[1]);
        
        // Test invalid parsing (negative number)
        parameters = new object?[] { "-456", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
        
        // Test invalid parsing
        parameters = new object?[] { "abc", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
    }
    
    [AvaloniaFact]
    public void NumericUIntUpDown_Zero_Should_Return_Correct_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericUIntUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var zeroProperty = typeof(NumericUIntUpDown).GetProperty("Zero", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(zeroProperty);
        
        var result = zeroProperty.GetValue(numericUpDown);
        Assert.Equal(0u, result);
    }
    
    #endregion
    
    #region NumericDoubleUpDown Method Tests
    
    [AvaloniaFact]
    public void NumericDoubleUpDown_Add_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericDoubleUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var addMethod = typeof(NumericDoubleUpDown).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(addMethod);
        
        // Test basic addition
        var result = addMethod.Invoke(numericUpDown, new object?[] { 10.5, 5.3 });
        Assert.Equal(15.8, result);
        
        // Test addition with negative numbers
        result = addMethod.Invoke(numericUpDown, new object?[] { -5.2, 3.1 });
        Assert.Equal(-2.1, (double)result!, 1);
        
        // Test addition with null values
        result = addMethod.Invoke(numericUpDown, new object?[] { null, 5.0 });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericDoubleUpDown_Minus_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericDoubleUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var minusMethod = typeof(NumericDoubleUpDown).GetMethod("Minus", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(minusMethod);
        
        // Test basic subtraction
        var result = minusMethod.Invoke(numericUpDown, new object?[] { 10.5, 5.3 });
        Assert.Equal(5.2, (double)result!, 1);
        
        // Test subtraction with null values
        result = minusMethod.Invoke(numericUpDown, new object?[] { null, 5.0 });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericDoubleUpDown_ParseText_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericDoubleUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericDoubleUpDown).GetMethod("ParseText", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(parseMethod);
        
        // Test valid double parsing
        var parameters = new object?[] { "123.45", null };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal(123.45, parameters[1]);
        
        // Test negative double parsing
        parameters = new object?[] { "-456.78", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal(-456.78, parameters[1]);
        
        // Test invalid parsing
        parameters = new object?[] { "abc", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
    }
    
    [AvaloniaFact]
    public void NumericDoubleUpDown_Zero_Should_Return_Correct_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericDoubleUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var zeroProperty = typeof(NumericDoubleUpDown).GetProperty("Zero", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(zeroProperty);
        
        var result = zeroProperty.GetValue(numericUpDown);
        Assert.Equal(0.0, result);
    }
    
    #endregion
    
    #region NumericByteUpDown Method Tests
    
    [AvaloniaFact]
    public void NumericByteUpDown_Add_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var addMethod = typeof(NumericByteUpDown).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(addMethod);
        
        // Test basic addition
        var result = addMethod.Invoke(numericUpDown, new object?[] { (byte)10, (byte)5 });
        Assert.Equal((byte)15, result);
        
        // Test addition with null values
        result = addMethod.Invoke(numericUpDown, new object?[] { null, (byte)5 });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericByteUpDown_Minus_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var minusMethod = typeof(NumericByteUpDown).GetMethod("Minus", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(minusMethod);
        
        // Test basic subtraction
        var result = minusMethod.Invoke(numericUpDown, new object?[] { (byte)10, (byte)5 });
        Assert.Equal((byte)5, result);
        
        // Test subtraction with null values
        result = minusMethod.Invoke(numericUpDown, new object?[] { null, (byte)5 });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericByteUpDown_ParseText_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericByteUpDown).GetMethod("ParseText", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(parseMethod);
        
        // Test valid byte parsing
        var parameters = new object?[] { "123", null };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal((byte)123, parameters[1]);
        
        // Test invalid parsing (out of range)
        parameters = new object?[] { "300", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
        
        // Test invalid parsing
        parameters = new object?[] { "abc", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
    }
    
    [AvaloniaFact]
    public void NumericByteUpDown_Zero_Should_Return_Correct_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var zeroProperty = typeof(NumericByteUpDown).GetProperty("Zero", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(zeroProperty);
        
        var result = zeroProperty.GetValue(numericUpDown);
        Assert.Equal((byte)0, result);
    }
    
    #endregion
    
    #region NumericSByteUpDown Method Tests
    
    [AvaloniaFact]
    public void NumericSByteUpDown_Add_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericSByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var addMethod = typeof(NumericSByteUpDown).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(addMethod);
        
        // Test basic addition
        var result = addMethod.Invoke(numericUpDown, new object?[] { (sbyte)10, (sbyte)5 });
        Assert.Equal((sbyte)15, result);
        
        // Test addition with negative numbers
        result = addMethod.Invoke(numericUpDown, new object?[] { (sbyte)-5, (sbyte)3 });
        Assert.Equal((sbyte)-2, result);
        
        // Test addition with null values
        result = addMethod.Invoke(numericUpDown, new object?[] { null, (sbyte)5 });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericSByteUpDown_Minus_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericSByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var minusMethod = typeof(NumericSByteUpDown).GetMethod("Minus", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(minusMethod);
        
        // Test basic subtraction
        var result = minusMethod.Invoke(numericUpDown, new object?[] { (sbyte)10, (sbyte)5 });
        Assert.Equal((sbyte)5, result);
        
        // Test subtraction with negative numbers
        result = minusMethod.Invoke(numericUpDown, new object?[] { (sbyte)-5, (sbyte)3 });
        Assert.Equal((sbyte)-8, result);
        
        // Test subtraction with null values
        result = minusMethod.Invoke(numericUpDown, new object?[] { null, (sbyte)5 });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericSByteUpDown_ParseText_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericSByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericSByteUpDown).GetMethod("ParseText", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(parseMethod);
        
        // Test valid sbyte parsing
        var parameters = new object?[] { "123", null };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal((sbyte)123, parameters[1]);
        
        // Test negative sbyte parsing
        parameters = new object?[] { "-100", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal((sbyte)-100, parameters[1]);
        
        // Test invalid parsing
        parameters = new object?[] { "abc", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
    }
    
    [AvaloniaFact]
    public void NumericSByteUpDown_Zero_Should_Return_Correct_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericSByteUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var zeroProperty = typeof(NumericSByteUpDown).GetProperty("Zero", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(zeroProperty);
        
        var result = zeroProperty.GetValue(numericUpDown);
        Assert.Equal((sbyte)0, result);
    }
    
    #endregion
    
    #region NumericShortUpDown Method Tests
    
    [AvaloniaFact]
    public void NumericShortUpDown_Add_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var addMethod = typeof(NumericShortUpDown).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(addMethod);
        
        // Test basic addition
        var result = addMethod.Invoke(numericUpDown, new object?[] { (short)10, (short)5 });
        Assert.Equal((short)15, result);
        
        // Test addition with negative numbers
        result = addMethod.Invoke(numericUpDown, new object?[] { (short)-5, (short)3 });
        Assert.Equal((short)-2, result);
        
        // Test addition with null values
        result = addMethod.Invoke(numericUpDown, new object?[] { null, (short)5 });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericShortUpDown_Minus_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var minusMethod = typeof(NumericShortUpDown).GetMethod("Minus", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(minusMethod);
        
        // Test basic subtraction
        var result = minusMethod.Invoke(numericUpDown, new object?[] { (short)10, (short)5 });
        Assert.Equal((short)5, result);
        
        // Test subtraction with negative numbers
        result = minusMethod.Invoke(numericUpDown, new object?[] { (short)-5, (short)3 });
        Assert.Equal((short)-8, result);
        
        // Test subtraction with null values
        result = minusMethod.Invoke(numericUpDown, new object?[] { null, (short)5 });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericShortUpDown_ParseText_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericShortUpDown).GetMethod("ParseText", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(parseMethod);
        
        // Test valid short parsing
        var parameters = new object?[] { "1234", null };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal((short)1234, parameters[1]);
        
        // Test negative short parsing
        parameters = new object?[] { "-1000", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal((short)-1000, parameters[1]);
        
        // Test invalid parsing
        parameters = new object?[] { "abc", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
    }
    
    [AvaloniaFact]
    public void NumericShortUpDown_Zero_Should_Return_Correct_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var zeroProperty = typeof(NumericShortUpDown).GetProperty("Zero", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(zeroProperty);
        
        var result = zeroProperty.GetValue(numericUpDown);
        Assert.Equal((short)0, result);
    }
    
    #endregion
    
    #region NumericUShortUpDown Method Tests
    
    [AvaloniaFact]
    public void NumericUShortUpDown_Add_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericUShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var addMethod = typeof(NumericUShortUpDown).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(addMethod);
        
        // Test basic addition
        var result = addMethod.Invoke(numericUpDown, new object?[] { (ushort)10, (ushort)5 });
        Assert.Equal((ushort)15, result);
        
        // Test addition with null values
        result = addMethod.Invoke(numericUpDown, new object?[] { null, (ushort)5 });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericUShortUpDown_Minus_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericUShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var minusMethod = typeof(NumericUShortUpDown).GetMethod("Minus", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(minusMethod);
        
        // Test basic subtraction
        var result = minusMethod.Invoke(numericUpDown, new object?[] { (ushort)10, (ushort)5 });
        Assert.Equal((ushort)5, result);
        
        // Test subtraction with null values
        result = minusMethod.Invoke(numericUpDown, new object?[] { null, (ushort)5 });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericUShortUpDown_ParseText_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericUShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericUShortUpDown).GetMethod("ParseText", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(parseMethod);
        
        // Test valid ushort parsing
        var parameters = new object?[] { "1234", null };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal((ushort)1234, parameters[1]);
        
        // Test invalid parsing (negative number)
        parameters = new object?[] { "-1000", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
        
        // Test invalid parsing
        parameters = new object?[] { "abc", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
    }
    
    [AvaloniaFact]
    public void NumericUShortUpDown_Zero_Should_Return_Correct_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericUShortUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var zeroProperty = typeof(NumericUShortUpDown).GetProperty("Zero", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(zeroProperty);
        
        var result = zeroProperty.GetValue(numericUpDown);
        Assert.Equal((ushort)0, result);
    }
    
    #endregion
    
    #region NumericLongUpDown Method Tests
    
    [AvaloniaFact]
    public void NumericLongUpDown_Add_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericLongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var addMethod = typeof(NumericLongUpDown).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(addMethod);
        
        // Test basic addition
        var result = addMethod.Invoke(numericUpDown, new object?[] { 10L, 5L });
        Assert.Equal(15L, result);
        
        // Test addition with negative numbers
        result = addMethod.Invoke(numericUpDown, new object?[] { -5L, 3L });
        Assert.Equal(-2L, result);
        
        // Test addition with null values
        result = addMethod.Invoke(numericUpDown, new object?[] { null, 5L });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericLongUpDown_Minus_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericLongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var minusMethod = typeof(NumericLongUpDown).GetMethod("Minus", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(minusMethod);
        
        // Test basic subtraction
        var result = minusMethod.Invoke(numericUpDown, new object?[] { 10L, 5L });
        Assert.Equal(5L, result);
        
        // Test subtraction with negative numbers
        result = minusMethod.Invoke(numericUpDown, new object?[] { -5L, 3L });
        Assert.Equal(-8L, result);
        
        // Test subtraction with null values
        result = minusMethod.Invoke(numericUpDown, new object?[] { null, 5L });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericLongUpDown_ParseText_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericLongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericLongUpDown).GetMethod("ParseText", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(parseMethod);
        
        // Test valid long parsing
        var parameters = new object?[] { "123456789", null };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal(123456789L, parameters[1]);
        
        // Test negative long parsing
        parameters = new object?[] { "-123456789", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal(-123456789L, parameters[1]);
        
        // Test invalid parsing
        parameters = new object?[] { "abc", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
    }
    
    [AvaloniaFact]
    public void NumericLongUpDown_Zero_Should_Return_Correct_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericLongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var zeroProperty = typeof(NumericLongUpDown).GetProperty("Zero", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(zeroProperty);
        
        var result = zeroProperty.GetValue(numericUpDown);
        Assert.Equal(0L, result);
    }
    
    #endregion
    
    #region NumericULongUpDown Method Tests
    
    [AvaloniaFact]
    public void NumericULongUpDown_Add_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericULongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var addMethod = typeof(NumericULongUpDown).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(addMethod);
        
        // Test basic addition
        var result = addMethod.Invoke(numericUpDown, new object?[] { 10UL, 5UL });
        Assert.Equal(15UL, result);
        
        // Test addition with null values
        result = addMethod.Invoke(numericUpDown, new object?[] { null, 5UL });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericULongUpDown_Minus_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericULongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var minusMethod = typeof(NumericULongUpDown).GetMethod("Minus", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(minusMethod);
        
        // Test basic subtraction
        var result = minusMethod.Invoke(numericUpDown, new object?[] { 10UL, 5UL });
        Assert.Equal(5UL, result);
        
        // Test subtraction with null values
        result = minusMethod.Invoke(numericUpDown, new object?[] { null, 5UL });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericULongUpDown_ParseText_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericULongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericULongUpDown).GetMethod("ParseText", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(parseMethod);
        
        // Test valid ulong parsing
        var parameters = new object?[] { "123456789", null };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal(123456789UL, parameters[1]);
        
        // Test invalid parsing (negative number)
        parameters = new object?[] { "-123456789", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
        
        // Test invalid parsing
        parameters = new object?[] { "abc", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
    }
    
    [AvaloniaFact]
    public void NumericULongUpDown_Zero_Should_Return_Correct_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericULongUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var zeroProperty = typeof(NumericULongUpDown).GetProperty("Zero", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(zeroProperty);
        
        var result = zeroProperty.GetValue(numericUpDown);
        Assert.Equal(0UL, result);
    }
    
    #endregion
    
    #region NumericFloatUpDown Method Tests
    
    [AvaloniaFact]
    public void NumericFloatUpDown_Add_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericFloatUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var addMethod = typeof(NumericFloatUpDown).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(addMethod);
        
        // Test basic addition
        var result = addMethod.Invoke(numericUpDown, new object?[] { 10.5f, 5.3f });
        Assert.Equal(15.8f, (float)result!, 1);
        
        // Test addition with negative numbers
        result = addMethod.Invoke(numericUpDown, new object?[] { -5.2f, 3.1f });
        Assert.Equal(-2.1f, (float)result!, 1);
        
        // Test addition with null values
        result = addMethod.Invoke(numericUpDown, new object?[] { null, 5.0f });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericFloatUpDown_Minus_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericFloatUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var minusMethod = typeof(NumericFloatUpDown).GetMethod("Minus", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(minusMethod);
        
        // Test basic subtraction
        var result = minusMethod.Invoke(numericUpDown, new object?[] { 10.5f, 5.3f });
        Assert.Equal(5.2f, (float)result!, 1);
        
        // Test subtraction with null values
        result = minusMethod.Invoke(numericUpDown, new object?[] { null, 5.0f });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericFloatUpDown_ParseText_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericFloatUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericFloatUpDown).GetMethod("ParseText", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(parseMethod);
        
        // Test valid float parsing
        var parameters = new object?[] { "123.45", null };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal(123.45f, parameters[1]);
        
        // Test negative float parsing
        parameters = new object?[] { "-456.78", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal(-456.78f, parameters[1]);
        
        // Test invalid parsing
        parameters = new object?[] { "abc", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
    }
    
    [AvaloniaFact]
    public void NumericFloatUpDown_Zero_Should_Return_Correct_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericFloatUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var zeroProperty = typeof(NumericFloatUpDown).GetProperty("Zero", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(zeroProperty);
        
        var result = zeroProperty.GetValue(numericUpDown);
        Assert.Equal(0.0f, result);
    }
    
    #endregion
    
    #region NumericDecimalUpDown Method Tests
    
    [AvaloniaFact]
    public void NumericDecimalUpDown_Add_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericDecimalUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var addMethod = typeof(NumericDecimalUpDown).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(addMethod);
        
        // Test basic addition
        var result = addMethod.Invoke(numericUpDown, new object?[] { 10.5m, 5.3m });
        Assert.Equal(15.8m, result);
        
        // Test addition with negative numbers
        result = addMethod.Invoke(numericUpDown, new object?[] { -5.2m, 3.1m });
        Assert.Equal(-2.1m, result);
        
        // Test addition with null values
        result = addMethod.Invoke(numericUpDown, new object?[] { null, 5.0m });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericDecimalUpDown_Minus_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericDecimalUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var minusMethod = typeof(NumericDecimalUpDown).GetMethod("Minus", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(minusMethod);
        
        // Test basic subtraction
        var result = minusMethod.Invoke(numericUpDown, new object?[] { 10.5m, 5.3m });
        Assert.Equal(5.2m, result);
        
        // Test subtraction with null values
        result = minusMethod.Invoke(numericUpDown, new object?[] { null, 5.0m });
        Assert.Null(result);
    }
    
    [AvaloniaFact]
    public void NumericDecimalUpDown_ParseText_Should_Work_Correctly()
    {
        var window = new Window();
        var numericUpDown = new NumericDecimalUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var parseMethod = typeof(NumericDecimalUpDown).GetMethod("ParseText", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(parseMethod);
        
        // Test valid decimal parsing
        var parameters = new object?[] { "123.45", null };
        var result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal(123.45m, parameters[1]);
        
        // Test negative decimal parsing
        parameters = new object?[] { "-456.78", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.True(result);
        Assert.Equal(-456.78m, parameters[1]);
        
        // Test invalid parsing
        parameters = new object?[] { "abc", null };
        result = (bool)parseMethod.Invoke(numericUpDown, parameters)!;
        Assert.False(result);
    }
    
    [AvaloniaFact]
    public void NumericDecimalUpDown_Zero_Should_Return_Correct_Value()
    {
        var window = new Window();
        var numericUpDown = new NumericDecimalUpDown();
        window.Content = numericUpDown;
        window.Show();
        
        var zeroProperty = typeof(NumericDecimalUpDown).GetProperty("Zero", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.NotNull(zeroProperty);
        
        var result = zeroProperty.GetValue(numericUpDown);
        Assert.Equal(0.0m, result);
    }
    
    #endregion
}