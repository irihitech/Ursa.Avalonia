using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ursa.Controls;

namespace Ursa.Demo.Pages;

public partial class VerificationCodeDemo : UserControl
{
    public VerificationCodeDemo()
    {
        InitializeComponent();
    }

    private async void VerificationCode_OnComplete(object? sender, VerificationCodeCompleteEventArgs e)
    {
        var text = string.Join(string.Empty, e.Code);
        await MessageBox.ShowOverlayAsync(text);
    }
}