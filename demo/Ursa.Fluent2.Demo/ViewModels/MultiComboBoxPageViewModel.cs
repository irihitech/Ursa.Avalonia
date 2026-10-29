using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Fluent2.Demo.Services;

namespace Ursa.Fluent2.Demo.ViewModels;

public sealed partial class MultiComboBoxPageViewModel(IDemoInteractionService interactions)
    : DemoPageViewModel("MultiComboBox", interactions)
{
    public ObservableCollection<string> Items { get; } =
        ["Design", "Development", "Documentation", "Testing", "Support"];

    public ObservableCollection<string> SelectedItems { get; } = ["Development", "Testing"];

    [RelayCommand]
    private void ClearSelection() => SelectedItems.Clear();
}
