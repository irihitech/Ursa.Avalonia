using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_ThemeButton, typeof(Button))]
[PseudoClasses(PC_Dark, PC_Light, PC_Default)]
public class ThemeToggleButton: ThemeSelectorBase
{
    public const string PART_ThemeButton = "PART_ThemeButton";
    
    public const string PC_Light = ":light";
    public const string PC_Dark = ":dark";
    public const string PC_Default = ":default";
    
    private Button? _button;

    public static readonly StyledProperty<bool> IsThreeStateProperty = AvaloniaProperty.Register<ThemeToggleButton, bool>(
        nameof(IsThreeState));

    /// <summary>
    ///   Gets or sets a value indicating whether the control supports two or three states. This is a quick way to set <see cref="ThemeSelectorBase.ThemeSource"/>.
    ///   If <see cref="ThemeSelectorBase.ThemeSource"/> is already set, this property will be ignored. 
    /// </summary>
    public bool IsThreeState
    {
        get => GetValue(IsThreeStateProperty);
        set => SetValue(IsThreeStateProperty, value);
    }

    static ThemeToggleButton()
    {
        IsThreeStateProperty.Changed.AddClassHandler<ThemeToggleButton, bool>((button, args) => button.OnIsThreeStateChanged(args));
    }

    private void OnIsThreeStateChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        InitializeThemeTogglerSource(args.NewValue.Value);
    }
    
    private void InitializeThemeTogglerSource(bool isThreeState)
    {
        if (IsSet(ThemeSourceProperty)) return;
        if (isThreeState)
        {
            ThemeSource = [ThemeVariant.Light, ThemeVariant.Dark, ThemeVariant.Default];
        }
        else
        {
            ThemeSource = [ThemeVariant.Light, ThemeVariant.Dark];
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnButtonClicked, _button);
        _button = e.NameScope.Get<Button>(PART_ThemeButton);
        Button.ClickEvent.AddHandler(OnButtonClicked, _button);
        // ToggleButton.IsCheckedProperty.SetValue(_currentTheme == ThemeVariant.Light, _button);
        InitializeThemeTogglerSource(IsThreeState);
    }

    private void OnButtonClicked(object? sender, RoutedEventArgs e)
    {
        SelectedTheme = NextTheme();
        PseudoClasses.Set(PC_Light, SelectedTheme == ThemeVariant.Light);
        PseudoClasses.Set(PC_Dark, SelectedTheme == ThemeVariant.Dark);
        PseudoClasses.Set(PC_Default, SelectedTheme == ThemeVariant.Default);
    }

    

    protected override void OnTargetThemeChanged(ThemeVariant? theme)
    {
        base.OnTargetThemeChanged(theme);
        PseudoClasses.Set(PC_Light, theme == ThemeVariant.Light);
        PseudoClasses.Set(PC_Dark, theme == ThemeVariant.Dark);
        PseudoClasses.Set(PC_Default, theme == null || SelectedTheme == ThemeVariant.Default);
        
    }
    
    private ThemeVariant? NextTheme()
    {
        if (SelectedTheme is null) return null;
        if (ThemeSource is null || ThemeSource.Count == 0) return null;
        var index = ThemeSource.IndexOf(SelectedTheme);
        if (index == -1) return ThemeSource?[0];
        index++;
        if (index >= ThemeSource.Count) index = 0;
        return ThemeSource[index];
    }
}