using System.Diagnostics;
using System.Globalization;
using System.Net.Mime;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_Spinner, typeof(ButtonSpinner))]
[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PART_DragPanel, typeof(Panel))]
public abstract class NumericUpDown : TemplatedControl, IClearControl
{
    public const string PART_Spinner = "PART_Spinner";
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_DragPanel = "PART_DragPanel";

    protected internal ButtonSpinner? _spinner;
    protected internal TextBox? _textBox;
    protected internal Panel? _dragPanel;

    private Point? _point;
    protected internal bool _updateFromTextInput;


    protected internal bool _canIncrease = true;

    protected internal bool _canDecrease = true;


    public static readonly StyledProperty<bool> AllowDragProperty = AvaloniaProperty.Register<NumericUpDown, bool>(
        nameof(AllowDrag), defaultBindingMode: BindingMode.TwoWay);

    public bool AllowDrag
    {
        get => GetValue(AllowDragProperty);
        set => SetValue(AllowDragProperty, value);
    }

    public static readonly StyledProperty<bool> IsReadOnlyProperty = AvaloniaProperty.Register<NumericUpDown, bool>(
        nameof(IsReadOnly), defaultBindingMode: BindingMode.TwoWay);

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
       ContentControl.HorizontalContentAlignmentProperty.AddOwner<NumericUpDown>();
    public HorizontalAlignment HorizontalContentAlignment
    {
        get => GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
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

    public static readonly StyledProperty<NumberFormatInfo?> NumberFormatProperty = AvaloniaProperty.Register<NumericUpDown, NumberFormatInfo?>(
        nameof(NumberFormat), defaultValue: NumberFormatInfo.CurrentInfo);

    public NumberFormatInfo? NumberFormat
    {
        get => GetValue(NumberFormatProperty);
        set => SetValue(NumberFormatProperty, value);
    }

    public static readonly StyledProperty<string> FormatStringProperty = AvaloniaProperty.Register<NumericUpDown, string>(
        nameof(FormatString), string.Empty);

    public string FormatString
    {
        get => GetValue(FormatStringProperty);
        set => SetValue(FormatStringProperty, value);
    }

    public static readonly StyledProperty<NumberStyles> ParsingNumberStyleProperty = AvaloniaProperty.Register<NumericUpDown, NumberStyles>(
        nameof(ParsingNumberStyle), defaultValue: NumberStyles.Any);

    public NumberStyles ParsingNumberStyle
    {
        get => GetValue(ParsingNumberStyleProperty);
        set => SetValue(ParsingNumberStyleProperty, value);
    }

    public static readonly StyledProperty<IValueConverter?> TextConverterProperty = AvaloniaProperty.Register<NumericUpDown, IValueConverter?>(
        nameof(TextConverter));

    public IValueConverter? TextConverter
    {
        get => GetValue(TextConverterProperty);
        set => SetValue(TextConverterProperty, value);
    }

    public static readonly StyledProperty<bool> AllowSpinProperty = AvaloniaProperty.Register<NumericUpDown, bool>(
        nameof(AllowSpin), true);

    public bool AllowSpin
    {
        get => GetValue(AllowSpinProperty);
        set => SetValue(AllowSpinProperty, value);
    }

    public static readonly StyledProperty<bool> ShowButtonSpinnerProperty =
        ButtonSpinner.ShowButtonSpinnerProperty.AddOwner<NumericUpDown>();

    public bool ShowButtonSpinner
    {
        get => GetValue(ShowButtonSpinnerProperty);
        set => SetValue(ShowButtonSpinnerProperty, value);
    }

    public static readonly StyledProperty<bool> IsUpdateValueWhenLostFocusProperty =
        AvaloniaProperty.Register<NumericUpDown, bool>(nameof(IsUpdateValueWhenLostFocus), false);

    /// <summary>
    /// If true, the value will be updated when the user loses focus.
    /// </summary>
    public bool IsUpdateValueWhenLostFocus
    {
        get => GetValue(IsUpdateValueWhenLostFocusProperty);
        set => SetValue(IsUpdateValueWhenLostFocusProperty, value);
    }

    public static readonly StyledProperty<bool> IsShowReadButtonProperty =
    AvaloniaProperty.Register<NumericUpDown, bool>(nameof(IsShowReadButton), false);

    public bool IsShowReadButton
    {
        get => GetValue(IsShowReadButtonProperty);
        set => SetValue(IsShowReadButtonProperty, value);
    }

    public static readonly StyledProperty<bool> IsShowWriteButtonProperty =
        AvaloniaProperty.Register<NumericUpDown, bool>(nameof(IsShowWriteButton), false);

    public bool IsShowWriteButton
    {
        get => GetValue(IsShowWriteButtonProperty);
        set => SetValue(IsShowWriteButtonProperty, value);
    }

    public event EventHandler<SpinEventArgs>? Spinned;

    static NumericUpDown()
    {
        NumberFormatProperty.Changed.AddClassHandler<NumericUpDown>((o, e) => o.OnFormatChange(e));
        FormatStringProperty.Changed.AddClassHandler<NumericUpDown>((o, e) => o.OnFormatChange(e));
        IsReadOnlyProperty.Changed.AddClassHandler<NumericUpDown, bool>((o, args) => o.OnIsReadOnlyChanged(args));
        TextConverterProperty.Changed.AddClassHandler<NumericUpDown>((o, e) => o.OnFormatChange(e));
        AllowDragProperty.Changed.AddClassHandler<NumericUpDown, bool>((o, e) => o.OnAllowDragChange(e));
    }

    private void OnAllowDragChange(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        IsVisibleProperty.SetValue(args.NewValue.Value, _dragPanel);
    }

    private void OnIsReadOnlyChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        ChangeToSetSpinDirection(args, false);
        TextBox.IsReadOnlyProperty.SetValue(args.NewValue.Value, _textBox);
    }

