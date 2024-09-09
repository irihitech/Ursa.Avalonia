using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.Styling;

namespace Ursa.Controls;

public abstract class ThemeSelectorBase: TemplatedControl
{
    private bool _syncFromScope;
    private Application? _application;
    
    public static readonly StyledProperty<ThemeVariant?> SelectedThemeProperty = AvaloniaProperty.Register<ThemeSelectorBase, ThemeVariant?>(
        nameof(SelectedTheme));

    public ThemeVariant? SelectedTheme
    {
        get => GetValue(SelectedThemeProperty);
        set => SetValue(SelectedThemeProperty, value);
    }

    public static readonly StyledProperty<ThemeSelectorMode> ModeProperty = AvaloniaProperty.Register<ThemeSelectorBase, ThemeSelectorMode>(
        nameof(Mode));

    public ThemeSelectorMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public static readonly StyledProperty<ThemeVariantScope?> TargetScopeProperty =
        AvaloniaProperty.Register<ThemeSelectorBase, ThemeVariantScope?>(
            nameof(TargetScope));

    public ThemeVariantScope? TargetScope
    {
        get => GetValue(TargetScopeProperty);
        set => SetValue(TargetScopeProperty, value);
    }

    static ThemeSelectorBase()
    {
        SelectedThemeProperty.Changed.AddClassHandler<ThemeSelectorBase, ThemeVariant?>((s, e) => s.OnSelectedThemeChanged(e));
        TargetScopeProperty.Changed.AddClassHandler<ThemeSelectorBase, ThemeVariantScope?>((s, e) => s.OnTargetScopeChanged(e));
    }

    private void OnTargetScopeChanged(AvaloniaPropertyChangedEventArgs<ThemeVariantScope?> args)
    {
        if (args.OldValue.Value is { } oldTarget)
        {
            oldTarget.ActualThemeVariantChanged -= OnScopeThemeChanged;
        }
        if (args.NewValue.Value is { } newTarget)
        {
            newTarget.ActualThemeVariantChanged += OnScopeThemeChanged;
            SyncThemeFromScope(newTarget.ActualThemeVariant);
        }
    }

    private void OnScopeThemeChanged(object? sender, System.EventArgs e)
    {
        _syncFromScope = true;
        if (TargetScope is { } target)
        {
            SyncThemeFromScope(Mode == ThemeSelectorMode.Controller
                ? target.RequestedThemeVariant
                : target.ActualThemeVariant);
        }
        else if (_application is { } app)
        {
            SyncThemeFromScope(Mode == ThemeSelectorMode.Controller
                ? app.RequestedThemeVariant
                : app.ActualThemeVariant);
        }
        _syncFromScope = false;
    }

    protected virtual void SyncThemeFromScope(ThemeVariant? theme)
    {
        SelectedTheme = theme;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _application = Application.Current;
        _syncFromScope = true;

        if (TargetScope is not null)
        {
            TargetScope.ActualThemeVariantChanged += OnScopeThemeChanged;
            SyncThemeFromScope(Mode == ThemeSelectorMode.Controller
                ? TargetScope.RequestedThemeVariant
                : TargetScope.ActualThemeVariant);
        }
        else if (this.GetLogicalAncestors().FirstOrDefault(a => a is ThemeVariantScope) is ThemeVariantScope scope)
        {
            TargetScope = scope;
            TargetScope.ActualThemeVariantChanged += OnScopeThemeChanged;
            SyncThemeFromScope(Mode == ThemeSelectorMode.Controller
                ? TargetScope.RequestedThemeVariant
                : TargetScope.ActualThemeVariant);
        }
        else if (_application is not null)
        {
            _application.ActualThemeVariantChanged += OnScopeThemeChanged;
            SyncThemeFromScope(Mode == ThemeSelectorMode.Controller
                ? _application.RequestedThemeVariant
                : _application.ActualThemeVariant);
        }

        _syncFromScope = false;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (_application is not null)
        {
            _application.ActualThemeVariantChanged -= OnScopeThemeChanged;
        }

        if (TargetScope is not null)
        {
            TargetScope.ActualThemeVariantChanged -= OnScopeThemeChanged;
        }
    }

    protected virtual void OnSelectedThemeChanged(AvaloniaPropertyChangedEventArgs<ThemeVariant?> args)
    {
        if (_syncFromScope) return;
        ThemeVariant? newTheme = args.NewValue.Value;
        if (TargetScope is not null)
        {
            TargetScope.RequestedThemeVariant = newTheme;
            return;
        }

        if (_application is not null)
        {
            _application.RequestedThemeVariant = newTheme;
        }
    }
}