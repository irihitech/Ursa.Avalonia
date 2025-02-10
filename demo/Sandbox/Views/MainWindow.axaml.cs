using Avalonia.Controls;
using Avalonia.Interactivity;
using Ursa.Controls;

namespace Sandbox.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (content.Content is int s)
        {
            content.Content = 1.1;
        }
        else
        {
            content.Content = 1;
        }
    }
}