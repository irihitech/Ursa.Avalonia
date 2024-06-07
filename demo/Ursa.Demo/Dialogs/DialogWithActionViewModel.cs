using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Avalonia.Shared.Contracts;
using Ursa.Controls;

namespace Ursa.Demo.Dialogs;

public partial class DialogWithActionViewModel: ObservableObject, IDialogContext
{
    [ObservableProperty] private string _title;
    [ObservableProperty] private DateTime _date;

    public void Close()
    {
        RequestClose?.Invoke(this, false);
    }

    public event EventHandler<object?>? RequestClose;
    
    public ICommand OKCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    
    public ICommand DialogCommand { get; set; }
    
    public DialogWithActionViewModel()
    {
        OKCommand = new RelayCommand(OK);
        CancelCommand = new RelayCommand(Cancel);
        DialogCommand = new AsyncRelayCommand(ShowDialog);
        Title = "Please select a date";
        Date = DateTime.Now;
    }

    private void OK()
    {
        RequestClose?.Invoke(this, true);
    }

    private void Cancel()
    {
        RequestClose?.Invoke(this, false);
    }
    
    private async Task ShowDialog()
    {
        await OverlayDialog.ShowCustomModal<DialogWithAction, DialogWithActionViewModel, bool>(new DialogWithActionViewModel());
    }
}