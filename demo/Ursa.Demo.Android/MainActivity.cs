using Android.App;
using Android.Content.PM;
using Avalonia.Android;

namespace Ursa.Demo.Android;

[Activity(
    Label = "Ursa",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop, 
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{

}