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
    private bool? _state;

    public static readonly StyledProperty<bool> IsThreeStateProperty = AvaloniaProperty.Register<ThemeToggleButton, bool>(
        nameof(IsThreeState));

    public bool IsThreeState
    {
        get => GetValue(IsThreeStateProperty);
        set => SetValue(IsThreeStateProperty, value);
    }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnButtonClicked, _button);
        _button = e.NameScope.Get<Button>(PART_ThemeButton);
        Button.ClickEvent.AddHandler(OnButtonClicked, _button);
        // ToggleButton.IsCheckedProperty.SetValue(_currentTheme == ThemeVariant.Light, _button);
    }

    private void OnButtonClicked(object? sender, RoutedEventArgs e)
    {
        if (Mode == ThemeSelectorMode.Indicator || !IsThreeState)
        {
            _state = _state switch
            {
                true => false,
                false => true,
                null => throw new InvalidOperationException("Invalid state")
            };
        } 
        else
        {
            _state = _state switch
            {
                true => false,
                false => null,
                null => true
            };
        }
        
        SelectedTheme = _state switch
        {
            true => ThemeVariant.Light,
            false => ThemeVariant.Dark,
            null => ThemeVariant.Default
        };
        
        PseudoClasses.Set(PC_Light, SelectedTheme == ThemeVariant.Light);
        PseudoClasses.Set(PC_Dark, SelectedTheme == ThemeVariant.Dark);
        PseudoClasses.Set(PC_Default, SelectedTheme == ThemeVariant.Default);
    }

    protected override void SyncThemeFromScope(ThemeVariant? theme)
    {
        base.SyncThemeFromScope(theme);
        if (!IsThreeState && theme is null)
        {
            theme = ThemeVariant.Light;
        }
        
        PseudoClasses.Set(PC_Light, theme == ThemeVariant.Light);
        PseudoClasses.Set(PC_Dark, theme == ThemeVariant.Dark);
        PseudoClasses.Set(PC_Default, theme == null || SelectedTheme == ThemeVariant.Default);
        if (theme == ThemeVariant.Dark) _state = false;
        else if (theme == ThemeVariant.Light) _state = true;
        else _state = null;
    }
}