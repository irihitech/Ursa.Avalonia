using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace Ursa.Demo.ViewModels;

public class MessageBoxDemoViewModel: ObservableObject
{
    public ICommand DefaultMessageBoxCommand { get; set; }

    public MessageBoxDemoViewModel()
    {
        DefaultMessageBoxCommand = new AsyncRelayCommand(OnDefaultMessageAsync);
    }

    private async Task OnDefaultMessageAsync()
    {
        await MessageBox.ShowAsync("Hello Message Box");
    }
}