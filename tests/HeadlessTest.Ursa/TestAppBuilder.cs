using Avalonia;
using Avalonia.Headless;
using HeadlessTest.Ursa;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]

namespace HeadlessTest.Ursa;

public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder
            .Configure<App>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}

public class SkiaTestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder
            .Configure<App>()
            .UseSkia()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions(){ UseHeadlessDrawing = false});
}

