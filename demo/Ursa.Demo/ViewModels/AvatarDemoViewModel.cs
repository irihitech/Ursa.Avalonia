using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace Ursa.Demo.ViewModels;

public partial class AvatarDemoViewModel : ViewModelBase
{
    [ObservableProperty] private string _content = "AS";

    public ObservableCollection<string> Items => ["AS", "BM", "TJ", "ZL", "YZ"];

    public ObservableCollection<Person> People =>
    [
        new()
        {
            Name = "AS",
        },
        new()
        {
            Name = "BM",
        },
        new()
        {
            Name = "TJ",
        },
        new()
        {
            Name = "ZL",
        },
        new()
        {
            Name = "YZ",
        },
    ];

    [RelayCommand]
    private void Click()
    {
        Content = "BM";
    }

    [RelayCommand]
    private async Task Show(string name)
    {
        await MessageBox.ShowOverlayAsync($"Person '{name}' is clicked!");
    }
}

public partial class Person : ObservableObject
{
    [ObservableProperty] private string? _name;
}