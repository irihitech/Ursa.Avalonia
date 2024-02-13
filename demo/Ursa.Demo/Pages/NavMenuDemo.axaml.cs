using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class NavMenuDemo : UserControl
{
    public NavMenuDemo()
    {
        InitializeComponent();
        this.DataContext = new NavMenuDemoViewModel();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Rectangle c)
        {
            c.ApplyStyling();
            var ancestors = c.GetLogicalAncestors();
        }
    }
}