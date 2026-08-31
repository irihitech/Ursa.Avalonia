using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.DateOffsetPickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DatePickersPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DateOffsetPickerDemo))]
public partial class DateOffsetPickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "DateOffsetPicker";
    public const string Menu_Header = "Menu_Header_DateOffsetPicker";
    private const string DefaultBehaviorAnchorId = "date-offset-picker-default-behavior";
    private const string DisplayFormatAnchorId = "date-offset-picker-display-format";
    private const string NeedConfirmationAnchorId = "date-offset-picker-need-confirmation";
    private const string UseWithBindingAnchorId = "date-offset-picker-use-with-binding";
    private const string ReadonlyModeAnchorId = "date-offset-picker-readonly-mode";
    private const string DataValidationAnchorId = "date-offset-picker-data-validation";
    private const string StyleClassesAnchorId = "date-offset-picker-style-classes";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateOffsetPicker,
        Description = LanguageManager.Instance.Page_Description_DateOffsetPicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DatePickers), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateOffsetPicker)],
        Tags = ["DateOffsetPicker", "Date", "Offset"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOffsetPickerDemo/DateOffsetPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOffsetPickerDemo/DateOffsetPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTimeOffset? SelectedDate { get; set; }

    [ObservableProperty]
    [Required(ErrorMessage = "Please select a date")]
    public partial DateTimeOffset? ValidatedDate { get; set; }

    public DemoSectionViewModel DefaultBehaviorSection { get; }
    public DemoSectionViewModel DisplayFormatSection { get; }
    public DemoSectionViewModel NeedConfirmationSection { get; }
    public DemoSectionViewModel UseWithBindingSection { get; }
    public DemoSectionViewModel ReadonlyModeSection { get; }
    public DemoSectionViewModel DataValidationSection { get; }
    public DemoSectionViewModel StyleClassesSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new() { Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Default_Behavior_Header, AnchorId = DefaultBehaviorAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Display_Format_Header, AnchorId = DisplayFormatAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Need_Confirmation_Header, AnchorId = NeedConfirmationAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Use_With_Binding_Header, AnchorId = UseWithBindingAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Readonly_Mode_Header, AnchorId = ReadonlyModeAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Data_Validation_Header, AnchorId = DataValidationAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Style_Classes_Header, AnchorId = StyleClassesAnchorId }
    ];

    public DateOffsetPickerDemoViewModel()
    {
        SelectedDate = DateTimeOffset.Now;
        ValidatedDate = DateTimeOffset.Now;

        DefaultBehaviorSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Default_Behavior_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetPicker_Section_Default_Behavior_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DefaultBehaviorAnchorId
        };
        DefaultBehaviorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateOffsetPicker />
                          """
        });

        DisplayFormatSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Display_Format_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetPicker_Section_Display_Format_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DisplayFormatAnchorId
        };
        DisplayFormatSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBox InnerLeftContent="Display Format"
                                   Name="format"
                                   Text="MMM dd, yyyy HH:mm zzz" />
                          <u:DateOffsetPicker
                              Width="400"
                              DisplayFormat="{Binding #format.Text}"
                              ShowOffsetSelection="True"
                              SelectedDate="2025-06-26+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          """
        });

        NeedConfirmationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Need_Confirmation_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetPicker_Section_Need_Confirmation_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = NeedConfirmationAnchorId
        };
        NeedConfirmationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBlock Text="NeedConfirmation=False" />
                          <u:DateOffsetPicker
                              Width="400"
                              DisplayFormat="yyyy-MM-dd"
                              ShowOffsetSelection="True"
                              SelectedDate="2025-06-26+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          <TextBlock Text="NeedConfirmation=True" />
                          <u:DateOffsetPicker
                              Width="400"
                              DisplayFormat="yyyy-MM-dd"
                              ShowOffsetSelection="True"
                              NeedConfirmation="True"
                              SelectedDate="2025-06-26+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          """
        });

        UseWithBindingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Use_With_Binding_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetPicker_Section_Use_With_Binding_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = UseWithBindingAnchorId
        };
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBlock Text="{Binding SelectedDate, StringFormat='Selected: {0:yyyy-MM-dd zzz}'}" />
                          <u:DateOffsetPicker
                              Width="280"
                              DisplayFormat="yyyy-MM-dd"
                              ShowOffsetSelection="True"
                              SelectedDate="{Binding SelectedDate, Mode=TwoWay}"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          """
        });
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial DateTimeOffset? SelectedDate { get; set; }

                          public DateOffsetPickerDemoViewModel()
                          {
                              SelectedDate = DateTimeOffset.Now;
                          }
                          """
        });

        ReadonlyModeSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Readonly_Mode_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetPicker_Section_Readonly_Mode_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = ReadonlyModeAnchorId
        };
        ReadonlyModeSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateOffsetPicker
                              Width="280"
                              ShowOffsetSelection="True"
                              IsReadOnly="True"
                              DisplayFormat="yyyy-MM-dd"
                              SelectedDate="2025-06-26+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          """
        });

        DataValidationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Data_Validation_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetPicker_Section_Data_Validation_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = DataValidationAnchorId
        };
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateOffsetPicker
                              Width="280"
                              Classes="ClearButton"
                              DisplayFormat="yyyy-MM-dd"
                              ShowOffsetSelection="True"
                              SelectedDate="{Binding ValidatedDate, Mode=TwoWay}" />
                          """
        });
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty]
                          [Required(ErrorMessage = "Please select a date")]
                          public partial DateTimeOffset? ValidatedDate { get; set; }
                          """
        });

        StyleClassesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetPicker_Section_Style_Classes_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetPicker_Section_Style_Classes_Description },
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

                          <u:DateOffsetPicker
                              Width="280"
                              DisplayFormat="yyyy-MM-dd"
                              ShowOffsetSelection="True"
                              SelectedDate="2025-06-26+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00"
                              u:ClassSelector.Source="{Binding #sizeClassSelector}" />
                          """
        });
    }
}
