using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Controls;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.PopConfirmDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(PopConfirmDemo))]
public partial class PopConfirmDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "PopConfirm";
    public const string Menu_Header = "Menu_Header_PopConfirm";
    private const string BasicAnchorId = "popconfirm-basic";
    private const string TriggerModesAnchorId = "popconfirm-trigger-modes";
    private const string NonButtonTriggerAnchorId = "popconfirm-non-button-trigger";
    private const string PlacementAnchorId = "popconfirm-placement";
    private const string AsyncCommandAnchorId = "popconfirm-async-command";
    private const string IconStylesAnchorId = "popconfirm-icon-styles";
    private const string CustomIconAnchorId = "popconfirm-custom-icon";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_PopConfirm,
        Description = LanguageManager.Instance.Page_Description_PopConfirm,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_PopConfirm)],
        Tags = ["PopConfirm", "Confirm", "Popup"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PopConfirmDemo/PopConfirmDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PopConfirmDemo/PopConfirmDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Basic_Header,
            AnchorId = BasicAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Trigger_Modes_Header,
            AnchorId = TriggerModesAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Non_Button_Trigger_Header,
            AnchorId = NonButtonTriggerAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Placement_Header,
            AnchorId = PlacementAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Async_Command_Header,
            AnchorId = AsyncCommandAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Icon_Styles_Header,
            AnchorId = IconStylesAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Custom_Icon_Header,
            AnchorId = CustomIconAnchorId
        },
    ];

    public DemoSectionViewModel BasicSection { get; }
    public DemoSectionViewModel TriggerModesSection { get; }
    public DemoSectionViewModel NonButtonTriggerSection { get; }
    public DemoSectionViewModel PlacementSection { get; }
    public DemoSectionViewModel AsyncCommandSection { get; }
    public DemoSectionViewModel IconStylesSection { get; }
    public DemoSectionViewModel CustomIconSection { get; }

    [ObservableProperty] public partial bool HandleAsyncCommand { get; set; } = true;

    public PopConfirmDemoViewModel()
    {
        AsyncConfirmCommand = new AsyncRelayCommand(OnConfirmAsync);
        AsyncCancelCommand = new RelayCommand(OnCancelAsync);
        ConfirmCommand = new RelayCommand(OnConfirm);
        CancelCommand = new RelayCommand(OnCancel);

        BasicSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Basic_Header,
            Descriptions = { LanguageManager.Instance.Page_PopConfirm_Section_Basic_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = BasicAnchorId
        };
        AddSnippet(BasicSection, CodeLanguage.Axaml, LanguageManager.Instance.DemoSection_Tab_Xaml, """
            <u:PopConfirm PopupHeader="Confirm this action?"
                          PopupContent="This action cannot be undone."
                          ConfirmCommand="{Binding ConfirmCommand}"
                          CancelCommand="{Binding CancelCommand}">
                <Button Content="Run action" />
            </u:PopConfirm>
            """);
        AddSnippet(BasicSection, CodeLanguage.CSharp, LanguageManager.Instance.DemoSection_Tab_ViewModel, """
            ConfirmCommand = new RelayCommand(OnConfirm);
            CancelCommand = new RelayCommand(OnCancel);
            """);

        TriggerModesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Trigger_Modes_Header,
            Descriptions = { LanguageManager.Instance.Page_PopConfirm_Section_Trigger_Modes_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = TriggerModesAnchorId
        };
        AddSnippet(TriggerModesSection, CodeLanguage.Axaml, LanguageManager.Instance.DemoSection_Tab_Xaml, """
            <u:PopConfirm TriggerMode="Focus, Click"
                          PopupHeader="Confirm changes?"
                          PopupContent="Unsaved changes will be lost."
                          ConfirmCommand="{Binding ConfirmCommand}">
                <Button Content="Close" />
            </u:PopConfirm>
            """);

        NonButtonTriggerSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Non_Button_Trigger_Header,
            Descriptions = { LanguageManager.Instance.Page_PopConfirm_Section_Non_Button_Trigger_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = NonButtonTriggerAnchorId
        };
        AddSnippet(NonButtonTriggerSection, CodeLanguage.Axaml, LanguageManager.Instance.DemoSection_Tab_Xaml, """
            <u:PopConfirm PopupHeader="Confirm deletion?"
                          PopupContent="This item will be removed."
                          ConfirmCommand="{Binding ConfirmCommand}">
                <TextBlock Text="Delete item" />
            </u:PopConfirm>
            """);

        PlacementSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Placement_Header,
            Descriptions = { LanguageManager.Instance.Page_PopConfirm_Section_Placement_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = PlacementAnchorId
        };
        AddSnippet(PlacementSection, CodeLanguage.Axaml, LanguageManager.Instance.DemoSection_Tab_Xaml, """
            <u:EnumSelector x:Name="placement"
                            EnumType="{x:Type PlacementMode}"
                            Value="{x:Static PlacementMode.BottomEdgeAlignedLeft}" />

            <u:PopConfirm PopupHeader="Confirm"
                          PopupContent="Are you sure?"
                          Placement="{Binding #placement.Value}"
                          ConfirmCommand="{Binding ConfirmCommand}">
                <Button Content="Click me" />
            </u:PopConfirm>
            """);

        AsyncCommandSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Async_Command_Header,
            Descriptions = { LanguageManager.Instance.Page_PopConfirm_Section_Async_Command_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = AsyncCommandAnchorId
        };
        AddSnippet(AsyncCommandSection, CodeLanguage.Axaml, LanguageManager.Instance.DemoSection_Tab_Xaml, """
            <u:PopConfirm PopupHeader="Processing..."
                          PopupContent="This may take a moment."
                          HandleAsyncCommand="{Binding HandleAsyncCommand}"
                          ConfirmCommand="{Binding AsyncConfirmCommand}"
                          CancelCommand="{Binding AsyncCancelCommand}">
                <Button Content="Process" />
            </u:PopConfirm>
            """);
        AddSnippet(AsyncCommandSection, CodeLanguage.CSharp, LanguageManager.Instance.DemoSection_Tab_ViewModel, """
            AsyncConfirmCommand = new AsyncRelayCommand(OnConfirmAsync);

            private async Task OnConfirmAsync()
            {
                await Task.Delay(3000);
                CompleteOperation();
            }
            """);

        IconStylesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Icon_Styles_Header,
            Descriptions = { LanguageManager.Instance.Page_PopConfirm_Section_Icon_Styles_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = IconStylesAnchorId
        };
        AddSnippet(IconStylesSection, CodeLanguage.Axaml, LanguageManager.Instance.DemoSection_Tab_Xaml, """
            <u:PopConfirm Classes="Information" PopupHeader="Info" PopupContent="Information message.">
                <Button Content="Information" />
            </u:PopConfirm>

            <u:PopConfirm Classes="Success" PopupHeader="Success" PopupContent="Operation completed.">
                <Button Content="Success" />
            </u:PopConfirm>

            <u:PopConfirm Classes="Warning" PopupHeader="Warning" PopupContent="Check this action.">
                <Button Content="Warning" />
            </u:PopConfirm>

            <u:PopConfirm Classes="Error" PopupHeader="Error" PopupContent="Operation failed.">
                <Button Content="Error" />
            </u:PopConfirm>
            """);

        CustomIconSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PopConfirm_Section_Custom_Icon_Header,
            Descriptions = { LanguageManager.Instance.Page_PopConfirm_Section_Custom_Icon_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = CustomIconAnchorId
        };
        AddSnippet(CustomIconSection, CodeLanguage.Axaml, LanguageManager.Instance.DemoSection_Tab_Xaml, """
            <u:PopConfirm PopupHeader="Custom action"
                          PopupContent="Proceed with this action?"
                          ConfirmCommand="{Binding ConfirmCommand}">
                <u:PopConfirm.Icon>
                    <TextBlock Text="!" FontSize="18" FontWeight="Bold" />
                </u:PopConfirm.Icon>
                <Button Content="Custom icon" />
            </u:PopConfirm>
            """);
    }

    internal WindowToastManager? ToastManager { get; set; }

    public ICommand ConfirmCommand { get; }
    public ICommand CancelCommand { get; }

    public ICommand AsyncConfirmCommand { get; }
    public ICommand AsyncCancelCommand { get; }

    private void OnCancel()
    {
        ToastManager?.Show(new Toast("Canceled"), NotificationType.Error, classes: ["Light"]);
    }

    private void OnConfirm()
    {
        ToastManager?.Show(new Toast("Confirmed"), NotificationType.Success, classes: ["Light"]);
    }

    private async Task OnConfirmAsync()
    {
        await Task.Delay(3000);
        ToastManager?.Show(new Toast("Async Confirmed"), NotificationType.Success, classes: ["Light"]);
    }

    private void OnCancelAsync()
    {
        ToastManager?.Show(new Toast("Async Canceled"), NotificationType.Error, classes: ["Light"]);
    }

    private static void AddSnippet(
        DemoSectionViewModel section,
        CodeLanguage language,
        IObservable<string?> tabName,
        string code)
    {
        section.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = language,
            TabName = tabName,
            CodeSnippet = code
        });
    }
}
