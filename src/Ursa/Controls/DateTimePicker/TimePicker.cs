using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PartNames.PART_Popup, typeof(Popup))]
[TemplatePart(PART_Presenter, typeof(TimePickerPresenter))]
[TemplatePart(PART_Button, typeof(Button))]
public class TimePicker : TimePickerBase, IClearControl
{
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_Presenter = "PART_Presenter";
    public const string PART_Button = "PART_Button";

    public static readonly StyledProperty<TimeSpan?> SelectedTimeProperty =
        AvaloniaProperty.Register<TimePicker, TimeSpan?>(
            nameof(SelectedTime), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<string?> WatermarkProperty = AvaloniaProperty.Register<TimePicker, string?>(
        nameof(Watermark));

    private bool _suppressTextPresenterEvent;

    private Button? _button;
    private TimePickerPresenter? _presenter;
    private TextBox? _textBox;


    static TimePicker()
    {
        SelectedTimeProperty.Changed.AddClassHandler<TimePicker, TimeSpan?>((picker, args) =>
            picker.OnSelectionChanged(args));
        DisplayFormatProperty.Changed.AddClassHandler<TimePicker, string?>((picker, args) => picker.OnDisplayFormatChanged(args));
    }

    private void OnDisplayFormatChanged(AvaloniaPropertyChangedEventArgs<string?> _)
    {
        if (_textBox is null) return;
        SyncTimeToText(SelectedTime);
    }

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public TimeSpan? SelectedTime
    {
        get => GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public void Clear()
    {
        Focus(NavigationMethod.Pointer);
        _presenter?.SetValue(TimePickerPresenter.TimeProperty, null);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.RemoveHandler(OnTextChanged, _textBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPointerPressed, _textBox);
        Button.ClickEvent.RemoveHandler(OnButtonClick, _button);
        TimePickerPresenter.SelectedTimeChangedEvent.RemoveHandler(OnPresenterTimeChanged, _presenter);

        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        e.NameScope.Find<Popup>(PartNames.PART_Popup);
        _presenter = e.NameScope.Find<TimePickerPresenter>(PART_Presenter);
        _button = e.NameScope.Find<Button>(PART_Button);

        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.AddHandler(OnTextChanged, _textBox);
        PointerPressedEvent.AddHandler(OnTextBoxPointerPressed, RoutingStrategies.Tunnel, false, _textBox);
        Button.ClickEvent.AddHandler(OnButtonClick, _button);
        TimePickerPresenter.SelectedTimeChangedEvent.AddHandler(OnPresenterTimeChanged, _presenter);

        // SetCurrentValue(SelectedTimeProperty, DateTime.Now.TimeOfDay);
        _presenter?.SetValue(TimePickerPresenter.TimeProperty, SelectedTime);
        SyncTimeToText(SelectedTime);
    }

    private void OnPresenterTimeChanged(object? sender, TimeChangedEventArgs e)
    {
        if (_suppressTextPresenterEvent) return;
        SetCurrentValue(SelectedTimeProperty, e.NewTime);
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        Focus(NavigationMethod.Pointer);
        SetCurrentValue(IsDropdownOpenProperty, !IsDropdownOpen);
    }

    private void OnTextBoxPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        SetCurrentValue(IsDropdownOpenProperty, true);
    }

    private void OnTextBoxGetFocus(object? sender, GotFocusEventArgs e)
    {
        // SetCurrentValue(IsDropdownOpenProperty, true);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            SetCurrentValue(IsDropdownOpenProperty, false);
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Down)
        {
            SetCurrentValue(IsDropdownOpenProperty, true);
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Tab)
        {
            SetCurrentValue(IsDropdownOpenProperty, false);
            return;
        }

        base.OnKeyDown(e);
    }


    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(_textBox?.Text))
        {
            TimePickerPresenter.TimeProperty.SetValue(null, _presenter);
        }
        else if (DisplayFormat is null || DisplayFormat.Length == 0)
        {
            if (TimeSpan.TryParse(_textBox?.Text, out var defaultTime))
                TimePickerPresenter.TimeProperty.SetValue(defaultTime, _presenter);
        }
        else
        {
            if (DateTime.TryParseExact(_textBox?.Text, DisplayFormat, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                    out var time)) TimePickerPresenter.TimeProperty.SetValue(time.TimeOfDay, _presenter);
        }
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<TimeSpan?> args)
    {
        if (_textBox is null) return;
        _suppressTextPresenterEvent = true;
        _presenter?.SetValue(TimePickerPresenter.TimeProperty, args.NewValue.Value);
        SyncTimeToText(args.NewValue.Value);
        _suppressTextPresenterEvent = false;
    }

    private void SyncTimeToText(TimeSpan? time)
    {
        if (_textBox is null) return;
        if (time is null)
        {
            _textBox.Text = null;
            return;
        }
        var date = new DateTime(1, 1, 1, time.Value.Hours, time.Value.Minutes, time.Value.Seconds);
        var text = date.ToString(DisplayFormat);
        _textBox.Text = text;
    }

    public void Confirm()
    {
        _presenter?.Confirm();
        SetCurrentValue(IsDropdownOpenProperty, false);
        Focus();
    }

    public void Dismiss()
    {
        SetCurrentValue(IsDropdownOpenProperty, false);
        Focus();
    }

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);
        if (property == SelectedTimeProperty) DataValidationErrors.SetError(this, error);
    }
}