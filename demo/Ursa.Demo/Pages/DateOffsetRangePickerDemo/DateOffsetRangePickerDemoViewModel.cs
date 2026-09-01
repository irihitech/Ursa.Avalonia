using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.DateOffsetRangePickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DatePickersPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DateOffsetRangePickerDemo))]
public partial class DateOffsetRangePickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "DateOffsetRangePicker";
    public const string Menu_Header = "Menu_Header_DateOffsetRangePicker";
    private const string DefaultBehaviorAnchorId = "date-offset-range-picker-default-behavior";
    private const string ShowOffsetSelectionAnchorId = "date-offset-range-picker-show-offset-selection";
    private const string OffsetDefinitionsAnchorId = "date-offset-range-picker-offset-definitions";
    private const string UseWithBindingAnchorId = "date-offset-range-picker-use-with-binding";
    private const string ReadonlyModeAnchorId = "date-offset-range-picker-readonly-mode";
    private const string DataValidationAnchorId = "date-offset-range-picker-data-validation";
    private const string StyleClassesAnchorId = "date-offset-range-picker-style-classes";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateOffsetRangePicker,
        Description = LanguageManager.Instance.Page_Description_DateOffsetRangePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DatePickers), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateOffsetRangePicker)],
        Tags = ["DateOffsetRangePicker", "Date", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOffsetRangePickerDemo/DateOffsetRangePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOffsetRangePickerDemo/DateOffsetRangePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTimeOffset? StartDate { get; set; }
    [ObservableProperty] public partial DateTimeOffset? EndDate { get; set; }
    
    public ValidatedDateTimeOffsetRange ValidatedRange { get; } = new();
    public DemoSectionViewModel DefaultBehaviorSection { get; }
    public DemoSectionViewModel ShowOffsetSelectionSection { get; }
    public DemoSectionViewModel OffsetDefinitionsSection { get; }
    public DemoSectionViewModel UseWithBindingSection { get; }
    public DemoSectionViewModel ReadonlyModeSection { get; }
    public DemoSectionViewModel DataValidationSection { get; }
    public DemoSectionViewModel StyleClassesSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new() { Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Default_Behavior_Header, AnchorId = DefaultBehaviorAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Show_Offset_Selection_Header, AnchorId = ShowOffsetSelectionAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Offset_Definitions_Header, AnchorId = OffsetDefinitionsAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Use_With_Binding_Header, AnchorId = UseWithBindingAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Readonly_Mode_Header, AnchorId = ReadonlyModeAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Data_Validation_Header, AnchorId = DataValidationAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Style_Classes_Header, AnchorId = StyleClassesAnchorId }
    ];

    public DateOffsetRangePickerDemoViewModel()
    {
        StartDate = DateTimeOffset.Now;
        EndDate = DateTimeOffset.Now.AddDays(7);

        DefaultBehaviorSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Default_Behavior_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Default_Behavior_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DefaultBehaviorAnchorId
        };
        DefaultBehaviorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateOffsetRangePicker
                              Width="400"
                              DisplayFormat="yyyy-MM-dd"
                              ShowOffsetSelection="True" />
                          """
        });

        ShowOffsetSelectionSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Show_Offset_Selection_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Show_Offset_Selection_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ShowOffsetSelectionAnchorId
        };
        ShowOffsetSelectionSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBlock Text="ShowOffsetSelection=False" />
                          <u:DateOffsetRangePicker
                              Width="400"
                              DisplayFormat="yyyy-MM-dd"
                              ShowOffsetSelection="False"
                              SelectedStartDate="2025-06-26+08:00"
                              SelectedEndDate="2025-06-30+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          <TextBlock Text="ShowOffsetSelection=True" />
                          <u:DateOffsetRangePicker
                              Width="400"
                              DisplayFormat="yyyy-MM-dd"
                              ShowOffsetSelection="True"
                              SelectedStartDate="2025-06-26+08:00"
                              SelectedEndDate="2025-06-30+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          """
        });

        OffsetDefinitionsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Offset_Definitions_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Offset_Definitions_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = OffsetDefinitionsAnchorId
        };
        OffsetDefinitionsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateOffsetRangePicker
                              Width="400"
                              DisplayFormat="yyyy-MM-dd"
                              ShowOffsetSelection="True"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />

                          <u:DateOffsetRangePicker
                              Width="400"
                              DisplayFormat="yyyy-MM-dd"
                              ShowOffsetSelection="True">
                              <u:DateOffsetRangePicker.OffsetDefinitions>
                                  <u:OffsetDefinitions>
                                      <u:OffsetDefinition Offset="UTC" />
                                      <u:OffsetDefinition Offset="Local" />
                                      <u:OffsetDefinition DisplayName="Beijing (CST)" Offset="+08:00" />
                                      <u:OffsetDefinition DisplayName="New York (EST)" Offset="-05:00" />
                                  </u:OffsetDefinitions>
                              </u:DateOffsetRangePicker.OffsetDefinitions>
                          </u:DateOffsetRangePicker>
                          """
        });

        UseWithBindingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Use_With_Binding_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Use_With_Binding_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = UseWithBindingAnchorId
        };
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBlock Text="{Binding StartDate, StringFormat='Start: {0:yyyy-MM-dd zzz}'}" />
                          <TextBlock Text="{Binding EndDate, StringFormat='End: {0:yyyy-MM-dd zzz}'}" />
                          <u:DateOffsetRangePicker
                              Width="400"
                              DisplayFormat="yyyy-MM-dd"
                              SelectedStartDate="{Binding StartDate, Mode=TwoWay}"
                              SelectedEndDate="{Binding EndDate, Mode=TwoWay}"
                              ShowOffsetSelection="True"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          """
        });
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial DateTimeOffset? StartDate { get; set; }
                          [ObservableProperty] public partial DateTimeOffset? EndDate { get; set; }

                          public DateOffsetRangePickerDemoViewModel()
                          {
                              StartDate = DateTimeOffset.Now;
                              EndDate = DateTimeOffset.Now.AddDays(7);
                          }
                          """
        });

        ReadonlyModeSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Readonly_Mode_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Readonly_Mode_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = ReadonlyModeAnchorId
        };
        ReadonlyModeSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateOffsetRangePicker
                              Width="400"
                              DisplayFormat="yyyy-MM-dd"
                              IsReadOnly="True"
                              SelectedStartDate="2025-06-26+08:00"
                              SelectedEndDate="2025-06-30+08:00"
                              ShowOffsetSelection="True"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          """
        });

        DataValidationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Data_Validation_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Data_Validation_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = DataValidationAnchorId
        };
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateOffsetRangePicker
                              Width="400"
                              Classes="ClearButton"
                              DisplayFormat="yyyy-MM-dd"
                              ShowOffsetSelection="True"
                              SelectedStartDate="{Binding ValidatedRange.Start, Mode=TwoWay}"
                              SelectedEndDate="{Binding ValidatedRange.End, Mode=TwoWay}" />
                          """
        });
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public partial class ValidatedDateTimeOffsetRange : ObservableValidator
                          {
                              [ObservableProperty]
                              [Required(ErrorMessage = "Start date is required")]
                              public partial DateTimeOffset? Start { get; set; }

                              [ObservableProperty]
                              [Required(ErrorMessage = "End date is required")]
                              public partial DateTimeOffset? End { get; set; }
                          }
                          """
        });

        StyleClassesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Style_Classes_Header,
            Descriptions = { LanguageManager.Instance.Page_DateOffsetRangePicker_Section_Style_Classes_Description },
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

                          <u:DateOffsetRangePicker
                              Width="400"
                              DisplayFormat="yyyy-MM-dd"
                              SelectedStartDate="2025-06-26+08:00"
                              SelectedEndDate="2025-06-30+08:00"
                              ShowOffsetSelection="True"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00"
                              u:ClassSelector.Source="{Binding #sizeClassSelector}" />
                          """
        });
    }
}

public partial class ValidatedDateTimeOffsetRange : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Start date is required")]
    public partial DateTimeOffset? Start { get; set; }
    
    [ObservableProperty]
    [Required(ErrorMessage = "End date is required")]
    public partial DateTimeOffset? End { get; set; }

    public ValidatedDateTimeOffsetRange()
    {
        Start = DateTimeOffset.Now;
        End = DateTimeOffset.Now.AddDays(7);
    }
}
