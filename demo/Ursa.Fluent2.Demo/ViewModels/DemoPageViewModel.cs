using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Fluent2.Demo.Services;

namespace Ursa.Fluent2.Demo.ViewModels;

public partial class DemoPageViewModel(string pageId, IDemoInteractionService interactions) : ObservableObject
{
    public string PageId { get; } = pageId;

    public static DemoPageViewModel Create(string pageId, IDemoInteractionService interactions) => pageId switch
    {
        "Overview" => new OverviewPageViewModel(interactions),
        "AutoCompleteBox" => new AutoCompleteBoxPageViewModel(interactions),
        "MultiAutoCompleteBox" => new MultiAutoCompleteBoxPageViewModel(interactions),
        "MultiComboBox" => new MultiComboBoxPageViewModel(interactions),
        "TreeComboBox" => new TreeComboBoxPageViewModel(interactions),
        "SelectionList" => new SelectionListPageViewModel(interactions),
        "TagInput" => new TagInputPageViewModel(interactions),
        _ => new DemoPageViewModel(pageId, interactions)
    };

    [RelayCommand]
    private Task OpenSampleAsync(string name) => interactions.OpenSampleAsync(name);
}
