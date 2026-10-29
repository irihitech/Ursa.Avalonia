using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Fluent2.Demo.Services;

namespace Ursa.Fluent2.Demo.ViewModels;

public sealed partial class TagInputPageViewModel(IDemoInteractionService interactions)
    : DemoPageViewModel("TagInput", interactions)
{
    public ObservableCollection<string> Tags { get; } = ["Avalonia", "Fluent 2"];

    [RelayCommand]
    private void ClearTags() => Tags.Clear();
}
