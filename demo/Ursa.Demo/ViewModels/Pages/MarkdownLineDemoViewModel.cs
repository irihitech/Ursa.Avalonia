using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.ViewModels;

public partial class MarkdownLineDemoViewModel : ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new()
    {
        Title = "MarkdownLine",
        Description = "A lightweight TextBlock that renders a limited subset of inline Markdown formatting.",
        Breadcrumbs = ["Layout & Display", "MarkdownLine"],
        Tags = ["Markdown", "Text", "Inline", "Formatting"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/MarkdownLineDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/MarkdownLineDemoViewModel.cs",
        InlineXamlSupport = true,
    };
}
