using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Themes.Fluent2;
using Ursa.Themes.Fluent2;

namespace Ursa.Fluent2.Demo;

public sealed record DemoNavItem(string Title, DemoPage? Page, IReadOnlyList<DemoNavItem> Children)
{
    public Geometry Icon { get; } = StreamGeometry.Parse(
        "M2,2 H7 V7 H2 Z M10,2 H15 V7 H10 Z M2,10 H7 V15 H2 Z M10,10 H15 V15 H10 Z");
}

public sealed class MainViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly Application _application;
    private readonly Fluent2Theme _fluentTheme;
    private readonly UrsaFluent2Theme _ursaTheme;
    private string _searchText = string.Empty;
    private DemoNavItem? _selectedItem;
    private string _lastPageId = "IconButton";
    private int _themeIndex;
    private int _densityIndex;
    private int _languageIndex;
    private bool _updatingText;
    private bool _isCollapsed;

    public MainViewModel(Fluent2Theme fluentTheme, UrsaFluent2Theme ursaTheme)
    {
        _application = Application.Current!;
        _fluentTheme = fluentTheme;
        _ursaTheme = ursaTheme;
        _themeIndex = Application.Current!.RequestedThemeVariant == ThemeVariant.Dark ? 2 :
            Application.Current.RequestedThemeVariant == ThemeVariant.Light ? 1 : 0;
        _densityIndex = fluentTheme.DensityStyle == DensityStyle.Compact ? 1 : 0;
        _languageIndex = (ursaTheme.Locale?.Name ?? "zh-CN").StartsWith("zh", StringComparison.Ordinal) ? 1 : 0;
        Text = new DemoText(_languageIndex == 1);
        RebuildNavigation();
        _application.PropertyChanged += OnApplicationPropertyChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public DemoText Text { get; private set; }
    public IReadOnlyList<DemoNavItem> Categories { get; private set; } = [];
    public DemoPage? CurrentPage { get; private set; }
    public Control? Content { get; private set; }
    public bool HasPage => CurrentPage is not null;
    public bool NoResults => Categories.Count == 0;
    public string PageTitle => CurrentPage?.Id ?? Text.NoResults;
    public string PageCategory => CurrentPage is null ? Text.Controls : Text.Category(CurrentPage.Category);
    public string PageDescription => CurrentPage is null ? Text.NoResultsHint :
        _languageIndex == 1 ? CurrentPage.ChineseDescription : CurrentPage.Description;
    public string ResultCount => $"{Categories.Sum(category => category.Children.Count)} / {DemoCatalog.Pages.Count}";

    public string SearchText
    {
        get => _searchText;
        set
        {
            value ??= string.Empty;
            if (_searchText == value) return;
            _searchText = value;
            Notify();
            RebuildNavigation();
        }
    }

    public DemoNavItem? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (ReferenceEquals(_selectedItem, value)) return;
            _selectedItem = value;
            Notify();
            if (value?.Page is { } page)
            {
                _lastPageId = page.Id;
                if (CurrentPage != page)
                {
                    CurrentPage = page;
                    Content = page.CreateView();
                    Notify(nameof(Content));
                }
                NotifyPage();
            }
        }
    }

    public bool IsCollapsed
    {
        get => _isCollapsed;
        set
        {
            if (_isCollapsed == value) return;
            _isCollapsed = value;
            Notify();
        }
    }

    public int ThemeIndex
    {
        get => _themeIndex;
        set
        {
            if (_updatingText || value is < 0 or > 2 || _themeIndex == value) return;
            _themeIndex = value;
            Application.Current!.RequestedThemeVariant = value switch
            {
                1 => ThemeVariant.Light,
                2 => ThemeVariant.Dark,
                _ => ThemeVariant.Default
            };
            Notify();
        }
    }

    public int DensityIndex
    {
        get => _densityIndex;
        set
        {
            if (_updatingText || value is < 0 or > 1 || _densityIndex == value) return;
            _densityIndex = value;
            var density = value == 1 ? DensityStyle.Compact : DensityStyle.Normal;
            _fluentTheme.DensityStyle = density;
            _ursaTheme.DensityStyle = density;
            Notify();
        }
    }

    public int LanguageIndex
    {
        get => _languageIndex;
        set
        {
            if (_updatingText || value is < 0 or > 1 || _languageIndex == value) return;
            _languageIndex = value;
            _ursaTheme.Locale = CultureInfo.GetCultureInfo(value == 1 ? "zh-CN" : "en-US");
            _updatingText = true;
            Text = new DemoText(value == 1);
            Notify(nameof(Text));
            _updatingText = false;
            Notify(nameof(ThemeIndex));
            Notify(nameof(DensityIndex));
            Notify();
            RebuildNavigation();
        }
    }

    private void RebuildNavigation()
    {
        var query = _searchText.Trim();
        Categories = DemoCatalog.Pages.GroupBy(page => page.Category)
            .Select(group => new DemoNavItem(Text.Category(group.Key), null,
                group.Where(page => query.Length == 0 ||
                                    page.Id.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                    Text.Category(page.Category).Contains(query, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(page => page.Id, StringComparer.Ordinal)
                    .Select(page => new DemoNavItem(page.Id, page, []))
                    .ToArray()))
            .Where(category => category.Children.Count > 0)
            .ToArray();
        Notify(nameof(Categories));
        Notify(nameof(ResultCount));
        Notify(nameof(NoResults));
        var items = Categories.SelectMany(category => category.Children).ToArray();
        SelectedItem = items.FirstOrDefault(item => item.Page?.Id == _lastPageId) ?? items.FirstOrDefault();
        if (SelectedItem is null)
        {
            CurrentPage = null;
            Content = null;
            Notify(nameof(Content));
        }
        NotifyPage();
    }

    private void NotifyPage()
    {
        Notify(nameof(CurrentPage));
        Notify(nameof(HasPage));
        Notify(nameof(PageTitle));
        Notify(nameof(PageCategory));
        Notify(nameof(PageDescription));
    }

    private void OnApplicationPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property != Application.RequestedThemeVariantProperty) return;
        _themeIndex = _application.RequestedThemeVariant == ThemeVariant.Dark ? 2 :
            _application.RequestedThemeVariant == ThemeVariant.Light ? 1 : 0;
        Notify(nameof(ThemeIndex));
    }

    public void Dispose() => _application.PropertyChanged -= OnApplicationPropertyChanged;

    private void Notify([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
