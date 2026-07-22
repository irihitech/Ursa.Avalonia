using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class ImageViewerDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ImageViewer,
        Description = LanguageManager.Instance.Page_Description_ImageViewer,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ImageViewer)],
        Tags = ["ImageViewer", "Image", "Zoom"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ImageViewerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/ImageViewerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    [ObservableProperty] public partial IImage? Source { get; set; }
    public AsyncRelayCommand<IStorageFile[]> OpenFileCommand { get; set; }
    public RelayCommand ResetImageCommand { get; set; }

    public ImageViewerDemoViewModel()
    {
        OpenFileCommand = new AsyncRelayCommand<IStorageFile[]>(OnOpenFile);
        ResetImageCommand = new RelayCommand(() =>
        {
            Source = CreateDemoImage();
        });
        Source = CreateDemoImage();
    }

    private async Task OnOpenFile(IStorageFile[]? obj)
    {
        if (obj == null || obj.Length == 0) return;
        var stream = await obj.First().OpenReadAsync();
        var bitmap = new Bitmap(stream);
        Source = bitmap;
    }
    
    private static WriteableBitmap CreateDemoImage()
    {
        const int width = 600;
        const int height = 400;
        var bmp = new WriteableBitmap(
            new Avalonia.PixelSize(width, height),
            new Avalonia.Vector(96, 96),
            Avalonia.Platform.PixelFormat.Rgba8888);

        using var fb = bmp.Lock();
        unsafe
        {
            var pixels = (uint*)fb.Address;
            var stride = fb.RowBytes / 4;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var r = (byte)(255 * x / width);
                    var g = (byte)(128 + 127 * y / height);
                    var b = (byte)(255 * (width - x) / width);
                    var a = (byte)255;

                    // Draw a subtle checkerboard overlay so zoom/pixel-level panning is visible.
                    if (((x / 16) + (y / 16)) % 2 == 0)
                    {
                        r = (byte)(r * 0.85);
                        g = (byte)(g * 0.85);
                        b = (byte)(b * 0.85);
                    }

                    // Draw a crosshair at the centre.
                    var cx = width / 2;
                    var cy = height / 2;
                    if ((x >= cx - 2 && x <= cx + 2) || (y >= cy - 2 && y <= cy + 2))
                    {
                        r = 0; g = 0; b = 0;
                    }

                    // Draw border.
                    if (x < 1 || x >= width - 1 || y < 1 || y >= height - 1)
                    {
                        r = 40; g = 40; b = 40;
                    }

                    pixels[y * stride + x] = (uint)((a << 24) | (r << 16) | (g << 8) | b);
                }
            }
        }

        return bmp;
    }
}