    protected void ChangeToSetSpinDirection(AvaloniaPropertyChangedEventArgs e, bool afterInitialization = false)
    {
        if (afterInitialization)
        {
            if (IsInitialized)
            {
                SetValidSpinDirection();
            }
        }
        else
        {
            SetValidSpinDirection();
        }
    }

    protected virtual void OnFormatChange(AvaloniaPropertyChangedEventArgs arg)
    {
        if (IsInitialized)
        {
            SyncTextAndValue(false, null, true);//sync text update while OnFormatChange
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Spinner.SpinEvent.RemoveHandler(OnSpin, _spinner);
        PointerPressedEvent.RemoveHandler(OnDragPanelPointerPressed, _dragPanel);
        PointerMovedEvent.RemoveHandler(OnDragPanelPointerMoved, _dragPanel);
        PointerReleasedEvent.RemoveHandler(OnDragPanelPointerReleased, _dragPanel);
        _spinner = e.NameScope.Find<ButtonSpinner>(PART_Spinner);
        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _dragPanel = e.NameScope.Find<Panel>(PART_DragPanel);
        IsVisibleProperty.SetValue(AllowDrag, _dragPanel);
        TextBox.IsReadOnlyProperty.SetValue(IsReadOnly, _textBox);
        Spinner.SpinEvent.AddHandler(OnSpin, _spinner);
        PointerPressedEvent.AddHandler(OnDragPanelPointerPressed, _dragPanel);
        PointerMovedEvent.AddHandler(OnDragPanelPointerMoved, _dragPanel);
        PointerReleasedEvent.AddHandler(OnDragPanelPointerReleased, _dragPanel);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        if (IsUpdateValueWhenLostFocus)
        {
            CommitInput(true);
        }
        base.OnLostFocus(e);
        if (AllowDrag && _dragPanel is not null)
        {
            _dragPanel.IsVisible = true;
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var commitSuccess = CommitInput(true);
            e.Handled = !commitSuccess;
        }

        if (e.Key == Key.Escape)
        {
            if (AllowDrag && _dragPanel is not null)
            {
                _dragPanel.IsVisible = true;
                // _dragPanel.Focus();
                _textBox?.ClearSelection();
                _spinner?.Focus();
            }
        }
    }

    private void OnDragPanelPointerPressed(object sender, PointerPressedEventArgs e)
    {
        _point = e.GetPosition(this);
        if (e.ClickCount == 2 && _dragPanel is not null && AllowDrag)
        {
            IsVisibleProperty.SetValue(false, _dragPanel);
            _textBox?.Focus();
            TextBox.IsReadOnlyProperty.SetValue(IsReadOnly, _textBox);
        }
        else
        {
            _textBox?.Focus();
            TextBox.IsReadOnlyProperty.SetValue(true, _textBox);
        }
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        if (IsReadOnly) return;
        _textBox?.RaiseEvent(e);
    }

    private void OnDragPanelPointerReleased(object sender, PointerReleasedEventArgs e)
    {
        _point = null;
    }

    private void OnDragPanelPointerMoved(object sender, PointerEventArgs e)
    {
        if (!AllowDrag || IsReadOnly) return;
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        var point = e.GetPosition(this);
        var delta = point - _point;
        if (delta is null)
        {
            return;
        }
        int d = GetDelta(delta.Value);
        if (d > 0)
        {
            if (_canIncrease)
                Increase();
        }
        else if (d < 0)
        {
            if (_canDecrease)
                Decrease();
        }
        _point = point;
    }

    private int GetDelta(Point point)
    {
        bool horizontal = Math.Abs(point.X) > Math.Abs(point.Y);
        var value = horizontal ? point.X : -point.Y;
        return value switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => 0
        };
    }

