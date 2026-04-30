using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class PinCodeDemoViewModel: ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "PinCode",
        Description = "PinCode is an input control for entering PIN or verification codes.",
        Breadcrumbs = ["Buttons & Inputs", "PinCode"],
        Tags = ["PinCode", "Input", "Password"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PinCodeDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/PinCodeDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public ICommand CompleteCommand { get; set; }
    [ObservableProperty] private List<Exception>? _error;

    public PinCodeDemoViewModel()
    {
        CompleteCommand = new AsyncRelayCommand<IList<string>>(OnComplete);
        Error = [new Exception("Invalid verification code")];
    }

    private async Task OnComplete(IList<string>? obj)
    {
        if (obj is null) return;
        var code = string.Join("", obj);
        await OverlayMessageBox.ShowAsync(code);
    }
}