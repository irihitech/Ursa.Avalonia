using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public class AspectRatioLayoutItem : ContentControl
{
    public static readonly StyledProperty<AspectRatioMode> AcceptScaleModeProperty =
        AvaloniaProperty.Register<AspectRatioLayoutItem, AspectRatioMode>(
            nameof(AcceptAspectRatioMode));

    public static readonly StyledProperty<double> StartAspectRatioValueProperty =
        AvaloniaProperty.Register<AspectRatioLayoutItem, double>(
            nameof(StartAspectRatioValue), defaultValue: double.NaN);

    public double StartAspectRatioValue
    {
        get => GetValue(StartAspectRatioValueProperty);
        set => SetValue(StartAspectRatioValueProperty, value);
    }

    public static readonly StyledProperty<double> EndAspectRatioValueProperty =
        AvaloniaProperty.Register<AspectRatioLayoutItem, double>(
            nameof(EndAspectRatioValue), defaultValue: double.NaN);

    public double EndAspectRatioValue
    {
        get => GetValue(EndAspectRatioValueProperty);
        set => SetValue(EndAspectRatioValueProperty, value);
    }


    private bool _isUseAspectRatioRange;

    public static readonly DirectProperty<AspectRatioLayoutItem, bool> IsUseAspectRatioRangeProperty =
        AvaloniaProperty.RegisterDirect<AspectRatioLayoutItem, bool>(
            nameof(IsUseAspectRatioRange), o => o.IsUseAspectRatioRange);

    public bool IsUseAspectRatioRange
    {
        get => _isUseAspectRatioRange;
        private set => SetAndRaise(IsUseAspectRatioRangeProperty, ref _isUseAspectRatioRange, value);
    }

    public AspectRatioMode AcceptAspectRatioMode
    {
        get => GetValue(AcceptScaleModeProperty);
        set => SetValue(AcceptScaleModeProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == StartAspectRatioValueProperty ||
            change.Property == EndAspectRatioValueProperty)
        {
            UpdataIsUseAspectRatioRange();
        }
    }

    private void UpdataIsUseAspectRatioRange()
    {
        if (double.IsNaN(StartAspectRatioValue)
            || double.IsNaN(EndAspectRatioValue)
            || StartAspectRatioValue > EndAspectRatioValue)
            IsUseAspectRatioRange = false;
        else
            IsUseAspectRatioRange = true;
    }
}