using System.Collections.ObjectModel;
using Ursa.Fluent2.Demo.Services;

namespace Ursa.Fluent2.Demo.ViewModels;

public sealed class OverviewPageViewModel(IDemoInteractionService interactions)
    : DemoPageViewModel("Overview", interactions)
{
    public ObservableCollection<string> Technologies { get; } =
        ["Avalonia", "CommunityToolkit.Mvvm", "Fluent 2", "ReactiveUI", "Ursa"];

    public ObservableCollection<string> SelectedTechnologies { get; } = ["Avalonia"];

    public ObservableCollection<string> Tags { get; } = ["Fluent 2", "Cross-platform"];
}
