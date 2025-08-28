using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Media;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
public class ClosableTag : ContentControl
{
    public const string PART_CloseButton = "PART_CloseButton";

    public static readonly StyledProperty<ICommand?> CommandProperty = AvaloniaProperty.Register<ClosableTag, ICommand?>(
        nameof(Command));
    
    public static readonly StyledProperty<VisualBrush?> VisualContentProperty = AvaloniaProperty.Register<ClosableTag, VisualBrush?>(
        nameof(VisualContent));
    
    public static readonly StyledProperty<double?> VisualContentWidthProperty = AvaloniaProperty.Register<ClosableTag, double?>(
        nameof(VisualContentWidth));
    
    public static readonly StyledProperty<double?> VisualContentHeightProperty = AvaloniaProperty.Register<ClosableTag, double?>(
        nameof(VisualContentHeight));

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    
    public VisualBrush? VisualContent
    {
        get => GetValue(VisualContentProperty);
        set => SetValue(VisualContentProperty, value);
    }
    
    public double? VisualContentWidth
    {
        get => GetValue(VisualContentWidthProperty);
        set => SetValue(VisualContentWidthProperty, value);
    }
    
    public double? VisualContentHeight
    {
        get => GetValue(VisualContentHeightProperty);
        set => SetValue(VisualContentHeightProperty, value);
    }
}