using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.PinCodeDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(PinCodeDemo))]
public partial class PinCodeDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "PinCode";
    public const string Menu_Header = "Menu_Header_PinCode";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_PinCode,
        Description = LanguageManager.Instance.Page_Description_PinCode,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_PinCode)],
        Tags = ["PinCode", "Input", "Password"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PinCodeDemo/PinCodeDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PinCodeDemo/PinCodeDemoViewModel.cs",
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