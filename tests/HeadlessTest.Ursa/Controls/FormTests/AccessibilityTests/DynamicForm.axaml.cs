using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
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
    [ObservableProperty] private string? _label;
    [ObservableProperty] private string? _value;
}