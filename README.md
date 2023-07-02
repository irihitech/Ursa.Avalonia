# Ursa

<p align="center">
    <img src="./assets/Ursa.svg" alt="drawing" width="150" />
</p>

Ursa is a UI library for building cross-platform UIs with Avalonia UI.

## How to use
1. Ursa

Add nuget package:
```bash
dotnet add package Irihi.Ursa --version 0.1.0-beta20230702
```

You can now use Ursa controls in your Avalonia Application.
```xaml
<Window
    ...
    xmlns:u="https://irihi.tech/ursa"
    ...>
    <StackPanel Margin="20">
        <u:ButtonGroup Classes="Solid Warning">
            <Button Content="Hello" />
            <Button Content="World" />
        </u:ButtonGroup>
        <u:TagInput />
    </StackPanel>
</Window>
```

![Demo](./assets/demo.jpg)

2. Ursa.Themes.Semi

To make Ursa controls show up in your application, you need to reference to a theme package designed for Ursa. 
Ursa.Themes.Semi is a theme package for Ursa inspired by Semi Design. You can add it to your project by following steps.

Add nuget package:
```bash
dotnet add package Semi.Avalonia --version 11.0.0-rc1
dotnet add package Irihi.Ursa.Themes.Semi --version 0.1.0-beta20230702
```

Include Styles in application:
```xaml
    <Application.Styles>
        <StyleInclude Source="avares://Semi.Avalonia/Themes/Index.axaml" />
        <StyleInclude Source="avares://Ursa.Themes.Semi/Index.axaml" />
    </Application.Styles>
```