using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.PinCodeTests;

public class PasteTest
{
    [AvaloniaFact]
    public async void Paste_Should_Insert_Text()
    {
        var window = new Window();
        bool commandInvoked = false;
        var pinCode = new PinCode()
        {
            Count = 4,
            CompleteCommand = new RelayCommand(() => commandInvoked = true),
        };
        window.Content = pinCode;
        window.Show();
        var clipboard = window.Clipboard;
        Assert.NotNull(clipboard);
        await clipboard.SetTextAsync("abcd");
        pinCode.Focus();
        window.KeyPressQwerty(PhysicalKey.V, RawInputModifiers.Control);
        // add await for clipboard processing. 
        await Task.Delay(1);
        Assert.Equal("abcd", string.Join("", pinCode.Digits));
        Assert.True(commandInvoked);
    }
    
    [AvaloniaFact]
    public async void Paste_Should_Insert_Text_When_Text_Is_Shorter()
    {
        var window = new Window();
        bool commandInvoked = false;
        var pinCode = new PinCode()
        {
            Count = 4,
            CompleteCommand = new RelayCommand(() => commandInvoked = true),
        };
        window.Content = pinCode;
        window.Show();
        var clipboard = window.Clipboard;
        Assert.NotNull(clipboard);
        await clipboard.SetTextAsync("abc");
        pinCode.Focus();
        window.KeyPressQwerty(PhysicalKey.V, RawInputModifiers.Control);
        await Task.Delay(1);
        Assert.Equal("abc", string.Join("", pinCode.Digits));
        Assert.False(commandInvoked);
    }
    
    [AvaloniaFact]
    public async void Paste_Should_Insert_Text_When_Text_Is_Longer()
    {
        var window = new Window();
        bool commandInvoked = false;
        var pinCode = new PinCode()
        {
            Count = 4,
            CompleteCommand = new RelayCommand(() => commandInvoked = true),
        };
        window.Content = pinCode;
        window.Show();
        var clipboard = window.Clipboard;
        Assert.NotNull(clipboard);
        await clipboard.SetTextAsync("abcde");
        pinCode.Focus();
        window.KeyPressQwerty(PhysicalKey.V, RawInputModifiers.Control);
        await Task.Delay(1);
        Assert.Equal("abcd", string.Join("", pinCode.Digits));
        Assert.True(commandInvoked);
    }
    
    [AvaloniaFact]
    public async void Paste_Should_Not_Insert_Text_When_Text_Is_In_Invalid_Mode()
    {
        var window = new Window();
        var commandInvoked = false;
        var pinCode = new PinCode()
        {
            Count = 4,
            Mode = PinCodeMode.Digit,
            CompleteCommand = new RelayCommand(() => commandInvoked = true),
        };
        window.Content = pinCode;
        window.Show();
        var clipboard = window.Clipboard;
        Assert.NotNull(clipboard);
        await clipboard.SetTextAsync("abcde");
        pinCode.Focus();
        window.KeyPressQwerty(PhysicalKey.V, RawInputModifiers.Control);
        await Task.Delay(1);
        Assert.Equal("", string.Join("", pinCode.Digits));
        Assert.False(commandInvoked);
    }
}