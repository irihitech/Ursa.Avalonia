using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Irihi.Lingua;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.SettingsCardDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(SettingsCardDemo))]
public partial class SettingsCardDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "SettingsCard";
    public const string Menu_Header = "Menu_Header_SettingsCard";
    private const string BasicUsageAnchorId = "settings-card-basic-usage";
    private const string ExpandableAnchorId = "settings-card-expandable";
    private const string NavigableAnchorId = "settings-card-navigable";
    private const string ItemsSourceAnchorId = "settings-card-items-source";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_SettingsCard,
        Description = LanguageManager.Instance.Page_Description_SettingsCard,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_SettingsCard)],
        Tags = ["SettingsCard", "SettingsCardGroup", "Settings", "Card"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/SettingsCardDemo/SettingsCardDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/SettingsCardDemo/SettingsCardDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public SettingsCardDemoViewModel()
    {
        BasicSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_SettingsCard_Section_Basic_Usage_Header,
            Descriptions = { LanguageManager.Instance.Page_SettingsCard_Section_Basic_Usage_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = BasicUsageAnchorId
        };
        BasicSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:SettingsCardGroup Header="General" Description="Basic rows in one card">
                              <u:SettingsCard Icon="{DynamicResource SemiIconBell}"
                                              Content="Notifications"
                                              Description="Show a badge on the app icon">
                                  <u:SettingsCard.RightContent>
                                      <ToggleSwitch Theme="{DynamicResource SimpleToggleSwitch}"
                                                    IsChecked="{Binding NotificationsEnabled}" />
                                  </u:SettingsCard.RightContent>
                              </u:SettingsCard>
                              <u:SettingsCard Content="Language" Icon="{DynamicResource SemiIconGlobe}"
                                              Note="zh-CN" />
                              <u:SettingsCard Content="Theme" Description="Follow the system"
                                              Icon="{DynamicResource SemiIconDesktop}">
                                  <u:SettingsCard.Trailing>
                                      <u:Badge Classes="Light" Header="Beta" />
                                  </u:SettingsCard.Trailing>
                              </u:SettingsCard>
                          </u:SettingsCardGroup>
                          """
        });

        ExpandableSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_SettingsCard_Section_Expandable_Header,
            Descriptions = { LanguageManager.Instance.Page_SettingsCard_Section_Expandable_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ExpandableAnchorId
        };
        ExpandableSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:SettingsCardGroup>
                              <u:SettingsCard Content="Fuzzy sounds"
                                              Description="Click the row to expand inline details"
                                              IsExpanded="{Binding DetailExpanded}">
                                  <u:SettingsCard.Detail>
                                      <StackPanel>
                                          <u:SettingsCard>
                                              <RadioButton GroupName="fuzzy"
                                                           Content="Flat vs. retroflex"
                                                           IsChecked="{Binding FuzzyEnabled}" />
                                          </u:SettingsCard>
                                          <u:SettingsCard>
                                              <RadioButton GroupName="fuzzy" Content="Nasal finals" />
                                          </u:SettingsCard>
                                      </StackPanel>
                                  </u:SettingsCard.Detail>
                              </u:SettingsCard>
                          </u:SettingsCardGroup>
                          """
        });

        NavigableSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_SettingsCard_Section_Navigable_Header,
            Descriptions = { LanguageManager.Instance.Page_SettingsCard_Section_Navigable_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = NavigableAnchorId
        };
        NavigableSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:SettingsCardGroup>
                              <u:SettingsCard Content="About"
                                              Icon="{DynamicResource SemiIconSetting}"
                                              Command="{Binding NavigateCommand}"
                                              CommandParameter="About" />
                              <u:SettingsCard Content="Advanced settings"
                                              Description="Navigable rows get hover feedback and focus"
                                              Icon="{DynamicResource SemiIconLock}"
                                              Command="{Binding NavigateCommand}"
                                              CommandParameter="Advanced"
                                              ShowChevron="True" />
                          </u:SettingsCardGroup>
                          """
        });

        ItemsSourceSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_SettingsCard_Section_ItemsSource_Header,
            Descriptions = { LanguageManager.Instance.Page_SettingsCard_Section_ItemsSource_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ItemsSourceAnchorId
        };
        ItemsSourceSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:SettingsCardGroup Header="Quick options"
                                               ItemsSource="{Binding QuickOptions}" />

                          <u:SettingsCardGroup Header="Profiles"
                                               ItemsSource="{Binding Profiles}">
                              <u:SettingsCardGroup.ItemTemplate>
                                  <DataTemplate>
                                      <u:SettingsCard Content="{Binding Name}"
                                                      Note="{Binding Status}"
                                                      IsExpanded="{Binding IsExpanded}">
                                          <u:SettingsCard.Detail>
                                              <StackPanel>
                                                  <u:SettingsCard Content="Sync across devices">
                                                      <u:SettingsCard.RightContent>
                                                          <CheckBox IsChecked="{Binding SyncEnabled}" />
                                                      </u:SettingsCard.RightContent>
                                                  </u:SettingsCard>
                                                  <u:SettingsCard Content="Send usage statistics" />
                                              </StackPanel>
                                          </u:SettingsCard.Detail>
                                      </u:SettingsCard>
                                  </DataTemplate>
                              </u:SettingsCardGroup.ItemTemplate>
                          </u:SettingsCardGroup>
                          """
        });
        ItemsSourceSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ObservableCollection<string> QuickOptions { get; } =
                              ["Simplified", "Traditional", "Pinyin"];

                          public ObservableCollection<ProfileViewModel> Profiles { get; } =
                          [
                              new("Rime defaults", "Active"),
                              new("MingLan", "Idle"),
                          ];

                          public partial class ProfileViewModel : ObservableObject
                          {
                              public ProfileViewModel(string name, string status)
                              {
                                  Name = name;
                                  Status = status;
                              }

                              public string Name { get; }
                              public string Status { get; }
                              [ObservableProperty] public partial bool IsExpanded { get; set; }
                              [ObservableProperty] public partial bool SyncEnabled { get; set; } = true;
                          }
                          """
        });
    }

    [ObservableProperty] public partial bool NotificationsEnabled { get; set; } = true;

    [ObservableProperty] public partial bool FuzzyEnabled { get; set; } = true;

    [ObservableProperty] public partial bool DetailExpanded { get; set; }

    [ObservableProperty] public partial string? Status { get; set; }

    public ObservableCollection<string> QuickOptions { get; } = ["Simplified", "Traditional", "Pinyin"];

    public ObservableCollection<ProfileViewModel> Profiles { get; } =
    [
        new("Rime defaults", "Active"),
        new("MingLan", "Idle"),
    ];

    public DemoSectionViewModel BasicSection { get; }
    public DemoSectionViewModel ExpandableSection { get; }
    public DemoSectionViewModel NavigableSection { get; }
    public DemoSectionViewModel ItemsSourceSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_SettingsCard_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_SettingsCard_Section_Expandable_Header,
            AnchorId = ExpandableAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_SettingsCard_Section_Navigable_Header,
            AnchorId = NavigableAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_SettingsCard_Section_ItemsSource_Header,
            AnchorId = ItemsSourceAnchorId
        }
    ];

    [RelayCommand]
    private void Navigate(string target)
    {
        Status = $"Navigate to: {target}";
    }
}

public partial class ProfileViewModel : ObservableObject
{
    public ProfileViewModel(string name, string status)
    {
        Name = name;
        Status = status;
    }

    public string Name { get; }
    public string Status { get; }

    [ObservableProperty] public partial bool IsExpanded { get; set; }

    [ObservableProperty] public partial bool SyncEnabled { get; set; } = true;
}
