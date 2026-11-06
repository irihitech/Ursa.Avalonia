using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Irihi.Lingua;
using Semi.Avalonia;
using Ursa.Controls;
using Ursa.Demo.Localizations;
using Ursa.Themes.Semi;

namespace Ursa.Demo.Pages.Settings;

public partial class SettingsPage : UserControl
{
    private readonly List<IDisposable> _displayNameSubscriptions = [];

    public SettingsPage()
    {
        InitializeComponent();
        var manager = LanguageManager.Instance;
        WindowStateSelector.EnumValues = new List<EnumItemTuple>
        {
            CreateTuple(WindowState.Normal, manager.Settings_WindowState_Normal),
            CreateTuple(WindowState.Minimized, manager.Settings_WindowState_Minimized),
            CreateTuple(WindowState.Maximized, manager.Settings_WindowState_Maximized),
            CreateTuple(WindowState.FullScreen, manager.Settings_WindowState_FullScreen),
        };
    }

    private EnumItemTuple CreateTuple(WindowState state, IObservable<string?> name)
    {
        var tuple = new EnumItemTuple { Value = state };
        _displayNameSubscriptions.Add(name.Subscribe(new ActionObserver(v => tuple.DisplayName = v)));
        return tuple;
    }

    private sealed class ActionObserver(Action<string?> onNext) : IObserver<string?>
    {
        public void OnCompleted() { }
        public void OnError(Exception error) { }
        public void OnNext(string? value) => onNext(value);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        foreach (var subscription in _displayNameSubscriptions) subscription.Dispose();
        _displayNameSubscriptions.Clear();
    }

    private void CulturePicker_OnCultureChanged(object? sender, CultureChangedEventArgs e)
    {
        if (Application.Current is not null)
        {
            SemiTheme.OverrideLocaleResources(Application.Current, e.Culture?.Culture);
            UrsaSemiTheme.OverrideLocaleResources(Application.Current, e.Culture?.Culture);
        }
    }
}
