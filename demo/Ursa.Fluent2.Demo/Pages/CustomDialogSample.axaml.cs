using Avalonia.Controls;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Contracts;
using Ursa.Controls;

namespace Ursa.Fluent2.Demo.Pages;

public partial class CustomDialogSample : UserControl
{
    public CustomDialogSample() => InitializeComponent();

    private void CloseDialog(object? sender, RoutedEventArgs e) =>
        ((CustomDialogSampleViewModel)DataContext!).Close(((Button)sender!).Tag);
}

public sealed class CustomDialogSampleViewModel : IDialogContext
{
    public event EventHandler<object?>? RequestClose;

    public void Close() => Close(DialogResult.Cancel);

    public void Close(object? result) => RequestClose?.Invoke(this, result);
}
