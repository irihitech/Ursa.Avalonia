using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class ToolBarDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ToolBar,
        Description = LanguageManager.Instance.Page_Description_ToolBar,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_NavigationAndMenus), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ToolBar)],
        Tags = ["ToolBar", "Navigation", "Button"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ToolBarDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/ToolBarDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public ObservableCollection<ToolBarItemViewModel> Items { get; set; }
    [ObservableProperty] public partial Orientation PanelOrientation { get; set; } = Orientation.Vertical;
    [ObservableProperty] public partial Orientation ToolBarOrientation { get; set; } = Orientation.Horizontal;

    partial void OnToolBarOrientationChanged(Orientation value)
    {
        if (value == Orientation.Horizontal)
        {
            PanelOrientation = Orientation.Vertical;
        }
        else
        {
            PanelOrientation = Orientation.Horizontal;
        }
    }

    public ToolBarDemoViewModel()
    {
        Items = new()
        {
            new ToolBarButtonItemViewModel { Content = "New", OverflowMode = OverflowMode.AsNeeded },
            new ToolBarButtonItemViewModel { Content = "Open" },
            new ToolBarButtonItemViewModel { Content = "Save1" },
            new ToolBarButtonItemViewModel { Content = "Save2" },
            new ToolBarSeparatorViewModel(),
            new ToolBarButtonItemViewModel { Content = "Save3" },
            new ToolBarButtonItemViewModel { Content = "Save4" },
            new ToolBarButtonItemViewModel { Content = "Save5" },
            new ToolBarButtonItemViewModel { Content = "Save6" },
            new ToolBarButtonItemViewModel { Content = "Save7" },
            new ToolBarSeparatorViewModel(),
            new ToolBarButtonItemViewModel { Content = "Save8" },
            new ToolBarCheckBoxItemViweModel { Content = "Bold" },
            new ToolBarCheckBoxItemViweModel { Content = "Italic", OverflowMode = OverflowMode.Never },
            new ToolBarComboBoxItemViewModel { Content = "Font Size", Items = ["10", "12", "14"] }
        };
    }
}

public abstract class ToolBarItemViewModel : ObservableObject
{
    public OverflowMode OverflowMode { get; set; }
}

public class ToolBarButtonItemViewModel : ToolBarItemViewModel
{
    public string? Content { get; set; }
    public ICommand? Command { get; set; }

    public ToolBarButtonItemViewModel()
    {
        Command = new AsyncRelayCommand(async () => { await OverlayMessageBox.ShowAsync(Content ?? string.Empty); });
    }
}

public class ToolBarCheckBoxItemViweModel : ToolBarItemViewModel
{
    public string? Content { get; set; }
    public bool IsChecked { get; set; }
    public ICommand? Command { get; set; }

    public ToolBarCheckBoxItemViweModel()
    {
        Command = new AsyncRelayCommand(async () => { await OverlayMessageBox.ShowAsync(Content ?? string.Empty); });
    }
}

public class ToolBarComboBoxItemViewModel : ToolBarItemViewModel
{
    public string? Content { get; set; }
    public ObservableCollection<string>? Items { get; set; }

    private string? _selectedItem;

    public string? SelectedItem
    {
        get => _selectedItem;
        set
        {
            SetProperty(ref _selectedItem, value);
            _ = OverlayMessageBox.ShowAsync(value ?? string.Empty);
        }
    }
}

public class ToolBarSeparatorViewModel : ToolBarItemViewModel
{
}