    private void OnSpin(object sender, SpinEventArgs e)
    {
        if (AllowSpin && !IsReadOnly)
        {
            var spin = !e.UsingMouseWheel;
            spin |= _textBox is { IsFocused: true };
            if (spin)
            {
                e.Handled = true;
                var handler = Spinned;
                handler?.Invoke(this, e);
                if (e.Direction == SpinDirection.Increase)
                {
                    Increase();
                }
                else
                {
                    Decrease();
                }
            }
        }
    }

    protected abstract void SetValidSpinDirection();

    protected abstract void Increase();
    protected abstract void Decrease();

    protected virtual bool CommitInput(bool forceTextUpdate = false)
    {
        return SyncTextAndValue(true, _textBox?.Text, forceTextUpdate);
    }

    protected abstract bool SyncTextAndValue(bool fromTextToValue = false, string? text = null,
        bool forceTextUpdate = false);

    public abstract void Clear();
}

public abstract class NumericUpDownBase<T> : NumericUpDown where T : struct, IComparable<T>
{
    protected static string TrimString(string? text, NumberStyles numberStyles)
    {
        if (text is null) return string.Empty;
        text = text.Trim();
        if (text.Contains("_")) // support _ like 0x1024_1024(hex), 10_24 (normal)
        {
            text = text.Replace("_", "");
        }

        if ((numberStyles & NumberStyles.AllowHexSpecifier) != 0)
        {
            if (text.StartsWith("0x") || text.StartsWith("0X")) // support 0x hex while user input
            {
                text = text.Substring(2);
            }
            else if (text.StartsWith("h'") || text.StartsWith("H'")) // support verilog hex while user input
            {
                text = text.Substring(2);
            }
            else if (text.StartsWith("h") || text.StartsWith("H")) // support  hex while user input
            {
                text = text.Substring(1);
            }
        }
#if NET8_0_OR_GREATER
        else if ((numberStyles & NumberStyles.AllowBinarySpecifier) != 0)
        {
            if (text.StartsWith("0b") || text.StartsWith("0B")) // support 0b bin while user input
            {
                text = text.Substring(2);
            }
            else if (text.StartsWith("b'") || text.StartsWith("B'")) // support verilog bin while user input
            {
                text = text.Substring(2);
            }
            else if (text.StartsWith("b") || text.StartsWith("B")) // support bin while user input
            {
                text = text.Substring(1);
            }
        }

#endif
        return text;
    }

