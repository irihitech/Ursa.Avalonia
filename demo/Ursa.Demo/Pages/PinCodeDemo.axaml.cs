using Avalonia.Controls;
using Ursa.Controls;

namespace Ursa.Demo.Pages;

public partial class PinCodeDemo : UserControl
{
    public PinCodeDemo()
    {
        InitializeComponent();
    }

    private async void VerificationCode_OnComplete(object? _, PinCodeCompleteEventArgs e)
    {
        var text = string.Join(string.Empty, e.Code);
        await MessageBox.ShowOverlayAsync(text);
    }
}