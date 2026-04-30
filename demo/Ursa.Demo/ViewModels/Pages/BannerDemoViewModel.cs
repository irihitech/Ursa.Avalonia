using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class BannerDemoViewModel : ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Banner",
        Description = "Banner displays a prominent informational or alert message bar.",
        Breadcrumbs = ["Layout & Display", "Banner"],
        Tags = ["Banner", "Alert", "Message"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/BannerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/BannerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    private string? _oldTitle = string.Empty;
    private string? _oldContent = string.Empty;
    [ObservableProperty] private string? _title = "Welcome to Ursa";
    [ObservableProperty] private string? _content = "This is the Demo of Ursa Banner.";
    [ObservableProperty] private bool _bordered;

    [ObservableProperty] private bool _setTitleNull = true;
    [ObservableProperty] private bool _setContentNull = true;

    partial void OnSetTitleNullChanged(bool value)
    {
        if (value)
        {
            Title = _oldTitle;
        }
        else
        {
            _oldTitle = Title;
            Title = null;
        }
    }

    partial void OnSetContentNullChanged(bool value)
    {
        if (value)
        {
            Content = _oldContent;
        }
        else
        {
            _oldContent = Content;
            Content = null;
        }
    }
}