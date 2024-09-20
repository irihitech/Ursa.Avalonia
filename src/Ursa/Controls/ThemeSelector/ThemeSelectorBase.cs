using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Styling;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public abstract class ThemeSelectorBase : TemplatedControl
{
    public static readonly StyledProperty<ThemeVariant?> SelectedThemeProperty =
        AvaloniaProperty.Register<ThemeSelectorBase, ThemeVariant?>(
            nameof(SelectedTheme));

    public static readonly StyledProperty<ThemeSelectorMode> ModeProperty =
        AvaloniaProperty.Register<ThemeSelectorBase, ThemeSelectorMode>(
            nameof(Mode));

    public static readonly StyledProperty<ThemeVariantScope?> TargetScopeProperty =
        AvaloniaProperty.Register<ThemeSelectorBase, ThemeVariantScope?>(
            nameof(TargetScope));

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
        _requestedThemeChangedSubscription?.Dispose();
        _actualThemeChangedSubscription?.Dispose();
        GetEffectiveThemeScope();
        _requestedThemeChangedSubscription = ActualThemeScope
            .GetObservable(ThemeVariantScope.RequestedThemeVariantProperty)
            .Subscribe(OnRequestedThemeChanged);
        _actualThemeChangedSubscription = ActualThemeScope
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
        var theme = Mode == ThemeSelectorMode.Controller
            ? ActualThemeScope?.GetValue(ThemeVariantScope.RequestedThemeVariantProperty)
            : ActualThemeScope?.GetValue(ThemeVariantScope.ActualThemeVariantProperty);
        SetValue(SelectedThemeProperty, theme);
        OnTargetThemeChanged(theme);
    }

    protected virtual void OnTargetThemeChanged(ThemeVariant? theme)
    {
        
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        GetEffectiveThemeScope();
        _requestedThemeChangedSubscription = ActualThemeScope
            .GetObservable(ThemeVariantScope.RequestedThemeVariantProperty)
            .Subscribe(OnRequestedThemeChanged);
        _actualThemeChangedSubscription = ActualThemeScope
            .GetObservable(ThemeVariantScope.ActualThemeVariantProperty)
            .Subscribe(OnActualThemeChanged);
        SyncCurrentTheme();
    }

    private void GetEffectiveThemeScope()
    {
        TargetScope ??= this.GetLogicalAncestors().FirstOrDefault(a => a is ThemeVariantScope) as ThemeVariantScope;
        ActualThemeScope ??= TargetScope;
        ActualThemeScope ??= Application.Current;
        return;
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