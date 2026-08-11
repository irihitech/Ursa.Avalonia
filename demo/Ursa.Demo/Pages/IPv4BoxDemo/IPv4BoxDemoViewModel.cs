using System;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.IPv4BoxDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(IPv4BoxDemo))]
public partial class IPv4BoxDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "IPv4Box";
    public const string Menu_Header = "Menu_Header_IPv4Box";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_IPv4Box,
        Description = LanguageManager.Instance.Page_Description_IPv4Box,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_IPv4Box)],
        Tags = ["IPv4Box", "Input", "IP"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/IPv4BoxDemo/IPv4BoxDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/IPv4BoxDemo/IPv4BoxDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial IPAddress? Address { get; set; }
    
    public IPv4BoxDemoViewModel()
    {
        Address = IPAddress.Parse("192.168.1.1");
    }
    public void ChangeAddress()
    {
        long l = Random.Shared.NextInt64(0x00000000FFFFFFFF);
        Address = new IPAddress(l);
    }
}