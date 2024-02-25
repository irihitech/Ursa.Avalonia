using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Shapes;

namespace Ursa.Controls;

public class DisabledAdorner
{
    public static readonly AttachedProperty<bool> IsEnabledProperty =
        AvaloniaProperty.RegisterAttached<DisabledAdorner, InputElement, bool>("IsEnabled");

    public static void SetIsEnabled(InputElement obj, bool value) => obj.SetValue(IsEnabledProperty, value);
    public static bool GetIsEnabled(InputElement obj) => obj.GetValue(IsEnabledProperty);

    public static readonly AttachedProperty<object?> DisabledTipProperty =
        AvaloniaProperty.RegisterAttached<DisabledAdorner, InputElement, object?>("DisabledTip");

    public static void SetDisabledTip(InputElement obj, object? value) => obj.SetValue(DisabledTipProperty, value);
    public static object? GetDisabledTip(InputElement obj) => obj.GetValue(DisabledTipProperty);

    static DisabledAdorner()
    {
        IsEnabledProperty.Changed.AddClassHandler<InputElement, bool>(OnIsEnabledChanged);
    }

    private static void OnIsEnabledChanged(InputElement arg1, AvaloniaPropertyChangedEventArgs<bool> arg2)
    {
        if (arg2.NewValue.Value)
        {
            var pureRectangle = new PureRectangle()
            {
                Background = Brushes.Transparent,
                IsHitTestVisible = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Cursor = new Cursor(StandardCursorType.No),
                [!ToolTip.TipProperty] = arg1[!DisabledTipProperty],
            };
            var binding = arg1.GetObservable(InputElement.IsEnabledProperty, converter: (a) => !a).ToBinding();
            pureRectangle.Bind(Visual.IsVisibleProperty, binding);
            AdornerLayer.SetAdorner(arg1, pureRectangle);
        }
        else
        {
            AdornerLayer.SetAdorner(arg1, null);
        }

    }
}