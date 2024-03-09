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

    public static readonly StyledProperty<int> CountOfDigitProperty = AvaloniaProperty.Register<VerificationCode, int>(
        nameof(CountOfDigit));

    public int CountOfDigit
    {
        get => GetValue(CountOfDigitProperty);
        set => SetValue(CountOfDigitProperty, value);
    }

    public static readonly StyledProperty<char> PasswordCharProperty =
        AvaloniaProperty.Register<VerificationCode, char>(
            nameof(PasswordChar));

    public char PasswordChar
    {
        get => GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }

    public static readonly DirectProperty<VerificationCode, AvaloniaList<string>> DigitsProperty = AvaloniaProperty.RegisterDirect<VerificationCode, AvaloniaList<string>>(
        nameof(Digits), o => o.Digits, (o, v) => o.Digits = v);
    
    private AvaloniaList<string> _digits = [];
    internal AvaloniaList<string> Digits
    {
        get => _digits;
        set => SetAndRaise(DigitsProperty, ref _digits, value);
    }

    static VerificationCode()
    {
        CountOfDigitProperty.Changed.AddClassHandler<VerificationCode, int>((code, args) => code.OnCountOfDigitChanged(args));
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
            Digits = new AvaloniaList<string>(Enumerable.Repeat(string.Empty, newValue));
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
            var text = t.FindLogicalAncestorOfType<TextBox>();
            if (text != null)
            {
                _currentIndex = _itemsControl?.IndexFromContainer(text) ?? 0;
            }
        }
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        if (e.Text?.Length == 1 && _currentIndex < CountOfDigit)
        {
            Digits[_currentIndex] = e.Text;
            var presenter = _itemsControl?.ContainerFromIndex(_currentIndex) as TextBox;
            if (presenter is null) return;
            _currentIndex++;
            var newPresenter = _itemsControl?.ContainerFromIndex(_currentIndex)?.Focus();
            presenter.Text = e.Text;
            if (_currentIndex == CountOfDigit)
            {
                CompleteCommand?.Execute(Digits);
            }
        }
    }
}