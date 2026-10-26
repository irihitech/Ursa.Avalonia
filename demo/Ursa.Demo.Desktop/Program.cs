using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Dialogs;
using Avalonia.Media;

namespace Ursa.Demo.Desktop;

[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("macos")]
[Experimental("AVALONIA_X11_FORCE_CSD")]
class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .With(new FontManagerOptions
        {
            FontFallbacks = new[]
            {
                new FontFallback
                {
                    FontFamily = new FontFamily("Microsoft YaHei")
                }
            }
        })
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UseManagedSystemDialogs()
            .WithDataAnnotationsValidation()
            .UsePlatformDetect()
            .With(new X11PlatformOptions()
            {
                ForceDrawnDecorations = true
            })
            .LogToTrace();
}