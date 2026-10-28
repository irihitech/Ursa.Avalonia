using Avalonia;
using Avalonia.Headless;
using Ursa.Fluent2.Demo;

[assembly: AvaloniaTestApplication(typeof(Ursa.Themes.Fluent2.Tests.TestAppBuilder))]

namespace Ursa.Themes.Fluent2.Tests;

public static class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>().UseHeadless(new AvaloniaHeadlessPlatformOptions());
}
