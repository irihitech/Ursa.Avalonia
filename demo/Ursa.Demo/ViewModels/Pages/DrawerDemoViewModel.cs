using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Common;
using Ursa.Controls;
using Ursa.Controls.Options;
using Ursa.Demo.Dialogs;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class DrawerDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Drawer,
        Description = LanguageManager.Instance.Page_Description_Drawer,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Drawer)],
        Tags = ["Drawer", "Panel", "Overlay"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DrawerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DrawerDemoViewModel.cs",
        InlineXamlSupport = false,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    public ICommand ShowDialogCommand { get; set; }

    [ObservableProperty] public partial Position Position { get; set; }
    [ObservableProperty] public partial DialogButton Buttons { get; set; }

    [ObservableProperty] public partial bool CanLightDismiss { get; set; }
    [ObservableProperty] public partial bool IsModal { get; set; }
    [ObservableProperty] public partial bool? IsCloseButtonVisible { get; set; }
    [ObservableProperty] public partial string? Title { get; set; }

    [ObservableProperty] public partial bool Custom { get; set; }
    [ObservableProperty] public partial bool IsLocal { get; set; }
    [ObservableProperty] public partial bool CanResize { get; set; }

    public DrawerDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDefaultDialog);
        Position = Position.Right;
        IsModal = true;
        Title = "Add New";
    }

    private async Task ShowDefaultDialog()
    {
        var options = new DrawerOptions()
        {
            Position = Position,
            Buttons = Buttons,
            CanLightDismiss = CanLightDismiss,
            IsCloseButtonVisible = IsCloseButtonVisible,
            Title = Title,
            CanResize = CanResize,
        };
        var hostId = IsLocal ? "LocalHost" : null;
        if (Custom)
        {
            var vm = new CustomDemoDialogViewModel();
            if (IsModal)
            {
                await OverlayDrawer.ShowCustomAsync<CustomDemoDialog, CustomDemoDialogViewModel, object?>(vm, hostId, options);
            }
            else
            {
                OverlayDrawer.ShowCustom<CustomDemoDialog, CustomDemoDialogViewModel>(vm, hostId, options);
            }
        }
        else
        {
            var vm = new DefaultDemoDialogViewModel();
            if (IsModal)
            {
                await OverlayDrawer.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(vm, hostId, options);
            }
            else
            {
                OverlayDrawer.ShowStandard<DefaultDemoDialog, DefaultDemoDialogViewModel>(vm, hostId, options);
            }
        }
        
    }
}
