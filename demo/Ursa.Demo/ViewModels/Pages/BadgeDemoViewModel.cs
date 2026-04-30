using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class BadgeDemoViewModel: ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Badge",
        Description = "Badge displays a small count or status indicator on another element.",
        Breadcrumbs = ["Layout & Display", "Badge"],
        Tags = ["Badge", "Label", "Status"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/BadgeDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/BadgeDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    [ObservableProperty] private string? _text = null;

    public BadgeDemoViewModel()
    {
        
    }

    [RelayCommand]
    public void ChangeText()
    {
        if (Text == null)
        {
            Text = DateTime.Now.ToShortDateString();
        }
        else
        {
            Text = null;
        }
    }
}