using System.Windows.Input;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace Ursa.Demo.ViewModels;

public partial class PopConfirmDemoViewModel: ObservableObject
{
    internal WindowToastManager? ToastManager { get; set; }
    
    public ICommand ConfirmCommand { get; }
    public ICommand CancelCommand { get; }
    
    public PopConfirmDemoViewModel()
    {
        ConfirmCommand = new RelayCommand(OnConfirm);
        CancelCommand = new RelayCommand(OnCancel);
    }
    
    private void OnConfirm()
    {
        ToastManager?.Show(new Toast("Confirmed"), type: NotificationType.Success, classes: ["Light"]);
    }
    
    private void OnCancel()
    {
        ToastManager?.Show(new Toast("Canceled"), type:NotificationType.Error, classes: ["Light"]);
    }
}