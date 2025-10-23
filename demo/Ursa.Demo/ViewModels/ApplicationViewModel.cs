using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Ursa.Demo.ViewModels;

public partial class ApplicationViewModel : ObservableObject
{
    [RelayCommand]
    private void JumpTo(string header)
    {
        WeakReferenceMessenger.Default.Send(header, "JumpTo");
    }
}