using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Ursa.Common;

namespace Ursa.Controls;

[TemplatePart(PART_ThemeToggleButton, typeof(ToggleButton))]
public class ThemeToggleButton: ThemeSelectorBase
{
    public const string PART_ThemeToggleButton = "PART_ThemeToggleButton";

    /// <summary>
    /// This button IsChecked=true means ThemeVariant.Light, IsChecked=false means ThemeVariant.Dark.
    /// </summary>
    private ToggleButton? _button;
    private ThemeVariant? _currentTheme;
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _currentTheme = this.ActualThemeVariant;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        EventHelper.UnregisterEvent(Button.ClickEvent, OnButtonClickedChanged, _button);
        _button = e.NameScope.Get<ToggleButton>(PART_ThemeToggleButton);
        EventHelper.RegisterEvent(Button.ClickEvent, OnButtonClickedChanged, _button);
        PropertyHelper.SetValue(ToggleButton.IsCheckedProperty, _currentTheme == ThemeVariant.Light, _button);
    }

    private void OnButtonClickedChanged(object sender, RoutedEventArgs e)
    {
        var newTheme = (sender as ToggleButton)!.IsChecked;
        if (newTheme is null) return;
        SetCurrentValue(SelectedThemeProperty, newTheme.Value ? ThemeVariant.Light : ThemeVariant.Dark);
    }

    protected internal override void SyncThemeFromScope(ThemeVariant? theme)
    {
        base.SyncThemeFromScope(theme);
        PropertyHelper.SetValue(ToggleButton.IsCheckedProperty, theme == ThemeVariant.Light, _button);
    }
}