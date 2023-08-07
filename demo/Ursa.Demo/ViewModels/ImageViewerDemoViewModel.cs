using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;

namespace Ursa.Demo.ViewModels;

public partial class ImageViewerDemoViewModel : ObservableObject
{
    public ICommand CutCommand { get; set; }
    public ICommand ResetCommand { get; set; }
    [ObservableProperty] public double _translateX;
    [ObservableProperty] public double _translateY;
    [ObservableProperty] public double _scale;
    [ObservableProperty] public IImage? _source;

    public ImageViewerDemoViewModel()
    {
        Reset();
        CutCommand = new RelayCommand(Cut);
        ResetCommand = new RelayCommand(Reset);
    }

    private void Reset()
    {
        Source = new Bitmap("C:/Projects/irihi/Ursa.Avalonia/demo/Ursa.Demo/Assets/WORLD.png");
        Scale = 1d;
        TranslateX = 0;
        TranslateY = 0;
    }

    private void Cut()
    {
        using var skData =
            SKData.CreateCopy(File.ReadAllBytes("C:/Projects/irihi/Ursa.Avalonia/demo/Ursa.Demo/Assets/WORLD.png")
                .ToArray());
        SKBitmap bitmap = SKBitmap.Decode(skData);
        SKBitmap newb = new SKBitmap();

        var x = Source.Size.Width / 2 - TranslateX / Scale;
        var y = Source.Size.Height / 2 - TranslateY / Scale;

        var left = (int)(x - 90 / Scale);
        var top = (int)(y - 160 / Scale);
        var right = (int)(x + 90 / Scale);
        var bottom = (int)(y + 160 / Scale);
        if (!bitmap.ExtractSubset(newb, new SKRectI(left, top, right, bottom)))
            return;


        SKData d = newb.Encode(SKEncodedImageFormat.Png, 1);

        Bitmap newBitmap = new Bitmap(new MemoryStream(d.ToArray()));
        var temp = Scale;
        Source = newBitmap;


        Scale = temp;
        TranslateX = 0;
        TranslateY = 0;
    }
}