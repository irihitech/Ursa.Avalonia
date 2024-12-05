using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_ItemsControl, typeof(ItemsControl))]
public class PinCode : TemplatedControl
{
    public const string PART_ItemsControl = "PART_ItemsControl";
    private ItemsControl? _itemsControl;
    private int _currentIndex;

    public static readonly StyledProperty<ICommand?> CompleteCommandProperty = AvaloniaProperty.Register<PinCode, ICommand?>(
        nameof(CompleteCommand));

    public ICommand? CompleteCommand
    {
        get => GetValue(CompleteCommandProperty);
        set => SetValue(CompleteCommandProperty, value);
    }

    public static readonly StyledProperty<int> CountProperty = AvaloniaProperty.Register<PinCode, int>(
        nameof(Count));

    public int Count
    {
        get => GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public static readonly StyledProperty<char> PasswordCharProperty =
        AvaloniaProperty.Register<PinCode, char>(
            nameof(PasswordChar));

    public char PasswordChar
    {
        get => GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }

    public static readonly StyledProperty<PinCodeMode> ModeProperty =
        AvaloniaProperty.Register<PinCode, PinCodeMode>(
            nameof(Mode), defaultValue: PinCodeMode.Digit | PinCodeMode.Letter);

    public PinCodeMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public static readonly DirectProperty<PinCode, IList<string>> DigitsProperty = AvaloniaProperty.RegisterDirect<PinCode, IList<string>>(
        nameof(Digits), o => o.Digits);

    private IList<string> _digits = [];
    public IList<string> Digits
    {
        get => _digits;
        private set => SetAndRaise(DigitsProperty, ref _digits, value);
    }

    public static readonly RoutedEvent<PinCodeCompleteEventArgs> CompleteEvent =
        RoutedEvent.Register<PinCode, PinCodeCompleteEventArgs>(
            nameof(Complete), RoutingStrategies.Bubble);

    public event EventHandler<PinCodeCompleteEventArgs> Complete
    {
        add => AddHandler(CompleteEvent, value);
        remove => RemoveHandler(CompleteEvent, value);
    }

    static PinCode()
    {
        CountProperty.Changed.AddClassHandler<PinCode, int>((code, args) => code.OnCountOfDigitChanged(args));
        FocusableProperty.OverrideDefaultValue<PinCode>(true);
        KeyDownEvent.AddClassHandler<PinCode>((o, e) => o.OnPreviewKeyDown(e), RoutingStrategies.Tunnel);
    }

    public PinCode()
    {
        InputMethod.SetIsInputMethodEnabled(this, false);
    }

    private void OnCountOfDigitChanged(AvaloniaPropertyChangedEventArgs<int> args)
    {
        var newValue = args.NewValue.Value;
        if (newValue > 0)
        {
            Digits = new List<string>(Enumerable.Repeat(string.Empty, newValue));
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _itemsControl = e.NameScope.Get<ItemsControl>(PART_ItemsControl);
        PointerPressedEvent.AddHandler(OnControlPressed, RoutingStrategies.Tunnel, false, this);
    }

    private void OnControlPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is Control t)
        {

            var item = t.FindLogicalAncestorOfType<PinCodeItem>();
            if (item != null)
            {
                item.Focus();
                _currentIndex = _itemsControl?.IndexFromContainer(item) ?? 0;
            }
            else
            {
                _currentIndex = MathHelpers.SafeClamp(_currentIndex, 0, Count - 1);
                _itemsControl?.ContainerFromIndex(_currentIndex)?.Focus();
            }

        }
        e.Handled = true;
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        if (e.Text?.Length == 1 && _currentIndex < Count)
        {
            var presenter = _itemsControl?.ContainerFromIndex(_currentIndex) as PinCodeItem;
            if (presenter is null) return;
            char c = e.Text[0];
            if (!Valid(c, this.Mode)) return;
            presenter.Text = e.Text;
            Digits[_currentIndex] = e.Text;
            _currentIndex++;
            _itemsControl?.ContainerFromIndex(_currentIndex)?.Focus();
            if (_currentIndex == Count)
            {
                CompleteCommand?.Execute(Digits);
                RaiseEvent(new PinCodeCompleteEventArgs(Digits, CompleteEvent));
                _currentIndex--;
            }
        }
    }

    private bool Valid(char c, PinCodeMode mode)
    {
        bool isDigit = char.IsDigit(c);
        bool isLetter = char.IsLetter(c);
        return mode switch
        {
            PinCodeMode.Digit => isDigit,
            PinCodeMode.Letter => isLetter,
            PinCodeMode.Digit | PinCodeMode.Letter => isDigit || isLetter,
            _ => true
        };
    }

    protected async void OnPreviewKeyDown(KeyEventArgs e)
    {
        TextBox b = new TextBox();
        var pasteKeys = Application.Current?.PlatformSettings?.HotkeyConfiguration.Paste;
        if (pasteKeys?.Any(a => a.Matches(e)) == true)
        {
            var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
            if (clipboard is null) return;
            var text = await clipboard.GetTextAsync();
            if (text is not null)
            {
                var newText = text.Where(c => Valid(c, Mode)).Take(Count).ToArray();
                for (int i = 0; i < newText.Length; i++)
                {
                    Digits[i] = newText[i].ToString();
                    var presenter = _itemsControl?.ContainerFromIndex(i) as PinCodeItem;
                    if (presenter is not null)
                    {
                        presenter.Focus();
                        presenter.Text = newText[i].ToString();
                    }
                }
                if (newText.Length == Count)
                {
                    CompleteCommand?.Execute(Digits);
                    RaiseEvent(new PinCodeCompleteEventArgs(Digits, CompleteEvent));
                }
            }
            return;
        }
        if (e.Key == Key.Tab && e.Source is PinCodeItem)
        {
            _currentIndex = MathHelpers.SafeClamp(_currentIndex, 0, Count - 1);
            if (e.KeyModifiers == KeyModifiers.Shift)
                _currentIndex--;
            else
                _currentIndex++;
            _currentIndex = MathHelpers.SafeClamp(_currentIndex, 0, Count - 1);
        }
        else if (e.Key == Key.Back && _currentIndex >= 0)
        {
            _currentIndex = MathHelpers.SafeClamp(_currentIndex, 0, Count - 1);
            var presenter = _itemsControl?.ContainerFromIndex(_currentIndex) as PinCodeItem;
            if (presenter is null) return;
            Digits[_currentIndex] = string.Empty;
            presenter.Text = string.Empty;
            if (_currentIndex == 0) return;
            _currentIndex--;
            _itemsControl?.ContainerFromIndex(_currentIndex)?.Focus();
        }
        else if (e.Key is Key.Left or Key.FnLeftArrow)
        {
            _currentIndex--;
            _currentIndex = MathHelpers.SafeClamp(_currentIndex, 0, Count - 1);
            _itemsControl?.ContainerFromIndex(_currentIndex)?.Focus();
        }
        else if (e.Key is Key.Right or Key.FnRightArrow)
        {
            _currentIndex++;
            _currentIndex = MathHelpers.SafeClamp(_currentIndex, 0, Count - 1);
            _itemsControl?.ContainerFromIndex(_currentIndex)?.Focus();
        }
        else if (e.Key is Key.Enter or Key.Return)
        {
            CompleteCommand?.Execute(Digits);
            RaiseEvent(new PinCodeCompleteEventArgs(Digits, CompleteEvent));
        }
        else
        {
            base.OnKeyDown(e);
        }
    }
}