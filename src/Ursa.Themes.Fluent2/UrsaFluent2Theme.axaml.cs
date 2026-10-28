using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Themes.Fluent2;
using Ursa.Themes.Fluent2.Locale;
using Ursa.Themes.Fluent2.SizeAnimations;

namespace Ursa.Themes.Fluent2;

/// <summary>Fluent 2 theme resources and styles for Ursa controls.</summary>
public partial class UrsaFluent2Theme : Styles, IResourceNode
{
    private static readonly Dictionary<CultureInfo, ResourceDictionary> LocaleToResource = new()
    {
        { new CultureInfo("zh-CN"), new zh_cn() },
        { new CultureInfo("en-US"), new en_us() },
        { new CultureInfo("de-DE"), new de_de() },
        { new CultureInfo("fr-FR"), new fr_fr() },
        { new CultureInfo("ru-RU"), new ru_ru() },
        { new CultureInfo("pl-PL"), new pl_pl() },
        { new CultureInfo("cs-CZ"), new cs_cz() },
        { new CultureInfo("ko-KR"), new ko_kr() },
        { new CultureInfo("ja-JP"), new ja_jp() },
    };

    private static readonly ResourceDictionary DefaultLocale = new zh_cn();
    private readonly ResourceDictionary _compactStyles;
    private DensityStyle _densityStyle;
    private CultureInfo? _locale;

    public UrsaFluent2Theme(IServiceProvider? serviceProvider = null)
    {
        AvaloniaXamlLoader.Load(serviceProvider, this);
        _compactStyles = (ResourceDictionary)(Resources["CompactStyles"]
            ?? throw new InvalidOperationException("The CompactStyles resource is missing."));
        Resources.Remove("CompactStyles");
        Resources.MergedDictionaries.Add(new DefaultSizeAnimations());
        Resources.MergedDictionaries.Add(new NavMenuSizeAnimations());
    }

    public static readonly DirectProperty<UrsaFluent2Theme, DensityStyle> DensityStyleProperty =
        AvaloniaProperty.RegisterDirect<UrsaFluent2Theme, DensityStyle>(
            nameof(DensityStyle), theme => theme.DensityStyle, (theme, value) => theme.DensityStyle = value);

    /// <summary>Gets or sets whether Ursa uses normal or compact Fluent 2 dimensions.</summary>
    public DensityStyle DensityStyle
    {
        get => _densityStyle;
        set => SetAndRaise(DensityStyleProperty, ref _densityStyle, value);
    }

    /// <summary>Gets or sets the Ursa control localization. Defaults to Simplified Chinese.</summary>
    public CultureInfo? Locale
    {
        get => _locale;
        set
        {
            if (value is not null && LocaleToResource.TryGetValue(value, out var resources))
            {
                _locale = value;
                SetResources(Resources, resources);
                return;
            }

            _locale = CultureInfo.GetCultureInfo("zh-CN");
            SetResources(Resources, DefaultLocale);
        }
    }

    public static void OverrideLocaleResources(Application application, CultureInfo? culture)
    {
        if (culture is not null && LocaleToResource.TryGetValue(culture, out var resources))
            SetResources(application.Resources, resources);
    }

    public static void OverrideLocaleResources(StyledElement element, CultureInfo? culture)
    {
        if (culture is not null && LocaleToResource.TryGetValue(culture, out var resources))
            SetResources(element.Resources, resources);
    }

    private static void SetResources(IResourceDictionary target, IResourceDictionary source)
    {
        if (target is ResourceDictionary dictionary)
            dictionary.SetItems(source);
        else
            foreach (var (key, value) in source)
                target[key] = value;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == DensityStyleProperty)
            Owner?.NotifyHostedResourcesChanged(ResourcesChangedEventArgs.Create());
    }

    bool IResourceNode.TryGetResource(object key, ThemeVariant? theme, out object? value)
    {
        if (_densityStyle == DensityStyle.Compact && _compactStyles.TryGetResource(key, theme, out value))
            return true;
        return base.TryGetResource(key, theme, out value);
    }
}
