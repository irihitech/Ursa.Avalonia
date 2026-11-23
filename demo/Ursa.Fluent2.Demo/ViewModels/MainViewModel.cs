using System.Globalization;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Themes.Fluent2;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Fluent2.Demo.Services;
using Ursa.Themes.Fluent2;

namespace Ursa.Fluent2.Demo.ViewModels;

public sealed class DemoNavItem(string title, string? pageId, IReadOnlyList<DemoNavItem> children)
{
    public string Title { get; } = title;
    public string? PageId { get; } = pageId;
    public IReadOnlyList<DemoNavItem> Children { get; } = children;
    public Geometry Icon { get; } = StreamGeometry.Parse(
        "M2,2 H7 V7 H2 Z M10,2 H15 V7 H10 Z M2,10 H7 V15 H2 Z M10,10 H15 V15 H10 Z");
}

public sealed partial class MainViewModel : ObservableObject, IDisposable
{
    private readonly Application _application;
    private readonly Fluent2Theme _fluentTheme;
    private readonly UrsaFluent2Theme _ursaTheme;
    private readonly IDemoInteractionService _interactions;
    private string _searchText = string.Empty;
    private string _lastPageId = "Overview";
    private int _themeIndex;
    private int _densityIndex;
    private int _languageIndex;
    private bool _updatingText;

    public MainViewModel(Fluent2Theme fluentTheme, UrsaFluent2Theme ursaTheme,
        IDemoInteractionService interactions)
    {
        _application = Application.Current!;
        _fluentTheme = fluentTheme;
        _ursaTheme = ursaTheme;
        _interactions = interactions;
        _themeIndex = _application.RequestedThemeVariant == ThemeVariant.Dark ? 2 :
            _application.RequestedThemeVariant == ThemeVariant.Light ? 1 : 0;
        _densityIndex = fluentTheme.DensityStyle == DensityStyle.Compact ? 1 : 0;
        _languageIndex = (ursaTheme.Locale?.Name ?? "zh-CN").StartsWith("zh", StringComparison.Ordinal) ? 1 : 0;
        Text = new DemoText(_languageIndex == 1);
        RebuildNavigation();
        _application.PropertyChanged += OnApplicationPropertyChanged;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PageTitle), nameof(PageCategory), nameof(PageDescription))]
    public partial DemoText Text { get; private set; } = new(false);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NoResults), nameof(ResultCount))]
    public partial IReadOnlyList<DemoNavItem> Categories { get; private set; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPage), nameof(PageTitle), nameof(PageCategory), nameof(PageDescription))]
    public partial DemoPage? CurrentPage { get; private set; }

    [ObservableProperty]
    public partial DemoPageViewModel? Content { get; private set; }

    [ObservableProperty]
    public partial DemoNavItem? SelectedItem { get; set; }

    [ObservableProperty]
    public partial bool IsCollapsed { get; set; }

    public bool HasPage => CurrentPage is not null;
    public bool NoResults => Categories.Count == 0;
    public string PageTitle => CurrentPage?.Id is { } id ? Text.PageTitle(id) : Text.NoResults;
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
            if (SetProperty(ref _searchText, value)) RebuildNavigation();
        }
    }

    partial void OnSelectedItemChanged(DemoNavItem? value)
    {
        if (value?.PageId is { } pageId)
        {
            _lastPageId = pageId;
            if (CurrentPage?.Id != pageId)
            {
                CurrentPage = DemoCatalog.GetPage(pageId);
                Content = DemoPageViewModel.Create(pageId, _interactions);
            }
        }
    }

    [RelayCommand]
    private void ClearSearch() => SearchText = string.Empty;

    public int ThemeIndex
    {
        get => _themeIndex;
        set
        {
            if (_updatingText || value is < 0 or > 2 || _themeIndex == value) return;
            SetProperty(ref _themeIndex, value);
            _application.RequestedThemeVariant = value switch
            {
                1 => ThemeVariant.Light,
                2 => ThemeVariant.Dark,
                _ => ThemeVariant.Default
            };
        }
    }

    public int DensityIndex
    {
        get => _densityIndex;
        set
        {
            if (_updatingText || value is < 0 or > 1 || _densityIndex == value) return;
            SetProperty(ref _densityIndex, value);
            var density = value == 1 ? DensityStyle.Compact : DensityStyle.Normal;
            _fluentTheme.DensityStyle = density;
            _ursaTheme.DensityStyle = density;
        }
    }

    public int LanguageIndex
    {
        get => _languageIndex;
        set
        {
            if (_updatingText || value is < 0 or > 1 || _languageIndex == value) return;
            SetProperty(ref _languageIndex, value);
            _ursaTheme.Locale = CultureInfo.GetCultureInfo(value == 1 ? "zh-CN" : "en-US");
            _updatingText = true;
            Text = new DemoText(value == 1);
            _updatingText = false;
            OnPropertyChanged(nameof(ThemeIndex));
            OnPropertyChanged(nameof(DensityIndex));
            OnPropertyChanged(nameof(LanguageIndex));
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
                                    Text.PageTitle(page.Id).Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                    Text.Category(page.Category).Contains(query, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(page => page.Id != "Overview")
                    .ThenBy(page => page.Id, StringComparer.Ordinal)
                    .Select(page => new DemoNavItem(Text.PageTitle(page.Id), page.Id, []))
                    .ToArray()))
            .Where(category => category.Children.Count > 0)
            .ToArray();
        var items = Categories.SelectMany(category => category.Children).ToArray();
        SelectedItem = items.FirstOrDefault(item => item.PageId == _lastPageId) ?? items.FirstOrDefault();
        if (SelectedItem is null)
        {
            CurrentPage = null;
            Content = null;
        }
    }

    private void OnApplicationPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property != Application.RequestedThemeVariantProperty) return;
        _themeIndex = _application.RequestedThemeVariant == ThemeVariant.Dark ? 2 :
            _application.RequestedThemeVariant == ThemeVariant.Light ? 1 : 0;
        OnPropertyChanged(nameof(ThemeIndex));
    }

    public void Dispose() => _application.PropertyChanged -= OnApplicationPropertyChanged;
}
