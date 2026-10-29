using Avalonia;
using Avalonia.Media;

namespace Ursa.Fluent2.Demo;

public static class DemoFonts
{
    public const string CjkFamily = "avares://Ursa.Fluent2.Demo/Assets/Fonts#Source Han Sans CN";

    public static AppBuilder WithDemoFonts(this AppBuilder builder) =>
        builder.With(new FontManagerOptions
        {
            FontFallbacks = [new FontFallback { FontFamily = new FontFamily(CjkFamily) }]
        });
}
