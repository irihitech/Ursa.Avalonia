using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Ursa.Fluent2.Demo.ViewModels;

namespace Ursa.Fluent2.Demo;

public sealed class DemoPageViewLocator : IDataTemplate
{
    public bool Match(object? data) => data is DemoPageViewModel;

    public Control Build(object? data)
    {
        var viewModel = (DemoPageViewModel)data!;
        var view = DemoCatalog.GetPage(viewModel.PageId).CreateView();
        view.DataContext = viewModel;
        return view;
    }
}
