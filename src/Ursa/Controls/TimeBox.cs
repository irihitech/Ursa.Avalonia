using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public enum TimeBoxInputMode
{
    Normal,

    // In fast mode, automatically move to next session after 2 digits input.
    Fast,
}

public enum TimeBoxDragOrientation
{
    Horizontal,
    Vertical,
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
    private readonly TextPresenter[] _presenters = new TextPresenter[4];
    private readonly Border[] _borders = new Border[4];
    private readonly Panel[] _dragPanels = new Panel[4];
    private readonly int[] _limits = new[] { 24, 60, 60, 1000 };
    private readonly int[] _values = new[] { 0, 0, 0, 0 };
    private readonly int[] _sectionLength = new[] { 2, 2, 2, 3 };
    private readonly bool[] _isShowedCaret = new[] { false, false, false, false };
    private int? _currentActiveSectionIndex;
    private bool _isDragging;
    private Point _pressedPosition;
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
        TextBox.SelectionBrushProperty.AddOwner<TimeBox>();

    public IBrush? SelectionBrush
    {
        get => GetValue(SelectionBrushProperty);
        set => SetValue(SelectionBrushProperty, value);
    }

    public static readonly StyledProperty<IBrush?> SelectionForegroundBrushProperty =
        TextBox.SelectionForegroundBrushProperty.AddOwner<TimeBox>();

    public IBrush? SelectionForegroundBrush
    {
        get => GetValue(SelectionForegroundBrushProperty);
        set => SetValue(SelectionForegroundBrushProperty, value);
    }

    public static readonly StyledProperty<IBrush?> CaretBrushProperty = TextBox.CaretBrushProperty.AddOwner<TimeBox>();

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

    public static readonly StyledProperty<TimeBoxDragOrientation> DragOrientationProperty
        = AvaloniaProperty.Register<TimeBox, TimeBoxDragOrientation>(nameof(DragOrientation),
            defaultValue: TimeBoxDragOrientation.Horizontal);

    public TimeBoxDragOrientation DragOrientation
    {
        get => GetValue(DragOrientationProperty);
        set => SetValue(DragOrientationProperty, value);
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
        AllowDragProperty.Changed.AddClassHandler<TimeBox, bool>((o, e) => o.OnAllowDragChanged(e));
    }

    #region Overrides

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _hourText = e.NameScope.Get<TextPresenter>(PART_HoursTextPresenter);
        _minuteText = e.NameScope.Get<TextPresenter>(PART_MinuteTextPresenter);
        _secondText = e.NameScope.Get<TextPresenter>(PART_SecondTextPresenter);
        _milliSecondText = e.NameScope.Get<TextPresenter>(PART_MillisecondTextPresenter);
        _hourBorder = e.NameScope.Get<Border>(PART_HourBorder);
        _minuteBorder = e.NameScope.Get<Border>(PART_MinuteBorder);
        _secondBorder = e.NameScope.Get<Border>(PART_SecondBorder);
        _milliSecondBorder = e.NameScope.Get<Border>(PART_MilliSecondBorder);
        _hourDragPanel = e.NameScope.Get<Panel>(PART_HourDragPanel);
        _minuteDragPanel = e.NameScope.Get<Panel>(PART_MinuteDragPanel);
        _secondDragPanel = e.NameScope.Get<Panel>(PART_SecondDragPanel);
        _milliSecondDragPanel = e.NameScope.Get<Panel>(PART_MilliSecondDragPanel);

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
        IsVisibleProperty.SetValue(AllowDrag, _dragPanels[0], _dragPanels[1], _dragPanels[2], _dragPanels[3]);

        _hourText.Text = Time != null ? Time.Value.Hours.ToString() : "0";
        _minuteText.Text = Time != null ? Time.Value.Minutes.ToString() : "0";
        _secondText.Text = Time != null ? Time.Value.Seconds.ToString() : "0";
        //_milliSecondText.Text = Time != null ? ClampMilliSecond(Time.Value.Milliseconds).ToString() : "0";
        _milliSecondText.Text = Time != null ? Time.Value.Milliseconds.ToString() : "0";
        ParseTimeSpan(ShowLeadingZero);
        
        PointerMovedEvent.AddHandler(OnDragPanelPointerMoved, _dragPanels[0], _dragPanels[1], _dragPanels[2],
            _dragPanels[3]);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (_currentActiveSectionIndex is null) return;
        var keymap = TopLevel.GetTopLevel(this)?.PlatformSettings?.HotkeyConfiguration;
        bool Match(List<KeyGesture> gestures) => gestures.Any(g => g.Matches(e));
        if (keymap is not null && Match(keymap.SelectAll))
        {
            _presenters[_currentActiveSectionIndex.Value].SelectionStart = 0;
            _presenters[_currentActiveSectionIndex.Value].SelectionEnd =
                _presenters[_currentActiveSectionIndex.Value].Text?.Length ?? 0;
            return;
        }

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
        if (s is null || string.IsNullOrEmpty(s)) return;
        if (!char.IsNumber(s[0])) return;
        if (_currentActiveSectionIndex is null) return;

