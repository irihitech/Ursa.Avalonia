using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Irihi.Dogma.Controls;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.PinCodeDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(PinCodeDemo))]
public partial class PinCodeDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "PinCode";
    public const string Menu_Header = "Menu_Header_PinCode";
    private const string BasicUsageAnchorId = "pin-code-basic-usage";
    private const string InputModesAnchorId = "pin-code-input-modes";
    private const string PasswordMaskAnchorId = "pin-code-password-mask";
    private const string ValidationAnchorId = "pin-code-validation";
    private const string StyleClassesAnchorId = "pin-code-style-classes";
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

    public PinCodeDemoViewModel()
    {
        CompleteCommand = new AsyncRelayCommand<IList<string>>(OnComplete);
        Error = [new Exception("Invalid verification code")];

        BasicUsageSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PinCode_Section_Basic_Usage_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_PinCode_Section_Basic_Usage_Description },
            AnchorId = BasicUsageAnchorId
        };
        BasicUsageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:PinCode Count="4"
                                     CompleteCommand="{Binding CompleteCommand}" />
                          """
        });
        BasicUsageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ICommand CompleteCommand { get; set; }
                          
                          private async Task OnComplete(IList<string>? code)
                          {
                              if (code is null)
                                  return;

                              var text = string.Join(string.Empty, code);
                              await OverlayMessageBox.ShowAsync(text);
                          }
                          """
        });

        InputModesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PinCode_Section_Input_Modes_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_PinCode_Section_Input_Modes_Description },
            AnchorId = InputModesAnchorId
        };
        InputModesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:FormGroup>
                              <u:PinCode u:FormItem.Label="Digit Only"
                                         Count="4"
                                         Mode="Digit" />
                              <u:PinCode u:FormItem.Label="Letter Only"
                                         Count="4"
                                         Mode="Letter" />
                          </u:FormGroup>
                          """
        });

        PasswordMaskSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PinCode_Section_Password_Mask_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_PinCode_Section_Password_Mask_Description },
            AnchorId = PasswordMaskAnchorId
        };
        PasswordMaskSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:PinCode Count="6"
                                     PasswordChar="•"
                                     Complete="VerificationCode_OnComplete" />
                          """
        });
        PasswordMaskSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          private async void VerificationCode_OnComplete(object? _, PinCodeCompleteEventArgs e)
                          {
                              var text = string.Join(string.Empty, e.Code);
                              await OverlayMessageBox.ShowAsync(text);
                          }
                          """
        });

        ValidationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PinCode_Section_Validation_Header,
            SectionTag = DemoSectionTag.Others,
            Descriptions = { LanguageManager.Instance.Page_PinCode_Section_Validation_Description },
            AnchorId = ValidationAnchorId
        };
        ValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:PinCode Count="4"
                                     Mode="Digit"
                                     DataValidationErrors.Errors="{Binding Error}" />
                          """
        });
        ValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty]
                          public partial List<Exception>? Error { get; set; }
                          
                          public PinCodeDemoViewModel()
                          {
                              Error = [new Exception("Invalid verification code")];
                          }
                          """
        });

        StyleClassesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PinCode_Section_Style_Classes_Header,
            SectionTag = DemoSectionTag.Style,
            Descriptions = { LanguageManager.Instance.Page_PinCode_Section_Style_Classes_Description },
            AnchorId = StyleClassesAnchorId
        };
        StyleClassesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <StackPanel Spacing="12">
                              <u:ClassSelector Target="{Binding #sizeTarget}">
                                  <u:ClassSelectorGroup Header="Size">
                                      <u:ClassSelectorItem ClassName="Small" />
                                      <u:ClassSelectorItem ClassName="Large" />
                                  </u:ClassSelectorGroup>
                              </u:ClassSelector>
                              <u:PinCode Name="sizeTarget"
                                         Count="6" />
                          </StackPanel>
                          """
        });
    }

    public ICommand CompleteCommand { get; set; }
    [ObservableProperty] public partial List<Exception>? Error { get; set; }

    public DemoSectionViewModel BasicUsageSection { get; }
    public DemoSectionViewModel InputModesSection { get; }
    public DemoSectionViewModel PasswordMaskSection { get; }
    public DemoSectionViewModel ValidationSection { get; }
    public DemoSectionViewModel StyleClassesSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_PinCode_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PinCode_Section_Input_Modes_Header,
            AnchorId = InputModesAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PinCode_Section_Password_Mask_Header,
            AnchorId = PasswordMaskAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PinCode_Section_Validation_Header,
            AnchorId = ValidationAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PinCode_Section_Style_Classes_Header,
            AnchorId = StyleClassesAnchorId
        }
    ];

    private async Task OnComplete(IList<string>? obj)
    {
        if (obj is null) return;
        var code = string.Join("", obj);
        await OverlayMessageBox.ShowAsync(code);
    }
}