using System.Threading.Tasks;
using Avalonia.Controls;
using Sandbox.ViewModels;
using Ursa.Controls;

namespace Sandbox.Views;

public partial class MainSplashWindow : SplashWindow
{
    public MainSplashWindow()
    {
        InitializeComponent();
    }
    
    protected override async Task<Window> CreateNextWindow()
    {
        return new MainWindow()
        {
            DataContext = new MainWindowViewModel()
        };
    }
}