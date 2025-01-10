using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;

namespace HeadlessTest.Ursa.Controls.FormTests.AccessibilityTests;

public class FormAccessibilityTests
{
    [AvaloniaFact]
    public void Form_Inner_Control_Accessible()
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
}