using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.EnumSelectorDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(EnumSelectorDemo))]
public partial class EnumSelectorDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "EnumSelector";
    public const string Menu_Header = "Menu_Header_EnumSelector";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_EnumSelector,
        Description = LanguageManager.Instance.Page_Description_EnumSelector,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_EnumSelector)],
        Tags = ["EnumSelector", "Enum", "Selector"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/EnumSelectorDemo/EnumSelectorDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/EnumSelectorDemo/EnumSelectorDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial Type? SelectedType { get; set; }
    [ObservableProperty] public partial object? Value { get; set; }
    [ObservableProperty] public partial object? Value2 { get; set; }
    [ObservableProperty] public partial object? Value3 { get; set; }

    public IList CustomEnumValues { get; set; } = new List<object>
    {
        DayOfWeek.Monday,
        DayOfWeek.Wednesday,
        DayOfWeek.Friday,
    };

    public ObservableCollection<Type?> Types { get; set; } =
    [
        typeof(HorizontalAlignment),
        typeof(VerticalAlignment),
        typeof(Orientation),
        typeof(Dock),
        typeof(GridResizeDirection),
        typeof(DayOfWeek),
        typeof(FillMode),
        typeof(IterationType),
        typeof(BindingMode),
        typeof(BindingPriority),
        typeof(StandardCursorType),
        typeof(Key),
        typeof(KeyModifiers),
        typeof(RoutingStrategies),
        typeof(CustomEnum)
    ];
}

public enum CustomEnum
{
    [Description("是")] Yes,
    [Description("否")] No,
}
