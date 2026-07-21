using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels.Controls;

public partial class PageMetadataViewModel: ObservableObject
{
    private IObservable<string?>? _title;
    private IObservable<string?>? _description;
    private IReadOnlyList<BreadcrumbItemData>? _breadcrumbs;

    public IObservable<string?>? Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public IObservable<string?>? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public IReadOnlyList<BreadcrumbItemData>? Breadcrumbs
    {
        get => _breadcrumbs;
        set => SetProperty(ref _breadcrumbs, value);
    }

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
