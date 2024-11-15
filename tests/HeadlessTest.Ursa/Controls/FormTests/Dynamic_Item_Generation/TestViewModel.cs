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
    [ObservableProperty] private string? _title;
    public ObservableCollection<IFromItemViewModel> Items { get; set; } = [];
}

public partial class FormTextViewModel : ObservableObject, IFromItemViewModel
{
    [ObservableProperty] private string? _label;
    [ObservableProperty] private string? _value;
}

public partial class FormAgeViewModel : ObservableObject, IFromItemViewModel
{
    [ObservableProperty] private uint? _age;
    [ObservableProperty] private string? _label;
}

public partial class FormDateRangeViewModel : ObservableObject, IFromItemViewModel
{
    [ObservableProperty] private DateTime? _end;
    [ObservableProperty] private string? _label;
    [ObservableProperty] private DateTime? _start;
}