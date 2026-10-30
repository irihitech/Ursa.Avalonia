using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Fluent2.Demo.Services;

namespace Ursa.Fluent2.Demo.ViewModels;

public sealed record DemoTreeNode(string Name, IReadOnlyList<DemoTreeNode> Children);

public sealed partial class TreeComboBoxPageViewModel(IDemoInteractionService interactions)
    : DemoPageViewModel("TreeComboBox", interactions)
{
    public ObservableCollection<DemoTreeNode> Items { get; } =
    [
        new("Engineering", [new("Desktop", []), new("Web", []), new("Mobile", [])]),
        new("Design", [new("Interaction", []), new("Visual", [])]),
        new("Operations", [new("Support", []), new("Documentation", [])])
    ];

    [ObservableProperty]
    public partial DemoTreeNode? SelectedItem { get; set; }

    [RelayCommand]
    private void ClearSelection() => SelectedItem = null;
}
