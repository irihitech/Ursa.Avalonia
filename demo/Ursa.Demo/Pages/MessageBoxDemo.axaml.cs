using Avalonia.Controls;
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