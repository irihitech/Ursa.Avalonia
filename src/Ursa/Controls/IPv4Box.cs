using System.Diagnostics;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.Media.TextFormatting;

namespace Ursa.Controls;

[TemplatePart(PART_FirstTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_SecondTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_ThirdTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_FourthTextPresenter, typeof(TextPresenter))]
public class IPv4Box: TemplatedControl
{
    public const string PART_FirstTextPresenter = "PART_FirstTextPresenter";
    public const string PART_SecondTextPresenter = "PART_SecondTextPresenter";
    public const string PART_ThirdTextPresenter = "PART_ThirdTextPresenter";
    public const string PART_FourthTextPresenter = "PART_FourthTextPresenter";
    private TextPresenter? _firstText;
    private TextPresenter? _secondText;
    private TextPresenter? _thirdText;
    private TextPresenter? _fourthText;
    private byte _firstByte;
    private byte _secondByte;
    private byte _thirdByte;
    private byte _fourthByte;
    private readonly TextPresenter?[] _presenters = new TextPresenter?[4];
    private byte[] _bytes = new byte[4];
    private TextPresenter? _currentActivePresenter;

    public static readonly StyledProperty<IPAddress?> IPAddressProperty = AvaloniaProperty.Register<IPv4Box, IPAddress?>(
        nameof(IPAddress));

    public IPAddress? IPAddress
    {
        get => GetValue(IPAddressProperty);
        set => SetValue(IPAddressProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _firstText = e.NameScope.Find<TextPresenter>(PART_FirstTextPresenter);
        _secondText = e.NameScope.Find<TextPresenter>(PART_SecondTextPresenter);
        _thirdText = e.NameScope.Find<TextPresenter>(PART_ThirdTextPresenter);
        _fourthText = e.NameScope.Find<TextPresenter>(PART_FourthTextPresenter);
        _presenters[0] = _firstText;
        _presenters[1] = _secondText;
        _presenters[2] = _thirdText;
        _presenters[3] = _fourthText;
    }
    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (_currentActivePresenter is null) return;
        var keymap = AvaloniaLocator.Current.GetRequiredService<PlatformHotkeyConfiguration>();
        bool Match(List<KeyGesture> gestures) => gestures.Any(g => g.Matches(e));
        if (e.Key == Key.Enter)
        {
            ParseBytes();
            SetIPAddress();
            return;
        }
        if (Match(keymap.SelectAll))
        {
            _currentActivePresenter.SelectionStart = 0;
            _currentActivePresenter.SelectionEnd = _currentActivePresenter.Text?.Length ?? 0;
            return;
        }
        if (e.Key == Key.Tab)
        {
            _currentActivePresenter?.HideCaret();
            ClearSelection(_currentActivePresenter);
            if (Equals(_currentActivePresenter, _fourthText))
            {
                base.OnKeyDown(e);
                return;
            }
            MoveToNextPresenter(_currentActivePresenter, true);
            _currentActivePresenter?.ShowCaret();
            SetIPAddress();
            e.Handled = true;
        }
        else if (e.Key == Key.Back)
        {
            DeleteImplementation(_currentActivePresenter);
        }
        else if (e.Key == Key.Right )
        {
            if (_currentActivePresenter != null)
            {
                if (_currentActivePresenter.CaretIndex >= _currentActivePresenter.Text?.Length)
                {
                    _currentActivePresenter.HideCaret();
                    MoveToNextPresenter(_currentActivePresenter, false);
                    _currentActivePresenter.SelectionStart = 0;
                    _currentActivePresenter.SelectionEnd = 0;
                    _currentActivePresenter?.ShowCaret();
                }
                else
                {
                    _currentActivePresenter.CaretIndex++;
                }
            }
        }
        else if (e.Key == Key.Left)
        {
            if (_currentActivePresenter != null)
            {
                if (_currentActivePresenter.CaretIndex == 0)
                {
                    _currentActivePresenter.HideCaret();
                    bool success = MoveToPreviousTextPresenter(_currentActivePresenter);
                    _currentActivePresenter.ShowCaret();
                    if (success)
                    {
                        _currentActivePresenter.CaretIndex = _currentActivePresenter.Text?.Length ?? 0;
                    }
                    
                }
                else
                {
                    _currentActivePresenter.CaretIndex--;
                }
            }
        }
        else
        {
            base.OnKeyDown(e);
        }
        
    }

    private void SelectAll(TextPresenter? presenter)
    {
        if(presenter is null) return;
        presenter.SelectionStart = 0;
        presenter.SelectionEnd = presenter.Text?.Length+1??0;
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        if (e.Handled) return;
        string? s = e.Text;
        if (string.IsNullOrEmpty(s)) return;
        if (!char.IsNumber(s[0])) return;
        if (_currentActivePresenter != null)
        {
            int index = _currentActivePresenter.CaretIndex;
            string? oldText = _currentActivePresenter.Text;
            if (oldText is null)
            {
                _currentActivePresenter.Text = s;
                _currentActivePresenter.MoveCaretHorizontal();
            }
            else
            {
                DeleteSelection(_currentActivePresenter);
                ClearSelection(_currentActivePresenter);
                oldText = _currentActivePresenter.Text;

                string? newText = string.IsNullOrEmpty(oldText)
                    ? s
                    : (oldText?.Substring(0, index) + s + oldText?.Substring(index));
                if (newText.Length > 3)
                {
                    newText = newText.Substring(0, 3);
                }
                _currentActivePresenter.Text = newText;
                _currentActivePresenter.MoveCaretHorizontal();
                if (_currentActivePresenter.CaretIndex == 3)
                {
                    _currentActivePresenter.HideCaret();
                    bool success = MoveToNextPresenter(_currentActivePresenter, true);
                    _currentActivePresenter.ShowCaret();
                    if (success)
                    {
                        SelectAll(_currentActivePresenter);
                        _currentActivePresenter.CaretIndex = 0;
                    }
                }
            }
        }
    }

