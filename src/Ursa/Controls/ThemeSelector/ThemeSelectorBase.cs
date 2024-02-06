using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.Styling;
using Ursa.Common;

namespace Ursa.Controls;

public abstract class ThemeSelectorBase: TemplatedControl
{
    private bool _syncFromScope;
    private Application? _application;
    private ThemeVariantScope? _scope;
    
    public static readonly StyledProperty<ThemeVariant?> SelectedThemeProperty = AvaloniaProperty.Register<ThemeSelectorBase, ThemeVariant?>(
        nameof(SelectedTheme));

    public ThemeVariant? SelectedTheme
    {
        get => GetValue(SelectedThemeProperty);
        set => SetValue(SelectedThemeProperty, value);
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
        var target = args.NewValue.Value;
        if (target is not null)
        {
            SyncThemeFromScope(target.ActualThemeVariant);
            target.ActualThemeVariantChanged += OnScopeThemeChanged;
        }
    }

    private void OnScopeThemeChanged(object sender, EventArgs e)
    {
        _syncFromScope = true;
        if (this.TargetScope is { } target)
        {
            SyncThemeFromScope(target.ActualThemeVariant);
        }
        else if (this._scope is { } scope)
        {
            SyncThemeFromScope(scope.ActualThemeVariant);
        }
        else if (_application is { } app)
        {
            SyncThemeFromScope(app.ActualThemeVariant);
        }
        _syncFromScope = false;
    }

    protected internal virtual void SyncThemeFromScope(ThemeVariant? theme)
    {
        this.SelectedTheme = theme;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _application = Application.Current;
        if (_application is not null)
        {
            _application.ActualThemeVariantChanged += OnScopeThemeChanged;
            SyncThemeFromScope(_application.ActualThemeVariant);
        }
        _scope = this.GetLogicalAncestors().FirstOrDefault(a => a is ThemeVariantScope) as ThemeVariantScope;
        if (_scope is not null)
        {
            _scope.ActualThemeVariantChanged += OnScopeThemeChanged;
            SyncThemeFromScope(_scope.ActualThemeVariant);
        }

        if (TargetScope is not null)
        {
            SyncThemeFromScope(TargetScope.ActualThemeVariant);
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (_application is not null)
        {
            _application.ActualThemeVariantChanged -= OnScopeThemeChanged;
        }

        if (_scope is not null)
        {
            _scope.ActualThemeVariantChanged -= OnScopeThemeChanged;
        }
    }

    protected virtual void OnSelectedThemeChanged(AvaloniaPropertyChangedEventArgs<ThemeVariant?> args)
    {
        if (_syncFromScope) return;
        ThemeVariant? newTheme = args.NewValue.Value;
        if (newTheme is null) return;
        if (TargetScope is not null)
        {
            TargetScope.RequestedThemeVariant = newTheme;
            return;
        }
        if (_scope is not null)
        {
            _scope.RequestedThemeVariant = newTheme;
            return;
        }
        if (_application is not null)
        {
            _application.RequestedThemeVariant = newTheme;
            return;
        }
    }
}