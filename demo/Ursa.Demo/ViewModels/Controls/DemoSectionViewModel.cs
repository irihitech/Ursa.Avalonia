using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;

namespace Ursa.Demo.ViewModels.Controls;

public partial class DemoSectionViewModel : ObservableObject
{
    [ObservableProperty] public partial string? Header { get; set; }
    [ObservableProperty] public partial string? Description { get; set; }
    [ObservableProperty] public partial string? AnchorId { get; set; }
    public ObservableCollection<DemoSectionCodeSnippetViewModel> CodeSnippets { get; } = [];
}

public partial class DemoSectionCodeSnippetViewModel : ObservableObject
{
    [ObservableProperty] public partial string? CodeSnippet { get; set; }
    [ObservableProperty] public partial CodeLanguage? CodeSnippetLanguage { get; set; }
    [ObservableProperty] public partial string? TabName { get; set; }
}
