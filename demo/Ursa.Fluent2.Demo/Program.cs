using Avalonia;

namespace Ursa.Fluent2.Demo;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
    {
        var builder = AppBuilder.Configure<App>().UsePlatformDetect().WithDemoFonts().LogToTrace();
#if DEBUG
        builder = builder.WithDeveloperTools();
#endif
        return builder;
    }
}
