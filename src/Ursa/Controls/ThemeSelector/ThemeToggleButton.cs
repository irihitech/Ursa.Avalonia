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
        EventHelper.UnregisterEvent(ToggleButton.IsCheckedChangedEvent, OnButtonCheckedChanged, _button);
        _button = e.NameScope.Get<ToggleButton>(PART_ThemeToggleButton);
        EventHelper.RegisterEvent(ToggleButton.IsCheckedChangedEvent, OnButtonCheckedChanged, _button);
        PropertyHelper.SetValue(ToggleButton.IsCheckedProperty, _currentTheme == ThemeVariant.Light, _button);
    }

    private void OnButtonCheckedChanged(object sender, RoutedEventArgs e)
    {
        var newTheme = (sender as ToggleButton)!.IsChecked;
        if (newTheme is null) return;
        SelectedTheme = newTheme.Value ? ThemeVariant.Light : ThemeVariant.Dark;
    }
    
    
}