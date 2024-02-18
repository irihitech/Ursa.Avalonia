using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

public class FormItem: ContentControl
{
    public static readonly StyledProperty<object?> LabelProperty = AvaloniaProperty.Register<FormItem, object?>(
        nameof(Label));

    public object? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly StyledProperty<bool> IsRequiredProperty = AvaloniaProperty.Register<FormItem, bool>(
        nameof(IsRequired));

    public bool IsRequired
    {
        get => GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }

    public static readonly StyledProperty<GridLength> LabelWidthProperty = AvaloniaProperty.Register<FormItem, GridLength>(
        nameof(LabelWidth));

    public GridLength LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }
    
    static FormItem()
    {
        LabelWidthProperty.Changed.AddClassHandler<FormItem, GridLength>((x, args) => x.LabelWidthChanged(args));
    }

    private void LabelWidthChanged(AvaloniaPropertyChangedEventArgs<GridLength> args)
    {
        GridLength? length = args.GetNewValue<GridLength>();
        
    }
}