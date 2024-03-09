using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace Ursa.Demo.ViewModels;

public class VerificationCodeDemoViewModel: ObservableObject
{
    public ICommand CompleteCommand { get; set; }

    public VerificationCodeDemoViewModel()
    {
        CompleteCommand = new AsyncRelayCommand<IList<string>>(OnComplete);
    }

    private async Task OnComplete(IList<string>? obj)
    {
        if (obj is null) return;
        var code = string.Join("", obj);
        await MessageBox.ShowOverlayAsync(code);
    }
}