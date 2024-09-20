using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Styling;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public abstract class ThemeSelectorBase : TemplatedControl
{
    public static readonly StyledProperty<ThemeVariant?> SelectedThemeProperty =
        AvaloniaProperty.Register<ThemeSelectorBase, ThemeVariant?>(
            nameof(SelectedTheme), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<ThemeSelectorMode> ModeProperty =
        AvaloniaProperty.Register<ThemeSelectorBase, ThemeSelectorMode>(
            nameof(Mode));

    public static readonly StyledProperty<ThemeVariantScope?> TargetScopeProperty =
        AvaloniaProperty.Register<ThemeSelectorBase, ThemeVariantScope?>(
            nameof(TargetScope));

    public static readonly StyledProperty<ObservableCollection<ThemeVariant>?> ThemeSourceProperty =
        AvaloniaProperty.Register<ThemeSelectorBase, ObservableCollection<ThemeVariant>?>(
            nameof(ThemeSource));

    public ObservableCollection<ThemeVariant>? ThemeSource
    {
        get => GetValue(ThemeSourceProperty);
        set => SetValue(ThemeSourceProperty, value);
    }

    private IDisposable? _actualThemeChangedSubscription;
    private IDisposable? _requestedThemeChangedSubscription;
    private bool _syncFromScope;
    protected AvaloniaObject? ActualThemeScope;

    static ThemeSelectorBase()
    {
        SelectedThemeProperty.Changed.AddClassHandler<ThemeSelectorBase, ThemeVariant?>((selector, args) =>
            selector.OnSelectedThemeChanged(args));
        TargetScopeProperty.Changed.AddClassHandler<ThemeSelectorBase, ThemeVariantScope?>((selector, args) =>
            selector.OnTargetScopeChanged(args));
    }

    public ThemeVariant? SelectedTheme
    {
        get => GetValue(SelectedThemeProperty);
        set => SetValue(SelectedThemeProperty, value);
    }

    public ThemeSelectorMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public ThemeVariantScope? TargetScope
    {
        get => GetValue(TargetScopeProperty);
        set => SetValue(TargetScopeProperty, value);
    }

    private void OnTargetScopeChanged(AvaloniaPropertyChangedEventArgs<ThemeVariantScope?> args)
    {
        var newScope = GetEffectiveThemeScope();
        if (ActualThemeScope == newScope) return;
        _requestedThemeChangedSubscription?.Dispose();
        _actualThemeChangedSubscription?.Dispose();
        ActualThemeScope = newScope;
        _requestedThemeChangedSubscription = ActualThemeScope?
            .GetObservable(ThemeVariantScope.RequestedThemeVariantProperty)
            .Subscribe(OnRequestedThemeChanged);
        _actualThemeChangedSubscription = ActualThemeScope?
            .GetObservable(ThemeVariantScope.ActualThemeVariantProperty)
            .Subscribe(OnActualThemeChanged);
        SyncCurrentTheme();
    }

    /// <summary>
    ///     For indicator mode, this method is called when the actual theme is changed.
    /// </summary>
    /// <param name="theme"></param>
    private void OnRequestedThemeChanged(ThemeVariant? theme)
    {
        if (Mode == ThemeSelectorMode.Controller) return;
        _syncFromScope = true;
        SelectedTheme = theme;
        SyncCurrentTheme();
        _syncFromScope = false;
    }

    /// <summary>
    ///     For controller mode, this method is called when the requested theme is changed.
    /// </summary>
    /// <param name="theme"></param>
    private void OnActualThemeChanged(ThemeVariant? theme)
    {
        if (Mode == ThemeSelectorMode.Indicator) return;
        _syncFromScope = true;
        SelectedTheme = theme;
        SyncCurrentTheme();
        _syncFromScope = false;
    }

    private void SyncCurrentTheme()
    {
        var theme = ActualThemeScope?.GetValue(ThemeVariantScope.RequestedThemeVariantProperty) ?? ThemeVariant.Default;
        SetValue(SelectedThemeProperty, theme);
        OnTargetThemeChanged(theme);
    }

    protected virtual void OnTargetThemeChanged(ThemeVariant? theme)
    {
        
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        ActualThemeScope = GetEffectiveThemeScope();
        _requestedThemeChangedSubscription = ActualThemeScope?
            .GetObservable(ThemeVariantScope.RequestedThemeVariantProperty)
            .Subscribe(OnRequestedThemeChanged);
        _actualThemeChangedSubscription = ActualThemeScope?
            .GetObservable(ThemeVariantScope.ActualThemeVariantProperty)
            .Subscribe(OnActualThemeChanged);
        SyncCurrentTheme();
    }

    private AvaloniaObject? GetEffectiveThemeScope()
    {
        AvaloniaObject? result = TargetScope;
        result ??= this.GetLogicalAncestors().OfType<ThemeVariantScope>().FirstOrDefault();
        result ??= TargetScope;
        result ??= Application.Current;
        return result;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _requestedThemeChangedSubscription?.Dispose();
        _actualThemeChangedSubscription?.Dispose();
    }

    private void OnSelectedThemeChanged(AvaloniaPropertyChangedEventArgs<ThemeVariant?> args)
    {
        if (_syncFromScope) return;
        var newTheme = args.NewValue.Value;
        if (ActualThemeScope is not null)
            ActualThemeScope.SetValue(ThemeVariantScope.RequestedThemeVariantProperty, newTheme);
    }
}