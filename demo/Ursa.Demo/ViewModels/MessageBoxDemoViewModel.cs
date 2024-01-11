using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace Ursa.Demo.ViewModels;

public class MessageBoxDemoViewModel: ObservableObject
{
    public ICommand DefaultMessageBoxCommand { get; set; }
    public ICommand OkCommand { get; set; }
    public ICommand YesNoCommand { get; set; }
    public ICommand YesNoCancelCommand { get; set; }
    public ICommand OkCancelCommand { get; set; }

    public MessageBoxDemoViewModel()
    {
        DefaultMessageBoxCommand = new AsyncRelayCommand(OnDefaultMessageAsync);
        OkCommand = new AsyncRelayCommand(OnOkAsync);
        YesNoCommand = new AsyncRelayCommand(OnYesNoAsync);
        YesNoCancelCommand = new AsyncRelayCommand(OnYesNoCancelAsync);
        OkCancelCommand = new AsyncRelayCommand(OnOkCancelAsync);
    }

    private async Task OnDefaultMessageAsync()
    {
        var result = await MessageBox.ShowAsync("Hello Message Box");
    }
    
    private async Task OnOkAsync()
    {
        var result = await MessageBox.ShowAsync("Hello Message Box", "Hello", MessageBoxButton.OK);
    }
    
    private async Task OnYesNoAsync()
    {
        var result = await MessageBox.ShowAsync("Hello Message Box", "Hello", MessageBoxButton.YesNo);
    }
    
    private async Task OnYesNoCancelAsync()
    {
        var result = await MessageBox.ShowAsync("Hello Message Box", "Hello", MessageBoxButton.YesNoCancel);
    }
    
    private async Task OnOkCancelAsync()
    {
        var result = await MessageBox.ShowAsync("Hello Message Box", "Hello", MessageBoxButton.OKCancel);
    }
}