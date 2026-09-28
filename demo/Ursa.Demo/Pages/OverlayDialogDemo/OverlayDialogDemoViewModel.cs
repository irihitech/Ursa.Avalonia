using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Controls;
using Ursa.Demo.Dialogs;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.OverlayDialogDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(OverlayDialogDemo))]
public partial class OverlayDialogDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "OverlayDialog";
    public const string Menu_Header = "Menu_Header_OverlayDialog";
    public const string LocalHost = "LocalHost";
    private const string BasicUsageAnchorId = "overlay-dialog-basic-usage";
    private const string PositioningAnchorId = "overlay-dialog-positioning";
    private const string LightDismissAnchorId = "overlay-dialog-light-dismiss";
    private const string LocalHostAnchorId = "overlay-dialog-local-host";
    private const string ModalityAnchorId = "overlay-dialog-modality";
    private const string CustomContentAnchorId = "overlay-dialog-custom-content";
    private const string StyleClassAnchorId = "overlay-dialog-style-class";

    public PageMetadataViewModel PageMetadata { get; set; } = new()
    {
        Title = LanguageManager.Instance.Page_Title_OverlayDialog,
        Description = LanguageManager.Instance.Page_Description_OverlayDialog,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_OverlayDialog)],
        Tags = ["Dialog", "Modal", "Overlay"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/OverlayDialogDemo/OverlayDialogDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/OverlayDialogDemo/OverlayDialogDemoViewModel.cs",
        InlineXamlSupport = false,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    public OverlayDialogDemoViewModel()
    {
        BasicUsageSection = CreateSection(
            LanguageManager.Instance.Page_OverlayDialog_Section_Basic_Usage_Header,
            LanguageManager.Instance.Page_OverlayDialog_Section_Basic_Usage_Description,
            BasicUsageAnchorId, DemoSectionTag.Function,
            """
            <Button Command="{Binding StandardDialog.ShowDialogCommand}"
                    Content="Show overlay dialog" />
            """,
            """
            await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                new DefaultDemoDialogViewModel(), null);
            """);
        PositioningSection = CreateSection(
            LanguageManager.Instance.Page_OverlayDialog_Section_Positioning_Header,
            LanguageManager.Instance.Page_OverlayDialog_Section_Positioning_Description,
            PositioningAnchorId, DemoSectionTag.Function,
            """
            <u:EnumSelector EnumType="u:HorizontalPosition" Value="{Binding HorizontalAnchor}" />
            <u:EnumSelector EnumType="u:VerticalPosition" Value="{Binding VerticalAnchor}" />
            <u:NumericDoubleUpDown Value="{Binding HorizontalOffset}" />
            <u:NumericDoubleUpDown Value="{Binding VerticalOffset}" />
            <Button Command="{Binding ShowDialogCommand}" Content="Show positioned dialog" />
            """,
            """
            var options = new OverlayDialogOptions
            {
                HorizontalAnchor = HorizontalAnchor,
                VerticalAnchor = VerticalAnchor,
                HorizontalOffset = HorizontalOffset,
                VerticalOffset = VerticalOffset,
            };
            await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                new DefaultDemoDialogViewModel(), null, options: options);
            """);
        LightDismissSection = CreateSection(
            LanguageManager.Instance.Page_OverlayDialog_Section_Light_Dismiss_Header,
            LanguageManager.Instance.Page_OverlayDialog_Section_Light_Dismiss_Description,
            LightDismissAnchorId, DemoSectionTag.Function,
            """
            <CheckBox IsChecked="{Binding CanLightDismiss}" Content="Can LightDismiss" />
            <Button Command="{Binding ShowDialogCommand}" Content="Show overlay dialog" />
            """,
            """
            var options = new OverlayDialogOptions { CanLightDismiss = CanLightDismiss };
            await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                new DefaultDemoDialogViewModel(), null, options: options);
            """);
        LocalHostSection = CreateSection(
            LanguageManager.Instance.Page_OverlayDialog_Section_Local_Host_Header,
            LanguageManager.Instance.Page_OverlayDialog_Section_Local_Host_Description,
            LocalHostAnchorId, DemoSectionTag.Function,
            """
            <ToggleSwitch IsChecked="{Binding UseLocalHost}"
                          OffContent="Global" OnContent="Local" />
            <Button Command="{Binding ShowDialogCommand}" Content="Show overlay dialog" />
            <u:OverlayDialogHost HostId="{x:Static vm:OverlayDialogDemoViewModel.LocalHost}" />
            """,
            """
            string? hostId = UseLocalHost ? OverlayDialogDemoViewModel.LocalHost : null;
            await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                new DefaultDemoDialogViewModel(), hostId);
            """);
        ModalitySection = CreateSection(
            LanguageManager.Instance.Page_OverlayDialog_Section_Modality_Header,
            LanguageManager.Instance.Page_OverlayDialog_Section_Modality_Description,
            ModalityAnchorId, DemoSectionTag.Function,
            """
            <CheckBox IsChecked="{Binding IsModal}" Content="Modal" />
            <Button Command="{Binding ShowDialogCommand}" Content="Show overlay dialog" />
            """,
            """
            if (IsModal)
                await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                    new DefaultDemoDialogViewModel(), null);
            else
                OverlayDialog.ShowStandard<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                    new DefaultDemoDialogViewModel(), null);
            """);
        CustomContentSection = CreateSection(
            LanguageManager.Instance.Page_OverlayDialog_Section_Custom_Content_Header,
            LanguageManager.Instance.Page_OverlayDialog_Section_Custom_Content_Description,
            CustomContentAnchorId, DemoSectionTag.Others,
            """
            <Button Command="{Binding CustomDialog.ShowDialogCommand}"
                    Content="Show custom overlay dialog" />
            """,
            """
            await OverlayDialog.ShowCustomAsync<CustomDemoDialog, CustomDemoDialogViewModel, object>(
                new CustomDemoDialogViewModel(), null);
            """);
        StyleClassSection = CreateSection(
            LanguageManager.Instance.Page_OverlayDialog_Section_Style_Class_Header,
            LanguageManager.Instance.Page_OverlayDialog_Section_Style_Class_Description,
            StyleClassAnchorId, DemoSectionTag.Style,
            """
            <TextBox Text="{Binding StyleClass}" PlaceholderText="Custom" />
            <Button Command="{Binding ShowDialogCommand}" Content="Show styled dialog" />
            """,
            """
            var options = new OverlayDialogOptions { StyleClass = StyleClass };
            await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                new DefaultDemoDialogViewModel(), null, options: options);
            """);
    }

    private static DemoSectionViewModel CreateSection(
        IObservable<string?> header, IObservable<string?> description, string anchorId,
        DemoSectionTag tag, string axaml, string csharp)
    {
        var section = new DemoSectionViewModel
        {
            Header = header,
            Descriptions = { description },
            SectionTag = tag,
            AnchorId = anchorId,
        };
        section.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = axaml,
        });
        section.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = csharp,
        });
        return section;
    }

    public StandardOverlayDialogDemoViewModel StandardDialog { get; } = new();
    public OverlayDialogPositionDemoViewModel Positioning { get; } = new();
    public OverlayDialogDismissDemoViewModel LightDismiss { get; } = new();
    public OverlayDialogHostDemoViewModel LocalHostDemo { get; } = new();
    public OverlayDialogModalityDemoViewModel Modality { get; } = new();
    public CustomOverlayDialogDemoViewModel CustomDialog { get; } = new();
    public OverlayDialogStyleDemoViewModel Styling { get; } = new();
    public DemoSectionViewModel BasicUsageSection { get; }
    public DemoSectionViewModel PositioningSection { get; }
    public DemoSectionViewModel LightDismissSection { get; }
    public DemoSectionViewModel LocalHostSection { get; }
    public DemoSectionViewModel ModalitySection { get; }
    public DemoSectionViewModel CustomContentSection { get; }
    public DemoSectionViewModel StyleClassSection { get; }
    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new() { Header = LanguageManager.Instance.Page_OverlayDialog_Section_Basic_Usage_Header, AnchorId = BasicUsageAnchorId },
        new() { Header = LanguageManager.Instance.Page_OverlayDialog_Section_Positioning_Header, AnchorId = PositioningAnchorId },
        new() { Header = LanguageManager.Instance.Page_OverlayDialog_Section_Light_Dismiss_Header, AnchorId = LightDismissAnchorId },
        new() { Header = LanguageManager.Instance.Page_OverlayDialog_Section_Local_Host_Header, AnchorId = LocalHostAnchorId },
        new() { Header = LanguageManager.Instance.Page_OverlayDialog_Section_Modality_Header, AnchorId = ModalityAnchorId },
        new() { Header = LanguageManager.Instance.Page_OverlayDialog_Section_Custom_Content_Header, AnchorId = CustomContentAnchorId },
        new() { Header = LanguageManager.Instance.Page_OverlayDialog_Section_Style_Class_Header, AnchorId = StyleClassAnchorId },
    ];
}

