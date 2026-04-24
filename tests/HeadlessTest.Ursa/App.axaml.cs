using Avalonia;
using Avalonia.Markup.Xaml;

namespace HeadlessTest.Ursa;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
