using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Dogma.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.FormDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(FormDemo))]
public partial class FormDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "Form";
    public const string Menu_Header = "Menu_Header_Form";
    private const string BasicUsageAnchorId = "form-basic-usage";
    private const string MvvmValidationAnchorId = "form-mvvm-validation";
    private const string FormGroupAnchorId = "form-group";
    private const string LabelPositionAnchorId = "form-label-position";
    private const string LabelWidthAnchorId = "form-label-width";
    private const string LabelAlignmentAnchorId = "form-label-alignment";
    private const string FullFormItemAnchorId = "form-full-form-item";
    private const string DynamicFormItemsAnchorId = "form-dynamic-form-items";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Form,
        Description = LanguageManager.Instance.Page_Description_Form,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Form)],
        Tags = ["Form", "Layout", "Label"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/FormDemo/FormDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/FormDemo/FormDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DataModel Model { get; set; }
    [ObservableProperty] public partial bool ShowAdvancedField { get; set; }
    public DemoSectionViewModel BasicUsageSection { get; }
    public DemoSectionViewModel MvvmValidationSection { get; }
    public DemoSectionViewModel FormGroupSection { get; }
    public DemoSectionViewModel LabelPositionSection { get; }
    public DemoSectionViewModel LabelWidthSection { get; }
    public DemoSectionViewModel LabelAlignmentSection { get; }
    public DemoSectionViewModel FullFormItemSection { get; }
    public DemoSectionViewModel DynamicFormItemsSection { get; }
    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_Form_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Form_Section_Mvvm_Validation_Header,
            AnchorId = MvvmValidationAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Form_Section_Form_Group_Header,
            AnchorId = FormGroupAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Form_Section_Label_Position_Header,
            AnchorId = LabelPositionAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Form_Section_Label_Width_Header,
            AnchorId = LabelWidthAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Form_Section_Label_Alignment_Header,
            AnchorId = LabelAlignmentAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Form_Section_Full_Form_Item_Header,
            AnchorId = FullFormItemAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Form_Section_Dynamic_Form_Items_Header,
            AnchorId = DynamicFormItemsAnchorId
        }
    ];

    public FormDemoViewModel()
    {
        BasicUsageSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Form_Section_Basic_Usage_Header,
            Descriptions = { LanguageManager.Instance.Page_Form_Section_Basic_Usage_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = BasicUsageAnchorId
        };
        BasicUsageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Form LabelPosition="Left" LabelWidth="*">
                              <TextBox Width="300" u:FormItem.Label="Name" u:FormItem.IsRequired="True" />
                              <TextBox Width="300" u:FormItem.Label="Email" />
                              <TextBox Width="300" u:FormItem.Label="Message" Classes="TextArea" />
                          </u:Form>
                          """
        });
        
        MvvmValidationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Form_Section_Mvvm_Validation_Header,
            Descriptions = { LanguageManager.Instance.Page_Form_Section_Mvvm_Validation_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = MvvmValidationAnchorId
        };
        MvvmValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Form LabelPosition="Left" LabelWidth="*">
                              <TextBox
                                  Width="300"
                                  Text="{Binding Model.Name, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                                  u:FormItem.Label="Name (MinLength 10)" />
                              <TextBox
                                  Width="300"
                                  Text="{Binding Model.Email, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                                  u:FormItem.Label="Email (EmailAddress)" />
                              <u:NumericDoubleUpDown
                                  Width="300"
                                  Value="{Binding Model.Number, Mode=TwoWay}"
                                  u:FormItem.Label="Score (Range 0-10)" />
                          </u:Form>
                          """
        });
        MvvmValidationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial DataModel Model { get; set; } = new();

                          public class DataModel : ObservableValidator
                          {
                              [MinLength(10)]
                              public string Name
                              {
                                  get => _name;
                                  set => SetProperty(ref _name, value, true);
                              }

                              [Range(0.0, 10.0)]
                              public double Number
                              {
                                  get => _number;
                                  set => SetProperty(ref _number, value, true);
                              }

                              [EmailAddress]
                              public string Email
                              {
                                  get => _email;
                                  set => SetProperty(ref _email, value, true);
                              }
                          }
                          """
        });

        FormGroupSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Form_Section_Form_Group_Header,
            Descriptions = { LanguageManager.Instance.Page_Form_Section_Form_Group_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = FormGroupAnchorId
        };
        FormGroupSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Form LabelPosition="Left" LabelWidth="*">
                              <u:FormGroup Header="Basic Information">
                                  <TextBox Width="300" u:FormItem.Label="Name" />
                                  <TextBox Width="300" u:FormItem.Label="Email" />
                              </u:FormGroup>
                              <u:FormGroup Header="Education Information">
                                  <TextBox Width="300" u:FormItem.Label="Collage" />
                                  <u:FormItem Label="Study Time">
                                      <u:DateRangePicker Width="300" />
                                  </u:FormItem>
                              </u:FormGroup>
                              <Button
                                  Content="Submit"
                                  HorizontalAlignment="Stretch"
                                  Theme="{DynamicResource SolidButton}"
                                  u:FormItem.NoLabel="True" />
                          </u:Form>
                          """
        });

        LabelPositionSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Form_Section_Label_Position_Header,
            Descriptions = { LanguageManager.Instance.Page_Form_Section_Label_Position_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = LabelPositionAnchorId
        };
        LabelPositionSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Form LabelPosition="Left" LabelWidth="140">
                              <TextBox Width="300" u:FormItem.Label="Name" />
                              <TextBox Width="300" u:FormItem.Label="Email" />
                          </u:Form>
                          <u:Form LabelPosition="Top">
                              <TextBox Width="300" u:FormItem.Label="Name" />
                              <TextBox Width="300" u:FormItem.Label="Email" />
                          </u:Form>
                          """
        });

        LabelWidthSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Form_Section_Label_Width_Header,
            Descriptions = { LanguageManager.Instance.Page_Form_Section_Label_Width_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = LabelWidthAnchorId
        };
        LabelWidthSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Form LabelPosition="Left" LabelWidth="100">
                              <TextBox Width="300" u:FormItem.Label="Short Label" />
                              <TextBox Width="300" u:FormItem.Label="A longer label" />
                          </u:Form>
                          <u:Form LabelPosition="Left" LabelWidth="220">
                              <TextBox Width="300" u:FormItem.Label="Short Label" />
                              <TextBox Width="300" u:FormItem.Label="A longer label" />
                          </u:Form>
                          <u:Form LabelPosition="Left" LabelWidth="*">
                              <TextBox Width="300" u:FormItem.Label="ID" />
                              <TextBox Width="300" u:FormItem.Label="Display Name With More Text" />
                          </u:Form>
                          """
        });

        LabelAlignmentSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Form_Section_Label_Alignment_Header,
            Descriptions = { LanguageManager.Instance.Page_Form_Section_Label_Alignment_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = LabelAlignmentAnchorId
        };
        LabelAlignmentSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Form LabelPosition="Left" LabelWidth="180" LabelAlignment="Left">
                              <TextBox Width="300" u:FormItem.Label="Left" />
                          </u:Form>
                          <u:Form LabelPosition="Left" LabelWidth="180" LabelAlignment="Center">
                              <TextBox Width="300" u:FormItem.Label="Center" />
                          </u:Form>
                          <u:Form LabelPosition="Left" LabelWidth="180" LabelAlignment="Right">
                              <TextBox Width="300" u:FormItem.Label="Right" />
                          </u:Form>
                          """
        });

        FullFormItemSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Form_Section_Full_Form_Item_Header,
            Descriptions = { LanguageManager.Instance.Page_Form_Section_Full_Form_Item_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = FullFormItemAnchorId
        };
        FullFormItemSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <ToggleSwitch
                              Content="Show advanced item"
                              IsChecked="{Binding ShowAdvancedField}" />
                          <u:Form LabelPosition="Left" LabelWidth="*">
                              <TextBox Width="300" u:FormItem.Label="Name" />
                              <u:FormItem IsVisible="{Binding ShowAdvancedField}">
                                  <u:FormItem.Label>
                                      <StackPanel Orientation="Horizontal" Spacing="6">
                                          <TextBlock Text="Advanced Email" />
                                          <TextBlock Text="(custom label)" />
                                      </StackPanel>
                                  </u:FormItem.Label>
                                  <TextBox Width="300" Watermark="name@company.com" />
                              </u:FormItem>
                          </u:Form>
                          """
        });

        FullFormItemSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial bool ShowAdvancedField { get; set; }
                          """
        });
        
        DynamicFormItemsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Form_Section_Dynamic_Form_Items_Header,
            Descriptions = { LanguageManager.Instance.Page_Form_Section_Dynamic_Form_Items_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DynamicFormItemsAnchorId
        };
        DynamicFormItemsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Form ItemsSource="{Binding FormGroups}" HorizontalAlignment="Stretch" LabelPosition="Left" LabelWidth="*">
                              <u:Form.Styles>
                                  <Style Selector="u|FormGroup" x:DataType="vm:IFormGroupViewModel">
                                      <Setter Property="Header" Value="{Binding Title}" />
                                      <Setter Property="ItemsSource" Value="{Binding Items}" />
                                  </Style>
                                  <Style Selector="u|FormItem" x:DataType="vm:IFromItemViewModel">
                                      <Setter Property="Label" Value="{Binding Label}" />
                                  </Style>
                              </u:Form.Styles>
                              <u:Form.ItemTemplate>
                                  <dataTemplates:FormDataTemplateSelector>
                                      <DataTemplate x:Key="{x:Type vm:FormTextViewModel}" DataType="vm:FormTextViewModel">
                                          <TextBox Text="{Binding Value}" />
                                      </DataTemplate>
                                      <DataTemplate x:Key="{x:Type vm:FormAgeViewModel}" DataType="vm:FormAgeViewModel">
                                          <u:NumericUIntUpDown Value="{Binding Age}" />
                                      </DataTemplate>
                                      <DataTemplate x:Key="{x:Type vm:FormDateRangeViewModel}" DataType="vm:FormDateRangeViewModel">
                                          <u:DateRangePicker SelectedStartDate="{Binding Start}" SelectedEndDate="{Binding End}" />
                                      </DataTemplate>
                                  </dataTemplates:FormDataTemplateSelector>
                              </u:Form.ItemTemplate>
                          </u:Form>
                          """
        });
        DynamicFormItemsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ObservableCollection<IFormElement> FormGroups { get; set; } =
                          [
                              new FormGroupViewModel
                              {
                                  Title = "Basic Information",
                                  Items =
                                  [
                                      new FormTextViewModel { Label = "Name" },
                                      new FormAgeViewModel { Label = "Age" },
                                      new FormTextViewModel { Label = "Email" }
                                  ]
                              },
                              new FormGroupViewModel
                              {
                                  Title = "Education Information",
                                  Items =
                                  [
                                      new FormTextViewModel { Label = "College" },
                                      new FormDateRangeViewModel { Label = "Study Time" }
                                  ]
                              },
                              new FormTextViewModel { Label = "Other" }
                          ];
                          """
        });

        Model = new DataModel();
        ShowAdvancedField = true;
        FormGroups = new ObservableCollection<IFormElement>
        {
            new FormGroupViewModel
            {
                Title = "Basic Information",
                Items = new ObservableCollection<IFromItemViewModel>
                {
                    new FormTextViewModel { Label = "Name" },
                    new FormAgeViewModel { Label = "Age" },
                    new FormTextViewModel { Label = "Email" }
                }
            },
            new FormGroupViewModel
            {
                Title = "Education Information",
                Items = new ObservableCollection<IFromItemViewModel>
                {
                    new FormTextViewModel { Label = "College" },
                    new FormDateRangeViewModel { Label = "Study Time" }
                }
            },
            new FormTextViewModel(){ Label = "Other" }
        };
    }

    public ObservableCollection<IFormElement> FormGroups { get; set; }
}

public class DataModel : ObservableValidator
{
    private DateTime _date;

    private string _email = string.Empty;
    private string _name = string.Empty;

    private double _number;

    public DataModel()
    {
        Date = DateTime.Today;
    }

    [MinLength(10)]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value, true);
    }

    [Range(0.0, 10.0)]
    public double Number
    {
        get => _number;
        set => SetProperty(ref _number, value, true);
    }

    [EmailAddress]
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value, true);
    }

    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }
}

public interface IFormElement
{
    
}

public interface IFormGroupViewModel : IFormGroup, IFormElement
{
    public string? Title { get; set; }
    public ObservableCollection<IFromItemViewModel> Items { get; set; }
}

public interface IFromItemViewModel: IFormElement
{
    public string? Label { get; set; }
}

public partial class FormGroupViewModel : ObservableObject, IFormGroupViewModel
{
    [ObservableProperty] public partial string? Title { get; set; }
    public ObservableCollection<IFromItemViewModel> Items { get; set; } = [];
}

public partial class FormTextViewModel : ObservableObject, IFromItemViewModel
{
    [ObservableProperty] public partial string? Label { get; set; }
    [ObservableProperty] public partial string? Value { get; set; }
}

public partial class FormAgeViewModel : ObservableObject, IFromItemViewModel
{
    [ObservableProperty] public partial uint? Age { get; set; }
    [ObservableProperty] public partial string? Label { get; set; }
}

public partial class FormDateRangeViewModel : ObservableObject, IFromItemViewModel
{
    [ObservableProperty] public partial DateTime? End { get; set; }
    [ObservableProperty] public partial string? Label { get; set; }
    [ObservableProperty] public partial DateTime? Start { get; set; }
}