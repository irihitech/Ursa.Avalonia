using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Demo.Models;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Irihi.Dogma.Controls;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.AutoCompleteBoxDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(AutoCompleteBoxDemo))]
public partial class AutoCompleteBoxDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "AutoCompleteBox";
    public const string Menu_Header = "Menu_Header_AutoCompleteBox";
    private const string BasicUsageAnchorId = "auto-complete-box-basic-usage";
    private const string ItemTemplateSupportAnchorId = "auto-complete-box-item-template-support";
    private const string ClearButtonSupportAnchorId = "auto-complete-box-clear-button-support";
    private const string InnerContentSupportAnchorId = "auto-complete-box-inner-content-support";

    public PageMetadataViewModel PageMetadata { get; set; } = new()
    {
        Title = LanguageManager.Instance.Page_Title_AutoCompleteBox,
        Description = LanguageManager.Instance.Page_Description_AutoCompleteBox,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_AutoCompleteBox)],
        Tags = ["AutoCompleteBox", "Input", "Search"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/AutoCompleteBoxDemo/AutoCompleteBoxDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/AutoCompleteBoxDemo/AutoCompleteBoxDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public AutoCompleteBoxDemoViewModel()
    {
        Controls = new ObservableCollection<ControlData>(GetControlData());
        BasicSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_AutoCompleteBox_Section_Basic_Usage_Header,
            Description = LanguageManager.Instance.Page_AutoCompleteBox_Section_Basic_Usage_Description,
            AnchorId = BasicUsageAnchorId
        };
        BasicSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:AutoCompleteBox
                              ItemsSource="{Binding Controls}"
                              PlaceholderText="Please select a Control"
                              SelectedItem="{Binding SelectedControl, Mode=TwoWay}"
                              ValueMemberBinding="{ReflectionBinding MenuHeader}" />
                          """
        });
        BasicSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ObservableCollection<ControlData> Controls { get; set; }
                          
                          [ObservableProperty] 
                          public partial ControlData? SelectedControl { get; set; }

                          """
        });
        ItemTemplateSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_AutoCompleteBox_Section_ItemTemplate_Support_Header,
            Description = LanguageManager.Instance.Page_AutoCompleteBox_Section_ItemTemplate_Support_Description,
            AnchorId = ItemTemplateSupportAnchorId
        };
        ItemTemplateSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:AutoCompleteBox ItemsSource="{Binding Controls}"
                                            PlaceholderText="Choose a control">
                             <u:AutoCompleteBox.ItemTemplate>
                                 <DataTemplate DataType="models:ControlData">
                                     <StackPanel Orientation="Horizontal" Spacing="8">
                                         <TextBlock Text="{Binding MenuHeader}" />
                                         <TextBlock Classes="Secondary"
                                                    Text="{Binding Chinese}" />
                                     </StackPanel>
                                 </DataTemplate>
                             </u:AutoCompleteBox.ItemTemplate>
                          </u:AutoCompleteBox>
                          """
        });
        ClearButtonSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_AutoCompleteBox_Section_ClearButton_Support_Header,
            Description = LanguageManager.Instance.Page_AutoCompleteBox_Section_ClearButton_Support_Description,
            AnchorId = ClearButtonSupportAnchorId
        };
        ClearButtonSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:AutoCompleteBox
                             Classes="ClearButton"
                             ItemsSource="{Binding Controls}"
                             SelectedItem="{Binding ClearButtonSelectedControl, Mode=TwoWay}"
                             Text="{Binding ClearButtonText, Mode=TwoWay}"
                             ValueMemberBinding="{ReflectionBinding MenuHeader}" />
                          """
        });
        InnerContentSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_AutoCompleteBox_Section_InnerContent_Support_Header,
            Description = LanguageManager.Instance.Page_AutoCompleteBox_Section_InnerContent_Support_Description,
            AnchorId = InnerContentSupportAnchorId
        };
        InnerContentSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:AutoCompleteBox
                             InnerLeftContent="https://"
                             InnerRightContent=".com"
                             ItemsSource="{Binding Controls}"
                             ValueMemberBinding="{ReflectionBinding MenuHeader}" />
                          """
        });
    }

    public ObservableCollection<ControlData> Controls { get; set; }
    public DemoSectionViewModel BasicSection { get; }
    public DemoSectionViewModel ItemTemplateSection { get; }
    public DemoSectionViewModel ClearButtonSection { get; }
    public DemoSectionViewModel InnerContentSection { get; }
    
    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; set; } =
    [
        new() { 
            Header = LanguageManager.Instance.Page_AutoCompleteBox_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_AutoCompleteBox_Section_ItemTemplate_Support_Header,
            AnchorId = ItemTemplateSupportAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_AutoCompleteBox_Section_ClearButton_Support_Header,
            AnchorId = ClearButtonSupportAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_AutoCompleteBox_Section_InnerContent_Support_Header,
            AnchorId = InnerContentSupportAnchorId
        },
    ];

    [ObservableProperty] public partial ControlData? SelectedControl { get; set; }
    [ObservableProperty] public partial ControlData? ClearButtonSelectedControl { get; set; }
    [ObservableProperty] public partial string? ClearButtonText { get; set; }

    private static ControlData[] GetControlData()
    {
        return new ControlData[]
        {
            new() { MenuHeader = "Button Group", Chinese = "按钮组" },
            new() { MenuHeader = "Icon Button", Chinese = "图标按钮" },
            new() { MenuHeader = "AutoCompleteBox", Chinese = "自动完成框" },
            new() { MenuHeader = "Class Input", Chinese = "类输入框" },
            new() { MenuHeader = "Enum Selector", Chinese = "枚举选择器" },
            new() { MenuHeader = "Form", Chinese = "表单" },
            new() { MenuHeader = "KeyGestureInput", Chinese = "快捷键输入" },
            new() { MenuHeader = "IPv4Box", Chinese = "IPv4输入框" },
            new() { MenuHeader = "MultiComboBox", Chinese = "多选组合框" },
            new() { MenuHeader = "Multi AutoCompleteBox", Chinese = "多项自动完成框" },
            new() { MenuHeader = "Numeric UpDown", Chinese = "数字上下调节" },
            new() { MenuHeader = "NumPad", Chinese = "数字键盘" },
            new() { MenuHeader = "PathPicker", Chinese = "路径选择器" },
            new() { MenuHeader = "PinCode", Chinese = "密码输入" },
            new() { MenuHeader = "RangeSlider", Chinese = "范围滑块" },
            new() { MenuHeader = "Rating", Chinese = "评分" },
            new() { MenuHeader = "Selection List", Chinese = "选择列表" },
            new() { MenuHeader = "TagInput", Chinese = "标签输入" },
            new() { MenuHeader = "Theme Toggler", Chinese = "主题切换" },
            new() { MenuHeader = "TreeComboBox", Chinese = "树形组合框" },
            new() { MenuHeader = "Dialog", Chinese = "对话框" },
            new() { MenuHeader = "Drawer", Chinese = "抽屉" },
            new() { MenuHeader = "Loading", Chinese = "加载" },
            new() { MenuHeader = "Message Box", Chinese = "消息框" },
            new() { MenuHeader = "Notification", Chinese = "通知" },
            new() { MenuHeader = "PopConfirm", Chinese = "气泡确认" },
            new() { MenuHeader = "Toast", Chinese = "吐司" },
            new() { MenuHeader = "Skeleton", Chinese = "骨架屏" },
            new() { MenuHeader = "Date Picker", Chinese = "日期选择器" },
            new() { MenuHeader = "Date Range Picker", Chinese = "日期范围选择器" },
            new() { MenuHeader = "Date Time Picker", Chinese = "日期时间选择器" },
            new() { MenuHeader = "Time Box", Chinese = "时间输入框" },
            new() { MenuHeader = "Time Picker", Chinese = "时间选择器" },
            new() { MenuHeader = "Time Range Picker", Chinese = "时间范围选择器" },
            new() { MenuHeader = "Clock", Chinese = "时钟" },
            new() { MenuHeader = "Anchor", Chinese = "锚点" },
            new() { MenuHeader = "Breadcrumb", Chinese = "面包屑" },
            new() { MenuHeader = "Nav Menu", Chinese = "导航菜单" },
            new() { MenuHeader = "Pagination", Chinese = "分页" },
            new() { MenuHeader = "ToolBar", Chinese = "工具栏" },
            new() { MenuHeader = "AspectRatioLayout", Chinese = "宽高比布局" },
            new() { MenuHeader = "Avatar", Chinese = "头像" },
            new() { MenuHeader = "Badge", Chinese = "徽章" },
            new() { MenuHeader = "Banner", Chinese = "横幅" },
            new() { MenuHeader = "Disable Container", Chinese = "禁用容器" },
            new() { MenuHeader = "Divider", Chinese = "分割线" },
            new() { MenuHeader = "DualBadge", Chinese = "双徽章" },
            new() { MenuHeader = "ImageViewer", Chinese = "图片查看器" },
            new() { MenuHeader = "ElasticWrapPanel", Chinese = "弹性换行面板" },
            new() { MenuHeader = "Marquee", Chinese = "跑马灯" },
            new() { MenuHeader = "Number Displayer", Chinese = "数字显示器" },
            new() { MenuHeader = "Scroll To", Chinese = "滚动到按钮" },
            new() { MenuHeader = "Timeline", Chinese = "时间轴" },
            new() { MenuHeader = "TwoTonePathIcon", Chinese = "双色路径图标" }
        };
    }
}