        int caretIndex = Math.Min(_presenters[_currentActiveSectionIndex.Value].CaretIndex
            , _presenters[_currentActiveSectionIndex.Value].Text?.Length ?? 0);

        if (_presenters[_currentActiveSectionIndex.Value].Text is null)
        {
            _presenters[_currentActiveSectionIndex.Value].Text = s;
            _presenters[_currentActiveSectionIndex.Value].MoveCaretHorizontal();
        }
        else
        {
            _presenters[_currentActiveSectionIndex.Value].DeleteSelection();
            _presenters[_currentActiveSectionIndex.Value].ClearSelection();
            string oldText = _presenters[_currentActiveSectionIndex.Value].Text ?? string.Empty;
            string newText = oldText.Length == 0
                ? s
                : oldText.Substring(0, caretIndex) + s + oldText.Substring(Math.Min(caretIndex, oldText.Length));

            // Limit the maximum number of input digits
            if (newText.Length > _sectionLength[_currentActiveSectionIndex.Value])
            {
                newText = newText.Substring(0, _sectionLength[_currentActiveSectionIndex.Value]);
            }

            _presenters[_currentActiveSectionIndex.Value].Text = newText;
            _presenters[_currentActiveSectionIndex.Value].MoveCaretHorizontal();
            if (_presenters[_currentActiveSectionIndex.Value].CaretIndex == 2 && InputMode == TimeBoxInputMode.Fast)
            {
                MoveToNextSection(_currentActiveSectionIndex.Value);
            }
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        _pressedPosition = e.GetPosition(_hourBorder);
        _lastDragPoint = _pressedPosition;
        for (int i = 0; i < 4; ++i)
        {
            if (!_borders[i].Bounds.Contains(_pressedPosition))
            {
                LeaveSection(i);
                continue;
            }
            
            _currentActiveSectionIndex = i;
            
            if (e.ClickCount == 2)
            {
                EnterSection(_currentActiveSectionIndex.Value);
                continue;
            }

            if (!_dragPanels[_currentActiveSectionIndex.Value].IsVisible)
            {
                MoveCaret(_currentActiveSectionIndex.Value);
            }
        }

        if (_currentActiveSectionIndex is not null)
        {
            e.Pointer.Capture(_presenters[_currentActiveSectionIndex.Value]);
        }
        e.Handled = true;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (_currentActiveSectionIndex is null) return;
        if (_isDragging)
        {
            _isDragging = false;
            _lastDragPoint = null;
            return;
        }
        if(_dragPanels[_currentActiveSectionIndex.Value].IsVisible)
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

    #endregion

    private void OnFormatChange(AvaloniaPropertyChangedEventArgs arg)
    {
        // this function will be call ahead of OnApplyTemplate() if Set ShowLeadingZero in axaml, so that _xxxText could be null
        if (_hourText is null || _minuteText is null || _secondText is null || _milliSecondText is null)
            return;

        bool showLeadingZero = arg.GetNewValue<bool>();
        ParseTimeSpan(showLeadingZero);
    }

    private void OnAllowDragChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        IsVisibleProperty.SetValue(args.NewValue.Value, _dragPanels[0], _dragPanels[1], _dragPanels[2], _dragPanels[3]);
    }

    private void OnTimeChanged(AvaloniaPropertyChangedEventArgs arg)
    {
        // this function will be call ahead of OnApplyTemplate() if bind Time in axaml, so that _xxxText could be null
        if (_hourText is null || _minuteText is null || _secondText is null || _milliSecondText is null)
            return;

        TimeSpan? timeSpan = arg.GetNewValue<TimeSpan?>();
        if (timeSpan is null)
        {
            _hourText.Text = String.Empty;
            _minuteText.Text = String.Empty;
            _secondText.Text = String.Empty;
            _milliSecondText.Text = String.Empty;
            ParseTimeSpan(ShowLeadingZero);
        }
        else
        {
            _hourText.Text = timeSpan.Value.Hours.ToString();
            _minuteText.Text = timeSpan.Value.Minutes.ToString();
            _secondText.Text = timeSpan.Value.Seconds.ToString();
            //_milliSecondText.Text = ClampMilliSecond(timeSpan.Value.Milliseconds).ToString();
            _milliSecondText.Text = timeSpan.Value.Milliseconds.ToString();
            ParseTimeSpan(ShowLeadingZero);
        }
    }

    private void ParseTimeSpan(bool showLeadingZero, bool skipParseFromText = false)
    {
        string format = showLeadingZero ? "D2" : "";
        string millisecondFormat = showLeadingZero ? "D3" : "";

        if (!skipParseFromText)
        {
            _values[0] = int.TryParse(_hourText?.Text, out int hour) ? hour : 0;
            _values[1] = int.TryParse(_minuteText?.Text, out int minute) ? minute : 0;
            _values[2] = int.TryParse(_secondText?.Text, out int second) ? second : 0;
            _values[3] = int.TryParse(_milliSecondText?.Text, out int millisecond) ? millisecond : 0;
        }

        VerifyTimeValue();

        _hourText?.SetValue(TextPresenter.TextProperty, _values[0].ToString(format));
        _minuteText?.SetValue(TextPresenter.TextProperty, _values[1].ToString(format));
        _secondText?.SetValue(TextPresenter.TextProperty, _values[2].ToString(format));
        _milliSecondText?.SetValue(TextPresenter.TextProperty, _values[3].ToString(millisecondFormat));
    }

