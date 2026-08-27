# AutoCompleteBox demo section pattern reference

This file captures the expected shape for sectioned demo pages in this repository.

## XAML shape

Use this structure in `<ControlName>Demo.axaml`:

```xml
<TabControl>
    <TabItem Header="{Translate Key={x:Static localizations:LanguageManager+Keys.Tab_Header_Guide}}"
             Icon="{DynamicResource SemiIconBook}">
        <controls:AnchorScrollViewer AnchorItems="{Binding AnchorItems}">
            <StackPanel HorizontalAlignment="Stretch" Spacing="20">
                <controls:DemoSectionView SectionContext="{Binding BasicSection}">
                    <!-- interactive demo content -->
                </controls:DemoSectionView>

                <controls:DemoSectionView SectionContext="{Binding AdvancedSection}">
                    <!-- interactive demo content -->
                </controls:DemoSectionView>
            </StackPanel>
        </controls:AnchorScrollViewer>
    </TabItem>
</TabControl>
```

## ViewModel shape

Use this structure in `<ControlName>DemoViewModel.cs`:

```csharp
private const string BasicAnchorId = "<control>-basic-usage";
private const string AdvancedAnchorId = "<control>-advanced-usage";

public DemoSectionViewModel BasicSection { get; }
public DemoSectionViewModel AdvancedSection { get; }

public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; set; } =
[
    new()
    {
        Header = LanguageManager.Instance.Page_<ControlName>_Section_Basic_Usage_Header,
        AnchorId = BasicAnchorId
    },
    new()
    {
        Header = LanguageManager.Instance.Page_<ControlName>_Section_Advanced_Usage_Header,
        AnchorId = AdvancedAnchorId
    }
];

BasicSection = new DemoSectionViewModel
{
    Header = LanguageManager.Instance.Page_<ControlName>_Section_Basic_Usage_Header,
    Descriptions = { LanguageManager.Instance.Page_<ControlName>_Section_Basic_Usage_Description },
    AnchorId = BasicAnchorId
};
BasicSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
{
    CodeSnippetLanguage = CodeLanguage.Axaml,
    TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
    CodeSnippet = """
                  <!-- xaml snippet -->
                  """
});
```

## Localization keys

For each section, add both keys:

1. `Page_<ControlName>_Section_<SectionName>_Header`
2. `Page_<ControlName>_Section_<SectionName>_Description`

Add the keys to:

1. `Resources.resx`
2. `Resources.cs-CZ.resx`
3. `Resources.de-DE.resx`
4. `Resources.fr-FR.resx`
5. `Resources.pl-PL.resx`
6. `Resources.ru-RU.resx`
7. `Resources.zh-Hans.resx`

## Consistency rules

1. Section order in XAML, section properties in ViewModel, and `AnchorItems` must match.
2. Anchor IDs must be unique and stable.
3. Keep snippets representative of the exact section UI.
4. Preserve existing page metadata and behavior.

