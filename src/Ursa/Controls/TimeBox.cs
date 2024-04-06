using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using Avalonia.Metadata;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public enum TimeBoxInputMode
{
    Normal,

    // In fast mode, automatically move to next session after 2 digits input.
    Fast,
}

[TemplatePart(PART_HoursTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_MinuteTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_SecondTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_MillisecondTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_HourBorder, typeof(Border))]
[TemplatePart(PART_MinuteBorder, typeof(Border))]
[TemplatePart(PART_SecondBorder, typeof(Border))]
[TemplatePart(PART_MilliSecondBorder, typeof(Border))]
[TemplatePart(PART_HourDragPanel, typeof(Panel))]
[TemplatePart(PART_MinuteDragPanel, typeof(Panel))]
[TemplatePart(PART_SecondDragPanel, typeof(Panel))]
[TemplatePart(PART_MilliSecondDragPanel, typeof(Panel))]
public class TimeBox : TemplatedControl
{
    public const string PART_HoursTextPresenter = "PART_HourTextPresenter";
    public const string PART_MinuteTextPresenter = "PART_MinuteTextPresenter";
    public const string PART_SecondTextPresenter = "PART_SecondTextPresenter";
    public const string PART_MillisecondTextPresenter = "PART_MillisecondTextPresenter";
    public const string PART_HourBorder = "PART_HourBorder";
    public const string PART_MinuteBorder = "PART_MinuteBorder";
    public const string PART_SecondBorder = "PART_SecondBorder";
    public const string PART_MilliSecondBorder = "PART_MilliSecondBorder";
    public const string PART_HourDragPanel = "PART_HourDragPanel";
    public const string PART_MinuteDragPanel = "PART_MinuteDragPanel";
    public const string PART_SecondDragPanel = "PART_SecondDragPanel";
    public const string PART_MilliSecondDragPanel = "PART_MilliSecondDragPanel";
    private TextPresenter? _hourText;
    private TextPresenter? _minuteText;
    private TextPresenter? _secondText;
    private TextPresenter? _milliSecondText;
    private Border? _hourBorder;
    private Border? _minuteBorder;
    private Border? _secondBorder;
    private Border? _milliSecondBorder;
    private Panel? _hourDragPanel;
    private Panel? _minuteDragPanel;
    private Panel? _secondDragPanel;
    private Panel? _milliSecondDragPanel;
    private readonly TextPresenter?[] _presenters = new TextPresenter?[4];
    private readonly Border?[] _borders = new Border?[4];
    private readonly Panel?[] _dragPanels = new Panel?[4];
    private readonly int[] _limits = new[] { 24, 60, 60, 100 };
    private int[] _values = new int[4];
    private bool[] _isShowedCaret = new bool[4];
    private int? _currentActiveSectionIndex;
    private bool _isAlreadyDrag = false;
    private Point _pressedPosition = new Point();
    private Point? _lastDragPoint;

    public static readonly StyledProperty<TimeSpan?> TimeProperty = AvaloniaProperty.Register<TimeBox, TimeSpan?>(
        nameof(Time), defaultBindingMode: BindingMode.TwoWay);

