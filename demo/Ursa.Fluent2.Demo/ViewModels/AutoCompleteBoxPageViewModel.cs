using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Fluent2.Demo.Services;

namespace Ursa.Fluent2.Demo.ViewModels;

public sealed partial class AutoCompleteBoxPageViewModel(IDemoInteractionService interactions)
    : DemoPageViewModel("AutoCompleteBox", interactions)
{
    public ObservableCollection<string> Items { get; } =
        ["Amsterdam", "Berlin", "London", "Paris", "Shanghai", "Tokyo"];

    [ObservableProperty]
    public partial string? SelectedItem { get; set; }

    [ObservableProperty]
    public partial string? SearchText { get; set; }

    [RelayCommand]
    private void ClearSelection()
    {
        SelectedItem = null;
        SearchText = string.Empty;
    }
}
