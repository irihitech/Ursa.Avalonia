using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.TimePickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = TimePickersPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(TimePickerDemo))]
public partial class TimePickerDemoViewModel: ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "TimePicker";
    public const string Menu_Header = "Menu_Header_TimePicker";
    private const string BasicAnchorId = "time-picker-basic";
    private const string DisplayFormatAnchorId = "time-picker-display-format";
    private const string PanelFormatAnchorId = "time-picker-panel-format";
    private const string UseWithBindingAnchorId = "time-picker-use-with-binding";
    private const string NeedConfirmationAnchorId = "time-picker-need-confirmation";
    private const string ReadonlyModeAnchorId = "time-picker-readonly-mode";
    private const string DataValidationAnchorId = "time-picker-data-validation";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TimePicker,
        Description = LanguageManager.Instance.Page_Description_TimePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_TimePickers), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TimePicker)],
        Tags = ["TimePicker", "Time", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimePickerDemo/TimePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimePickerDemo/TimePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial TimeSpan? Time { get; set; }

    [ObservableProperty]
    [Required(ErrorMessage = "Please select a time")]
    public partial TimeSpan? ValidatedTime { get; set; }

    public DemoSectionViewModel BasicSection { get; }
    public DemoSectionViewModel DisplayFormatSection { get; }
    public DemoSectionViewModel PanelFormatSection { get; }
    public DemoSectionViewModel UseWithBindingSection { get; }
    public DemoSectionViewModel NeedConfirmationSection { get; }
    public DemoSectionViewModel ReadonlyModeSection { get; }
    public DemoSectionViewModel DataValidationSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new() { Header = LanguageManager.Instance.Page_TimePicker_Section_Basic_Header, AnchorId = BasicAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimePicker_Section_Display_Format_Header, AnchorId = DisplayFormatAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimePicker_Section_Panel_Format_Header, AnchorId = PanelFormatAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimePicker_Section_Use_With_Binding_Header, AnchorId = UseWithBindingAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimePicker_Section_Need_Confirmation_Header, AnchorId = NeedConfirmationAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimePicker_Section_Readonly_Mode_Header, AnchorId = ReadonlyModeAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimePicker_Section_Data_Validation_Header, AnchorId = DataValidationAnchorId }
    ];

    public TimePickerDemoViewModel()
    {
        Time = new TimeSpan(12, 20, 0);
        ValidatedTime = new TimeSpan(12, 20, 0);

        BasicSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimePicker_Section_Basic_Header,
            Descriptions = { LanguageManager.Instance.Page_TimePicker_Section_Basic_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = BasicAnchorId
        };
        BasicSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TimePicker />
                          """
        });

        DisplayFormatSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimePicker_Section_Display_Format_Header,
            Descriptions = { LanguageManager.Instance.Page_TimePicker_Section_Display_Format_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DisplayFormatAnchorId
        };
        DisplayFormatSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <DockPanel HorizontalSpacing="12">
                              <TextBox
                                  Name="displayFormatBox"
                                  DockPanel.Dock="Right"
                                  Width="280"
                                  InnerLeftContent="Display Format"
                                  Text="HH:mm:ss" />
                              <u:TimePicker
                                  Width="220"
                                  DisplayFormat="{Binding #displayFormatBox.Text}"
                                  SelectedTime="12:20:30" />
                          </DockPanel>
                          """
        });

        PanelFormatSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimePicker_Section_Panel_Format_Header,
            Descriptions = { LanguageManager.Instance.Page_TimePicker_Section_Panel_Format_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = PanelFormatAnchorId
        };
        PanelFormatSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <DockPanel HorizontalSpacing="12">
                              <TextBox
                                  Name="panelFormatBox"
                                  DockPanel.Dock="Right"
                                  Width="280"
                                  InnerLeftContent="Panel Format"
                                  Text="tt HH mm ss" />
                              <u:TimePicker
                                  Width="220"
                                  DisplayFormat="HH:mm:ss"
                                  PanelFormat="{Binding #panelFormatBox.Text}"
                                  SelectedTime="12:20:30" />
                          </DockPanel>
                          """
        });

        UseWithBindingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimePicker_Section_Use_With_Binding_Header,
            Descriptions = { LanguageManager.Instance.Page_TimePicker_Section_Use_With_Binding_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = UseWithBindingAnchorId
        };
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBlock Text="{Binding Time, StringFormat='Selected: {0}'}" />
                          <u:TimePicker
                              Width="220"
                              DisplayFormat="HH:mm:ss"
                              SelectedTime="{Binding Time, Mode=TwoWay}" />
                          """
        });
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial TimeSpan? Time { get; set; }

                          public TimePickerDemoViewModel()
                          {
                              Time = new TimeSpan(12, 20, 0);
                          }
                          """
        });

        NeedConfirmationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimePicker_Section_Need_Confirmation_Header,
            Descriptions = { LanguageManager.Instance.Page_TimePicker_Section_Need_Confirmation_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = NeedConfirmationAnchorId
        };
        NeedConfirmationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <ToggleSwitch Name="needConfirmToggle" Content="Need Confirmation" />
                          <u:TimePicker
                              Width="220"
                              DisplayFormat="HH:mm:ss"
                              NeedConfirmation="{Binding #needConfirmToggle.IsChecked}"
                              SelectedTime="12:20:30" />
                          """
        });

        ReadonlyModeSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimePicker_Section_Readonly_Mode_Header,
            Descriptions = { LanguageManager.Instance.Page_TimePicker_Section_Readonly_Mode_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = ReadonlyModeAnchorId
        };
        ReadonlyModeSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TimePicker
                              Width="220"
                              DisplayFormat="HH:mm:ss"
                              IsReadOnly="True"
                              SelectedTime="10:30:00" />
                          """
        });

        DataValidationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimePicker_Section_Data_Validation_Header,
            Descriptions = { LanguageManager.Instance.Page_TimePicker_Section_Data_Validation_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = DataValidationAnchorId
        };
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TimePicker
                              Width="220"
                              Classes="ClearButton"
                              DisplayFormat="HH:mm:ss"
                              SelectedTime="{Binding ValidatedTime, Mode=TwoWay}" />
                          """
        });
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty]
                          [Required(ErrorMessage = "Please select a time")]
                          public partial TimeSpan? ValidatedTime { get; set; }
                          """
        });
    }
}