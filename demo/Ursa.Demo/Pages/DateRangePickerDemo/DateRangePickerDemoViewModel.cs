using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.DateRangePickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DatePickersPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DateRangePickerDemo))]
public partial class DateRangePickerDemoViewModel: ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "DateRangePicker";
    public const string Menu_Header = "Menu_Header_DateRangePicker";
    private const string DefaultBehaviorAnchorId = "date-range-picker-default-behavior";
    private const string DisplayFormatAnchorId = "date-range-picker-display-format";
    private const string NeedConfirmationAnchorId = "date-range-picker-need-confirmation";
    private const string UseWithBindingAnchorId = "date-range-picker-use-with-binding";
    private const string ReadonlyModeAnchorId = "date-range-picker-readonly-mode";
    private const string DataValidationAnchorId = "date-range-picker-data-validation";
    private const string StyleClassesAnchorId = "date-range-picker-style-classes";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateRangePicker,
        Description = LanguageManager.Instance.Page_Description_DateRangePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DatePickers), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateRangePicker)],
        Tags = ["DateRangePicker", "Date", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateRangePickerDemo/DateRangePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateRangePickerDemo/DateRangePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTime? StartDate { get; set; }
    [ObservableProperty] public partial DateTime? EndDate { get; set; }
    
    public ValidatedDateRange ValidatedRange { get; } = new();

    public DemoSectionViewModel DefaultBehaviorSection { get; }
    public DemoSectionViewModel DisplayFormatSection { get; }
    public DemoSectionViewModel NeedConfirmationSection { get; }
    public DemoSectionViewModel UseWithBindingSection { get; }
    public DemoSectionViewModel ReadonlyModeSection { get; }
    public DemoSectionViewModel DataValidationSection { get; }
    public DemoSectionViewModel StyleClassesSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new() { Header = LanguageManager.Instance.Page_DateRangePicker_Section_Default_Behavior_Header, AnchorId = DefaultBehaviorAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateRangePicker_Section_Display_Format_Header, AnchorId = DisplayFormatAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateRangePicker_Section_Need_Confirmation_Header, AnchorId = NeedConfirmationAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateRangePicker_Section_Use_With_Binding_Header, AnchorId = UseWithBindingAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateRangePicker_Section_Readonly_Mode_Header, AnchorId = ReadonlyModeAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateRangePicker_Section_Data_Validation_Header, AnchorId = DataValidationAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateRangePicker_Section_Style_Classes_Header, AnchorId = StyleClassesAnchorId }
    ];

    public DateRangePickerDemoViewModel()
    {
        StartDate = DateTime.Today;
        EndDate = DateTime.Today.AddDays(7);

        DefaultBehaviorSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateRangePicker_Section_Default_Behavior_Header,
            Descriptions = { LanguageManager.Instance.Page_DateRangePicker_Section_Default_Behavior_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DefaultBehaviorAnchorId
        };
        DefaultBehaviorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateRangePicker />
                          """
        });

        DisplayFormatSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateRangePicker_Section_Display_Format_Header,
            Descriptions = { LanguageManager.Instance.Page_DateRangePicker_Section_Display_Format_Description },
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
                                   Text="yyyy-MM-dd" />
                          <u:DateRangePicker
                              Width="360"
                              DisplayFormat="{Binding #format.Text}"
                              SelectedStartDate="2025-06-26"
                              SelectedEndDate="2025-06-30" />
                          """
        });

        NeedConfirmationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateRangePicker_Section_Need_Confirmation_Header,
            Descriptions = { LanguageManager.Instance.Page_DateRangePicker_Section_Need_Confirmation_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = NeedConfirmationAnchorId
        };
        NeedConfirmationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <!-- DateRangePicker applies the range immediately and does not expose NeedConfirmation. -->
                          <u:DateRangePicker
                              DisplayFormat="yyyy-MM-dd"
                              SelectedStartDate="2025-06-26"
                              SelectedEndDate="2025-06-30" />
                          """
        });

        UseWithBindingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateRangePicker_Section_Use_With_Binding_Header,
            Descriptions = { LanguageManager.Instance.Page_DateRangePicker_Section_Use_With_Binding_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = UseWithBindingAnchorId
        };
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBlock Text="{Binding StartDate, StringFormat='Start: {0:yyyy-MM-dd}'}" />
                          <TextBlock Text="{Binding EndDate, StringFormat='End: {0:yyyy-MM-dd}'}" />
                          <u:DateRangePicker
                              Width="360"
                              DisplayFormat="yyyy-MM-dd"
                              SelectedStartDate="{Binding StartDate, Mode=TwoWay}"
                              SelectedEndDate="{Binding EndDate, Mode=TwoWay}" />
                          """
        });
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial DateTime? StartDate { get; set; }
                          [ObservableProperty] public partial DateTime? EndDate { get; set; }

                          public DateRangePickerDemoViewModel()
                          {
                              StartDate = DateTime.Today;
                              EndDate = DateTime.Today.AddDays(7);
                          }
                          """
        });

        ReadonlyModeSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateRangePicker_Section_Readonly_Mode_Header,
            Descriptions = { LanguageManager.Instance.Page_DateRangePicker_Section_Readonly_Mode_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = ReadonlyModeAnchorId
        };
        ReadonlyModeSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateRangePicker
                              IsReadOnly="True"
                              DisplayFormat="yyyy-MM-dd"
                              SelectedStartDate="2025-06-26"
                              SelectedEndDate="2025-06-30" />
                          """
        });

        DataValidationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateRangePicker_Section_Data_Validation_Header,
            Descriptions = { LanguageManager.Instance.Page_DateRangePicker_Section_Data_Validation_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = DataValidationAnchorId
        };
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateRangePicker
                              Width="360"
                              Classes="ClearButton"
                              DisplayFormat="yyyy-MM-dd"
                              SelectedStartDate="{Binding ValidatedRange.Start, Mode=TwoWay}"
                              SelectedEndDate="{Binding ValidatedRange.End, Mode=TwoWay}" />
                          """
        });
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public partial class ValidatedDateRange : ObservableValidator
                          {
                              [ObservableProperty]
                              [Required(ErrorMessage = "Start date is required")]
                              public partial DateTime? Start { get; set; }

                              [ObservableProperty]
                              [Required(ErrorMessage = "End date is required")]
                              public partial DateTime? End { get; set; }
                          }
                          """
        });

        StyleClassesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateRangePicker_Section_Style_Classes_Header,
            Descriptions = { LanguageManager.Instance.Page_DateRangePicker_Section_Style_Classes_Description },
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

                          <u:DateRangePicker
                              Width="360"
                              DisplayFormat="yyyy-MM-dd"
                              SelectedStartDate="2025-06-26"
                              SelectedEndDate="2025-06-30"
                              u:ClassSelector.Source="{Binding #sizeClassSelector}" />
                          """
        });
    }
}

public partial class ValidatedDateRange : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Start date is required")]
    public partial DateTime? Start { get; set; }
    
    [ObservableProperty]
    [Required(ErrorMessage = "End date is required")]
    public partial DateTime? End { get; set; }

    public ValidatedDateRange()
    {
        Start = DateTime.Today;
        End = DateTime.Today.AddDays(7);
    }
}