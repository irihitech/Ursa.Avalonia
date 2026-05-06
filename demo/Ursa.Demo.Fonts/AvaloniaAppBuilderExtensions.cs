using Avalonia;
using Avalonia.Media;

namespace Ursa.Demo.Fonts;

public static class AvaloniaAppBuilderExtensions
{
    public static AppBuilder WithSourceHanSansCNFont(this AppBuilder builder)
    {
        const string uri = "avares://Ursa.Demo.Fonts/Assets#Source Han Sans CN";
        return builder.With(new FontManagerOptions
        {
            DefaultFamilyName = uri, FontFallbacks = [new FontFallback { FontFamily = new FontFamily(uri) }]
        });
    }
}
