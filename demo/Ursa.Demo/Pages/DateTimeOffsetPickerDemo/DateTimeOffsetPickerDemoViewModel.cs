using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.DateTimeOffsetPickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DateAndTimePage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DateTimeOffsetPickerDemo))]
public partial class DateTimeOffsetPickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "DateTimeOffsetPicker";
    public const string Menu_Header = "Menu_Header_DateTimeOffsetPicker";
    private const string DefaultBehaviorAnchorId = "date-time-offset-picker-default-behavior";
    private const string ShowOffsetSelectionAnchorId = "date-time-offset-picker-show-offset-selection";
    private const string OffsetDefinitionsAnchorId = "date-time-offset-picker-offset-definitions";
    private const string NeedConfirmationAnchorId = "date-time-offset-picker-need-confirmation";
    private const string UseWithBindingAnchorId = "date-time-offset-picker-use-with-binding";
    private const string ReadonlyModeAnchorId = "date-time-offset-picker-readonly-mode";
    private const string DataValidationAnchorId = "date-time-offset-picker-data-validation";
    private const string StyleClassesAnchorId = "date-time-offset-picker-style-classes";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateTimeOffsetPicker,
        Description = LanguageManager.Instance.Page_Description_DateTimeOffsetPicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateTimeOffsetPicker)],
        Tags = ["DateTimeOffsetPicker", "DateTime", "Offset"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateTimeOffsetPickerDemo/DateTimeOffsetPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateTimeOffsetPickerDemo/DateTimeOffsetPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTimeOffset? SelectedDateTime { get; set; }

    [ObservableProperty]
    [Required(ErrorMessage = "Please select a date and time")]
    public partial DateTimeOffset? ValidatedDateTime { get; set; }

    public DemoSectionViewModel DefaultBehaviorSection { get; }
    public DemoSectionViewModel ShowOffsetSelectionSection { get; }
    public DemoSectionViewModel OffsetDefinitionsSection { get; }
    public DemoSectionViewModel NeedConfirmationSection { get; }
    public DemoSectionViewModel UseWithBindingSection { get; }
    public DemoSectionViewModel ReadonlyModeSection { get; }
    public DemoSectionViewModel DataValidationSection { get; }
    public DemoSectionViewModel StyleClassesSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new() { Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Default_Behavior_Header, AnchorId = DefaultBehaviorAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Show_Offset_Selection_Header, AnchorId = ShowOffsetSelectionAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Offset_Definitions_Header, AnchorId = OffsetDefinitionsAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Need_Confirmation_Header, AnchorId = NeedConfirmationAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Use_With_Binding_Header, AnchorId = UseWithBindingAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Readonly_Mode_Header, AnchorId = ReadonlyModeAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Data_Validation_Header, AnchorId = DataValidationAnchorId },
        new() { Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Style_Classes_Header, AnchorId = StyleClassesAnchorId }
    ];

    public DateTimeOffsetPickerDemoViewModel()
    {
        SelectedDateTime = DateTimeOffset.Now;
        ValidatedDateTime = DateTimeOffset.Now;

        DefaultBehaviorSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Default_Behavior_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Default_Behavior_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DefaultBehaviorAnchorId
        };
        DefaultBehaviorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateTimeOffsetPicker ShowOffsetSelection="True" />
                          """
        });

        ShowOffsetSelectionSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Show_Offset_Selection_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Show_Offset_Selection_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ShowOffsetSelectionAnchorId
        };
        ShowOffsetSelectionSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBlock Text="ShowOffsetSelection=False" />
                          <u:DateTimeOffsetPicker
                              Width="360"
                              ShowOffsetSelection="False"
                              SelectedDate="2025-06-26T10:30:00+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          <TextBlock Text="ShowOffsetSelection=True" />
                          <u:DateTimeOffsetPicker
                              Width="360"
                              ShowOffsetSelection="True"
                              SelectedDate="2025-06-26T10:30:00+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          """
        });

        OffsetDefinitionsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Offset_Definitions_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Offset_Definitions_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = OffsetDefinitionsAnchorId
        };
        OffsetDefinitionsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateTimeOffsetPicker
                              Width="360"
                              ShowOffsetSelection="True"
                              SelectedDate="2025-06-26T10:30:00+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />

                          <u:DateTimeOffsetPicker
                              Width="360"
                              ShowOffsetSelection="True"
                              SelectedDate="2025-06-26T10:30:00+08:00">
                              <u:DateTimeOffsetPicker.OffsetDefinitions>
                                  <u:OffsetDefinitions>
                                      <u:OffsetDefinition Offset="UTC" />
                                      <u:OffsetDefinition Offset="Local" />
                                      <u:OffsetDefinition DisplayName="Beijing (CST)" Offset="+08:00" />
                                      <u:OffsetDefinition DisplayName="New York (EST)" Offset="-05:00" />
                                  </u:OffsetDefinitions>
                              </u:DateTimeOffsetPicker.OffsetDefinitions>
                          </u:DateTimeOffsetPicker>
                          """
        });

        NeedConfirmationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Need_Confirmation_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Need_Confirmation_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = NeedConfirmationAnchorId
        };
        NeedConfirmationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBlock Text="NeedConfirmation=False" />
                          <u:DateTimeOffsetPicker
                              Width="360"
                              ShowOffsetSelection="True"
                              SelectedDate="2025-06-26T10:30:00+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          <TextBlock Text="NeedConfirmation=True" />
                          <u:DateTimeOffsetPicker
                              Width="360"
                              NeedConfirmation="True"
                              ShowOffsetSelection="True"
                              SelectedDate="2025-06-26T10:30:00+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          """
        });

        UseWithBindingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Use_With_Binding_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Use_With_Binding_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = UseWithBindingAnchorId
        };
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBlock Text="{Binding SelectedDateTime, StringFormat='Selected: {0:yyyy-MM-dd HH:mm:ss zzz}'}" />
                          <u:DateTimeOffsetPicker
                              Width="360"
                              ShowOffsetSelection="True"
                              SelectedDate="{Binding SelectedDateTime, Mode=TwoWay}"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          """
        });
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial DateTimeOffset? SelectedDateTime { get; set; }

                          public DateTimeOffsetPickerDemoViewModel()
                          {
                              SelectedDateTime = DateTimeOffset.Now;
                          }
                          """
        });

        ReadonlyModeSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Readonly_Mode_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Readonly_Mode_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = ReadonlyModeAnchorId
        };
        ReadonlyModeSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateTimeOffsetPicker
                              Width="360"
                              IsReadOnly="True"
                              ShowOffsetSelection="True"
                              SelectedDate="2025-06-26T10:30:00+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00" />
                          """
        });

        DataValidationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Data_Validation_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Data_Validation_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = DataValidationAnchorId
        };
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:DateTimeOffsetPicker
                              Width="360"
                              Classes="ClearButton"
                              ShowOffsetSelection="True"
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
                          public partial DateTimeOffset? ValidatedDateTime { get; set; }
                          """
        });

        StyleClassesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Style_Classes_Header,
            Descriptions = { LanguageManager.Instance.Page_DateTimeOffsetPicker_Section_Style_Classes_Description },
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

                          <u:DateTimeOffsetPicker
                              Width="360"
                              ShowOffsetSelection="True"
                              SelectedDate="2025-06-26T10:30:00+08:00"
                              OffsetDefinitions="Utc, Local, +8:00, -5:00"
                              u:ClassSelector.Source="{Binding #sizeClassSelector}" />
                          """
        });
    }
}
