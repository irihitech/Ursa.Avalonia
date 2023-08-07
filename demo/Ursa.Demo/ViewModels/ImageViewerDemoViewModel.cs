using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;

namespace Ursa.Demo.ViewModels;

public partial class ImageViewerDemoViewModel : ObservableObject
{
    private string? _path;
    public ICommand CutCommand { get; set; }
    public ICommand ResetCommand { get; set; }
    public ICommand OpenCommand { get; set; }
    [ObservableProperty] public double _translateX;
    [ObservableProperty] public double _translateY;
    [ObservableProperty] public double _scale;
    [ObservableProperty] public IImage? _source;

    public ImageViewerDemoViewModel()
    {
        Reset();
        CutCommand = new RelayCommand(Cut);
        ResetCommand = new RelayCommand(Reset);
        OpenCommand = new AsyncRelayCommand(Open);
    }

    private void Reset()
    {
        if (_path is not null)
        {
            Source = new Bitmap(_path);
        }
        Scale = 1d;
        TranslateX = 0;
        TranslateY = 0;
    }

    private async Task Open()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow.StorageProvider: { } sp })
        {
            var files = await sp.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                AllowMultiple = false,
                FileTypeFilter = new List<FilePickerFileType>()
                {
                    new("Media file")
                    {
                        Patterns = new List<string>() { "*.jpg", "*.jpeg", "*.png", },
                    }
                }
            });
            var file = files.FirstOrDefault();
            if (file is null) return;
            _path = file.Path.LocalPath;
            Source = new Bitmap(_path);
        }
    }

    private void Cut()
    {
        if (_path is null) return;
        using var skData =
            SKData.CreateCopy(File.ReadAllBytes(_path)
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