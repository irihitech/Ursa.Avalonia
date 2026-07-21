using System.Collections.Generic;
using Ursa.Demo.Localizations;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public List<BreadcrumbItemData> BreadcrumbItems { get; set; } = new List<BreadcrumbItemData>
    {
        new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs),
        new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_IconButton),
    };
    public MainViewViewModel MainViewViewModel { get; set; } = new MainViewViewModel();
}
