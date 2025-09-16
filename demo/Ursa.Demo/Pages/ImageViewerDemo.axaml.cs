using Avalonia.Controls;

namespace Ursa.Demo.Pages;

public partial class ImageViewerDemo : UserControl
{
    public ImageViewerDemo()
    {
        InitializeComponent();
    }

    Avalonia.Media.IImage? oldImg;

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(viewer.Source!=null)
        {
            oldImg = viewer.Source;
            viewer.Source = null;
        }
        else
        {
            viewer.Source = oldImg;
        }
    }
}