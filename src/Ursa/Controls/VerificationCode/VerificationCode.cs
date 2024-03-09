using System.Windows.Input;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_ItemsControl, typeof(ItemsControl))]
public class VerificationCode: TemplatedControl
{
    public const string PART_ItemsControl = "PART_ItemsControl";
    private ItemsControl? _itemsControl;
    private int _currentIndex = 0;
    
    public static readonly StyledProperty<ICommand?> CompleteCommandProperty = AvaloniaProperty.Register<VerificationCode, ICommand?>(
        nameof(CompleteCommand));

    public ICommand? CompleteCommand
    {
        get => GetValue(CompleteCommandProperty);
        set => SetValue(CompleteCommandProperty, value);
    }

    public static readonly StyledProperty<int> CountProperty = AvaloniaProperty.Register<VerificationCode, int>(
        nameof(Count));

    public int Count
    {
        get => GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public static readonly StyledProperty<char> PasswordCharProperty =
        AvaloniaProperty.Register<VerificationCode, char>(
            nameof(PasswordChar));

    public char PasswordChar
    {
        get => GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }

    public static readonly StyledProperty<VerificationCodeMode> ModeProperty =
        AvaloniaProperty.Register<VerificationCode, VerificationCodeMode>(
            nameof(Mode), defaultValue: VerificationCodeMode.Digit | VerificationCodeMode.Letter);

    public VerificationCodeMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public static readonly DirectProperty<VerificationCode, IList<string>> DigitsProperty = AvaloniaProperty.RegisterDirect<VerificationCode, IList<string>>(
        nameof(Digits), o => o.Digits, (o, v) => o.Digits = v);
    
    private IList<string> _digits = [];
    internal IList<string> Digits
    {
        get => _digits;
        set => SetAndRaise(DigitsProperty, ref _digits, value);
    }
    
    public static readonly RoutedEvent<VerificationCodeCompleteEventArgs> CompleteEvent =
        RoutedEvent.Register<VerificationCode, VerificationCodeCompleteEventArgs>(
            nameof(Complete), RoutingStrategies.Bubble);
    
    public event EventHandler<VerificationCodeCompleteEventArgs> Complete
    {
        add => AddHandler(CompleteEvent, value);
        remove => RemoveHandler(CompleteEvent, value);
    }

    static VerificationCode()
    {
        CountProperty.Changed.AddClassHandler<VerificationCode, int>((code, args) => code.OnCountOfDigitChanged(args));
        FocusableProperty.OverrideDefaultValue<VerificationCode>(true);
    }

    public VerificationCode()
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

    private void OnControlPressed(object sender, PointerPressedEventArgs e)
    {
        if (e.Source is Control t)
        {
            var text = t.FindLogicalAncestorOfType<VerificationCodeItem>();
            if (text != null)
            {
                text.Focus();
                _currentIndex = _itemsControl?.IndexFromContainer(text) ?? 0;
            }
        }
        e.Handled = true;
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        if (e.Text?.Length == 1 && _currentIndex < Count)
        {
            var presenter = _itemsControl?.ContainerFromIndex(_currentIndex) as VerificationCodeItem;
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
                RaiseEvent(new VerificationCodeCompleteEventArgs(Digits, CompleteEvent));
            }
        }
    }

    private bool Valid(char c, VerificationCodeMode mode)
    {
        bool isDigit = char.IsDigit(c);
        bool isLetter = char.IsLetter(c);
        return mode switch
        {
            VerificationCodeMode.Digit => isDigit,
            VerificationCodeMode.Letter => isLetter,
            VerificationCodeMode.Digit | VerificationCodeMode.Letter => isDigit || isLetter,
            _ => true
        };
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.Key == Key.Back && _currentIndex >= 0)
        {
            var presenter = _itemsControl?.ContainerFromIndex(_currentIndex) as VerificationCodeItem;
            if (presenter is null) return;
            Digits[_currentIndex] = string.Empty;
            presenter.Text = string.Empty;
            if (_currentIndex == 0) return;
            _currentIndex--;
            _itemsControl?.ContainerFromIndex(_currentIndex)?.Focus();
        }
    }
}