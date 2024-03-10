using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class NumPad: TemplatedControl
{
    public static readonly StyledProperty<InputElement?> TargetProperty = AvaloniaProperty.Register<NumPad, InputElement?>(
        nameof(Target));

    public InputElement? Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public static readonly StyledProperty<bool> NumModeProperty = AvaloniaProperty.Register<NumPad, bool>(
        nameof(NumMode), defaultValue: true);

    public bool NumMode
    {
        get => GetValue(NumModeProperty);
        set => SetValue(NumModeProperty, value);
    }

    public static readonly AttachedProperty<bool> AttachProperty =
        AvaloniaProperty.RegisterAttached<NumPad, InputElement, bool>("Attach");

    public static void SetAttach(InputElement obj, bool value) => obj.SetValue(AttachProperty, value);
    public static bool GetAttach(InputElement obj) => obj.GetValue(AttachProperty);

    static NumPad()
    {
        AttachProperty.Changed.AddClassHandler<InputElement, bool>(OnAttachNumPad);
    }

    private static void OnAttachNumPad(InputElement input, AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.NewValue.Value)
        {
            GotFocusEvent.AddHandler(OnTargetGotFocus, input);
        }
        else
        {
            GotFocusEvent.RemoveHandler(OnTargetGotFocus, input);
        }
    }
    
    private static void OnTargetGotFocus(object sender, GotFocusEventArgs e)
    {
        if (sender is not InputElement) return;
        var existing = OverlayDialog.Recall<NumPad>(null);
        if (existing is not null)
        {
            existing.Target = sender as InputElement;
            return;
        }
        var numPad = new NumPad() { Target = sender as InputElement };
        OverlayDialog.Show(numPad, new object(), options: new OverlayDialogOptions() { Buttons = DialogButton.None });
    }

    private Dictionary<Key, string> _keyInputMapping = new()
    {
        [Key.NumPad0] = "0",
        [Key.NumPad1] = "1",
        [Key.NumPad2] = "2",
        [Key.NumPad3] = "3",
        [Key.NumPad4] = "4",
        [Key.NumPad5] = "5",
        [Key.NumPad6] = "6",
        [Key.NumPad7] = "7",
        [Key.NumPad8] = "8",
        [Key.NumPad9] = "9",
        [Key.Add] = "+",
        [Key.Subtract] = "-",
        [Key.Multiply] = "*",
        [Key.Divide] = "/",
        [Key.Decimal] = ".",
    };

    public void ProcessClick(object o)
    {
        if (o is NumPadButton b)
        {
            if (b.NumKey is null)
            {
                Target?.RaiseEvent(new KeyEventArgs()
                {
                    Source = this,
                    RoutedEvent = KeyDownEvent,
                    Key = b.FunctionKey ?? Key.None,
                });
            }
            if (b is { NumMode: true, NumKey: not null })
            {
                Target?.RaiseEvent(new TextInputEventArgs()
                {
                    Source = this,
                    RoutedEvent = TextInputEvent,
                    Text = _keyInputMapping.TryGetValue(b.NumKey.Value, out var text)? text:string.Empty,
                });
            }
            else if (b is { NumMode: false, FunctionKey: null, NumKey: not null })
            {
                Target?.RaiseEvent(new TextInputEventArgs()
                {
                    Source = this,
                    RoutedEvent = TextInputEvent,
                    Text = _keyInputMapping.TryGetValue(b.NumKey.Value, out var text)? text:string.Empty,
                });
            }
            else
            {
                Target?.RaiseEvent(new KeyEventArgs()
                {
                    Source = this,
                    RoutedEvent = KeyDownEvent,
                    Key = b.FunctionKey ?? Key.None,
                });
            }
        }
        
    }
}