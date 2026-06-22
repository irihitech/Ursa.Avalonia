using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Ursa.Demo.Pages;

public partial class ImageViewerDemo : UserControl
{
    public ImageViewerDemo()
    {
        InitializeComponent();
    }

    IImage? oldImg;

    private void ResetView_Click(object? sender, RoutedEventArgs e)
    {
        Viewer.FitToView();
    }
}
