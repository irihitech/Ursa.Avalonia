using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Fluent2.Demo.Services;

namespace Ursa.Fluent2.Demo.ViewModels;

public sealed partial class SelectionListPageViewModel(IDemoInteractionService interactions)
    : DemoPageViewModel("SelectionList", interactions)
{
    public ObservableCollection<string> Items { get; } = ["Overview", "Activity", "Members", "Settings"];

    [ObservableProperty]
    public partial string? SelectedItem { get; set; } = "Overview";

    [RelayCommand]
    private void ClearSelection() => SelectedItem = null;
}