    public static readonly StyledProperty<T?> ValueProperty = AvaloniaProperty.Register<NumericUpDownBase<T>, T?>(
        nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public T? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly StyledProperty<T> MaximumProperty = AvaloniaProperty.Register<NumericUpDownBase<T>, T>(
        nameof(Maximum), defaultBindingMode: BindingMode.TwoWay, coerce: CoerceMaximum);

    public T Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly StyledProperty<T> MinimumProperty = AvaloniaProperty.Register<NumericUpDownBase<T>, T>(
        nameof(Minimum), defaultBindingMode: BindingMode.TwoWay, coerce: CoerceMinimum);

    public T Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    #region Max and Min Coerce
    private static T CoerceMaximum(AvaloniaObject instance, T value)
    {
        if (instance is NumericUpDownBase<T> n)
        {
            return n.CoerceMaximum(value);
        }

        return value;
    }

    private T CoerceMaximum(T value)
    {
        if (value.CompareTo(Minimum) < 0)
        {
            return Minimum;
        }
        return value;
    }

    private static T CoerceMinimum(AvaloniaObject instance, T value)
    {
        if (instance is NumericUpDownBase<T> n)
        {
            return n.CoerceMinimum(value);
        }

        return value;
    }

    private T CoerceMinimum(T value)
    {
        if (value.CompareTo(Maximum) > 0)
        {
            return Maximum;
        }
        return value;
    }

    #endregion

    public static readonly StyledProperty<T> StepProperty = AvaloniaProperty.Register<NumericUpDownBase<T>, T>(
        nameof(Step));

    public T Step
    {
        get => GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    public static readonly StyledProperty<T?> EmptyInputValueProperty =
        AvaloniaProperty.Register<NumericUpDownBase<T>, T?>(
            nameof(EmptyInputValue), defaultValue: null);

    public T? EmptyInputValue
    {
        get => GetValue(EmptyInputValueProperty);
        set => SetValue(EmptyInputValueProperty, value);
    }


    public static readonly StyledProperty<ICommand?> CommandProperty = AvaloniaProperty.Register<Pagination, ICommand?>(
        nameof(Command));

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<Pagination, object?>(nameof(CommandParameter));

    public object? CommandParameter
    {
        get => this.GetValue(CommandParameterProperty);
        set => this.SetValue(CommandParameterProperty, value);
    }

    private void InvokeCommand(object? cp)
    {
        if (this.Command != null && this.Command.CanExecute(cp))
        {
            this.Command.Execute(cp);
        }
    }

    /// <summary>
    /// Defines the <see cref="ValueChanged"/> event.
    /// </summary>
    public static readonly RoutedEvent<ValueChangedEventArgs<T>> ValueChangedEvent =
        RoutedEvent.Register<NumericUpDown, ValueChangedEventArgs<T>>(nameof(ValueChanged), RoutingStrategies.Bubble);

    /// <summary>
    /// Raised when the <see cref="Value"/> changes.
    /// </summary>
    public event EventHandler<ValueChangedEventArgs<T>>? ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }

    static NumericUpDownBase()
    {
        StepProperty.Changed.AddClassHandler<NumericUpDownBase<T>>((o, e) => o.ChangeToSetSpinDirection(e));
        MaximumProperty.Changed.AddClassHandler<NumericUpDownBase<T>>((o, e) => o.OnConstraintChanged(e));
        MinimumProperty.Changed.AddClassHandler<NumericUpDownBase<T>>((o, e) => o.OnConstraintChanged(e));
        ValueProperty.Changed.AddClassHandler<NumericUpDownBase<T>>((o, e) => o.OnValueChanged(e));
    }

    private void OnConstraintChanged(AvaloniaPropertyChangedEventArgs avaloniaPropertyChangedEventArgs)
    {
        if (IsInitialized)
        {
            SetValidSpinDirection();
        }
        if (Value.HasValue)
        {
            SetCurrentValue(ValueProperty, Clamp(Value, Maximum, Minimum));
        }
    }

    private void OnValueChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (IsInitialized)
        {
            SyncTextAndValue(false, null, true);
            SetValidSpinDirection();
            T? oldValue = args.GetOldValue<T?>();
            T? newValue = args.GetNewValue<T?>();
            var e = new ValueChangedEventArgs<T>(ValueChangedEvent, oldValue, newValue);
            RaiseEventCommand(e);
        }
    }

