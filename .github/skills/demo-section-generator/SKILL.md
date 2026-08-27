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
   - Mirror keys into other localized `.resx` files in the same folder (default to English text if translation is unavailable).

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
6. Set `DemoSectionViewModel.SectionTag` for each section using enum values:
   - `DemoSectionTag.Function`
   - `DemoSectionTag.Style`
   - `DemoSectionTag.Others`
   Do not set tag text in page view models. Tag display text is handled inside `DemoSectionView`.

## Implementation checklist

1. Read the target page's existing `.axaml` and `ViewModel.cs`.
2. Add/adjust required namespaces (`controls`, `localizations`, `u`, etc.).
3. Wrap each demo block in `DemoSectionView`.
4. Add section metadata/snippets in the view model.
5. Sync `AnchorItems` ordering with visual section ordering.
6. Add localization keys for every section header/description.
7. Build and run the project's standard validation commands defined in repository instructions.
