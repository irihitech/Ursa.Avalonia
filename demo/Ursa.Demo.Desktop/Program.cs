using System;
using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Dialogs;
using Avalonia.Media;
using Ursa.Demo.Fonts;

namespace Ursa.Demo.Desktop;
#pragma warning disable AVALONIA_X11_CSD, AVALONIA_X11_FORCE_CSD

[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("macos")]
class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UseManagedSystemDialogs()
            .UsePlatformDetect()
            .With(new Win32PlatformOptions())
            .With(new X11PlatformOptions { EnableDrawnDecorations = true, ForceDrawnDecorations = true })
            .WithSourceHanSansCNFont()
            .LogToTrace();
}
