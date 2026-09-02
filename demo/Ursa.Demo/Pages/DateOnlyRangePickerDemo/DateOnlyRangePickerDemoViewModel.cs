using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.DateOnlyRangePickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DatePickersPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DateOnlyRangePickerDemo))]
public partial class DateOnlyRangePickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "DateOnlyRangePicker";
    public const string Menu_Header = "Menu_Header_DateOnlyRangePicker";
    private const string DefaultBehaviorAnchorId = "date-only-range-picker-default-behavior";
    private const string DisplayFormatAnchorId = "date-only-range-picker-display-format";
    private const string NeedConfirmationAnchorId = "date-only-range-picker-need-confirmation";
    private const string UseWithBindingAnchorId = "date-only-range-picker-use-with-binding";
    private const string ReadonlyModeAnchorId = "date-only-range-picker-readonly-mode";
    private const string DataValidationAnchorId = "date-only-range-picker-data-validation";
    private const string StyleClassesAnchorId = "date-only-range-picker-style-classes";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateOnlyRangePicker,
        Description = LanguageManager.Instance.Page_Description_DateOnlyRangePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DatePickers), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateOnlyRangePicker)],
        Tags = ["DateOnlyRangePicker", "Date", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOnlyRangePickerDemo/DateOnlyRangePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOnlyRangePickerDemo/DateOnlyRangePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateOnly? StartDate { get; set; }
    [ObservableProperty] public partial DateOnly? EndDate { get; set; }
    
    public ValidatedDateOnlyRange ValidatedRange { get; } = new();

    public DemoSectionViewModel DefaultBehaviorSection { get; }
    public DemoSectionViewModel DisplayFormatSection { get; }
    public DemoSectionViewModel NeedConfirmationSection { get; }
    public DemoSectionViewModel UseWithBindingSection { get; }
    public DemoSectionViewModel ReadonlyModeSection { get; }
    public DemoSectionViewModel DataValidationSection { get; }
    public DemoSectionViewModel StyleClassesSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new() { Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Default_Behavior_Header, AnchorId = DefaultBehaviorAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Display_Format_Header, AnchorId = DisplayFormatAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Need_Confirmation_Header, AnchorId = NeedConfirmationAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Use_With_Binding_Header, AnchorId = UseWithBindingAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Readonly_Mode_Header, AnchorId = ReadonlyModeAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Data_Validation_Header, AnchorId = DataValidationAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Style_Classes_Header, AnchorId = StyleClassesAnchorId }
    ];

    public DateOnlyRangePickerDemoViewModel()
    {
        StartDate = DateOnly.FromDateTime(DateTime.Today);
        EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7));

        DefaultBehaviorSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Default_Behavior_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Default_Behavior_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DefaultBehaviorAnchorId
        };
        DefaultBehaviorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateOnlyRangePicker />
                          """
        });

        DisplayFormatSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Display_Format_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Display_Format_Description },
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
                          <u:DateOnlyRangePicker
                              Width="360"
                              DisplayFormat="{Binding #format.Text}"
                              SelectedStartDate="2025-06-26"
                              SelectedEndDate="2025-06-30" />
                          """
        });

        NeedConfirmationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Need_Confirmation_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Need_Confirmation_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = NeedConfirmationAnchorId
        };
        NeedConfirmationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <!-- DateOnlyRangePicker applies the range immediately and does not expose NeedConfirmation. -->
                          <u:DateOnlyRangePicker
                              DisplayFormat="yyyy-MM-dd"
                              SelectedStartDate="2025-06-26"
                              SelectedEndDate="2025-06-30" />
                          """
        });

        UseWithBindingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Use_With_Binding_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Use_With_Binding_Description },
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
                          <u:DateOnlyRangePicker
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
                          [ObservableProperty] public partial DateOnly? StartDate { get; set; }
                          [ObservableProperty] public partial DateOnly? EndDate { get; set; }

                          public DateOnlyRangePickerDemoViewModel()
                          {
                              StartDate = DateOnly.FromDateTime(DateTime.Today);
                              EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7));
                          }
                          """
        });

        ReadonlyModeSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Readonly_Mode_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Readonly_Mode_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = ReadonlyModeAnchorId
        };
        ReadonlyModeSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateOnlyRangePicker
                              IsReadOnly="True"
                              DisplayFormat="yyyy-MM-dd"
                              SelectedStartDate="2025-06-26"
                              SelectedEndDate="2025-06-30" />
                          """
        });

        DataValidationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Data_Validation_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Data_Validation_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = DataValidationAnchorId
        };
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateOnlyRangePicker
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
                          public partial class ValidatedDateOnlyRange : ObservableValidator
                          {
                              [ObservableProperty]
                              [Required(ErrorMessage = "Start date is required")]
                              public partial DateOnly? Start { get; set; }

                              [ObservableProperty]
                              [Required(ErrorMessage = "End date is required")]
                              public partial DateOnly? End { get; set; }
                          }
                          """
        });

        StyleClassesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Style_Classes_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOnlyRangePicker_Section_Style_Classes_Description },
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

                          <u:DateOnlyRangePicker
                              Width="360"
                              DisplayFormat="yyyy-MM-dd"
                              SelectedStartDate="2025-06-26"
                              SelectedEndDate="2025-06-30"
                              u:ClassSelector.Source="{Binding #sizeClassSelector}" />
                          """
        });
    }
}

public partial class ValidatedDateOnlyRange : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Start date is required")]
    public partial DateOnly? Start { get; set; }
    
    [ObservableProperty]
    [Required(ErrorMessage = "End date is required")]
    public partial DateOnly? End { get; set; }

    public ValidatedDateOnlyRange()
    {
        Start = DateOnly.FromDateTime(DateTime.Today);
        End = DateOnly.FromDateTime(DateTime.Today.AddDays(7));
    }
}
