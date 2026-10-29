# Ursa

[English](README.md) | 简体中文

<p align="center">
    <img src="./assets/Ursa.svg" alt="drawing" height="64" />
</p>

[![Irihi.Ursa](https://img.shields.io/nuget/v/Irihi.Ursa.svg?color=red&style=flat-square)](https://www.nuget.org/packages/Irihi.Ursa/)
[![Irihi.Ursa](https://img.shields.io/nuget/dt/Irihi.Ursa.svg?style=flat-square)](https://www.nuget.org/packages/Irihi.Ursa/)
[![GitCode](https://gitcode.com/IRIHI_Technology/Ursa.Avalonia/star/badge.svg)](https://gitcode.com/IRIHI_Technology/Ursa.Avalonia)

Ursa 是一个企业级 Avalonia UI 库，助力开发者快速构建跨平台应用程序。

![Demo](./assets/dark-demo.jpg)

## .NET 基金会

本项目由 [.NET 基金会](https://dotnetfoundation.org) 支持。

## 快速开始

1. Ursa

添加 nuget 包：
```bash
dotnet add package Irihi.Ursa
```

2. Ursa.Themes.Semi

为了让 Ursa 控件在您的应用程序中显示，您需要引用为 Ursa 设计的主题包。
Ursa.Themes.Semi 是受 Semi Design 启发的 Ursa 主题包。您可以通过以下步骤将其添加到您的项目中。

添加 nuget 包：
```bash
dotnet add package Semi.Avalonia
dotnet add package Irihi.Ursa.Themes.Semi
```

在应用程序中包含样式：
```xaml
<Application...
    xmlns:semi="https://irihi.tech/semi"
    ....>

    <Application.Styles>
        <semi:SemiTheme Locale="zh-CN" />
        <semi:UrsaSemiTheme Locale="zh-CN"/>
    </Application.Styles>
    ...
```


现在您可以在 Avalonia 应用程序中使用 Ursa 控件了。
```xaml
<Window
    ...
    xmlns:u="https://irihi.tech/ursa"
    ...>
    <StackPanel Margin="20">
        <u:TagInput />
    </StackPanel>
</Window>
```



## 行为准则

本项目采用了由贡献者公约定义的行为准则，以明确社区的预期行为。
有关更多信息，请参阅 [.NET 基金会行为准则](https://dotnetfoundation.org/code-of-conduct)。

## 兼容性说明
Ursa 目前兼容 Avalonia 11.1.x 到 11.3.x，但明确不支持 Avalonia 11.2.0。

## 扩展

### Fluent 2 主题（实验性）

`src/Ursa.Themes.Fluent2` 提供独立于 Semi 的 Fluent 2 主题。它依赖 Avalonia
尚未正式发布的 CI 包，版本固定在 `Directory.Packages.props` 中，并通过仓库的
`NuGet.Config` 还原。现有 Semi 项目的依赖版本保持不变。

使用时以 `<Fluent2Theme />` 和 `<fluent2:UrsaFluent2Theme />` 替换 Semi 主题，
先注册 Avalonia 基础主题，再注册 Ursa 主题，其中
`xmlns:fluent2="https://irihi.tech/ursa/themes/fluent2"`。
应用中的所有 Avalonia 包需要使用相同的 Fluent 2 CI 版本。

运行 `dotnet run --project demo\Ursa.Fluent2.Demo` 可查看控件示例。示例使用支持搜索和分类的
`NavMenu` 侧边栏，每个控件具有独立页面，相关选项和面板示例归入所属控件的页面。
通过外观设置可切换浅色/深色、普通/紧凑密度以及中英文。
Fluent 2 示例使用 CommunityToolkit.Mvvm 的可观察视图模型和命令。
默认打开控件总览页，以六个分区集中展示可交互的控件，布局随窗口宽度自动换行；
侧边栏仍可访问各控件的独立示例页面。
示例嵌入仓库已有的思源黑体（Source Han Sans CN）常规体和粗体作为 CJK 回退字体，
保留 Fluent 2 原生拉丁文字字体，无需安装系统字体。输出目录附带字体的 SIL Open Font License。
AutoCompleteBox、MultiAutoCompleteBox、MultiComboBox、TreeComboBox、SelectionList
和 TagInput 使用独立页面视图模型，演示可观察数据源、选中项或标签绑定及清空命令。
TreeComboBox 使用分层数据。
导航项仅保存路由 ID，不持有页面或工厂；`DemoPageViewLocator` 通过目录解析页面视图，
并由 `DemoInteractionService` 处理窗口和浮层交互。
密度需同时设置两个主题的 `DensityStyle`；Ursa 字符串通过 `Locale` 配置，
默认简体中文。目前尚不支持高对比度主题。

### Prism 扩展
如果您需要将 Ursa 与 Prism.Avalonia 集成，可以使用 Irihi.Ursa.PrismExtension 包。该包提供对话框相关服务，以便以 Prism 风格使用 Ursa 对话框。

### ReactiveUI 扩展
如果需要将 Ursa 与 ReactiveUI.Avalonia 集成，可以使用 Irihi.Ursa.ReactiveUIExtension 包。该包实现了 UrsaWindow 和 UrsaView 的 ReactiveUI 版本。详细信息请参阅 [wiki](https://github.com/irihitech/Ursa.Avalonia/wiki/Ursa-ReactiveUI-extension)。

## 支持

我们为 Semi Avalonia 和 Ursa 提供有限的免费社区支持。请通过飞书加入我们的群组

<p>
    <img src="./assets/community-support.png" alt="drawing" width="600" />
</p>
