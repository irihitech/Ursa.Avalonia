using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.LogicalTree;
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

    /// <summary>
    /// Target 目标内部的 TextBox 控件 
    /// </summary>
    private TextBox? _targetInnerText;

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
    
    private static void OnTargetGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (sender is not InputElement) return;
        var existing = OverlayDialog.Recall<NumPad>(null);
        if (existing is not null)
        {
            if(existing.Target is IPv4Box pv4Box)
            {
                pv4Box.IsTargetByNumPad = false; // 取消 IPv4Box 的 NumPad 输入模式
            }
            existing.Target = sender as InputElement;
            existing._targetInnerText = FindTextBoxInTarget((sender as InputElement)!);

            if (existing.Target is IPv4Box pv4Box2)
            {
                pv4Box2.IsTargetByNumPad = true;
            }
            return;
        }
        var numPad = new NumPad() 
        {
            Target = sender as InputElement ,
            _targetInnerText = FindTextBoxInTarget((sender as InputElement)!)
        };
        OverlayDialog.Show(numPad, new object(), options: new OverlayDialogOptions() { Buttons = DialogButton.None });
    }

    private static readonly Dictionary<Key, string> KeyInputMapping = new()
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
        if (Target is null || o is not NumPadButton b) return;
        var key = (b.NumMode ? b.NumKey : b.FunctionKey)?? Key.None;

        // 如果存在内部为 TextBox 的目标控件，则使用该 TextBox 作为输入目标
        var realTarget = _targetInnerText ?? Target;
        if (KeyInputMapping.TryGetValue(key, out var s))
        {
            realTarget.RaiseEvent(new TextInputEventArgs()
            {
                Source = this,
                RoutedEvent = TextInputEvent,
                Text = s,
            });
        }
        else
        {
            realTarget.RaiseEvent(new KeyEventArgs()
            {
                Source = this,
                RoutedEvent = KeyDownEvent,
                Key = key,
            });
        }
    }
    /// <summary>
    /// 在目标控件中查找 TextBox 控件
    /// </summary>
    /// <param name="target">目标控件</param>
    /// <returns>找到的 TextBox，如果没有找到则返回 null</returns>
    private static TextBox? FindTextBoxInTarget(InputElement target)
    {
        // 如果目标本身就是 TextBox
        if (target is TextBox textBox)
            return textBox;
        
        // 如果目标是 TemplatedControl，并且已经应用了模板
        if (target is TemplatedControl templatedControl && templatedControl.IsInitialized)
        {
            // 尝试通过模板查找 PART_TextBox
            if (templatedControl.GetTemplateChildren().FirstOrDefault(c => c is TextBox) is TextBox partTextBox)
                return partTextBox;
        }

        // 如果目标是 ILogical，使用 LogicalTree 扩展方法查找
        if (target is ILogical logical)
        {
            // 使用 GetLogicalDescendants 方法查找所有逻辑子控件
            var textBoxes = logical.GetLogicalDescendants().OfType<TextBox>();
            return textBoxes.FirstOrDefault();
        }

        return null;
    }
}