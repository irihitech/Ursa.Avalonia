using System.Collections;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace Ursa.Controls;

public class EnumItemTuple
{
    public string? DisplayName { get; set; }
    public object? Value { get; set; }
}

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
        nameof(Value), defaultBindingMode: BindingMode.TwoWay, coerce:OnValueCoerce);

    private static object? OnValueCoerce(AvaloniaObject o,  object? value)
    {
        if (o is not EnumSelector selector) return null;
        if (!selector.IsInitialized) return value;
        if (value is null) return null;
        if (selector.EnumType is not null && value.GetType() != selector.EnumType) return null;
        var first = selector.Values?.FirstOrDefault(a => Equals(a.Value, value));
        if (first is null) return null;
        return value;
    }


    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DirectProperty<EnumSelector, EnumItemTuple?> SelectedValueProperty = AvaloniaProperty.RegisterDirect<EnumSelector, EnumItemTuple?>(
        nameof(SelectedValue), o => o.SelectedValue, (o, v) => o.SelectedValue = v);

    public EnumItemTuple? SelectedValue
    {
        get;
        private set => SetAndRaise(SelectedValueProperty, ref field, value);
    }

    public static readonly DirectProperty<EnumSelector, IList<EnumItemTuple>?> ValuesProperty = AvaloniaProperty.RegisterDirect<EnumSelector, IList<EnumItemTuple>?>(
        nameof(Values), o => o.Values);

    public IList<EnumItemTuple>? Values
    {
        get;
        private set => SetAndRaise(ValuesProperty, ref field, value);
    }

    public static readonly StyledProperty<bool> DisplayDescriptionProperty = AvaloniaProperty.Register<EnumSelector, bool>(
        nameof(DisplayDescription));

    public bool DisplayDescription
    {
        get => GetValue(DisplayDescriptionProperty);
        set => SetValue(DisplayDescriptionProperty, value);
    }

    public static readonly StyledProperty<IList?> EnumValuesProperty = AvaloniaProperty.Register<EnumSelector, IList?>(
        nameof(EnumValues));

    public IList? EnumValues
    {
        get => GetValue(EnumValuesProperty);
        set => SetValue(EnumValuesProperty, value);
    }
    
    static EnumSelector()
    {
        EnumTypeProperty.Changed.AddClassHandler<EnumSelector, Type?>((o, e) => o.OnTypeChanged(e));
        SelectedValueProperty.Changed.AddClassHandler<EnumSelector, EnumItemTuple?>((o, e) => o.OnSelectedValueChanged(e));
        ValueProperty.Changed.AddClassHandler<EnumSelector, object?>((o, e) => o.OnValueChanged(e));
        EnumValuesProperty.Changed.AddClassHandler<EnumSelector, IList?>((o, e) => o.OnEnumValuesChanged(e));
    }

    private void OnValueChanged(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (_updateFromComboBox) return;
        var newValue = args.NewValue.Value;
        if (newValue is null)
        {
            if (Values?.Any() == true)
            {
                SetCurrentValue(SelectedValueProperty, null);
            }
        }
        else
        {
            if (newValue.GetType() != EnumType)
            {
                SetCurrentValue(SelectedValueProperty, null);
            }
            var tuple = Values?.FirstOrDefault(x => Equals(x.Value, newValue));
            SetCurrentValue(SelectedValueProperty, tuple);
        }   
    }

    private bool _updateFromComboBox;

    private void OnSelectedValueChanged(AvaloniaPropertyChangedEventArgs<EnumItemTuple?> args)
    {
        _updateFromComboBox = true;
        var newValue = args.NewValue.Value;
        SetCurrentValue(ValueProperty, newValue?.Value);
        _updateFromComboBox = false;
    }
    
    

    private void OnTypeChanged(AvaloniaPropertyChangedEventArgs<Type?> args)
    {
        Values?.Clear();
        var newType = args.GetNewValue<Type?>();
        if (newType is null || !newType.IsEnum)
        {
            return;
        }
        Values = GenerateItemTuple();
        var first = Values?.FirstOrDefault(a => Equals(a.Value, this.Value));
        SetCurrentValue(SelectedValueProperty, first);
    }

    private void OnEnumValuesChanged(AvaloniaPropertyChangedEventArgs<IList?> args)
    {
        Values?.Clear();
        Values = GenerateItemTuple();
        var first = Values?.FirstOrDefault(a => Equals(a.Value, this.Value));
        SetCurrentValue(SelectedValueProperty, first);
    }

    private List<EnumItemTuple> GenerateItemTuple()
    {
        if (EnumType is null) return new List<EnumItemTuple>();
        var values = Enum.GetValuesAsUnderlyingType(EnumType);
        var enumValues = EnumValues;
        values = GetValidValues(values, enumValues, EnumType);
        List<EnumItemTuple> list = new();
        foreach (var value in values)
        {
            // value is underlying type like int/byte/short
            var enumValue = Enum.ToObject(EnumType, value);
            var displayName = Enum.GetName(EnumType, value);
            if(displayName is null) continue;
            var field = EnumType.GetField(displayName);
            var description = field?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
            if (description is not null)
            {
                displayName = ((DescriptionAttribute) description).Description;
            }
            list.Add(new EnumItemTuple()
            {
                DisplayName = displayName,
                Value = enumValue
            });
        }

        return list;
    }

    private static Array GetValidValues(Array values, IList? enumValues, Type enumType)
    {
        if (enumValues is null || enumValues.Count == 0) return values;
        var result = new List<object>();
        foreach (var item in enumValues)
        {
            if (item is null) continue;
            var underlyingValue = Convert.ChangeType(item, Enum.GetUnderlyingType(enumType));
            foreach (var v in values)
            {
                if (Equals(v, underlyingValue))
                {
                    result.Add(v);
                    break;
                }
            }
        }
        return result.ToArray();
    }
}