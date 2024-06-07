using System.Globalization;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Ursa.Themes.Semi;

/// <summary>
/// Notice: Don't set Locale if your app is in InvariantGlobalization mode.
/// </summary>
public class SemiTheme: Styles
{
    private static readonly Lazy<Dictionary<CultureInfo, string>> _localeToResource = new Lazy<Dictionary<CultureInfo, string>>(
        () => new Dictionary<CultureInfo, string>
        {
            { new CultureInfo("zh-CN"), "avares://Ursa.Themes.Semi/Locale/zh-CN.axaml" },
            { new CultureInfo("en-US"), "avares://Ursa.Themes.Semi/Locale/en-US.axaml" },
        });
    
    private static readonly string _defaultResource = "avares://Ursa.Themes.Semi/Locale/zh-CN.axaml";
    
    private readonly IServiceProvider? sp;
    public SemiTheme(IServiceProvider? provider = null)
    {
        sp = provider;
        AvaloniaXamlLoader.Load(provider, this);
    }

    private CultureInfo? _locale;
    public CultureInfo? Locale
    {
        get => _locale;
        set
        {
            try
            {
                _locale = value;
                var resource = TryGetLocaleResource(value);
                var d = AvaloniaXamlLoader.Load(sp, new Uri(resource)) as ResourceDictionary;
                if (d is null) return;
                foreach (var kv in d)
                {
                    this.Resources.Add(kv);
                }
            }
            catch
            {
                _locale = CultureInfo.InvariantCulture;
            }
            
        }
    }
    
    private static string TryGetLocaleResource(CultureInfo? locale)
    {
        if (Equals(locale, CultureInfo.InvariantCulture))
        {
            return _defaultResource;
        }
        if (locale is null)
        {
            return _localeToResource.Value[new CultureInfo("zh-CN")];
        }
        if (_localeToResource.Value.TryGetValue(locale, out var resource))
        {
            return resource;
        }
        return _localeToResource.Value[new CultureInfo("zh-CN")];
    }
}