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
    private readonly string _longMessage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

    private readonly string _shortMessage = "Welcome to Ursa Avalonia!";
    private string _message;
    private string? _title;

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

    private bool _useLong;

    public bool UseLong
    {
        get => _useLong;
        set
        {
            SetProperty(ref _useLong, value);
            _message = value ? _longMessage : _shortMessage;
        }
    }

    private bool _useTitle;

    public bool UseTitle
    {
        get => _useTitle;
        set
        {
            SetProperty(ref _useTitle, value);
            _title = value ? "Ursa MessageBox" : string.Empty;
        }
    }

    private bool _useOverlay;

    public bool UseOverlay
    {
        get => _useOverlay;
        set => SetProperty(ref _useOverlay, value);
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
        _message = _shortMessage;
    }

    private async Task OnDefaultMessageAsync()
    {
        if (UseOverlay)
        {
            Result = await MessageBox.ShowOverlayAsync(_message, _title, icon: SelectedIcon);
        }
        else
        {
            Result = await MessageBox.ShowAsync(_message, _title, icon: SelectedIcon);
        }
        
    }
    
    private async Task OnOkAsync()
    {
        await Show(MessageBoxButton.OK);
    }
    
    private async Task OnYesNoAsync()
    {
        await Show(MessageBoxButton.YesNo);
    }
    
    private async Task OnYesNoCancelAsync()
    {
        await Show(MessageBoxButton.YesNoCancel);
    }

    private async Task OnOkCancelAsync()
    {
        await Show(MessageBoxButton.OKCancel);
    }

    private async Task Show(MessageBoxButton button)
    {
        if (UseOverlay)
        {
            Result = await MessageBox.ShowOverlayAsync(_message, _title, icon: SelectedIcon, button:button);
        }
        else
        {
            Result = await MessageBox.ShowAsync(_message, _title, icon: SelectedIcon, button:button);
        }
    }
}