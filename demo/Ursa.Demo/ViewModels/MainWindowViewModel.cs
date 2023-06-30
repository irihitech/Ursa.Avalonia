
using CommunityToolkit.Mvvm.Messaging;

namespace Ursa.Demo.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MenuViewModel Menus { get; set; } = new MenuViewModel();

    private object? _content;
    public object? Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    public MainWindowViewModel()
    {
        WeakReferenceMessenger.Default.Register<MainWindowViewModel, string>(this, OnNavigation);
    }
    
    

    private void OnNavigation(MainWindowViewModel vm, string s)
    {
        Content = s switch
        {
            MenuKeys.MenuKeyBadge => new BadgeDemoViewModel(),
            MenuKeys.MenuKeyBanner => new BannerDemoViewModel(),
            MenuKeys.MenuKeyButtonGroup => new ButtonGroupDemoViewModel(),
            MenuKeys.MenuKeyDivider => new DividerDemoViewModel(),
            MenuKeys.MenuKeyIpBox => new IPv4BoxDemoViewModel(),
            MenuKeys.MenuKeyLoading => new LoadingDemoViewModel(),
            MenuKeys.MenuKeyNavigation => new NavigationMenuDemoViewModel(),
            MenuKeys.MenuKeyPagination => new PaginationDemoViewModel(),
            MenuKeys.MenuKeyTagInput => new TagInputDemoViewModel(),
            MenuKeys.MenuKeyTimeline => new TimelineDemoViewModel(),
        };
    }
}