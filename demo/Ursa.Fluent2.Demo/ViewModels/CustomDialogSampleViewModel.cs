using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Avalonia.Shared.Contracts;
using Ursa.Controls;

namespace Ursa.Fluent2.Demo.ViewModels;

public sealed partial class CustomDialogSampleViewModel : ObservableObject, IDialogContext
{
    [ObservableProperty]
    public partial string ProjectName { get; set; } = "Fluent 2 demo";

    public event EventHandler<object?>? RequestClose;

    [RelayCommand]
    private void Confirm() => Close(DialogResult.OK);

    [RelayCommand]
    private void Cancel() => Close();

    public void Close() => Close(DialogResult.Cancel);

    public void Close(object? result) => RequestClose?.Invoke(this, result);
}
