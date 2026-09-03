using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Common;
using Ursa.Controls;
using Ursa.Controls.Options;
using Ursa.Demo.Dialogs;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.DrawerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DrawerDemo))]
public partial class DrawerDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "Drawer";
    public const string Menu_Header = "Menu_Header_Drawer";
    private const string PositionAnchorId = "drawer-position";
    private const string ButtonsAnchorId = "drawer-buttons";
    private const string LightDismissAnchorId = "drawer-light-dismiss";
    private const string CloseButtonAnchorId = "drawer-close-button";
    private const string ResizeAnchorId = "drawer-resize";
    private const string VariantsAnchorId = "drawer-variants";

    public PageMetadataViewModel PageMetadata { get; set; } = new()
    {
        Title = LanguageManager.Instance.Page_Title_Drawer,
        Description = LanguageManager.Instance.Page_Description_Drawer,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Drawer)],
        Tags = ["Drawer", "Panel", "Overlay"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DrawerDemo/DrawerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DrawerDemo/DrawerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    public DrawerDemoViewModel()
    {
        Position = Ursa.Common.Position.Right;
        ShowStandardDrawerCommand = new AsyncRelayCommand(ShowStandardDrawerAsync);
        ShowCustomDrawerCommand = new RelayCommand(ShowCustomDrawer);

        PositionSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Drawer_Section_Position_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_Drawer_Section_Position_Description },
            AnchorId = PositionAnchorId,
        };
        PositionSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Form Width="320"
                                  LabelPosition="Top">
                              <u:EnumSelector u:FormItem.Label="Position"
                                              EnumType="common:Position"
                                              Value="{Binding Position}" />
                              <Button u:FormItem.NoLabel="True"
                                      Content="Show standard drawer"
                                      Command="{Binding ShowStandardDrawerCommand}" />
                          </u:Form>
                          """
        });
        PositionSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial Position Position { get; set; }
                          
                          private DrawerOptions CreateOptions() => new()
                          {
                              Position = Position,
                              Buttons = Buttons,
                              CanLightDismiss = CanLightDismiss,
                              IsCloseButtonVisible = IsCloseButtonVisible,
                              CanResize = CanResize,
                          };
                          
                          public async Task ShowStandardDrawerAsync()
                          {
                              await OverlayDrawer.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                                  new DefaultDemoDialogViewModel(),
                                  null,
                                  CreateOptions());
                          }
                          """
        });

        ButtonsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Drawer_Section_Buttons_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_Drawer_Section_Buttons_Description },
            AnchorId = ButtonsAnchorId,
        };
        ButtonsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Form Width="320"
                                  LabelPosition="Top">
                              <u:EnumSelector u:FormItem.Label="Buttons"
                                              EnumType="u:DialogButton"
                                              Value="{Binding Buttons}" />
                              <Button u:FormItem.NoLabel="True"
                                      Content="Show standard drawer"
                                      Command="{Binding ShowStandardDrawerCommand}" />
                          </u:Form>
                          """
        });
        ButtonsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial DialogButton Buttons { get; set; } = DialogButton.OKCancel;
                          
                          private DrawerOptions CreateOptions() => new()
                          {
                              Position = Position,
                              Buttons = Buttons,
                              CanLightDismiss = CanLightDismiss,
                              IsCloseButtonVisible = IsCloseButtonVisible,
                              CanResize = CanResize,
                          };
                          
                          public async Task ShowStandardDrawerAsync()
                          {
                              await OverlayDrawer.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                                  new DefaultDemoDialogViewModel(),
                                  X,
                                  CreateOptions());
                          }
                          """
        });

        LightDismissSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Drawer_Section_Light_Dismiss_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_Drawer_Section_Light_Dismiss_Description },
            AnchorId = LightDismissAnchorId,
        };
        LightDismissSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Form Width="320"
                                  LabelPosition="Top">
                              <CheckBox u:FormItem.Label="Can LightDismiss"
                                        IsChecked="{Binding CanLightDismiss}" />
                              <Button u:FormItem.NoLabel="True"
                                      Content="Show standard drawer"
                                      Command="{Binding ShowStandardDrawerCommand}" />
                          </u:Form>
                          """
        });
        LightDismissSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial bool CanLightDismiss { get; set; } = true;
                          
                          private DrawerOptions CreateOptions() => new()
                          {
                              Position = Position,
                              Buttons = Buttons,
                              CanLightDismiss = CanLightDismiss,
                              IsCloseButtonVisible = IsCloseButtonVisible,
                              CanResize = CanResize,
                          };
                          
                          public async Task ShowStandardDrawerAsync()
                          {
                              await OverlayDrawer.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                                  new DefaultDemoDialogViewModel(),
                                  null,
                                  CreateOptions());
                          }
                          """
        });

        CloseButtonSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Drawer_Section_Close_Button_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_Drawer_Section_Close_Button_Description },
            AnchorId = CloseButtonAnchorId,
        };
        CloseButtonSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Form Width="320"
                                  LabelPosition="Top">
                              <CheckBox u:FormItem.Label="Is Close Button Visible"
                                        IsChecked="{Binding IsCloseButtonVisible}"
                                        IsThreeState="True" />
                              <Button u:FormItem.NoLabel="True"
                                      Content="Show standard drawer"
                                      Command="{Binding ShowStandardDrawerCommand}" />
                          </u:Form>
                          """
        });
        CloseButtonSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial bool? IsCloseButtonVisible { get; set; } = true;
                          
                          private DrawerOptions CreateOptions() => new()
                          {
                              Position = Position,
                              Buttons = Buttons,
                              CanLightDismiss = CanLightDismiss,
                              IsCloseButtonVisible = IsCloseButtonVisible,
                              CanResize = CanResize,
                          };
                          
                          public async Task ShowStandardDrawerAsync()
                          {
                              await OverlayDrawer.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                                  new DefaultDemoDialogViewModel(),
                                  null,
                                  CreateOptions());
                          }
                          """
        });

        ResizeSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Drawer_Section_Resize_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_Drawer_Section_Resize_Description },
            AnchorId = ResizeAnchorId,
        };
        ResizeSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Form Width="320"
                                  LabelPosition="Top">
                              <CheckBox u:FormItem.Label="CanResize"
                                        IsChecked="{Binding CanResize}" />
                              <Button u:FormItem.NoLabel="True"
                                      Content="Show standard drawer"
                                      Command="{Binding ShowStandardDrawerCommand}" />
                          </u:Form>
                          """
        });
        ResizeSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial bool CanResize { get; set; }
                          
                          private DrawerOptions CreateOptions() => new()
                          {
                              Position = Position,
                              Buttons = Buttons,
                              CanLightDismiss = CanLightDismiss,
                              IsCloseButtonVisible = IsCloseButtonVisible,
                              CanResize = CanResize,
                          };
                          
                          public async Task ShowStandardDrawerAsync()
                          {
                              await OverlayDrawer.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                                  new DefaultDemoDialogViewModel(),
                                  null,
                                  CreateOptions());
                          }
                          """
        });

        DrawerVariantsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Drawer_Section_Drawer_Variants_Header,
            SectionTag = DemoSectionTag.Others,
            Descriptions = { LanguageManager.Instance.Page_Drawer_Section_Drawer_Variants_Description },
            AnchorId = VariantsAnchorId,
        };
        DrawerVariantsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <Grid ColumnDefinitions="*,*"
                                ColumnSpacing="20">
                              <StackPanel Spacing="12">
                                  <Button HorizontalAlignment="Left"
                                          Content="Open standard drawer"
                                          Command="{Binding ShowStandardDrawerCommand}" />
                              </StackPanel>
                              <StackPanel Grid.Column="1"
                                          Spacing="12">
                                  <Button HorizontalAlignment="Left"
                                          Content="Open custom drawer"
                                          Command="{Binding ShowCustomDrawerCommand}" />
                              </StackPanel>
                          </Grid>
                          """
        });
        DrawerVariantsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          private DrawerOptions CreateOptions() => new()
                          {
                              Position = Position,
                              Buttons = Buttons,
                              CanLightDismiss = CanLightDismiss,
                              IsCloseButtonVisible = IsCloseButtonVisible,
                              CanResize = CanResize,
                          };
                          
                          public async Task ShowStandardDrawerAsync()
                          {
                              await OverlayDrawer.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
                                  new DefaultDemoDialogViewModel(),
                                  null,
                                  CreateOptions());
                          }
                          
                          public void ShowCustomDrawer()
                          {
                              OverlayDrawer.ShowCustom<CustomDemoDialog, CustomDemoDialogViewModel>(
                                  new CustomDemoDialogViewModel(),
                                  null,
                                  CreateOptions());
                          }
                          """
        });
    }

    public DemoSectionViewModel PositionSection { get; }
    public DemoSectionViewModel ButtonsSection { get; }
    public DemoSectionViewModel LightDismissSection { get; }
    public DemoSectionViewModel CloseButtonSection { get; }
    public DemoSectionViewModel ResizeSection { get; }
    public DemoSectionViewModel DrawerVariantsSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; set; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_Drawer_Section_Position_Header,
            AnchorId = PositionAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Drawer_Section_Buttons_Header,
            AnchorId = ButtonsAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Drawer_Section_Light_Dismiss_Header,
            AnchorId = LightDismissAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Drawer_Section_Close_Button_Header,
            AnchorId = CloseButtonAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Drawer_Section_Resize_Header,
            AnchorId = ResizeAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Drawer_Section_Drawer_Variants_Header,
            AnchorId = VariantsAnchorId,
        },
    ];

    public ICommand ShowStandardDrawerCommand { get; }
    public ICommand ShowCustomDrawerCommand { get; }

    [ObservableProperty] public partial Position Position { get; set; }
    [ObservableProperty] public partial DialogButton Buttons { get; set; } = DialogButton.OKCancel;
    [ObservableProperty] public partial bool CanLightDismiss { get; set; } = true;
    [ObservableProperty] public partial bool? IsCloseButtonVisible { get; set; } = true;
    [ObservableProperty] public partial bool CanResize { get; set; }

    private DrawerOptions CreateOptions() => new()
    {
        Position = Position,
        Buttons = Buttons,
        CanLightDismiss = CanLightDismiss,
        IsCloseButtonVisible = IsCloseButtonVisible,
        CanResize = CanResize,
    };

    public async Task ShowStandardDrawerAsync()
    {
        await OverlayDrawer.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(
            new DefaultDemoDialogViewModel(),
            null,
            CreateOptions());
    }

    public void ShowCustomDrawer()
    {
        OverlayDrawer.ShowCustom<CustomDemoDialog, CustomDemoDialogViewModel>(
            new CustomDemoDialogViewModel(),
            null,
            CreateOptions());
    }

}