    private void DeleteSelection(TextPresenter? presenter)
    {
        if (presenter is null) return;
        int selectionStart = presenter.SelectionStart;
        int selectionEnd = presenter.SelectionEnd;
        if (selectionStart != selectionEnd)
        {
            var start = Math.Min(selectionStart, selectionEnd);
            var end = Math.Max(selectionStart, selectionEnd);
            var text = presenter.Text;

            string newText = text is null
                ? string.Empty
                : text.Substring(0, start) + text.Substring(Math.Min(end, text.Length));
            presenter.Text = newText;
            presenter.MoveCaretToTextPosition(start);
        }
    }

    private void ClearSelection(TextPresenter? presenter)
    {
        if(presenter is null) return;
        presenter.SelectionStart = 0;
        presenter.SelectionEnd = 0;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        var source = e.Source;
        PointerPoint? clickInfo = e.GetCurrentPoint(this);
        Point position = e.GetPosition(_firstText);
        foreach (var presenter in _presenters)
        {
            if (presenter?.Bounds.Contains(position)??false)
            {
                presenter?.ShowCaret();
                _currentActivePresenter = presenter;
                var caretPosition = position.WithX(position.X - presenter.Bounds.X);
                presenter?.MoveCaretToPoint(caretPosition);
            }
            else
            {
                presenter?.HideCaret();
                ClearSelection(presenter);
            }
        }
        Debug.WriteLine(_currentActivePresenter?.Name);
        base.OnPointerPressed(e);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        foreach (var pre in _presenters)
        {
            pre?.HideCaret();
            ClearSelection(pre);
        }
        _currentActivePresenter = null;
        ParseBytes();
        SetIPAddress();
    }

    private void ParseBytes()
    {
        _firstByte = byte.TryParse(_firstText?.Text, out byte b1) ? b1 : (byte)0;
        _secondByte = byte.TryParse(_secondText?.Text, out byte b2) ? b2 : (byte)0;
        _thirdByte = byte.TryParse(_thirdText?.Text, out byte b3) ? b3 : (byte)0;
        _fourthByte = byte.TryParse(_fourthText?.Text, out byte b4) ? b4 : (byte)0;
        if (_firstText != null) _firstText.Text = _firstByte.ToString();
        if (_secondText != null) _secondText.Text = _secondByte.ToString();
        if (_thirdText != null) _thirdText.Text = _thirdByte.ToString();
        if (_fourthText != null) _fourthText.Text = _fourthByte.ToString();
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        _currentActivePresenter = _firstText;
        _currentActivePresenter?.ShowCaret();
        base.OnGotFocus(e);
    }

    private bool MoveToNextPresenter(TextPresenter? presenter, bool selectAllAfterMove)
    {
        if (presenter is null) return false;
        if (Equals(presenter, _fourthText)) return false;
        if (Equals(presenter, _firstText)) _currentActivePresenter = _secondText;
        else if (Equals(presenter, _secondText)) _currentActivePresenter = _thirdText;
        else if (Equals(presenter, _thirdText)) _currentActivePresenter = _fourthText;
        if(selectAllAfterMove) SelectAll(_currentActivePresenter);
        return true;
    }

    private bool MoveToPreviousTextPresenter(TextPresenter? presenter)
    {
        if (presenter is null) return false;
        if (Equals(presenter, _firstText)) return false;
        if (Equals(presenter, _fourthText)) _currentActivePresenter = _thirdText;
        else if (Equals(presenter, _thirdText)) _currentActivePresenter = _secondText;
        else if (Equals(presenter, _secondText)) _currentActivePresenter = _firstText;
        return true;
    }

    public void Clear()
    {
        foreach (var presenter in _presenters)
        {
            if (presenter != null) presenter.Text = null;
        }
        IPAddress = null;
    }

    private void SetIPAddress()
    {
        long address = 0;
        address += _firstByte;
        address += _secondByte << 8;
        address += _thirdByte << 16;
        address += (long)_fourthByte << 24;
        try
        {
            IPAddress = new IPAddress(address);
        }
        catch
        {
            IPAddress = null;
        }
    }

    private void DeleteImplementation(TextPresenter? presenter)
    {
        if(presenter is null) return;
        var oldText = presenter.Text;
        if (presenter.SelectionStart != presenter.SelectionEnd)
        {
            int start = presenter.SelectionStart;
            int end = presenter.SelectionEnd;
            string newText = oldText?.Substring(0, start) + oldText?.Substring(end);
            presenter.Text = newText;
            presenter.CaretIndex = start;
        }
        else if (string.IsNullOrWhiteSpace(oldText))
        {
            presenter.HideCaret();
            MoveToPreviousTextPresenter(presenter);
            if (_currentActivePresenter != null)
            {
                _currentActivePresenter.ShowCaret();
                _currentActivePresenter.CaretIndex = _currentActivePresenter.Text?.Length ?? 0;
            }
        }
        else
        {
            int index = presenter.CaretIndex;
            if (index == 0) return;
            string newText = oldText?.Substring(0, index - 1) + oldText?.Substring(index);
            presenter.MoveCaretHorizontal(LogicalDirection.Backward);
            presenter.Text = newText;
        }
    }

    public async void Cut()
    {
        
    }
}