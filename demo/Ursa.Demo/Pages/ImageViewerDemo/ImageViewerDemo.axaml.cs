using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Ursa.Demo.Pages.ImageViewerDemo;

public partial class ImageViewerDemo : UserControl
{
    public ImageViewerDemo()
    {
        InitializeComponent();
    }

    private void ResetView_Click(object? sender, RoutedEventArgs e)
    {
        Viewer.FitToView();
    }
}
