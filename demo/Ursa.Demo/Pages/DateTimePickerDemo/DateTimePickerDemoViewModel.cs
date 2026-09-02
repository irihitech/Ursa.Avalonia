using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.DateTimePickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DateAndTimePage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DateTimePickerDemo))]
public partial class DateTimePickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "DateTimePicker";
    public const string Menu_Header = "Menu_Header_DateTimePicker";
    private const string DefaultBehaviorAnchorId = "date-time-picker-default-behavior";
    private const string DisplayFormatAnchorId = "date-time-picker-display-format";
    private const string PanelFormatAnchorId = "date-time-picker-panel-format";
    private const string NeedConfirmationAnchorId = "date-time-picker-need-confirmation";
    private const string UseWithBindingAnchorId = "date-time-picker-use-with-binding";
    private const string ReadonlyModeAnchorId = "date-time-picker-readonly-mode";
    private const string DataValidationAnchorId = "date-time-picker-data-validation";
    private const string StyleClassesAnchorId = "date-time-picker-style-classes";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateTimePicker,
        Description = LanguageManager.Instance.Page_Description_DateTimePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateTimePicker)],
        Tags = ["DateTimePicker", "Date", "Time"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateTimePickerDemo/DateTimePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateTimePickerDemo/DateTimePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTime? SelectedDateTime { get; set; }

    [ObservableProperty]
    [Required(ErrorMessage = "Please select a date and time")]
    public partial DateTime? ValidatedDateTime { get; set; }

    public DemoSectionViewModel DefaultBehaviorSection { get; }
    public DemoSectionViewModel DisplayFormatSection { get; }
    public DemoSectionViewModel PanelFormatSection { get; }
    public DemoSectionViewModel NeedConfirmationSection { get; }
    public DemoSectionViewModel UseWithBindingSection { get; }
    public DemoSectionViewModel ReadonlyModeSection { get; }
    public DemoSectionViewModel DataValidationSection { get; }
    public DemoSectionViewModel StyleClassesSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new() { Header = LanguageManager.Instance.Page_DateTimePicker_Section_Default_Behavior_Header, AnchorId = DefaultBehaviorAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimePicker_Section_Display_Format_Header, AnchorId = DisplayFormatAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimePicker_Section_Panel_Format_Header, AnchorId = PanelFormatAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimePicker_Section_Need_Confirmation_Header, AnchorId = NeedConfirmationAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimePicker_Section_Use_With_Binding_Header, AnchorId = UseWithBindingAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimePicker_Section_Readonly_Mode_Header, AnchorId = ReadonlyModeAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimePicker_Section_Data_Validation_Header, AnchorId = DataValidationAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimePicker_Section_Style_Classes_Header, AnchorId = StyleClassesAnchorId }
    ];

    public DateTimePickerDemoViewModel()
    {
        SelectedDateTime = DateTime.Now;
        ValidatedDateTime = DateTime.Now;

        DefaultBehaviorSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimePicker_Section_Default_Behavior_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimePicker_Section_Default_Behavior_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DefaultBehaviorAnchorId
        };
        DefaultBehaviorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateTimePicker />
                          """
        });

        DisplayFormatSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimePicker_Section_Display_Format_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimePicker_Section_Display_Format_Description },
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
                                  Width="300"
                                  InnerLeftContent="Display Format"
                                  Text="yyyy-MM-dd HH:mm:ss" />
                              <u:DateTimePicker
                                  Width="360"
                                  DisplayFormat="{Binding #displayFormatBox.Text}"
                                  SelectedDate="2025-06-26 10:30:00" />
                          </DockPanel>
                          """
        });

        PanelFormatSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimePicker_Section_Panel_Format_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimePicker_Section_Panel_Format_Description },
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
                                  Width="300"
                                  InnerLeftContent="Panel Format"
                                  Text="tt HH mm ss" />
                              <u:DateTimePicker
                                  Width="360"
                                  DisplayFormat="yyyy-MM-dd HH:mm:ss"
                                  PanelFormat="{Binding #panelFormatBox.Text}"
                                  SelectedDate="2025-06-26 10:30:00" />
                          </DockPanel>
                          """
        });

        NeedConfirmationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimePicker_Section_Need_Confirmation_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimePicker_Section_Need_Confirmation_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = NeedConfirmationAnchorId
        };
        NeedConfirmationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <ToggleSwitch Name="needConfirmToggle" Content="Need Confirmation" />
                          <u:DateTimePicker
                              Width="360"
                              DisplayFormat="yyyy-MM-dd HH:mm:ss"
                              NeedConfirmation="{Binding #needConfirmToggle.IsChecked}"
                              SelectedDate="2025-06-26 10:30:00" />
                          """
        });

        UseWithBindingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimePicker_Section_Use_With_Binding_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimePicker_Section_Use_With_Binding_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = UseWithBindingAnchorId
        };
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBlock Text="{Binding SelectedDateTime, StringFormat='Selected: {0:yyyy-MM-dd HH:mm:ss}'}" />
                          <u:DateTimePicker
                              Width="360"
                              DisplayFormat="yyyy-MM-dd HH:mm:ss"
                              SelectedDate="{Binding SelectedDateTime, Mode=TwoWay}" />
                          """
        });
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial DateTime? SelectedDateTime { get; set; }

                          public DateTimePickerDemoViewModel()
                          {
                              SelectedDateTime = DateTime.Now;
                          }
                          """
        });

        ReadonlyModeSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimePicker_Section_Readonly_Mode_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimePicker_Section_Readonly_Mode_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = ReadonlyModeAnchorId
        };
        ReadonlyModeSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateTimePicker
                              Width="360"
                              DisplayFormat="yyyy-MM-dd HH:mm:ss"
                              IsReadOnly="True"
                              SelectedDate="2025-06-26 10:30:00" />
                          """
        });

        DataValidationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimePicker_Section_Data_Validation_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimePicker_Section_Data_Validation_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = DataValidationAnchorId
        };
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateTimePicker
                              Width="360"
                              Classes="ClearButton"
                              DisplayFormat="yyyy-MM-dd HH:mm:ss"
                              SelectedDate="{Binding ValidatedDateTime, Mode=TwoWay}" />
                          """
        });
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty]
                          [Required(ErrorMessage = "Please select a date and time")]
                          public partial DateTime? ValidatedDateTime { get; set; }
                          """
        });

        StyleClassesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimePicker_Section_Style_Classes_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimePicker_Section_Style_Classes_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = StyleClassesAnchorId
        };
        StyleClassesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:ClassSelector Name="sizeClassSelector">
                              <u:ClassSelectorGroup Header="Size">
                                  <u:ClassSelectorItem ClassName="Small" />
                                  <u:ClassSelectorItem ClassName="Large" />
                              </u:ClassSelectorGroup>
                              <u:ClassSelectorGroup Header="Other">
                                  <u:ClassSelectorItem ClassName="ClearButton" />
                              </u:ClassSelectorGroup>
                          </u:ClassSelector>

                          <u:DateTimePicker
                              Width="360"
                              DisplayFormat="yyyy-MM-dd HH:mm:ss"
                              SelectedDate="2025-06-26 10:30:00"
                              u:ClassSelector.Source="{Binding #sizeClassSelector}" />
                          """
        });
    }
}