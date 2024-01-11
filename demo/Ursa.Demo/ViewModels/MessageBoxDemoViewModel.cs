using System;
using System.Collections.ObjectModel;
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
    
    public ObservableCollection<MessageBoxIcon> Icons { get; set; }
    
    private MessageBoxIcon _selectedIcon;
    public MessageBoxIcon SelectedIcon
    {
        get => _selectedIcon;
        set => SetProperty(ref _selectedIcon, value);
    }

    private MessageBoxResult _result;
    public MessageBoxResult Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    public MessageBoxDemoViewModel()
    {
        DefaultMessageBoxCommand = new AsyncRelayCommand(OnDefaultMessageAsync);
        OkCommand = new AsyncRelayCommand(OnOkAsync);
        YesNoCommand = new AsyncRelayCommand(OnYesNoAsync);
        YesNoCancelCommand = new AsyncRelayCommand(OnYesNoCancelAsync);
        OkCancelCommand = new AsyncRelayCommand(OnOkCancelAsync);
        Icons = new ObservableCollection<MessageBoxIcon>(
            Enum.GetValues<MessageBoxIcon>());
        SelectedIcon = MessageBoxIcon.None;
    }

    private async Task OnDefaultMessageAsync()
    {
        Result = await MessageBox.ShowAsync("Hello Message Box", icon: SelectedIcon);
    }
    
    private async Task OnOkAsync()
    {
        Result = await MessageBox.ShowAsync("Hello Message Box", "Hello", icon: SelectedIcon, button:MessageBoxButton.OK);
    }
    
    private async Task OnYesNoAsync()
    {
        Result = await MessageBox.ShowAsync("Hello Message Box", "Hello", icon: SelectedIcon, button: MessageBoxButton.YesNo);
    }
    
    private async Task OnYesNoCancelAsync()
    {
        Result = await MessageBox.ShowAsync("Hello Message Box", "Hello", icon: SelectedIcon, button: MessageBoxButton.YesNoCancel);
    }
    
    private async Task OnOkCancelAsync()
    {
        Result = await MessageBox.ShowAsync("Hello Message Box", "Hello", icon: SelectedIcon, button:MessageBoxButton.OKCancel);
    }
}