    private void RaiseEventCommand(ValueChangedEventArgs<T> e)
    {
        InvokeCommand(this.CommandParameter ?? e.NewValue);
        RaiseEvent(e);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_textBox != null)
        {
            _textBox.Text = ConvertValueToText(Value);
        }
        SetValidSpinDirection();
    }

    protected virtual T? Clamp(T? value, T max, T min)
    {
        if (value is null)
        {
            return null;
        }
        if (value.Value.CompareTo(max) > 0)
        {
            return max;
        }
        if (value.Value.CompareTo(min) < 0)
        {
            return min;
        }
        return value;
    }

    protected override void SetValidSpinDirection()
    {
        var validDirection = ValidSpinDirections.None;
        _canIncrease = false;
        _canDecrease = false;
        if (!IsReadOnly)
        {
            if (Value is null)
            {
                validDirection = ValidSpinDirections.Increase | ValidSpinDirections.Decrease;
            }
            if (Value.HasValue && Value.Value.CompareTo(Maximum) < 0)
            {
                validDirection |= ValidSpinDirections.Increase;
                _canIncrease = true;
            }

            if (Value.HasValue && Value.Value.CompareTo(Minimum) > 0)
            {
                validDirection |= ValidSpinDirections.Decrease;
                _canDecrease = true;
            }
        }
        if (_spinner != null)
        {
            _spinner.ValidSpinDirection = validDirection;
        }
    }

    private bool _isSyncingTextAndValue;

    protected override bool SyncTextAndValue(bool fromTextToValue = false, string? text = null, bool forceTextUpdate = false)
    {
        if (_isSyncingTextAndValue) return true;
        _isSyncingTextAndValue = true;
        var parsedTextIsValid = true;
        try
        {
            if (fromTextToValue)
            {
                try
                {
                    var newValue = ConvertTextToValue(text);
                    if (EmptyInputValue is not null && newValue is null)
                    {
                        newValue = EmptyInputValue;
                    }
                    if (!Equals(newValue, Value))
                    {
                        if (Equals(Clamp(newValue, Maximum, Minimum), newValue))
                        {
                            SetCurrentValue(ValueProperty, newValue);
                        }
                        else
                        {
                            parsedTextIsValid = false;
                        }
                    }
                }
                catch
                {
                    parsedTextIsValid = false;
                }
            }

            if (!_updateFromTextInput)
            {
                if (forceTextUpdate)
                {
                    var newText = ConvertValueToText(Value);
                    if (_textBox != null && !Equals(_textBox.Text, newText))
                    {
                        _textBox.Text = newText;
                        _textBox.CaretIndex = newText?.Length ?? 0;
                    }
                }
            }

            if (_updateFromTextInput && !parsedTextIsValid)
            {
                if (_spinner is not null)
                {
                    _spinner.ValidSpinDirection = ValidSpinDirections.None;
                }
            }
            else
            {
                SetValidSpinDirection();
            }
        }
        finally
        {
            _isSyncingTextAndValue = false;
        }
        return parsedTextIsValid;
    }

    protected virtual T? ConvertTextToValue(string? text)
    {
        T? result;
        if (string.IsNullOrWhiteSpace(text)) return null;
        if (TextConverter != null)
        {
            var valueFromText = TextConverter.Convert(text, typeof(T?), null, CultureInfo.CurrentCulture);
            return (T?)valueFromText;
        }
        else
        {
            text = TrimString(text, ParsingNumberStyle);
            if (!ParseText(text, out var outputValue))
            {
                throw new InvalidDataException("Input string was not in a correct format.");
            }

            result = outputValue;
        }
        return result;
    }

    protected virtual string? ConvertValueToText(T? value)
    {
        if (TextConverter is not null)
        {
            return TextConverter.ConvertBack(Value, typeof(int), null, CultureInfo.CurrentCulture)?.ToString();
        }

        if (FormatString.Contains("{0"))
        {
            return string.Format(NumberFormat, FormatString, value);
        }

        return ValueToString(Value);
    }

    protected override void Increase()
    {
        T? value;
        if (Value is not null)
        {
            value = Add(Value.Value, Step);
        }
        else
        {
            value = IsSet(MinimumProperty) ? Minimum : Zero;
        }
        SetCurrentValue(ValueProperty, Clamp(value, Maximum, Minimum));
    }

    protected override void Decrease()
    {
        T? value;
        if (Value is not null)
        {
            value = Minus(Value.Value, Step);
        }
        else
        {
            value = IsSet(MaximumProperty) ? Maximum : Zero;
        }

        SetCurrentValue(ValueProperty, Clamp(value, Maximum, Minimum));
    }

    protected abstract bool ParseText(string? text, out T number);
    protected abstract string? ValueToString(T? value);
    protected abstract T Zero { get; }
    protected abstract T? Add(T? a, T? b);
    protected abstract T? Minus(T? a, T? b);

    public override void Clear()
    {
        SetCurrentValue(ValueProperty, EmptyInputValue);
        SyncTextAndValue(false, forceTextUpdate: true);
    }
}