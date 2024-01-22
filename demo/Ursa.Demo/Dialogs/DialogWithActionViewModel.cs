using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace Ursa.Demo.Dialogs;

public partial class DialogWithActionViewModel: ObservableObject, IDialogContext
{
    [ObservableProperty] private string _title;
    [ObservableProperty] private DateTime _date;
    public object? DefaultCloseResult { get; set; } = true;
    public event EventHandler<object>? Closed;
    
    public ICommand OKCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    
    public DialogWithActionViewModel()
    {
        OKCommand = new RelayCommand(OK);
        CancelCommand = new RelayCommand(Cancel);
        Title = "Please select a date";
        Date = DateTime.Now;
    }

    private void OK()
    {
        Closed?.Invoke(this, true);
    }

    private void Cancel()
    {
        Closed?.Invoke(this, false);
    }
}