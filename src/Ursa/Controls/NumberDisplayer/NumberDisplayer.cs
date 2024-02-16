using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace Ursa.Controls;

public abstract class NumberDisplayerBase : TemplatedControl
{
    public static readonly DirectProperty<NumberDisplayerBase, string> InternalTextProperty = AvaloniaProperty.RegisterDirect<NumberDisplayerBase, string>(
        nameof(InternalText), o => o.InternalText, (o, v) => o.InternalText = v);
    private string _internalText;
    public string InternalText
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
    public static readonly StyledProperty<T?> ValueProperty = AvaloniaProperty.Register<NumberDisplayer<T>, T?>(
        nameof(Value), defaultBindingMode:BindingMode.TwoWay);

    public T? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    
    public static readonly StyledProperty<T?> InternalValueProperty = AvaloniaProperty.Register<NumberDisplayer<T>, T?>(
        nameof(InternalValue));

    public T? InternalValue
    {
        get => GetValue(InternalValueProperty);
        set => SetValue(InternalValueProperty, value);
    }

    static NumberDisplayer()
    {
        ValueProperty.Changed.AddClassHandler<NumberDisplayer<T>, T?>((item, args) =>
        {
            item.InternalValue = args.NewValue.Value;
        });
        InternalValueProperty.Changed.AddClassHandler<NumberDisplayer<T>, T?>((item, args) =>
        {
            item.InternalText = args.NewValue.Value is null ? string.Empty : item.GetString(args.NewValue.Value);
        });
        DurationProperty.Changed.AddClassHandler<NumberDisplayer<T>, TimeSpan>((item, args) =>item.OnDurationChanged(args));
    }
    
    private void OnDurationChanged(AvaloniaPropertyChangedEventArgs<TimeSpan> args)
    {
        this.Transitions ??= new Transitions();
        this.Transitions?.Clear();
        this.Transitions?.Add(GetTransition(args.NewValue.Value));
    }

    protected abstract ITransition GetTransition(TimeSpan duration);
    protected abstract string GetString(T? value);
}

public class Int32Displayer : NumberDisplayer<int>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumberDisplayerBase);

    protected override ITransition GetTransition(TimeSpan duration)
    {
        return new IntegerTransition()
        {
            Property = InternalValueProperty,
            Duration = duration
        };
    }

    protected override string GetString(int value)
    {
        return value.ToString(StringFormat);
    }
}

public class DoubleDisplayer : NumberDisplayer<double>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumberDisplayerBase);

    protected override ITransition GetTransition(TimeSpan duration)
    {
        return new DoubleTransition()
        {
            Property = InternalValueProperty,
            Duration = duration
        };
    }

    protected override string GetString(double value)
    {
        return value.ToString(StringFormat);
    }
}