public partial class StandardOverlayDialogDemoViewModel : ObservableObject
{
    public ICommand ShowDialogCommand { get; } = new AsyncRelayCommand(async () =>
        await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
            new DefaultDemoDialogViewModel(), null));
}

public partial class OverlayDialogPositionDemoViewModel : ObservableObject
{
    [ObservableProperty] public partial HorizontalPosition HorizontalAnchor { get; set; } = HorizontalPosition.Center;
    [ObservableProperty] public partial VerticalPosition VerticalAnchor { get; set; } = VerticalPosition.Center;
    [ObservableProperty] public partial double? HorizontalOffset { get; set; }
    [ObservableProperty] public partial double? VerticalOffset { get; set; }

    public ICommand ShowDialogCommand { get; }

    public OverlayDialogPositionDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
    }

    private async Task ShowDialog()
    {
        var options = new OverlayDialogOptions
        {
            HorizontalAnchor = HorizontalAnchor,
            VerticalAnchor = VerticalAnchor,
            HorizontalOffset = HorizontalOffset,
            VerticalOffset = VerticalOffset,
        };
        await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
            new DefaultDemoDialogViewModel(), null, options: options);
    }
}

public partial class OverlayDialogDismissDemoViewModel : ObservableObject
{
    [ObservableProperty] public partial bool CanLightDismiss { get; set; } = true;

