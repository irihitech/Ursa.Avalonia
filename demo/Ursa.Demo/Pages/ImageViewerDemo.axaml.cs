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

    private void Button_Click(object? sender, RoutedEventArgs e)
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