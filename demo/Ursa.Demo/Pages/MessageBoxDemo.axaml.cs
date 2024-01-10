using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class MessageBoxDemo : UserControl
{
    public MessageBoxDemo()
    {
        InitializeComponent();
        this.DataContext = new MessageBoxDemoViewModel();
    }
}