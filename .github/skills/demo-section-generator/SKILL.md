---
name: demo-section-generator
description: Generates anchored DemoSectionView-based demo sections for pages in demo/Ursa.Demo/Pages using the AutoCompleteBox demo as the canonical pattern.
license: MIT
---

Use this skill when asked to add or refactor demo sections for a control demo page.

## Canonical references

Always mirror these patterns unless the user asks for a deviation:

1. `demo/Ursa.Demo/Pages/AutoCompleteBoxDemo/AutoCompleteBoxDemo.axaml`
2. `demo/Ursa.Demo/Pages/AutoCompleteBoxDemo/AutoCompleteBoxDemoViewModel.cs`
3. `demo/Ursa.Demo/ViewModels/Controls/DemoSectionViewModel.cs`
4. `demo/Ursa.Demo/Controls/DemoSectionView.axaml`
5. `demo/Ursa.Demo/Controls/AnchorScrollViewer.cs`
6. `demo/Ursa.Demo/Themes/AnchorScrollViewer.axaml`
7. `reference/autocompletebox-demo-pattern.md` in this skill folder

## What to generate

For a target page `<ControlName>Demo`:

1. Update `<ControlName>Demo.axaml` to use:
   - A **Guide** tab (`Tab_Header_Guide`) when sectioned docs are being added.
   - `<controls:AnchorScrollViewer AnchorItems="{Binding AnchorItems}">`.
   - One `<controls:DemoSectionView SectionContext="{Binding XxxSection}">` per section.
2. Update `<ControlName>DemoViewModel.cs` to add:
   - Anchor ID constants (kebab-case string values).
   - `DemoSectionViewModel` properties (`XxxSection` naming).
   - Constructor initialization for each section (`Header`, `SectionTag`, `Descriptions`, `AnchorId`, and `CodeSnippets`).
   - `AnchorItems` entries aligned 1:1 with section headers and anchor IDs.
3. Update localization resources:
   - Add keys to `demo/Ursa.Demo/Localizations/Resources.resx`.
   - Add the same keys to every localized `.resx` in the same folder:
     - `Resources.cs-CZ.resx`
     - `Resources.de-DE.resx`
     - `Resources.fr-FR.resx`
     - `Resources.pl-PL.resx`
     - `Resources.ru-RU.resx`
     - `Resources.zh-Hans.resx`
   - Provide proper per-language translations. Do **not** copy English fallback text into non-English resource files unless the user explicitly requests it.

## Naming and structure rules

1. Follow key naming from AutoCompleteBox:
   - `Page_<ControlName>_Section_<SectionName>_Header`
   - `Page_<ControlName>_Section_<SectionName>_Description`
2. Use `LanguageManager.Instance` for section headers/descriptions and snippet tab names.
3. Use `CodeLanguage.Axaml` and `CodeLanguage.CSharp` for snippets where appropriate.
4. Keep `PageMetadata`, doc attributes, and existing behavior intact unless explicitly requested otherwise.
5. Ensure every section `AnchorId` is unique and appears in both:
   - `DemoSectionViewModel.AnchorId`
   - `AnchorItems` entry
6. Set `DemoSectionViewModel.SectionTag` as an enum value:
   - `DemoSectionTag.Function`
   - `DemoSectionTag.Style`
   - `DemoSectionTag.Others`
   Display text must be derived inside `DemoSectionView` from this enum via Lingua resources (`LanguageManager.Instance.DemoSection_Tag_*`).

## Localization quality rules

1. Translate all newly added section headers/descriptions in every locale file listed above.
2. Reuse existing terminology in each locale (for example wording already used for "section", "usage", "style", "custom", etc.).
3. Keep technical identifiers unchanged when needed (control names, enum names, `XAML`, `C#`, `EnumItemTuple`).
4. Ensure translated text is natural in the target language and semantically equivalent to English source text.
5. Before finishing, confirm there are no new English-only values left in non-English resource files for the keys introduced by this task.

## Implementation checklist

1. Read the target page's existing `.axaml` and `ViewModel.cs`.
2. Add/adjust required namespaces (`controls`, `localizations`, `u`, etc.).
3. Wrap each demo block in `DemoSectionView`.
4. Add section metadata/snippets in the view model.
5. Sync `AnchorItems` ordering with visual section ordering.
6. Add localization keys for every section header/description in all locale files with proper translations.
7. Validate by building only the demo project: `dotnet build demo/Ursa.Demo/Ursa.Demo.csproj`.
8. Do not run full repository build or test suites unless the user explicitly asks for them.
