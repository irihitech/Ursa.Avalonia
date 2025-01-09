using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class BannerDemoViewModel : ViewModelBase
{
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