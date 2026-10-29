using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Fluent2.Demo.Services;

namespace Ursa.Fluent2.Demo.ViewModels;

public sealed partial class MultiAutoCompleteBoxPageViewModel(IDemoInteractionService interactions)
    : DemoPageViewModel("MultiAutoCompleteBox", interactions)
{
    public ObservableCollection<string> Items { get; } =
        ["Avalonia", "CommunityToolkit.Mvvm", "Fluent 2", "ReactiveUI", "Ursa"];

    public ObservableCollection<string> SelectedItems { get; } = ["Avalonia"];

    [ObservableProperty]
    public partial string? SearchText { get; set; }

    [RelayCommand]
    private void ClearSelection()
    {
        SelectedItems.Clear();
        SearchText = string.Empty;
    }
}
