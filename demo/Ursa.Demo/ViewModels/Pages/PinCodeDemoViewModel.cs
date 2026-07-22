using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class PinCodeDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_PinCode,
        Description = LanguageManager.Instance.Page_Description_PinCode,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_PinCode)],
        Tags = ["PinCode", "Input", "Password"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PinCodeDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/PinCodeDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public ICommand CompleteCommand { get; set; }
    [ObservableProperty] public partial List<Exception>? Error { get; set; }

    public PinCodeDemoViewModel()
    {
        CompleteCommand = new AsyncRelayCommand<IList<string>>(OnComplete);
        Error = [new Exception("Invalid verification code")];
    }

    private async Task OnComplete(IList<string>? obj)
    {
        if (obj is null) return;
        var code = string.Join("", obj);
        await OverlayMessageBox.ShowAsync(code);
    }
}