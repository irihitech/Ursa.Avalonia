using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Ursa.Demo.ViewModels;

public partial class BadgeDemoViewModel: ViewModelBase
{
    [ObservableProperty] private string? _text = null;

    public BadgeDemoViewModel()
    {
        
    }

    [RelayCommand]
    public void ChangeText()
    {
        if (Text == null)
        {
            Text = DateTime.Now.ToShortDateString();
        }
        else
        {
            Text = null;
        }
    }
}