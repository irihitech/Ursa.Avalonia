using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace Ursa.Controls;

public class EnumSelector: TemplatedControl
{
    public static readonly StyledProperty<Type?> EnumTypeProperty = AvaloniaProperty.Register<EnumSelector, Type?>(
        nameof(EnumType), validate: OnTypeValidate);
    
    public Type? EnumType
    {
        get => GetValue(EnumTypeProperty);
        set => SetValue(EnumTypeProperty, value);
    }
    
    private static bool OnTypeValidate(Type? arg)
    {
        if (arg is null) return true;
        return arg.IsEnum;
    }

    public static readonly StyledProperty<object?> ValueProperty = AvaloniaProperty.Register<EnumSelector, object?>(
        nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    
    public static readonly DirectProperty<EnumSelector, IList<object?>?> ValuesProperty = AvaloniaProperty.RegisterDirect<EnumSelector, IList<object?>?>(
        nameof(Values), o => o.Values);
    
    private IList<object?>? _values;
    internal IList<object?>? Values
    {
        get => _values;
        private set => SetAndRaise(ValuesProperty, ref _values, value);
    }
    
    
    static EnumSelector()
    {
        EnumTypeProperty.Changed.AddClassHandler<EnumSelector, Type?>((o, e) => o.OnTypeChanged(e));
    }

    private void OnTypeChanged(AvaloniaPropertyChangedEventArgs<Type?> args)
    {
        var newType = args.GetNewValue<Type?>();
        if (newType is null || !newType.IsEnum)
        {
            return;
        }

        var values = Enum.GetValues(newType);
        List<object?> list = new();
        foreach (var value in values)
        {
            if (value.GetType() == newType)
                list.Add(value);
        }
        Values = list;
    }
}