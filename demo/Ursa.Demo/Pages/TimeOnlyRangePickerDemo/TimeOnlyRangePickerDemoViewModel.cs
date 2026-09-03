using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.TimeOnlyRangePickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = TimePickersPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(TimeOnlyRangePickerDemo))]
public partial class TimeOnlyRangePickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "TimeOnlyRangePicker";
    public const string Menu_Header = "Menu_Header_TimeOnlyRangePicker";
    private const string DefaultBehaviorAnchorId = "time-only-range-picker-default-behavior";
    private const string DisplayFormatAnchorId = "time-only-range-picker-display-format";
    private const string PanelFormatAnchorId = "time-only-range-picker-panel-format";
    private const string NeedConfirmationAnchorId = "time-only-range-picker-need-confirmation";
    private const string UseWithBindingAnchorId = "time-only-range-picker-use-with-binding";
    private const string ReadonlyModeAnchorId = "time-only-range-picker-readonly-mode";
    private const string DataValidationAnchorId = "time-only-range-picker-data-validation";
    private const string StyleClassesAnchorId = "time-only-range-picker-style-classes";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TimeOnlyRangePicker,
        Description = LanguageManager.Instance.Page_Description_TimeOnlyRangePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_TimePickers), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TimeOnlyRangePicker)],
        Tags = ["TimeOnlyRangePicker", "Time", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimeOnlyRangePickerDemo/TimeOnlyRangePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimeOnlyRangePickerDemo/TimeOnlyRangePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial TimeOnly? StartTime { get; set; }
    [ObservableProperty] public partial TimeOnly? EndTime { get; set; }
    
    public ValidatedTimeOnlyRange ValidatedRange { get; } = new();

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
        new() { Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Default_Behavior_Header, AnchorId = DefaultBehaviorAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Display_Format_Header, AnchorId = DisplayFormatAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Panel_Format_Header, AnchorId = PanelFormatAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Need_Confirmation_Header, AnchorId = NeedConfirmationAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Use_With_Binding_Header, AnchorId = UseWithBindingAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Readonly_Mode_Header, AnchorId = ReadonlyModeAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Data_Validation_Header, AnchorId = DataValidationAnchorId },
        new() { Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Style_Classes_Header, AnchorId = StyleClassesAnchorId }
    ];

    public TimeOnlyRangePickerDemoViewModel()
    {
        StartTime = new TimeOnly(8, 21, 0);
        EndTime = new TimeOnly(18, 22, 0);

        DefaultBehaviorSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Default_Behavior_Header,
            Descriptions = { LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Default_Behavior_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DefaultBehaviorAnchorId
        };
        DefaultBehaviorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TimeOnlyRangePicker />
                          """
        });

        DisplayFormatSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Display_Format_Header,
            Descriptions = { LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Display_Format_Description },
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
                                  Text="HH:mm:ss" />
                              <u:TimeOnlyRangePicker
                                  Width="300"
                                  DisplayFormat="{Binding #displayFormatBox.Text}"
                                  SelectedStartTime="10:30:00"
                                  SelectedEndTime="18:30:00" />
                          </DockPanel>
                          """
        });

        PanelFormatSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Panel_Format_Header,
            Descriptions = { LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Panel_Format_Description },
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
                              <u:TimeOnlyRangePicker
                                  Width="300"
                                  DisplayFormat="HH:mm:ss"
                                  PanelFormat="{Binding #panelFormatBox.Text}"
                                  SelectedStartTime="10:30:00"
                                  SelectedEndTime="18:30:00" />
                          </DockPanel>
                          """
        });

        NeedConfirmationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Need_Confirmation_Header,
            Descriptions = { LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Need_Confirmation_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = NeedConfirmationAnchorId
        };
        NeedConfirmationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <ToggleSwitch Name="needConfirmToggle" Content="Need Confirmation" />
                          <TextBlock Text="{Binding #confirmPicker.SelectedStartTime, StringFormat='Start: {0}'}" />
                          <TextBlock Text="{Binding #confirmPicker.SelectedEndTime, StringFormat='End: {0}'}" />
                          <u:TimeOnlyRangePicker
                              Name="confirmPicker"
                              Width="300"
                              DisplayFormat="HH:mm:ss"
                              NeedConfirmation="{Binding #needConfirmToggle.IsChecked}"
                              SelectedStartTime="10:30:00"
                              SelectedEndTime="18:30:00" />
                          """
        });

        UseWithBindingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Use_With_Binding_Header,
            Descriptions = { LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Use_With_Binding_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = UseWithBindingAnchorId
        };
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <TextBlock Text="{Binding StartTime, StringFormat='Start: {0}'}" />
                          <TextBlock Text="{Binding EndTime, StringFormat='End: {0}'}" />
                          <u:TimeOnlyRangePicker
                              Width="300"
                              DisplayFormat="HH:mm:ss"
                              SelectedStartTime="{Binding StartTime, Mode=TwoWay}"
                              SelectedEndTime="{Binding EndTime, Mode=TwoWay}" />
                          """
        });
        UseWithBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial TimeOnly? StartTime { get; set; }
                          [ObservableProperty] public partial TimeOnly? EndTime { get; set; }

                          public TimeOnlyRangePickerDemoViewModel()
                          {
                              StartTime = new TimeOnly(8, 21, 0);
                              EndTime = new TimeOnly(18, 22, 0);
                          }
                          """
        });

        ReadonlyModeSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Readonly_Mode_Header,
            Descriptions = { LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Readonly_Mode_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = ReadonlyModeAnchorId
        };
        ReadonlyModeSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TimeOnlyRangePicker
                              Width="300"
                              DisplayFormat="HH:mm:ss"
                              IsReadOnly="True"
                              SelectedStartTime="10:30:00"
                              SelectedEndTime="18:30:00" />
                          """
        });

        DataValidationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Data_Validation_Header,
            Descriptions = { LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Data_Validation_Description },
            SectionTag = DemoSectionTag.Others,
            AnchorId = DataValidationAnchorId
        };
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TimeOnlyRangePicker
                              Width="300"
                              Classes="ClearButton"
                              DisplayFormat="HH:mm:ss"
                              SelectedStartTime="{Binding ValidatedRange.Start, Mode=TwoWay}"
                              SelectedEndTime="{Binding ValidatedRange.End, Mode=TwoWay}" />
                          """
        });
        DataValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public partial class ValidatedTimeOnlyRange : ObservableValidator
                          {
                              [ObservableProperty]
                              [Required(ErrorMessage = "Start time is required")]
                              public partial TimeOnly? Start { get; set; }

                              [ObservableProperty]
                              [Required(ErrorMessage = "End time is required")]
                              public partial TimeOnly? End { get; set; }
                          }
                          """
        });

        StyleClassesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Style_Classes_Header,
            Descriptions = { LanguageManager.Instance.Page_TimeOnlyRangePicker_Section_Style_Classes_Description },
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

                          <u:TimeOnlyRangePicker
                              Width="300"
                              DisplayFormat="HH:mm:ss"
                              SelectedStartTime="10:30:00"
                              SelectedEndTime="18:30:00"
                              u:ClassSelector.Source="{Binding #sizeClassSelector}" />
                          """
        });
    }
}

public partial class ValidatedTimeOnlyRange : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Start time is required")]
    public partial TimeOnly? Start { get; set; }
    
    [ObservableProperty]
    [Required(ErrorMessage = "End time is required")]
    public partial TimeOnly? End { get; set; }

    public ValidatedTimeOnlyRange()
    {
        Start = new TimeOnly(8, 21, 0);
        End = new TimeOnly(18, 22, 0);
    }
}
