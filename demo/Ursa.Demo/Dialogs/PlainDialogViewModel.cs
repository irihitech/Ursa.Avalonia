using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.Dialogs;

public class PlainDialogViewModel: ObservableObject
{
    private DateTime? _date;

    public DateTime? Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }
}