    public TimeSpan? Time
    {
        get => GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public static readonly StyledProperty<TextAlignment> TextAlignmentProperty =
        TextBox.TextAlignmentProperty.AddOwner<TimeBox>();

    public TextAlignment TextAlignment
    {
        get => GetValue(TextAlignmentProperty);
        set => SetValue(TextAlignmentProperty, value);
    }

    public static readonly StyledProperty<IBrush?> SelectionBrushProperty =
        TextBox.SelectionBrushProperty.AddOwner<IPv4Box>();

    public IBrush? SelectionBrush
    {
        get => GetValue(SelectionBrushProperty);
        set => SetValue(SelectionBrushProperty, value);
    }

    public static readonly StyledProperty<IBrush?> SelectionForegroundBrushProperty =
        TextBox.SelectionForegroundBrushProperty.AddOwner<IPv4Box>();

    public IBrush? SelectionForegroundBrush
    {
        get => GetValue(SelectionForegroundBrushProperty);
        set => SetValue(SelectionForegroundBrushProperty, value);
    }

    public static readonly StyledProperty<IBrush?> CaretBrushProperty = TextBox.CaretBrushProperty.AddOwner<IPv4Box>();

    public IBrush? CaretBrush
    {
        get => GetValue(CaretBrushProperty);
        set => SetValue(CaretBrushProperty, value);
    }

    public static readonly StyledProperty<bool> ShowLeadingZeroProperty = AvaloniaProperty.Register<TimeBox, bool>(
        nameof(ShowLeadingZero));

    public bool ShowLeadingZero
    {
        get => GetValue(ShowLeadingZeroProperty);
        set => SetValue(ShowLeadingZeroProperty, value);
    }

    public static readonly StyledProperty<TimeBoxInputMode> InputModeProperty =
        AvaloniaProperty.Register<TimeBox, TimeBoxInputMode>(
            nameof(InputMode));

    public TimeBoxInputMode InputMode
    {
        get => GetValue(InputModeProperty);
        set => SetValue(InputModeProperty, value);
    }

    public static readonly StyledProperty<bool> AllowDragProperty = AvaloniaProperty.Register<TimeBox, bool>(
        nameof(AllowDrag), defaultBindingMode: BindingMode.TwoWay);

    public bool AllowDrag
    {
        get => GetValue(AllowDragProperty);
        set => SetValue(AllowDragProperty, value);
    }

    public static readonly StyledProperty<bool> IsReadOnlyProperty = AvaloniaProperty.Register<TimeBox, bool>(
        nameof(IsReadOnly), defaultValue: false, defaultBindingMode: BindingMode.TwoWay);

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly StyledProperty<bool> IsTimeLoopProperty = AvaloniaProperty.Register<TimeBox, bool>(
        nameof(IsTimeLoop), defaultBindingMode: BindingMode.TwoWay);

    public bool IsTimeLoop
    {
        get => GetValue(IsTimeLoopProperty);
        set => SetValue(IsTimeLoopProperty, value);
    }
    
    static TimeBox()
    {
        ShowLeadingZeroProperty.Changed.AddClassHandler<TimeBox>((o, e) => o.OnFormatChange(e));
        TimeProperty.Changed.AddClassHandler<TimeBox>((o, e) => o.OnTimeChanged(e));
        AllowDragProperty.Changed.AddClassHandler<TimeBox, bool>((o, e) => o.OnAllowDragChange(e));
    }

    private void OnAllowDragChange(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        IsVisibleProperty.SetValue(args.NewValue.Value, _dragPanels);
    }

    #region Overrides

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _hourText = e.NameScope.Find<TextPresenter>(PART_HoursTextPresenter);
        _minuteText = e.NameScope.Find<TextPresenter>(PART_MinuteTextPresenter);
        _secondText = e.NameScope.Find<TextPresenter>(PART_SecondTextPresenter);
        _milliSecondText = e.NameScope.Find<TextPresenter>(PART_MillisecondTextPresenter);
        _hourBorder = e.NameScope.Find<Border>(PART_HourBorder);
        _minuteBorder = e.NameScope.Find<Border>(PART_MinuteBorder);
        _secondBorder = e.NameScope.Find<Border>(PART_SecondBorder);
        _milliSecondBorder = e.NameScope.Find<Border>(PART_MilliSecondBorder);
        _hourDragPanel = e.NameScope.Find<Panel>(PART_HourDragPanel);
        _minuteDragPanel = e.NameScope.Find<Panel>(PART_MinuteDragPanel);
        _secondDragPanel = e.NameScope.Find<Panel>(PART_SecondDragPanel);
        _milliSecondDragPanel = e.NameScope.Find<Panel>(PART_MilliSecondDragPanel);
        _presenters[0] = _hourText;
        _presenters[1] = _minuteText;
        _presenters[2] = _secondText;
        _presenters[3] = _milliSecondText;
        _borders[0] = _hourBorder;
        _borders[1] = _minuteBorder;
        _borders[2] = _secondBorder;
        _borders[3] = _milliSecondBorder;
        _dragPanels[0] = _hourDragPanel;
        _dragPanels[1] = _minuteDragPanel;
        _dragPanels[2] = _secondDragPanel;
        _dragPanels[3] = _milliSecondDragPanel;
        IsVisibleProperty.SetValue(AllowDrag, _dragPanels);


        if (_hourText != null) _hourText.Text = Time != null ? Time.Value.Hours.ToString() : "0";
        if (_minuteText != null) _minuteText.Text = Time != null ? Time.Value.Minutes.ToString() : "0";
        if (_secondText != null) _secondText.Text = Time != null ? Time.Value.Seconds.ToString() : "0";
        if (_milliSecondText != null)
            _milliSecondText.Text = Time != null ? ClampMilliSecond(Time.Value.Milliseconds).ToString() : "0";
        ParseTimeSpan(ShowLeadingZero);

        PointerMovedEvent.AddHandler(OnDragPanelPointerMoved, _dragPanels);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (_currentActiveSectionIndex is null) return;
        var keymap = TopLevel.GetTopLevel(this)?.PlatformSettings?.HotkeyConfiguration;
        bool Match(List<KeyGesture> gestures) => gestures.Any(g => g.Matches(e));
        if (e.Key is Key.Enter or Key.Return)
        {
            ParseTimeSpan(ShowLeadingZero);
            SetTimeSpanInternal();
            base.OnKeyDown(e);
            return;
        }

        if (e.Key == Key.Tab)
        {
            if (_currentActiveSectionIndex.Value == 3)
            {
                base.OnKeyDown(e);
                return;
            }

            MoveToNextSection(_currentActiveSectionIndex.Value);
            e.Handled = true;
        }
        else if (e.Key == Key.Back)
        {
            DeleteImplementation(_currentActiveSectionIndex.Value);
        }
        else if (e.Key == Key.Right)
        {
            OnPressRightKey();
        }
        else if (e.Key == Key.Left)
        {
            OnPressLeftKey();
        }
        else
        {
            base.OnKeyDown(e);
        }
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        if (e.Handled) return;
        string? s = e.Text;
        if (string.IsNullOrEmpty(s)) return;
        if (!char.IsNumber(s![0])) return;
        if (_currentActiveSectionIndex.HasValue && _presenters[_currentActiveSectionIndex.Value] != null)
        {
            int caretIndex = Math.Min(_presenters[_currentActiveSectionIndex.Value].CaretIndex
                                        , _presenters[_currentActiveSectionIndex.Value].Text.Length);
            string? oldText = _presenters[_currentActiveSectionIndex.Value].Text;
            if (oldText is null)
            {
                _presenters[_currentActiveSectionIndex.Value].Text = s;
                _presenters[_currentActiveSectionIndex.Value].MoveCaretHorizontal();
            }
            else
            {
                _presenters[_currentActiveSectionIndex.Value].DeleteSelection();
                _presenters[_currentActiveSectionIndex.Value].ClearSelection();
                oldText = _presenters[_currentActiveSectionIndex.Value].Text;

                string newText = string.IsNullOrEmpty(oldText)
                    ? s
                    : oldText?.Substring(0, caretIndex) + s + oldText?.Substring(Math.Min(caretIndex, oldText.Length));
                if (newText.Length > 2)
                {
                    newText = newText.Substring(0, 2);
                }

                _presenters[_currentActiveSectionIndex.Value].Text = newText;
                Console.WriteLine(
                    $"OnTextInput @ _secondText HashCode: {_presenters[_currentActiveSectionIndex.Value]?.GetHashCode()}");
                _presenters[_currentActiveSectionIndex.Value].MoveCaretHorizontal();
                if (_presenters[_currentActiveSectionIndex.Value].CaretIndex == 2 && InputMode == TimeBoxInputMode.Fast)
                {
                    MoveToNextSection(_currentActiveSectionIndex.Value);
                }
            }
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        _pressedPosition = e.GetPosition(_hourBorder);
        _lastDragPoint = _pressedPosition;
        for (int i = 0; i < 4; ++i)
        {
            if (_borders[i]?.Bounds.Contains(_pressedPosition) ?? false)
            {
                _currentActiveSectionIndex = i;
            }
            else
            {
                LeaveSection(i);
            }
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (_currentActiveSectionIndex is null) return;
        if (_isAlreadyDrag)
        {
            _isAlreadyDrag = false;
        }
        else
        {
            EnterSection(_currentActiveSectionIndex.Value);
        }
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        for (int i = 0; i < 4; ++i)
        {
            LeaveSection(i);
        }

        _currentActiveSectionIndex = null;
        ParseTimeSpan(ShowLeadingZero);
        SetTimeSpanInternal();
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
    }

    #endregion

    private void OnFormatChange(AvaloniaPropertyChangedEventArgs arg)
    {
        bool showLeadingZero = arg.GetNewValue<bool>();
        ParseTimeSpan(showLeadingZero);
    }

    private void OnTimeChanged(AvaloniaPropertyChangedEventArgs arg)
    {
        TimeSpan? timeSpan = arg.GetNewValue<TimeSpan?>();
        if (timeSpan is null)
        {
            if (_hourText != null) _hourText.Text = String.Empty;
            if (_minuteText != null) _minuteText.Text = String.Empty;
            if (_secondText != null) _secondText.Text = String.Empty;
            if (_milliSecondText != null) _milliSecondText.Text = String.Empty;
            ParseTimeSpan(ShowLeadingZero);
        }
        else
        {
            if (_hourText != null) _hourText.Text = timeSpan.Value.Hours.ToString();
            if (_minuteText != null) _minuteText.Text = timeSpan.Value.Minutes.ToString();
            if (_secondText != null) _secondText.Text = timeSpan.Value.Seconds.ToString();
            if (_milliSecondText != null) _milliSecondText.Text = (timeSpan.Value.Milliseconds / 10).ToString();
            ParseTimeSpan(ShowLeadingZero);
        }
    }

    private void ParseTimeSpan(bool showLeadingZero, bool skipParseFromText = false)
    {
        string format = showLeadingZero ? "D2" : "";
        Console.WriteLine($"ParseTimeSpan @ _secondText HashCode: {_secondText?.GetHashCode()}");
        if (_hourText is null || _minuteText is null || _secondText is null || _milliSecondText is null)
        {
            _values[0] = 0;
            _values[1] = 0;
            _values[2] = 0;
            _values[3] = 0;
            return;
        }

        if (!skipParseFromText)
        {
            _values[0] = int.TryParse(_hourText.Text, out int hour) ? hour : 0;
            _values[1] = int.TryParse(_minuteText.Text, out int minute) ? minute : 0;
            _values[2] = int.TryParse(_secondText.Text, out int second) ? second : 0;
            _values[3] = int.TryParse(_milliSecondText.Text, out int millisecond) ? millisecond : 0;
        }

        VerifyTimeValue();

        _hourText.Text = _values[0].ToString(format);
        _minuteText.Text = _values[1].ToString(format);
        _secondText.Text = _values[2].ToString(format);
        _milliSecondText.Text = _values[3].ToString(format);
    }
    private void OnDragPanelPointerMoved(object sender, PointerEventArgs e)
    {
        if (!AllowDrag || IsReadOnly) return;
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        var point = e.GetPosition(this);
        var delta = point - _lastDragPoint;
        if (delta is null)
        {
            return;
        }

        int d = GetDelta(delta.Value);
        if (d > 0)
        {
            Increase();
            _isAlreadyDrag = true;
        }
        else if (d < 0)
        {
            Decrease();
            _isAlreadyDrag = true;
        }

        _lastDragPoint = point;
    }

    private int GetDelta(Point point)
    {
        return point.X switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => 0
        };
    }

    private void EnterSection(int index)
    {
        if (!_isShowedCaret[index])
        {
            if (AllowDrag && _dragPanels[index] != null)
                _dragPanels[index].IsVisible = false;
            _presenters[index].ShowCaret();
            _isShowedCaret[index] = true;
            _presenters[index].SelectAll();
        }
        else
        {
            _presenters[index].ClearSelection();
            var caretPosition =
                _pressedPosition.WithX(_pressedPosition.X - _borders[index].Bounds.X);
            _presenters[index].MoveCaretToPoint(caretPosition);
        }
    }

    private void LeaveSection(int index)
    {
        if (_presenters[index] is null) return;
        _presenters[index].ClearSelection();
        if (_isShowedCaret[index])
        {
            _presenters[index].HideCaret();
            _isShowedCaret[index] = false;
        }

        if (AllowDrag && _dragPanels[index] != null)
            _dragPanels[index].IsVisible = true;
    }

    private bool MoveToNextSection(int index)
    {
        if (_presenters[index] is null) return false;
        if (index == 3) return false;
        LeaveSection(index);
        _currentActiveSectionIndex = index + 1;
        EnterSection(_currentActiveSectionIndex.Value);
        return true;
    }

    private bool MoveToPreviousSection(int index)
    {
        if (_presenters[index] is null) return false;
        if (index == 0) return false;
        LeaveSection(index);
        _currentActiveSectionIndex = index - 1;
        EnterSection(_currentActiveSectionIndex.Value);
        return true;
    }

    private void OnPressRightKey()
    {
        if (_currentActiveSectionIndex is null) return;
        var index = _currentActiveSectionIndex.Value;
        if (_presenters[index].IsTextSelected())
        {
            int end = _presenters[index].SelectionEnd;
            _presenters[index].ClearSelection();
            _presenters[index].MoveCaretToTextPosition(end);
            return;
        }

        if (_presenters[index].CaretIndex >= _presenters[index].Text?.Length)
        {
            MoveToNextSection(index);
        }
        else
        {
            _presenters[index].ClearSelection();
            _presenters[index].CaretIndex++;
        }
    }

    private void OnPressLeftKey()
    {
        if (_currentActiveSectionIndex is null) return;
        var index = _currentActiveSectionIndex.Value;
        if (_presenters[index].IsTextSelected())
        {
            int start = _presenters[index].SelectionStart;
            _presenters[index].ClearSelection();
            _presenters[index].MoveCaretToTextPosition(start);
            return;
        }

        if (_presenters[index].CaretIndex == 0)
        {
            MoveToPreviousSection(index);
        }
        else
        {
            _presenters[index].ClearSelection();
            _presenters[index].CaretIndex--;
        }
    }

    private void SetTimeSpanInternal()
    {
        try
        {
            Time = new TimeSpan(0, _values[0], _values[1], _values[2], _values[3] * 10);
        }
        catch
        {
            Time = TimeSpan.Zero;
        }
    }

    private void DeleteImplementation(int index)
    {
        if (_presenters[index] is null) return;
        var oldText = _presenters[index].Text;
        if (_presenters[index].SelectionStart != _presenters[index].SelectionEnd)
        {
            _presenters[index].DeleteSelection();
            _presenters[index].ClearSelection();
        }
        else if (string.IsNullOrWhiteSpace(oldText) || _presenters[index].CaretIndex == 0)
        {
            MoveToPreviousSection(index);
        }
        else
        {
            int caretIndex = _presenters[index].CaretIndex;
            string newText = oldText?.Substring(0, caretIndex - 1) +
                             oldText?.Substring(Math.Min(caretIndex, oldText.Length));
            _presenters[index].MoveCaretHorizontal(LogicalDirection.Backward);
            _presenters[index].Text = newText;
        }
    }
    
    private bool HandlingCarry(int index, int lowerCarry = 0)
    {
        if (index < 0)
            return IsTimeLoop;
        _values[index] += lowerCarry;
        int carry = _values[index] >= 0 ? _values[index] / _limits[index] : -1 + (_values[index] / _limits[index]);
        if (carry == 0) return true;
        bool success = false;
        if (carry > 0)
        {
            success = HandlingCarry(index - 1, carry);
            if (success)
            {
                _values[index] %= _limits[index];
            }
            else
            {
                _values[index] = _limits[index] - 1;
            }
        }
        else
        {
            success = HandlingCarry(index - 1, carry);
            if (success)
            {
                _values[index] += _limits[index];
            }
            else
            {
                _values[index] = 0;
            }
        }
        return success;
    }
    
    private void VerifyTimeValue()
    {
        for (int i = 3; i >= 0; --i)
        {
            HandlingCarry(i);
        }
    }
    
    private void Increase()
    {
        if(_currentActiveSectionIndex is null)return;
        if(_currentActiveSectionIndex.Value == 0)
            _values[0] += 1;
        else if(_currentActiveSectionIndex.Value == 1)
            _values[1] += 1;
        else if(_currentActiveSectionIndex.Value == 2)
            _values[2] += 1;
        else if(_currentActiveSectionIndex.Value == 3)
            _values[3] += 1;
        ParseTimeSpan(ShowLeadingZero, true);
        SetTimeSpanInternal();
    }
    
    private void Decrease()
    {
        if(_currentActiveSectionIndex is null)return;
        if(_currentActiveSectionIndex.Value == 0)
            _values[0] -= 1;
        else if(_currentActiveSectionIndex.Value == 1)
            _values[1] -= 1;
        else if(_currentActiveSectionIndex.Value == 2)
            _values[2] -= 1;
        else if(_currentActiveSectionIndex.Value == 3)
            _values[3] -= 1;
        ParseTimeSpan(ShowLeadingZero, true);
        SetTimeSpanInternal();
    }
    
    private int ClampMilliSecond(int milliSecond)
    {
        while (milliSecond % 100 != milliSecond)
        {
            milliSecond /= 10;
        }

        return milliSecond;
    }
}