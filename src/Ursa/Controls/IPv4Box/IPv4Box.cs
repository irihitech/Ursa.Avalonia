using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Input.TextInput;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
// ReSharper disable InconsistentNaming

namespace Ursa.Controls;

public enum IPv4BoxInputMode
{
    Normal,
    // In fast mode, automatically move to next session after 3 digits input.
    Fast,
}

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
    private byte? _firstByte;
    private byte? _secondByte;
    private byte? _thirdByte;
    private byte? _fourthByte;
    private readonly TextPresenter?[] _presenters = new TextPresenter?[4];
    private TextPresenter? _currentActivePresenter;
    
    public static readonly StyledProperty<IPAddress?> IPAddressProperty = AvaloniaProperty.Register<IPv4Box, IPAddress?>(
        nameof(IPAddress), defaultBindingMode: BindingMode.TwoWay);
    public IPAddress? IPAddress
    {
        get => GetValue(IPAddressProperty);
        set => SetValue(IPAddressProperty, value);
    }

    public static readonly StyledProperty<TextAlignment> TextAlignmentProperty =
        TextBox.TextAlignmentProperty.AddOwner<IPv4Box>();
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

    public static readonly StyledProperty<bool> ShowLeadingZeroProperty = AvaloniaProperty.Register<IPv4Box, bool>(
        nameof(ShowLeadingZero));
    public bool ShowLeadingZero
    {
        get => GetValue(ShowLeadingZeroProperty);
        set => SetValue(ShowLeadingZeroProperty, value);
    }

    public static readonly StyledProperty<IPv4BoxInputMode> InputModeProperty = AvaloniaProperty.Register<IPv4Box, IPv4BoxInputMode>(
        nameof(InputMode));
    public IPv4BoxInputMode InputMode
    {
        get => GetValue(InputModeProperty);
        set => SetValue(InputModeProperty, value);
    }
    private readonly IPv4BoxInputMethodClient _imClient = new IPv4BoxInputMethodClient();
    static IPv4Box()
    {
        ShowLeadingZeroProperty.Changed.AddClassHandler<IPv4Box>((o, e) => o.OnFormatChange(e));
        IPAddressProperty.Changed.AddClassHandler<IPv4Box>((o, e) => o.OnIPChanged(e));
        TextInputMethodClientRequestedEvent.AddClassHandler<IPv4Box>((tb, e) =>
        {
            e.Client = tb._imClient;
        });
    }
    
    #region Overrides
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
        if (this.IPAddress != null)
        {
            var sections = IPAddress.ToString().Split('.');
            for (int i = 0; i < 4; i++)
            {
                var presenter = _presenters[i];
                if (presenter != null)
                {
                    presenter.Text = sections[i];
                }
            }
            ParseBytes(ShowLeadingZero);
        }
    }
    
    protected override void OnKeyDown(KeyEventArgs e)
    {
        _currentActivePresenter ??= _presenters[0];
        var keymap = TopLevel.GetTopLevel(this)?.PlatformSettings?.HotkeyConfiguration;
        bool Match(List<KeyGesture> gestures) => gestures.Any(g => g.Matches(e));
        if (e.Key is Key.Enter or Key.Return)
        {
            ParseBytes(ShowLeadingZero);
            SetIPAddressInternal();
            base.OnKeyDown(e);
            e.Handled = true;
            return;
        }
        if (keymap is not null && Match(keymap.SelectAll))
        {
            if (_currentActivePresenter is not null)
            {
                _currentActivePresenter.SelectionStart = 0;
                _currentActivePresenter.SelectionEnd = _currentActivePresenter.Text?.Length ?? 0;
            }
            e.Handled = true;
            return;
        }

        if (keymap is not null && Match(keymap.Copy))
        {
            Copy();
            e.Handled = true;
            return;
        }

        if (keymap is not null && Match(keymap.Paste))
        {
            Paste();
            e.Handled = true;
            return;
        }

        if (keymap is not null && Match(keymap.Cut))
        {
            Cut();
            e.Handled = true;
            return;
        }
        if (e.Key == Key.Tab)
        {
            _currentActivePresenter?.HideCaret();
            _currentActivePresenter.ClearSelection();
            if (Equals(_currentActivePresenter, _fourthText))
            {
                base.OnKeyDown(e);
                return;
            }
            MoveToNextPresenter(_currentActivePresenter, true);
            _currentActivePresenter?.ShowCaret();
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Back)
        {
            DeleteImplementation(_currentActivePresenter);
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Right )
        {
            OnPressRightKey();
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Left)
        {
            OnPressLeftKey();
            e.Handled = true;
            return;
        }
        base.OnKeyDown(e);
    }
    
    protected override void OnTextInput(TextInputEventArgs e)
    {
        if (e.Handled) return;
        var s = e.Text;
        if (s is null || string.IsNullOrEmpty(s)) return;
        if (s == ".")
        {
            _currentActivePresenter?.HideCaret();
            _currentActivePresenter.ClearSelection();
            if (MoveToNextPresenter(_currentActivePresenter, false))
            {
                _currentActivePresenter?.ShowCaret();
                _currentActivePresenter.MoveCaretToStart();
            }
            e.Handled = false;
            return;
        }
        if (!char.IsNumber(s[0])) return;
        if (_currentActivePresenter != null)
        {
            int index = Math.Min(_currentActivePresenter.CaretIndex, _currentActivePresenter.Text?.Length ?? 0);
            string? oldText = _currentActivePresenter.Text;
            if (oldText is null)
            {
                _currentActivePresenter.Text = s;
                _currentActivePresenter.MoveCaretHorizontal();
            }
            else
            {
                _currentActivePresenter.DeleteSelection();
                _currentActivePresenter.ClearSelection();
                oldText = _currentActivePresenter.Text??string.Empty;

                var newText = string.IsNullOrEmpty(oldText)
                    ? s
                    : oldText.Substring(0, index) + s + oldText.Substring(Math.Min(index, oldText.Length));
                if (newText.Length > 3)
                {
                    newText = newText.Substring(0, 3);
                }
                _currentActivePresenter.Text = newText;
                _currentActivePresenter.MoveCaretHorizontal();
                if (_currentActivePresenter.CaretIndex == 3 && InputMode == IPv4BoxInputMode.Fast)
                {
                    _currentActivePresenter.HideCaret();
                    bool success = MoveToNextPresenter(_currentActivePresenter, true);
                    _currentActivePresenter.ShowCaret();
                    if (success)
                    {
                        _currentActivePresenter.SelectAll();
                        _currentActivePresenter.MoveCaretToStart();
                    }
                }
            }
        }
    }
    
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        Point position = e.GetPosition(_firstText);
        foreach (var presenter in _presenters)
        {
            if (presenter?.Bounds.Contains(position)??false)
            {
                if (e.ClickCount == 1)
                {
                    _imClient.SetPresenter(presenter);
                    presenter.ShowCaret();
                    _currentActivePresenter = presenter;
                    var caretPosition = position.WithX(position.X - presenter.Bounds.X);
                    presenter.MoveCaretToPoint(caretPosition);
                }
                else if (e.ClickCount == 2)
                {
                    presenter.SelectAll();
                    presenter.MoveCaretToEnd();
                }
            }
            else
            {
                presenter?.HideCaret();
                presenter.ClearSelection();
            }
        }
        base.OnPointerPressed(e);
        e.Handled = true;
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        foreach (var pre in _presenters)
        {
            pre?.HideCaret();
            pre.ClearSelection();
        }
        _currentActivePresenter = null;
        ParseBytes(ShowLeadingZero);
        SetIPAddressInternal();
    }
    
    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        _currentActivePresenter = _firstText;
        if (_currentActivePresenter is null)
        {
            base.OnGotFocus(e);
            return;
        }
        _currentActivePresenter.ShowCaret();
        _currentActivePresenter.MoveCaretToStart();
        base.OnGotFocus(e);
    }
    #endregion

    private void OnFormatChange(AvaloniaPropertyChangedEventArgs arg)
    {
        bool showLeadingZero = arg.GetNewValue<bool>();
        ParseBytes(showLeadingZero);
    }

    private void OnIPChanged(AvaloniaPropertyChangedEventArgs arg)
    {
        IPAddress? address = arg.GetNewValue<IPAddress?>();
        if (address is null)
        {
            foreach (var presenter in _presenters)
            {
                if(presenter!=null) presenter.Text = string.Empty;
            }
            ParseBytes(ShowLeadingZero);
        }
        else
        {
            var sections = address.ToString().Split('.');
            for (int i = 0; i < 4; i++)
            {
                var presenter = _presenters[i];
                if (presenter != null)
                {
                    presenter.Text = sections[i];
                }
            }
            ParseBytes(ShowLeadingZero);
        }
    }

    private void ParseBytes(bool showLeadingZero)
    {
        string format = showLeadingZero ? "D3" : "";
        if (string.IsNullOrEmpty(_firstText?.Text) && string.IsNullOrEmpty(_secondText?.Text) && string.IsNullOrEmpty(_thirdText?.Text) && string.IsNullOrEmpty(_fourthText?.Text))
        {
            _firstByte = null;
            _secondByte = null;
            _thirdByte = null;
            _fourthByte = null;
            return;
        }
        _firstByte = byte.TryParse(_firstText?.Text, out byte b1) ? b1 : (byte)0;
        _secondByte = byte.TryParse(_secondText?.Text, out byte b2) ? b2 : (byte)0;
        _thirdByte = byte.TryParse(_thirdText?.Text, out byte b3) ? b3 : (byte)0;
        _fourthByte = byte.TryParse(_fourthText?.Text, out byte b4) ? b4 : (byte)0;
        if (_firstText != null) _firstText.Text = _firstByte?.ToString(format);
        if (_secondText != null) _secondText.Text = _secondByte?.ToString(format);
        if (_thirdText != null) _thirdText.Text = _thirdByte?.ToString(format);
        if (_fourthText != null) _fourthText.Text = _fourthByte?.ToString(format);
    }

    

    private bool MoveToNextPresenter(TextPresenter? presenter, bool selectAllAfterMove)
    {
        if (presenter is null) return false;
        if (Equals(presenter, _fourthText)) return false;
        presenter.ClearSelection();
        if (Equals(presenter, _firstText)) _currentActivePresenter = _secondText;
        else if (Equals(presenter, _secondText)) _currentActivePresenter = _thirdText;
        else if (Equals(presenter, _thirdText)) _currentActivePresenter = _fourthText;
        if(selectAllAfterMove) _currentActivePresenter.SelectAll();
        return true;
    }

    private bool MoveToPreviousTextPresenter(TextPresenter? presenter)
    {
        if (presenter is null) return false;
        if (Equals(presenter, _firstText)) return false;
        presenter.ClearSelection();
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

        _firstByte = null;
        _secondByte = null;
        _thirdByte = null;
        _fourthByte = null;
        IPAddress = null;
    }
    
    private void SetIPAddressInternal()
    {
        if (_firstByte is null && _secondByte is null && _thirdByte is null && _fourthByte is null)
        {
            IPAddress = null;
            return;
        }
        long address = 0;
        address += _firstByte??0;
        address += (_secondByte??0) << 8;
        address += (_thirdByte??0) << 16;
        address += ((long?)_fourthByte ?? 0) << 24;
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
        var oldText = presenter.Text ?? string.Empty;
        if (presenter.SelectionStart != presenter.SelectionEnd)
        {
            presenter.DeleteSelection();
            presenter.ClearSelection();
        }
        else if (string.IsNullOrWhiteSpace(oldText) || presenter.CaretIndex == 0)
        {
            presenter.HideCaret();
            MoveToPreviousTextPresenter(presenter);
            if (_currentActivePresenter != null)
            {
                _currentActivePresenter.ShowCaret();
                _currentActivePresenter.MoveCaretToEnd();
            }
        }
        else
        {
            int index = presenter.CaretIndex;
            string newText = oldText.Substring(0, index - 1) + oldText.Substring(Math.Min(index, oldText.Length));
            presenter.MoveCaretHorizontal(LogicalDirection.Backward);
            presenter.Text = newText;
        }
    }

    private void OnPressRightKey()
    {
        if (_currentActivePresenter is null) return;
        if (_currentActivePresenter.IsTextSelected())
        {
            int end = _currentActivePresenter.SelectionEnd;
            _currentActivePresenter.ClearSelection();
            _currentActivePresenter.MoveCaretToTextPosition(end);
            return;
        }
        if (_currentActivePresenter.CaretIndex >= _currentActivePresenter.Text?.Length)
        {
            _currentActivePresenter.HideCaret();
            bool success = MoveToNextPresenter(_currentActivePresenter, false);
            _currentActivePresenter.ClearSelection();
            _currentActivePresenter.ShowCaret();
            if (success)
            {
                _currentActivePresenter.MoveCaretToStart();
            }
        }
        else
        {
            _currentActivePresenter.ClearSelection();
            _currentActivePresenter.CaretIndex++;
        }
    }

    private void OnPressLeftKey()
    {
        if (_currentActivePresenter is null) return;
        if (_currentActivePresenter.IsTextSelected())
        {
            int start = _currentActivePresenter.SelectionStart;
            _currentActivePresenter.ClearSelection();
            _currentActivePresenter.MoveCaretToTextPosition(start);
            return;
        }
        if (_currentActivePresenter.CaretIndex == 0)
        {
            _currentActivePresenter.HideCaret();
            bool success = MoveToPreviousTextPresenter(_currentActivePresenter);
            _currentActivePresenter.ClearSelection();
            _currentActivePresenter.ShowCaret();
            if (success)
            {
                _currentActivePresenter.MoveCaretToEnd();
            }
        }
        else
        {
            _currentActivePresenter.ClearSelection();
            _currentActivePresenter.CaretIndex--;
        }
    }

    public async void Copy()
    {
        string s = string.Join(".", _firstText?.Text, _secondText?.Text, _thirdText?.Text, _fourthText?.Text);
        IClipboard? clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if (clipboard is null) return;
        await clipboard.SetTextAsync(s);
    }
    
    public static KeyGesture? CopyKeyGesture { get; } = Application.Current?.PlatformSettings?.HotkeyConfiguration.Copy.FirstOrDefault();
    public static KeyGesture? PasteKeyGesture { get; } = Application.Current?.PlatformSettings?.HotkeyConfiguration.Paste.FirstOrDefault();
    public static KeyGesture? CutKeyGesture { get; } = Application.Current?.PlatformSettings?.HotkeyConfiguration.Cut.FirstOrDefault();

    public async void Paste()
    {
        IClipboard? clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if (clipboard is null) return;
        var s = await clipboard.GetTextAsync();
        if (s is not null && IPAddress.TryParse(s, out var address))
        {
            IPAddress = address;
        }
    }

    public async void Cut()
    {
        IClipboard? clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if(clipboard is null) return;
        string s = string.Join(".", _firstText?.Text, _secondText?.Text, _thirdText?.Text, _fourthText?.Text);
        await clipboard.SetTextAsync(s);
        Clear();
    }
}

public static class TextPresenterHelper
{
    public static void MoveCaretToStart(this TextPresenter? presenter)
    {
        if (presenter is null) return;
        presenter.MoveCaretToTextPosition(0);
    }

    public static void MoveCaretToEnd(this TextPresenter? presenter)
    {
        if(presenter is null) return;
        presenter.MoveCaretToTextPosition(presenter.Text?.Length ?? 0);
    }

    public static void ClearSelection(this TextPresenter? presenter)
    {
        if (presenter is null) return;
        presenter.SelectionStart = 0;
        presenter.SelectionEnd = 0;
    }

    public static void SelectAll(this TextPresenter? presenter)
    {
        if(presenter is null) return;
        if(presenter.Text is null) return;
        presenter.SelectionStart = 0;
        presenter.SelectionEnd = presenter.Text.Length;
    }

    public static void DeleteSelection(this TextPresenter? presenter)
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

    public static bool IsTextSelected(this TextPresenter? presenter)
    {
        if (presenter is null) return false;
        return presenter.SelectionStart != presenter.SelectionEnd;
    }
}