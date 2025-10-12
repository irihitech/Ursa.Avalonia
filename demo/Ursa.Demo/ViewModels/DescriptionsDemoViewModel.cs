using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class DescriptionsDemoViewModel: ObservableObject
{
    public ObservableCollection<DescriptionItemViewModel> Items { get; set; }

    public DescriptionsDemoViewModel()
    {
        Items = new ObservableCollection<DescriptionItemViewModel>()
        {
            new()
            {
                Label = "Actual Users",
                Description = "1,480,000"
            },
            new()
            {
                Label = "7-day Retention",
                Description = "98%"
            },
            new()
            {
                Label = "Security Level",
                Description = "III"
            }
        };
    }
}

public partial class DescriptionItemViewModel : ObservableObject
{
    [ObservableProperty] private string? _label;
    [ObservableProperty] private object? _description;
}