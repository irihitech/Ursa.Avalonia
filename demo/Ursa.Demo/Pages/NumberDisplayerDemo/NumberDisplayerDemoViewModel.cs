using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.NumberDisplayerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(NumberDisplayerDemo))]
public partial class NumberDisplayerDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "NumberDisplayer";
    public const string Menu_Header = "Menu_Header_NumberDisplayer";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_NumberDisplayer,
        Description = LanguageManager.Instance.Page_Description_NumberDisplayer,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_NumberDisplayer)],
        Tags = ["NumberDisplayer", "Number", "Animation"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/NumberDisplayerDemo/NumberDisplayerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/NumberDisplayerDemo/NumberDisplayerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    [ObservableProperty] public partial int Value { get; set; }
    [ObservableProperty] public partial long LongValue { get; set; }
    [ObservableProperty] public partial double DoubleValue { get; set; }
    [ObservableProperty] public partial DateTime DateValue { get; set; }
    public ICommand IncreaseCommand { get; }
    public NumberDisplayerDemoViewModel()
    {
        IncreaseCommand = new RelayCommand(OnChange);
        Value = 0;
        LongValue = 0L;
        DoubleValue = 0d;
        DateValue = DateTime.Now;
    }

    private void OnChange()
    {
        Random r = new Random();
        Value = r.Next(int.MaxValue);
        LongValue = ((long)r.Next(int.MaxValue)) * 1000 + r.Next(1000);
        DoubleValue = r.NextDouble() * 100000;
        DateValue = DateTime.Today.AddDays(r.Next(1000));
    }
}