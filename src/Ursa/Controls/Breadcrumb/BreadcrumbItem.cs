using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public class BreadcrumbItem: ContentControl
{
    public static readonly StyledProperty<object?> SeparatorProperty =
        AvaloniaProperty.Register<BreadcrumbItem, object?>(
            nameof(Separator), "/");

    public object? Separator
    {
        get => GetValue(SeparatorProperty);
        set => SetValue(SeparatorProperty, value);
    }

    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<BreadcrumbItem, object?>(
        nameof(Icon));

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<ICommand?> CommandProperty = AvaloniaProperty.Register<BreadcrumbItem, ICommand?>(
        nameof(Command));

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    static BreadcrumbItem()
    {
        
    }
}