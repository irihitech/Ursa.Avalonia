using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels.Controls;

public partial class PageMetadataViewModel: ObservableObject
{
    [ObservableProperty] private string? _title;
    [ObservableProperty] private string? _description;
    [ObservableProperty] private string[]? _breadcrumbs;
    [ObservableProperty] private string? _demoViewUrl;
    [ObservableProperty] private string? _demoViewModelUrl;
    [ObservableProperty] private string? _sourceUrl;
    [ObservableProperty] private string? _themeUrl;
    [ObservableProperty] private string[]? _tags;
    [ObservableProperty] private bool? _mvvmSupport;
    [ObservableProperty] private bool? _inlineXamlSupport;
    [ObservableProperty] private bool? _avaloniaExclusive;

    public PageMetadataViewModel()
    {
        AvaloniaExclusive = false;
    }
}
