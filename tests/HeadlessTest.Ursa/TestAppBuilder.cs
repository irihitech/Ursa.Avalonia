using Avalonia;
using Avalonia.Headless;
using HeadlessTest.Ursa;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]

namespace HeadlessTest.Ursa;

public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>().UseHeadless(new AvaloniaHeadlessPlatformOptions());
}