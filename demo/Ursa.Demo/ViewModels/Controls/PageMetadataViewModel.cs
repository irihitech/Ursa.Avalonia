using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels.Controls;

public partial class PageMetadataViewModel: ObservableObject
{
    [ObservableProperty] public partial IObservable<string?>? Title { get; set; }
    [ObservableProperty] public partial IObservable<string?>? Description { get; set; }
    [ObservableProperty] public partial IReadOnlyList<BreadcrumbItemData>? Breadcrumbs { get; set; }
    [ObservableProperty] public partial string? DemoViewUrl { get; set; }
    [ObservableProperty] public partial string? DemoViewModelUrl { get; set; }
    [ObservableProperty] public partial string? SourceUrl { get; set; }
    [ObservableProperty] public partial string? ThemeUrl { get; set; }
    [ObservableProperty] public partial string[]? Tags { get; set; }
    [ObservableProperty] public partial bool? MvvmSupport { get; set; }
    [ObservableProperty] public partial bool? InlineXamlSupport { get; set; }
    [ObservableProperty] public partial bool? AvaloniaExclusive { get; set; }

    public PageMetadataViewModel()
    {
        AvaloniaExclusive = false;
    }
}
