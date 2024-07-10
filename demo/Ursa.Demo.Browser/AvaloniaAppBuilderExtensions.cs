using Avalonia;
using Avalonia.Media;

namespace Ursa.Demo.Browser;

public static class AvaloniaAppBuilderExtensions
{
    private static string DefaultFontFamily => "avares://Ursa.Demo.Browser/Assets#Source Han Sans CN";

    public static AppBuilder WithSourceHanSansCNFont(this AppBuilder builder) =>
        builder.With(new FontManagerOptions
        {
            DefaultFamilyName = DefaultFontFamily,
            FontFallbacks = new[] { new FontFallback { FontFamily = new FontFamily(DefaultFontFamily) } }
        });
}