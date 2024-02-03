using Avalonia.Controls;
using Prism.Ioc;

namespace Ursa.PrismExtension;

public static class UrsaDialogServiceExtension
{
    internal const string UrsaDialogViewPrefix = "URSA_DIALOG_VIEW_";
    
    public static void RegisterUrsaDialogService(this IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IUrsaDialogService, UrsaDialogService>();
        containerRegistry.RegisterSingleton<IUrsaOverlayDialogService, UrsaOverlayDialogService>();
    }
    
    public static void RegisterUrsaDialogView<T>(this IContainerRegistry containerRegistry, string name) where T : Control
    {
        containerRegistry.Register<Control, T>(UrsaDialogViewPrefix+name);
    }
}