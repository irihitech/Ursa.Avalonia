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
public class UrsaSemiTheme : Styles
{
    private static readonly Dictionary<CultureInfo, ResourceDictionary> LocaleToResource = new()
    {
        { new CultureInfo("zh-CN"), new zh_cn() },
        { new CultureInfo("en-US"), new en_us() },
        { new CultureInfo("fr-FR"), new fr_fr() },
        { new CultureInfo("ru-RU"), new ru_ru() },
    };

    private static readonly ResourceDictionary DefaultResource = new zh_cn();

    public UrsaSemiTheme(IServiceProvider? provider = null)
    {
        AvaloniaXamlLoader.Load(provider, this);
        Resources.MergedDictionaries.Add(new SizeAnimations.DefaultSizeAnimations());
        Resources.MergedDictionaries.Add(new SizeAnimations.NavMenuSizeAnimations());
    }

    public CultureInfo? Locale
    {
        get;
        set
        {
            try
            {
                if (TryGetLocaleResource(value, out var resource) && resource is not null)
                {
                    field = value;
                    SetResources(this.Resources, resource);
                }
                else
                {
                    field = new CultureInfo("zh-CN");
                    SetResources(Resources, DefaultResource);
                }
            }
            catch
            {
                field = CultureInfo.InvariantCulture;
            }
        }
    }

    private static bool TryGetLocaleResource(CultureInfo? locale, out ResourceDictionary? resourceDictionary)
    {
        if (Equals(locale, CultureInfo.InvariantCulture))
        {
            resourceDictionary = DefaultResource;
            return true;
        }

        if (locale is null)
        {
            resourceDictionary = DefaultResource;
            return false;
        }

        if (LocaleToResource.TryGetValue(locale, out var resource))
        {
            resourceDictionary = resource;
            return true;
        }

        resourceDictionary = DefaultResource;
        return false;
    }

    public static void OverrideLocaleResources(Application application, CultureInfo? culture)
    {
        if (culture is null) return;
        if (!LocaleToResource.TryGetValue(culture, out var resources)) return;
        SetResources(application.Resources, resources);
    }

    public static void OverrideLocaleResources(StyledElement element, CultureInfo? culture)
    {
        if (culture is null) return;
        if (!LocaleToResource.TryGetValue(culture, out var resources)) return;
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
