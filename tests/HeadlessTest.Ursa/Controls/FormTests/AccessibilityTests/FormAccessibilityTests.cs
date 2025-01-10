using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.FormTests.AccessibilityTests;

public class FormAccessibilityTests
{
    [AvaloniaFact]
    public void Static_Form_Inner_Control_Accessible()
    {
        var window = new Window();
        var form = new StaticForm();
        window.Content = form;
        window.Show();

        Assert.False(form.NameBox.IsFocused);
        Assert.False(form.EmailBox.IsFocused);
        window.KeyPressQwerty(PhysicalKey.N, RawInputModifiers.Alt);
        Assert.True(form.NameBox.IsFocused);
        Assert.False(form.EmailBox.IsFocused);
        window.KeyPressQwerty(PhysicalKey.E, RawInputModifiers.Alt);
        Assert.False(form.NameBox.IsFocused);
        Assert.True(form.EmailBox.IsFocused);
    }
    
    [AvaloniaFact]
    public void Static_Form_With_FormItem_Accessible()
    {
        var window = new Window();
        var form = new StaticForm2();
        window.Content = form;
        window.Show();

        Assert.False(form.NameBox.IsFocused);
        Assert.False(form.EmailBox.IsFocused);
        window.KeyPressQwerty(PhysicalKey.N, RawInputModifiers.Alt);
        Assert.True(form.NameBox.IsFocused);
        Assert.False(form.EmailBox.IsFocused);
        window.KeyPressQwerty(PhysicalKey.E, RawInputModifiers.Alt);
        Assert.False(form.NameBox.IsFocused);
        Assert.True(form.EmailBox.IsFocused);
    }

    [AvaloniaFact]
    public void Dynamic_Form_Inner_Control_Accessible()
    {
        var window = new Window();
        var form = new DynamicForm();
        window.Content = form;
        window.Show();

        var logicalChildren = form.form.GetLogicalChildren().ToList();
        Assert.Equal(2, logicalChildren.Count);
        var first = logicalChildren[0] as FormItem;
        var second = logicalChildren[1] as FormItem;
        Assert.NotNull(first);
        Assert.NotNull(second);

        var text1 = first.GetLogicalChildren().OfType<TextBox>().FirstOrDefault();
        var text2 = second.GetLogicalChildren().OfType<TextBox>().FirstOrDefault();
        
        Assert.NotNull(text1);
        Assert.NotNull(text2);
        
        Assert.False(text1.IsFocused);
        Assert.False(text2.IsFocused);
        window.KeyPressQwerty(PhysicalKey.N, RawInputModifiers.Alt);
        Assert.True(text1.IsFocused);
        Assert.False(text2.IsFocused);
        window.KeyPressQwerty(PhysicalKey.E, RawInputModifiers.Alt);
        Assert.False(text1.IsFocused);
        Assert.True(text2.IsFocused);
    }
}