    public ICommand ShowDialogCommand { get; }

    public OverlayDialogDismissDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
    }

    private async Task ShowDialog()
    {
        var options = new OverlayDialogOptions { CanLightDismiss = CanLightDismiss };
        await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
            new DefaultDemoDialogViewModel(), null, options: options);
    }
}

public partial class OverlayDialogHostDemoViewModel : ObservableObject
{
    [ObservableProperty] public partial bool UseLocalHost { get; set; }

    public ICommand ShowDialogCommand { get; }

    public OverlayDialogHostDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
    }

    private async Task ShowDialog()
    {
        string? hostId = UseLocalHost ? OverlayDialogDemoViewModel.LocalHost : null;
        await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
            new DefaultDemoDialogViewModel(), hostId);
    }
}

public partial class OverlayDialogModalityDemoViewModel : ObservableObject
{
    [ObservableProperty] public partial bool IsModal { get; set; } = true;

    public ICommand ShowDialogCommand { get; }

    public OverlayDialogModalityDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
    }

    private async Task ShowDialog()
    {
        if (IsModal)
            await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                new DefaultDemoDialogViewModel(), null);
        else
            OverlayDialog.ShowStandard<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                new DefaultDemoDialogViewModel(), null);
    }
}

public partial class CustomOverlayDialogDemoViewModel : ObservableObject
{
    public ICommand ShowDialogCommand { get; } = new AsyncRelayCommand(async () =>
        await OverlayDialog.ShowCustomAsync<CustomDemoDialog, CustomDemoDialogViewModel, object>(
            new CustomDemoDialogViewModel(), null));
}

public partial class OverlayDialogStyleDemoViewModel : ObservableObject
{
    [ObservableProperty] public partial string? StyleClass { get; set; } = "Custom";

    public ICommand ShowDialogCommand { get; }

    public OverlayDialogStyleDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
    }

    private async Task ShowDialog()
    {
        var options = new OverlayDialogOptions { StyleClass = StyleClass };
        await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
            new DefaultDemoDialogViewModel(), null, options: options);
    }
}
