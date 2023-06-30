using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class ButtonGroupDemo : UserControl
{
    public ButtonGroupDemo()
    {
        InitializeComponent();
        this.DataContext = new ButtonGroupDemoViewModel();
    }
}