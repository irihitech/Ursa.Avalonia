using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Ursa.Controls;

[TemplatePart(PART_Spinner, typeof(ButtonSpinner))]
[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PART_DragPanel, typeof(Panel))]
public abstract class NumericUpDown : TemplatedControl
{
    public const string PART_Spinner = "PART_Spinner";
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_DragPanel = "PART_DragPanel";
    
    protected internal ButtonSpinner? _spinner;
    protected internal TextBox? _textBox;
    protected internal Panel? _dragPanel;

    private Point? _point;
    private bool _updateFromTextInput;
    
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

        if (_dragPanel is not null)
        {
            _dragPanel.PointerPressed -= OnDragPanelPointerPressed;
            _dragPanel.PointerMoved -= OnDragPanelPointerMoved;
            _dragPanel.PointerReleased -= OnDragPanelPointerReleased;
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

        if (_dragPanel is not null)
        {
            _dragPanel.PointerPressed+= OnDragPanelPointerPressed;
            _dragPanel.PointerMoved += OnDragPanelPointerMoved;
            _dragPanel.PointerReleased += OnDragPanelPointerReleased;
        }
        
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        CommitInput();
        base.OnLostFocus(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var commitSuccess = CommitInput();
            e.Handled = !commitSuccess;
        }
    }

    private void OnDragPanelPointerPressed(object sender, PointerPressedEventArgs e)
    {
        _point = e.GetPosition(this);
    }
    
    private void OnDragPanelPointerReleased(object sender, PointerReleasedEventArgs e)
    {
        _point = null;
    }
    
    private void OnDragPanelPointerMoved(object sender, PointerEventArgs e)
    {
        var point = e.GetPosition(this);
        var delta = point - _point;
        if (delta is null)
        {
            return;
        }
    }

    private void OnTextChange(object sender, TextChangedEventArgs e)
    {
        _updateFromTextInput = true;
        UpdateTextToValue(_textBox?.Text ?? string.Empty);
        _updateFromTextInput = false;
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
    protected abstract void UpdateTextToValue(string x);
    protected abstract bool CommitInput();
    protected abstract void SyncTextAndValue();
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

    public static readonly StyledProperty<T> StepProperty = AvaloniaProperty.Register<NumericUpDownBase<T>, T>(
        nameof(Step));

    public T Step
    {
        get => GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    protected abstract T Clamp();
}