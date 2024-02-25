using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class NumericUpDownDemo : UserControl
{
    public NumericUpDownDemo()
    {
        InitializeComponent();
        DataContext = new NumericUpDownDemoViewModel();
    }
}