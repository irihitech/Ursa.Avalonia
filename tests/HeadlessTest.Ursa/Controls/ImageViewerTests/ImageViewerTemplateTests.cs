using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.VisualTree;
using HeadlessTest.Ursa.TestHelpers;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.ImageViewerTests;

public class ImageViewerTemplateTests
{
    [AvaloniaFact]
    public void ImageViewer_Should_Load_Template_With_PART_Image()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        var presenter = viewer.GetTemplateChildOfType<ImageViewerPresenter>(ImageViewer.PART_Image);
        Assert.NotNull(presenter);
    }

    [AvaloniaFact]
    public void ImageViewer_Should_Load_Template_With_PART_Layer()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        var layer = viewer.GetTemplateDescendants()
            .OfType<VisualLayerManager>()
            .FirstOrDefault(a => a.Name == ImageViewer.PART_Layer);
        Assert.NotNull(layer);
    }

    [AvaloniaFact]
    public void ImageViewer_Template_Should_Bind_Source_Property()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        var presenter = viewer.GetTemplateChildOfType<ImageViewerPresenter>(ImageViewer.PART_Image);
        Assert.NotNull(presenter);

        // Source should be propagated to presenter (both null initially)
        Assert.Null(presenter.Source);
    }

    [AvaloniaFact]
    public void ImageViewer_Template_Should_Bind_Zoom_Property()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        var presenter = viewer.GetTemplateChildOfType<ImageViewerPresenter>(ImageViewer.PART_Image);
        Assert.NotNull(presenter);

        viewer.Zoom = 2.0;
        Assert.Equal(2.0, presenter.Zoom);
    }

    [AvaloniaFact]
    public void ImageViewer_Template_Should_Bind_OffsetX_Property()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        var presenter = viewer.GetTemplateChildOfType<ImageViewerPresenter>(ImageViewer.PART_Image);
        Assert.NotNull(presenter);

        viewer.OffsetX = 50.0;
        Assert.Equal(50.0, presenter.OffsetX);
    }

    [AvaloniaFact]
    public void ImageViewer_Template_Should_Bind_OffsetY_Property()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        var presenter = viewer.GetTemplateChildOfType<ImageViewerPresenter>(ImageViewer.PART_Image);
        Assert.NotNull(presenter);

        viewer.OffsetY = 75.0;
        Assert.Equal(75.0, presenter.OffsetY);
    }

    [AvaloniaFact]
    public void ImageViewer_Template_Should_Bind_Stretch_Property()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        var presenter = viewer.GetTemplateChildOfType<ImageViewerPresenter>(ImageViewer.PART_Image);
        Assert.NotNull(presenter);

        Assert.Equal(viewer.Stretch, presenter.Stretch);

        viewer.Stretch = Stretch.Fill;
        Assert.Equal(Stretch.Fill, presenter.Stretch);
    }

    [AvaloniaFact]
    public void ImageViewer_Template_Should_Bind_BlendMode_Property()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        var presenter = viewer.GetTemplateChildOfType<ImageViewerPresenter>(ImageViewer.PART_Image);
        Assert.NotNull(presenter);

        viewer.BlendMode = BitmapBlendingMode.Screen;
        Assert.Equal(BitmapBlendingMode.Screen, presenter.BlendMode);
    }

    [AvaloniaFact]
    public void ImageViewer_Should_Apply_Background_From_Theme()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        // Background should be set by theme (SemiGrey1)
        Assert.NotNull(viewer.Background);
    }
}
