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

    private string? _text;
    public string? Text
    {
        get => _text;
        set => SetProperty(ref _text, value);
    }

    public PlainDialogViewModel()
    {
        Text = "I am PlainDialogViewModel!";
    }
}
