using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Styling;

namespace Ursa.Controls;

public abstract class NumberDisplayerBase : TemplatedControl
{
    public static readonly DirectProperty<NumberDisplayerBase, string> InternalTextProperty = AvaloniaProperty.RegisterDirect<NumberDisplayerBase, string>(
        nameof(InternalText), o => o.InternalText, (o, v) => o.InternalText = v);
    private string _internalText;
    
    internal string InternalText
    {
        get => _internalText;
        set => SetAndRaise(InternalTextProperty, ref _internalText, value);
    }

    public static readonly StyledProperty<TimeSpan> DurationProperty = AvaloniaProperty.Register<NumberDisplayerBase, TimeSpan>(
        nameof(Duration));

    public TimeSpan Duration
    {
        get => GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    public static readonly StyledProperty<string> StringFormatProperty = AvaloniaProperty.Register<NumberDisplayerBase, string>(
        nameof(StringFormat));

    public string StringFormat
    {
        get => GetValue(StringFormatProperty);
        set => SetValue(StringFormatProperty, value);
    }
}

public abstract class NumberDisplayer<T>: NumberDisplayerBase
{
    private Animation? _animation;
    private CancellationTokenSource _cts = new ();
    
    public static readonly StyledProperty<T?> ValueProperty = AvaloniaProperty.Register<NumberDisplayer<T>, T?>(
        nameof(Value), defaultBindingMode:BindingMode.TwoWay);

    public T? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private static readonly StyledProperty<T?> InternalValueProperty = AvaloniaProperty.Register<NumberDisplayer<T>, T?>(
        nameof(InternalValue), defaultBindingMode:BindingMode.TwoWay);

    private T? InternalValue
    {
        get => GetValue(InternalValueProperty);
        set => SetValue(InternalValueProperty, value);
    }

    static NumberDisplayer()
    {
        ValueProperty.Changed.AddClassHandler<NumberDisplayer<T>, T?>((item, args) =>
        {
            item.OnValueChanged(args.OldValue.Value, args.NewValue.Value);
        });
        InternalValueProperty.Changed.AddClassHandler<NumberDisplayer<T>, T?>((item, args) =>
        {
            item.InternalText = args.NewValue.Value is null ? string.Empty : item.GetString(args.NewValue.Value);
        });
        DurationProperty.Changed.AddClassHandler<NumberDisplayer<T>, TimeSpan>((item, args) =>item.OnDurationChanged(args));
    }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _animation = new Animation
        {
            Duration = Duration,
            FillMode = FillMode.Forward
        };
        _animation.Children.Add(new KeyFrame()
        {
            Cue = new Cue(0.0),
            Setters = { new Setter{Property = InternalValueProperty } }
        });
        _animation.Children.Add(new KeyFrame()
        {
            Cue = new Cue(1.0),
            Setters = { new Setter{Property = InternalValueProperty } }
        });
        Animation.SetAnimator(_animation.Children[0].Setters[0], GetAnimator());
        Animation.SetAnimator(_animation.Children[1].Setters[0], GetAnimator());
        
        // Display value directly to text on initialization in case value equals to default. 
        SetCurrentValue(InternalTextProperty, this.GetString(Value));
    }

    private void OnDurationChanged(AvaloniaPropertyChangedEventArgs<TimeSpan> args)
    {
        if (_animation is null) return;
        _animation.Duration = args.NewValue.Value;
    }

    private void OnValueChanged(T? oldValue, T? newValue)
    {
        if (_animation is null)
        {
            SetCurrentValue(InternalValueProperty, newValue);
            return;
        }
        _cts.Cancel();
        _cts = new CancellationTokenSource();
        (_animation.Children[0].Setters[0] as Setter)!.Value = oldValue;
        (_animation.Children[1].Setters[0] as Setter)!.Value = newValue;
        _animation.RunAsync(this, _cts.Token);
    }

    protected abstract InterpolatingAnimator<T> GetAnimator();
    
    protected abstract string GetString(T? value);
}

public class Int32Displayer : NumberDisplayer<int>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumberDisplayerBase);

    protected override InterpolatingAnimator<int> GetAnimator()
    {
        return new IntAnimator();
    }

    private class IntAnimator : InterpolatingAnimator<int>
    {
        public override int Interpolate(double progress, int oldValue, int newValue)
        {
            return oldValue + (int)((newValue - oldValue) * progress);
        }
    }

    protected override string GetString(int value)
    {
        return value.ToString(StringFormat);
    }
}

public class DoubleDisplayer : NumberDisplayer<double>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumberDisplayerBase);

    protected override InterpolatingAnimator<double> GetAnimator()
    {
        return new DoubleAnimator();
    }

    private class DoubleAnimator : InterpolatingAnimator<double>
    {
        public override double Interpolate(double progress, double oldValue, double newValue)
        {
            return oldValue + (newValue - oldValue) * progress;
        }
    }

    protected override string GetString(double value)
    {
        return value.ToString(StringFormat);
    }
}

public class DateDisplay : NumberDisplayer<DateTime>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumberDisplayerBase);

    protected override InterpolatingAnimator<DateTime> GetAnimator()
    {
        return new DateAnimator();
    }

    private class DateAnimator : InterpolatingAnimator<DateTime>
    {
        public override DateTime Interpolate(double progress, DateTime oldValue, DateTime newValue)
        {
            var diff = (newValue - oldValue).TotalSeconds;
            return oldValue + TimeSpan.FromSeconds(diff * progress);
        }
    }

    protected override string GetString(DateTime value)
    {
        return value.ToString(StringFormat);
    }
}
