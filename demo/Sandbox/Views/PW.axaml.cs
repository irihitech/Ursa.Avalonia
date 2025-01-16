using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Irihi.Avalonia.Shared.Contracts;
using Sandbox.ViewModels;
using Ursa.Controls;

namespace Sandbox.Views;

public partial class PW : UserControl
{
    public PW()
    {
        InitializeComponent();
        _overlayDialogHost.HostId = _hostid;
    }

    private string _hostid = Path.GetRandomFileName();

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
       Drawer.ShowCustom(new PW(), new TestVM(), _hostid);
    }

    private void Close(object? sender, RoutedEventArgs e)
    {
        (DataContext as TestVM)?.Close();
    }
}

public class TestVM : ViewModelBase, IDialogContext
{
    public void Close()
    {
        RequestClose?.Invoke(this, 12456789);
    }

    public event EventHandler<object?>? RequestClose;
}