using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Avalonia.Shared.Contracts;

namespace HeadlessTest.Ursa.Controls.FormTests.Dynamic_Item_Generation;

public class TestViewModel
{
    public ObservableCollection<IFormElement> Items { get; set; } = [];
}

public interface IFormElement
{
    
}

public interface IFormGroupViewModel : IFormGroup, IFormElement
{
    public string? Title { get; set; }
    public ObservableCollection<IFromItemViewModel> Items { get; set; }
}

public interface IFromItemViewModel: IFormElement
{
    public string? Label { get; set; }
}

public partial class FormGroupViewModel : ObservableObject, IFormGroupViewModel
{
    [ObservableProperty] public partial string? Title { get; set; }
    public ObservableCollection<IFromItemViewModel> Items { get; set; } = [];
}

public partial class FormTextViewModel : ObservableObject, IFromItemViewModel
{
    [ObservableProperty] public partial string? Label { get; set; }
    [ObservableProperty] public partial string? Value { get; set; }
}

public partial class FormAgeViewModel : ObservableObject, IFromItemViewModel
{
    [ObservableProperty] public partial uint? Age { get; set; }
    [ObservableProperty] public partial string? Label { get; set; }
}

public partial class FormDateRangeViewModel : ObservableObject, IFromItemViewModel
{
    [ObservableProperty] public partial DateTime? End { get; set; }
    [ObservableProperty] public partial string? Label { get; set; }
    [ObservableProperty] public partial DateTime? Start { get; set; }
}