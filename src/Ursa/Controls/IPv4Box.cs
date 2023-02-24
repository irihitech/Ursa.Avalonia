using System.Diagnostics;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.TextFormatting;

namespace Ursa.Controls;

[TemplatePart(PART_FirstTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_SecondTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_ThirdTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_FourthTextPresenter, typeof(TextPresenter))]
[TemplatePart(PART_PortTextPresenter, typeof(TextPresenter))]
public class IPv4Box: TemplatedControl
{
    public const string PART_FirstTextPresenter = "PART_FirstTextPresenter";
    public const string PART_SecondTextPresenter = "PART_SecondTextPresenter";
    public const string PART_ThirdTextPresenter = "PART_ThirdTextPresenter";
    public const string PART_FourthTextPresenter = "PART_FourthTextPresenter";
    public const string PART_PortTextPresenter = "PART_PortTextPresenter";
    private TextPresenter? _firstText;
    private TextPresenter? _secondText;
    private TextPresenter? _thirdText;
    private TextPresenter? _fourthText;
    private TextPresenter? _portText;
    private byte _firstByte;
    private byte _secondByte;
    private byte _thirdByte;
    private byte _fourthByte;
    private int _port;
    private TextPresenter?[] _presenters = new TextPresenter?[5];
    private byte[] _bytes = new byte[4];
    private TextPresenter? _currentActivePresenter;

    public static readonly StyledProperty<bool> ShowPortProperty = AvaloniaProperty.Register<IPv4Box, bool>(
        nameof(ShowPort));

    public bool ShowPort
    {
        get => GetValue(ShowPortProperty);
        set => SetValue(ShowPortProperty, value);
    }
    
    public static readonly StyledProperty<IPAddress?> IPAddressProperty = AvaloniaProperty.Register<IPv4Box, IPAddress?>(
        nameof(IPAddress));

    public IPAddress? IPAddress
    {
        get => GetValue(IPAddressProperty);
        set => SetValue(IPAddressProperty, value);
    }
    
    public static readonly StyledProperty<int> PortProperty = AvaloniaProperty.Register<IPv4Box, int>(
        nameof(Port));

    public int Port
    {
        get => GetValue(PortProperty);
        set => SetValue(PortProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _firstText = e.NameScope.Find<TextPresenter>(PART_FirstTextPresenter);
        _secondText = e.NameScope.Find<TextPresenter>(PART_SecondTextPresenter);
        _thirdText = e.NameScope.Find<TextPresenter>(PART_ThirdTextPresenter);
        _fourthText = e.NameScope.Find<TextPresenter>(PART_FourthTextPresenter);
        _portText = e.NameScope.Find<TextPresenter>(PART_PortTextPresenter);
        _presenters[0] = _portText;
        _presenters[1] = _firstText;
        _presenters[2] = _secondText;
        _presenters[3] = _thirdText;
        _presenters[4] = _fourthText;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
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
            DeleteCurrentCharacter(_currentActivePresenter);
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
                _currentActivePresenter.Text = newText;
                _currentActivePresenter.MoveCaretHorizontal();
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

            string newText = text is null ? string.Empty : text.Substring(0, start) + text.Substring(end);
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
                SetIPAddress();
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
        SetIPAddress();
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        _currentActivePresenter = _firstText;
        _currentActivePresenter?.ShowCaret();
        base.OnGotFocus(e);
    }

    private void MoveToNextPresenter(TextPresenter? presenter, bool selectAllAfterMove)
    {
        if (presenter is null) return;
        if (Equals(presenter, _firstText)) _currentActivePresenter = _secondText;
        else if (Equals(presenter, _secondText)) _currentActivePresenter = _thirdText;
        else if (Equals(presenter, _thirdText)) _currentActivePresenter = _fourthText;
        else if (Equals(presenter, _fourthText))
        {
            if (ShowPort)
            {
                _currentActivePresenter = _portText;
            }
        }
        if(selectAllAfterMove) SelectAll(_currentActivePresenter);
    }

    private bool MoveToPreviousTextPresenter(TextPresenter? presenter)
    {
        if (presenter is null) return false;
        if (Equals(presenter, _firstText)) return false;
        if (Equals(presenter, _portText)) _currentActivePresenter = _fourthText;
        else if (Equals(presenter, _fourthText)) _currentActivePresenter = _thirdText;
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
        address += _fourthByte << 24;
        IPAddress = new IPAddress(address);
    }

    private void DeleteCurrentCharacter(TextPresenter? presenter)
    {
        if(presenter is null) return;
        var oldText = presenter.Text;
        if (string.IsNullOrWhiteSpace(oldText))
        {
            MoveToPreviousTextPresenter(presenter);
            return;
        }
        int index = presenter.CaretIndex;
        if (index == 0) return;
        string newText = oldText?.Substring(0, index - 1) + oldText?.Substring(index);
        presenter.MoveCaretHorizontal(LogicalDirection.Backward);
        presenter.Text = newText;
    }
}