using System;
using System.Collections.ObjectModel;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Controls;

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
    private const string BasicInputModesAnchorId = "ipv4-box-basic-input-modes";
    private const string SourceBindingAndDisabledStateAnchorId = "ipv4-box-source-binding-and-disabled-state";
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
    public DemoSectionViewModel BasicInputModesSection { get; }
    public DemoSectionViewModel SourceBindingAndDisabledStateSection { get; }
    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_IPv4Box_Section_Basic_Input_Modes_Header,
            AnchorId = BasicInputModesAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_IPv4Box_Section_Source_Binding_And_Disabled_State_Header,
            AnchorId = SourceBindingAndDisabledStateAnchorId
        }
    ];
    
    public IPv4BoxDemoViewModel()
    {
        BasicInputModesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_IPv4Box_Section_Basic_Input_Modes_Header,
            Descriptions = { LanguageManager.Instance.Page_IPv4Box_Section_Basic_Input_Modes_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = BasicInputModesAnchorId
        };
        BasicInputModesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:IPv4Box Width="200" ShowLeadingZero="{Binding #format.IsChecked}" />
                          <u:IPv4Box Width="200" InputMode="Fast" ShowLeadingZero="{Binding #format.IsChecked}" />
                          """
        });

        SourceBindingAndDisabledStateSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_IPv4Box_Section_Source_Binding_And_Disabled_State_Header,
            Descriptions = { LanguageManager.Instance.Page_IPv4Box_Section_Source_Binding_And_Disabled_State_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = SourceBindingAndDisabledStateAnchorId
        };
        SourceBindingAndDisabledStateSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <RepeatButton Command="{Binding ChangeAddressCommand}" Content="Random" />
                          <u:IPv4Box Width="200" IPAddress="{Binding Address}" />
                          """
        });
        SourceBindingAndDisabledStateSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial IPAddress? Address { get; set; }

                          [RelayCommand]
                          private void ChangeAddress()
                          {
                              var value = Random.Shared.NextInt64(0x00000000FFFFFFFF);
                              Address = new IPAddress(value);
                          }
                          """
        });

        Address = IPAddress.Parse("192.168.1.1");
    }

    [RelayCommand]
    private void ChangeAddress()
    {
        var value = Random.Shared.NextInt64(0x00000000FFFFFFFF);
        Address = new IPAddress(value);
    }
}