using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HeadlessTest.Ursa.Controls.FormTests.AccessibilityTests;

public partial class DynamicForm : UserControl
{
    public DynamicForm()
    {
        InitializeComponent();
        this.DataContext = new DynamicFormViewModel();
    }
}

public partial class DynamicFormViewModel: ObservableObject
{
    public ObservableCollection<FormTextViewModel> Items { get; set; } =
    [
        new() { Label = "_Name" },
        new() { Label = "_Email" }
    ];
}

public partial class FormTextViewModel : ObservableObject
{
    [ObservableProperty] public partial string? Label { get; set; }
    [ObservableProperty] public partial string? Value { get; set; }
}