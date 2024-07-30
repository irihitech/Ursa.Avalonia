using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Styling;

namespace Ursa.Controls;

public abstract class NumberDisplayerBase : TemplatedControl
{
    public static readonly DirectProperty<NumberDisplayerBase, string?> InternalTextProperty = AvaloniaProperty.RegisterDirect<NumberDisplayerBase, string?>(
        nameof(InternalText), o => o.InternalText, (o, v) => o.InternalText = v);
    private string? _internalText;
    
    public string? InternalText
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

    public static readonly StyledProperty<bool> IsSelectableProperty = AvaloniaProperty.Register<NumberDisplayerBase, bool>(
        nameof(IsSelectable));

    public bool IsSelectable
    {
        get => GetValue(IsSelectableProperty);
        set => SetValue(IsSelectableProperty, value);
    }
}

public abstract class NumberDisplayer<T>: NumberDisplayerBase
{
    private Animation? _animation;
    private CancellationTokenSource _cts = new ();
    
#pragma warning disable AVP1002
    public static readonly StyledProperty<T?> ValueProperty = AvaloniaProperty.Register<NumberDisplayer<T>, T?>(
#pragma warning restore AVP1002
        nameof(Value), defaultBindingMode:BindingMode.TwoWay);

    public T? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

#pragma warning disable AVP1002
    private static readonly StyledProperty<T?> InternalValueProperty = AvaloniaProperty.Register<NumberDisplayer<T>, T?>(
#pragma warning restore AVP1002
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
