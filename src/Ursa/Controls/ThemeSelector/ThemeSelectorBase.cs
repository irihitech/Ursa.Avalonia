using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.Styling;

namespace Ursa.Controls;

public abstract class ThemeSelectorBase: TemplatedControl
{
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
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _application = Application.Current;
        _scope = this.GetLogicalAncestors().FirstOrDefault(a => a is ThemeVariantScope) as ThemeVariantScope;
    }

    protected virtual void OnSelectedThemeChanged(AvaloniaPropertyChangedEventArgs<ThemeVariant?> args)
    {
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