using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Ursa.Demo.ViewModels;

public partial class AvatarDemoViewModel : ViewModelBase
{
    [ObservableProperty] private string _content = "AS";
    [ObservableProperty] private bool _canClick = true;

    [RelayCommand(CanExecute = nameof(CanClick))]
    private void Click()
    {
        Content = "BM";
    }

    public ObservableCollection<string> Items => ["AS", "BM", "TJ", "ZL", "YZ"];
}