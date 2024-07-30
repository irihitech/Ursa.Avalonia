using Avalonia.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class ButtonGroupDemo : UserControl
{
    public ButtonGroupDemo()
    {
        InitializeComponent();
        DataContext = new ButtonGroupDemoViewModel();
    }
}