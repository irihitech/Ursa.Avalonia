using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Controls;
using Ursa.Demo.Dialogs;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.WindowDialogDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(WindowDialogDemo))]
public partial class WindowDialogDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "WindowDialog";
    public const string Menu_Header = "Menu_Header_WindowDialog";
    private const string BasicUsageAnchorId = "window-dialog-basic-usage";
    private const string PositioningAnchorId = "window-dialog-positioning";
    private const string CustomContentAnchorId = "window-dialog-custom-content";
    private const string ModalityAnchorId = "window-dialog-modality";
    private const string StyleClassAnchorId = "window-dialog-style-class";

    public PageMetadataViewModel PageMetadata { get; set; } = new()
    {
        Title = LanguageManager.Instance.Page_Title_WindowDialog,
        Description = LanguageManager.Instance.Page_Description_WindowDialog,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_WindowDialog)],
        Tags = ["Dialog", "Modal", "Window"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/WindowDialogDemo/WindowDialogDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/WindowDialogDemo/WindowDialogDemoViewModel.cs",
        InlineXamlSupport = false,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    public WindowDialogDemoViewModel()
    {
        BasicUsageSection = CreateSection(
            LanguageManager.Instance.Page_WindowDialog_Section_Basic_Usage_Header,
            LanguageManager.Instance.Page_WindowDialog_Section_Basic_Usage_Description,
            BasicUsageAnchorId, DemoSectionTag.Function,
            """
            <Button Command="{Binding StandardDialog.ShowDialogCommand}"
                    Content="Show standard dialog" />
            """,
            """
            await Dialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                new DefaultDemoDialogViewModel());
            """);
        PositioningSection = CreateSection(
            LanguageManager.Instance.Page_WindowDialog_Section_Positioning_Header,
            LanguageManager.Instance.Page_WindowDialog_Section_Positioning_Description,
            PositioningAnchorId, DemoSectionTag.Function,
            """
            <u:EnumSelector EnumType="WindowStartupLocation" Value="{Binding Location}" />
            <u:NumericIntUpDown InnerLeftContent="X" Value="{Binding X}" />
            <u:NumericIntUpDown InnerLeftContent="Y" Value="{Binding Y}" />
            <Button Command="{Binding ShowDialogCommand}" Content="Show at this position" />
            """,
            """
            var options = new DialogOptions { StartupLocation = Location };
            if (X.HasValue && Y.HasValue)
                options.Position = new PixelPoint(X.Value, Y.Value);
            await Dialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                new DefaultDemoDialogViewModel(), options: options);
            """);
        CustomContentSection = CreateSection(
            LanguageManager.Instance.Page_WindowDialog_Section_Custom_Content_Header,
            LanguageManager.Instance.Page_WindowDialog_Section_Custom_Content_Description,
            CustomContentAnchorId, DemoSectionTag.Function,
            """
            <Button Command="{Binding CustomDialog.ShowDialogCommand}"
                    Content="Show custom dialog" />
            """,
            """
            await Dialog.ShowCustomAsync<CustomDemoDialog, CustomDemoDialogViewModel, object>(
                new CustomDemoDialogViewModel());
            """);
        ModalitySection = CreateSection(
            LanguageManager.Instance.Page_WindowDialog_Section_Modality_Header,
            LanguageManager.Instance.Page_WindowDialog_Section_Modality_Description,
            ModalityAnchorId, DemoSectionTag.Function,
            """
            <CheckBox Content="Modal" IsChecked="{Binding IsModal}" />
            <Button Command="{Binding ShowDialogCommand}" Content="Show custom dialog" />
            """,
            """
            if (IsModal)
                await Dialog.ShowCustomAsync<CustomDemoDialog, CustomDemoDialogViewModel, object>(
                    new CustomDemoDialogViewModel());
            else
                Dialog.ShowCustom<CustomDemoDialog, CustomDemoDialogViewModel>(
                    new CustomDemoDialogViewModel());
            """);
        StyleClassSection = CreateSection(
            LanguageManager.Instance.Page_WindowDialog_Section_Style_Class_Header,
            LanguageManager.Instance.Page_WindowDialog_Section_Style_Class_Description,
            StyleClassAnchorId, DemoSectionTag.Style,
            """
            <TextBox Text="{Binding StyleClass}" PlaceholderText="Custom" />
            <Button Command="{Binding ShowDialogCommand}" Content="Show styled dialog" />
            """,
            """
            var options = new DialogOptions { StyleClass = StyleClass };
            await Dialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                new DefaultDemoDialogViewModel(), options: options);
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

    public StandardWindowDialogDemoViewModel StandardDialog { get; } = new();
    public WindowDialogPositionDemoViewModel Positioning { get; } = new();
    public CustomWindowDialogDemoViewModel CustomDialog { get; } = new();
    public WindowDialogModalityDemoViewModel Modality { get; } = new();
    public WindowDialogStyleDemoViewModel Styling { get; } = new();
    public DemoSectionViewModel BasicUsageSection { get; }
    public DemoSectionViewModel PositioningSection { get; }
    public DemoSectionViewModel CustomContentSection { get; }
    public DemoSectionViewModel ModalitySection { get; }
    public DemoSectionViewModel StyleClassSection { get; }
    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new() { Header = LanguageManager.Instance.Page_WindowDialog_Section_Basic_Usage_Header, AnchorId = BasicUsageAnchorId },
        new() { Header = LanguageManager.Instance.Page_WindowDialog_Section_Positioning_Header, AnchorId = PositioningAnchorId },
        new() { Header = LanguageManager.Instance.Page_WindowDialog_Section_Custom_Content_Header, AnchorId = CustomContentAnchorId },
        new() { Header = LanguageManager.Instance.Page_WindowDialog_Section_Modality_Header, AnchorId = ModalityAnchorId },
        new() { Header = LanguageManager.Instance.Page_WindowDialog_Section_Style_Class_Header, AnchorId = StyleClassAnchorId },
    ];
}

public partial class StandardWindowDialogDemoViewModel : ObservableObject
{
    public ICommand ShowDialogCommand { get; } = new AsyncRelayCommand(ShowDialog);

    private static async Task ShowDialog()
    {
        if (!await WindowDialogSupport.EnsureSupportedAsync())
            return;
        await Dialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
            new DefaultDemoDialogViewModel());
    }
}

public partial class WindowDialogPositionDemoViewModel : ObservableObject
{
    [ObservableProperty] public partial WindowStartupLocation Location { get; set; } = WindowStartupLocation.CenterScreen;
    [ObservableProperty] public partial int? X { get; set; }
    [ObservableProperty] public partial int? Y { get; set; }

    public ICommand ShowDialogCommand { get; }

    public WindowDialogPositionDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
    }

    private async Task ShowDialog()
    {
        if (!await WindowDialogSupport.EnsureSupportedAsync())
            return;
        var options = new DialogOptions { StartupLocation = Location };
        if (X.HasValue && Y.HasValue)
            options.Position = new PixelPoint(X.Value, Y.Value);
        await Dialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
            new DefaultDemoDialogViewModel(), options: options);
    }
}

public partial class CustomWindowDialogDemoViewModel : ObservableObject
{
    public ICommand ShowDialogCommand { get; } = new AsyncRelayCommand(ShowDialog);

    private static async Task ShowDialog()
    {
        if (!await WindowDialogSupport.EnsureSupportedAsync())
            return;
        await Dialog.ShowCustomAsync<CustomDemoDialog, CustomDemoDialogViewModel, object>(
            new CustomDemoDialogViewModel());
    }
}

public partial class WindowDialogModalityDemoViewModel : ObservableObject
{
    [ObservableProperty] public partial bool IsModal { get; set; } = true;
    public ICommand ShowDialogCommand { get; }

    public WindowDialogModalityDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
    }

    private async Task ShowDialog()
    {
        if (!await WindowDialogSupport.EnsureSupportedAsync())
            return;
        if (IsModal)
            await Dialog.ShowCustomAsync<CustomDemoDialog, CustomDemoDialogViewModel, object>(
                new CustomDemoDialogViewModel());
        else
            Dialog.ShowCustom<CustomDemoDialog, CustomDemoDialogViewModel>(
                new CustomDemoDialogViewModel());
    }
}

public partial class WindowDialogStyleDemoViewModel : ObservableObject
{
    [ObservableProperty] public partial string? StyleClass { get; set; } = "Custom";
    public ICommand ShowDialogCommand { get; }

    public WindowDialogStyleDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
    }

    private async Task ShowDialog()
    {
        if (!await WindowDialogSupport.EnsureSupportedAsync())
            return;
        var options = new DialogOptions { StyleClass = StyleClass };
        await Dialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
            new DefaultDemoDialogViewModel(), options: options);
    }
}

internal static class WindowDialogSupport
{
    public static async Task<bool> EnsureSupportedAsync()
    {
        if (!OperatingSystem.IsBrowser() && !OperatingSystem.IsAndroid() && !OperatingSystem.IsIOS())
            return true;

        await OverlayMessageBox.ShowAsync(
            "Window dialogs are not supported on this platform. Please use overlay dialogs instead.");
        return false;
    }
}