    private void OnDragPanelPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!AllowDrag) return;
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        var point = e.GetPosition(this);
        var delta = point - _lastDragPoint;
        if (delta is null) return;
        int d = GetDelta(delta.Value);
        if (d > 0)
        {
            Increase();
            _isDragging = true;
        }
        else if (d < 0)
        {
            Decrease();
            _isDragging = true;
        }

        _lastDragPoint = point;
    }

    private int GetDelta(Point point)
    {
        switch (DragOrientation)
        {
            case TimeBoxDragOrientation.Horizontal:
                return point.X switch
                {
                    > 0 => 1,
                    < 0 => -1,
                    _ => 0
                };
            case TimeBoxDragOrientation.Vertical:
                return point.Y switch
                {
                    > 0 => -1,
                    < 0 => 1,
                    _ => 0
                };
        }

        return 0;
    }

    /// <summary>
    /// Set dragPanel IsVisible to false if AllowDrag is true, and select all text in the section
    /// </summary>
    /// <param name="index">The index of section that will be entered</param>
    private void EnterSection(int index)
    {
        if (index < 0 || index > 3) return;

        if (AllowDrag)
            _dragPanels[index].IsVisible = false;
        ShowCaretInternal(index);
        _presenters[index].SelectAll();
    }

    private void MoveCaret(int index)
    {
        if (!_isShowedCaret[index])
        {
            ShowCaretInternal(index);
        }
        _presenters[index].ClearSelection();
        var caretPosition =
            _pressedPosition.WithX(_pressedPosition.X - _borders[index].Bounds.X);
        _presenters[index].MoveCaretToPoint(caretPosition);
    }
    
    /// <summary>
    /// Set dragPanel IsVisible to true if AllowDrag is true, and clear selection in the section
    /// </summary>
    /// <param name="index">The index of section that will be leave</param>
    private void LeaveSection(int index)
    {
        if (index < 0 || index > 3) return;
        _presenters[index].ClearSelection();
        if (_isShowedCaret[index])
        {
           HideCaretInternal(index);
        }

        if (AllowDrag)
            _dragPanels[index].IsVisible = true;
    }

    private bool MoveToNextSection(int index)
    {
        if (index < 0 || index >= 3) return false;
        LeaveSection(index);
        _currentActiveSectionIndex = index + 1;
        EnterSection(_currentActiveSectionIndex.Value);
        return true;
    }

    private bool MoveToPreviousSection(int index)
    {
        if (index <= 0 || index > 3) return false;
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
            //Time = new TimeSpan(0, _values[0], _values[1], _values[2], _values[3] * 10);
            Time = new TimeSpan(0, _values[0], _values[1], _values[2], _values[3]);
        }
        catch
        {
            Time = TimeSpan.Zero;
        }
    }

    private void DeleteImplementation(int index)
    {
        if (index < 0 || index > 3) return;
        var oldText = _presenters[index].Text??string.Empty;
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
            string newText = oldText.Substring(0, caretIndex - 1) +
                             oldText.Substring(Math.Min(caretIndex, oldText.Length));
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
        bool success;
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
        if (_currentActiveSectionIndex is null) return;
        if (_currentActiveSectionIndex.Value == 0)
            _values[0] += 1;
        else if (_currentActiveSectionIndex.Value == 1)
            _values[1] += 1;
        else if (_currentActiveSectionIndex.Value == 2)
            _values[2] += 1;
        else if (_currentActiveSectionIndex.Value == 3)
            _values[3] += 1;
        ParseTimeSpan(ShowLeadingZero, true);
        //SetTimeSpanInternal();
    }

    private void Decrease()
    {
        if (_currentActiveSectionIndex is null) return;
        if (_currentActiveSectionIndex.Value == 0)
            _values[0] -= 1;
        else if (_currentActiveSectionIndex.Value == 1)
            _values[1] -= 1;
        else if (_currentActiveSectionIndex.Value == 2)
            _values[2] -= 1;
        else if (_currentActiveSectionIndex.Value == 3)
            _values[3] -= 1;
        ParseTimeSpan(ShowLeadingZero, true);
        //SetTimeSpanInternal();
    }

    private int ClampMilliSecond(int milliSecond)
    {
        while (milliSecond % 100 != milliSecond)
        {
            milliSecond /= 10;
        }

        return milliSecond;
    }

    private void ShowCaretInternal(int index)
    {
        _presenters[index].ShowCaret();
        _isShowedCaret[index] = true;
    }
    
    private void HideCaretInternal(int index)
    {
        _presenters[index].HideCaret();
        _isShowedCaret[index] = false;
    }
}