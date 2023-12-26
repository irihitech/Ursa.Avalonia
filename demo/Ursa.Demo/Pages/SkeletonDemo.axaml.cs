using Avalonia.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages
{
    public partial class SkeletonDemo : UserControl
    {
        public SkeletonDemo()
        {
            InitializeComponent();
            DataContext = new SkeletonDemoViewModel();
        }
    }
}
