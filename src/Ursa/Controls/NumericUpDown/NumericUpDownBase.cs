using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

[TemplatePart(PART_Spinner, typeof(ButtonSpinner))]
[TemplatePart(PART_TextBox, typeof(TextBox))]
public abstract class NumericUpDown : TemplatedControl
{
    public const string PART_Spinner = "PART_Spinner";
    public const string PART_TextBox = "PART_TextBox";
    
    private Avalonia.Controls.NumericUpDown? _numericUpDown;
    private ButtonSpinner? _spinner;
    private TextBox? _textBox;
    
    public static readonly StyledProperty<bool> TextEditableProperty = AvaloniaProperty.Register<NumericUpDown, bool>(
        nameof(TextEditable), defaultValue: true);

    public bool TextEditable
    {
        get => GetValue(TextEditableProperty);
        set => SetValue(TextEditableProperty, value);
    }

    public static readonly StyledProperty<bool> IsReadOnlyProperty = AvaloniaProperty.Register<NumericUpDown, bool>(
        nameof(IsReadOnly));

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly StyledProperty<object?> InnerLeftContentProperty = AvaloniaProperty.Register<NumericUpDown, object?>(
        nameof(InnerLeftContent));

    public object? InnerLeftContent
    {
        get => GetValue(InnerLeftContentProperty);
        set => SetValue(InnerLeftContentProperty, value);
    }

    public static readonly StyledProperty<string?> WatermarkProperty = AvaloniaProperty.Register<NumericUpDown, string?>(
        nameof(Watermark));

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }
    
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if(_spinner is not null)
        {
            _spinner.Spin -= OnSpin;
        }
        if(_textBox is not null)
        {
            _textBox.TextChanged -= OnTextChange;
        }
        _spinner = e.NameScope.Find<ButtonSpinner>(PART_Spinner);
        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        if (_spinner is not null)
        {
            _spinner.Spin += OnSpin;
        }

        if (_textBox is not null)
        {
            _textBox.TextChanged += OnTextChange;
        }
        
    }

    private void OnTextChange(object sender, TextChangedEventArgs e)
    {
        
        
    }

    private void OnSpin(object sender, SpinEventArgs e)
    {
        if (e.Direction == SpinDirection.Increase)
        {
            Increase();
        }
        else
        {
            Decrease();
        }
    }

    protected abstract void Increase();
    protected abstract void Decrease();
}

public abstract class NumericUpDownBase<T>: NumericUpDown where T: struct, IComparable<T>
{
    public static readonly StyledProperty<T> ValueProperty = AvaloniaProperty.Register<NumericUpDownBase<T>, T>(
        nameof(Value));

    public T Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly StyledProperty<T> MaximumProperty = AvaloniaProperty.Register<NumericUpDownBase<T>, T>(
        nameof(Maximum));

    public T Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly StyledProperty<T> MinimumProperty = AvaloniaProperty.Register<NumericUpDownBase<T>, T>(
        nameof(Minimum));

    public T Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }
    
    
}