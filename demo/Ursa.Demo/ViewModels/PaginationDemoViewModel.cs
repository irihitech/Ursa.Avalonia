using System.Diagnostics;
using System.Windows.Input;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;

namespace Ursa.Demo.ViewModels;

public class PaginationDemoViewModel : ViewModelBase
{
    public AvaloniaList<int> PageSizes { get; set; } = new() { 10, 20, 50, 100 };

    public ICommand LoadPageCommand { get; }
    public PaginationDemoViewModel()
    {
        this.LoadPageCommand = new RelayCommand<int?>(LoadPage);
    }

    private void LoadPage(int? pageIndex)
    {
        Debug.WriteLine($"Loading page {pageIndex}");
    }
}