using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Ursa.Themes.Semi.Locale;

namespace Ursa.Themes.Semi;

/// <summary>
///     Notice: Don't set Locale if your app is in InvariantGlobalization mode.
/// </summary>
public class SemiTheme : Styles
{
    private static readonly Dictionary<CultureInfo, ResourceDictionary> _localeToResource = new()
    {
        { new CultureInfo("zh-CN"), new zh_cn() },
        { new CultureInfo("en-US"), new en_us() },
        { new CultureInfo("fr-FR"), new fr_fr() },
        { new CultureInfo("ru-RU"), new ru_ru() },
    };

    private static readonly ResourceDictionary _defaultResource = new zh_cn();

    private CultureInfo? _locale;

    public SemiTheme(IServiceProvider? provider = null)
    {
        AvaloniaXamlLoader.Load(provider, this);
        Resources.MergedDictionaries.Add(new SizeAnimations.DefaultSizeAnimations());
        Resources.MergedDictionaries.Add(new SizeAnimations.NavMenuSizeAnimations());
    }

    public static ThemeVariant Aquatic => new(nameof(Aquatic), ThemeVariant.Dark);
    public static ThemeVariant Desert => new(nameof(Desert), ThemeVariant.Light);
    public static ThemeVariant Dusk => new(nameof(Dusk), ThemeVariant.Dark);
    public static ThemeVariant NightSky => new(nameof(NightSky), ThemeVariant.Dark);

    public CultureInfo? Locale
    {
        get => _locale;
        set
        {
            try
            {
                if (TryGetLocaleResource(value, out var resource) && resource is not null)
                {
                    _locale = value;
                    SetResources(this.Resources, resource);
                }
                else
                {
                    _locale = new CultureInfo("zh-CN");
                    SetResources(Resources, _defaultResource);
                }
            }
            catch
            {
                _locale = CultureInfo.InvariantCulture;
            }
        }
    }

    private static bool TryGetLocaleResource(CultureInfo? locale, out ResourceDictionary? resourceDictionary)
    {
        if (Equals(locale, CultureInfo.InvariantCulture))
        {
            resourceDictionary = _defaultResource;
            return true;
        }

        if (locale is null)
        {
            resourceDictionary = _defaultResource;
            return false;
        }

        if (_localeToResource.TryGetValue(locale, out var resource))
        {
            resourceDictionary = resource;
            return true;
        }

        resourceDictionary = _defaultResource;
        return false;
    }

    public static void OverrideLocaleResources(Application application, CultureInfo? culture)
    {
        if (culture is null) return;
        if (!_localeToResource.TryGetValue(culture, out var resources)) return;
        SetResources(application.Resources, resources);
    }

    public static void OverrideLocaleResources(StyledElement element, CultureInfo? culture)
    {
        if (culture is null) return;
        if (!_localeToResource.TryGetValue(culture, out var resources)) return;
        SetResources(element.Resources, resources);
    }

    private static void SetResources(IResourceDictionary source, IResourceDictionary content)
    {
        if (source is ResourceDictionary resourceDictionary)
        {
             resourceDictionary.SetItems(content);
        }
        else
        {
            foreach (var kv in content) source[kv.Key] = kv.Value;
        }
    }
}
