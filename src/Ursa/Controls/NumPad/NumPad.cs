using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class NumPad : TemplatedControl
{
    private const double estimatedNumPadWidth = 282; // 弹窗宽度（像素）
    private const double estimatedNumPadHeight = 378; // 弹窗高度（像素）

    public static readonly StyledProperty<InputElement?> TargetProperty =
        AvaloniaProperty.Register<NumPad, InputElement?>(
            nameof(Target));

    public static readonly StyledProperty<bool> NumModeProperty = AvaloniaProperty.Register<NumPad, bool>(
        nameof(NumMode), defaultValue: true);

    public static readonly AttachedProperty<bool> AttachProperty =
        AvaloniaProperty.RegisterAttached<NumPad, InputElement, bool>("Attach");

    /// <summary>
    /// 附加属性，用于控制是否在显示NumPad时将光标移动到文本末尾
    /// </summary>
    public static readonly AttachedProperty<bool> MoveCaretToEndProperty =
        AvaloniaProperty.RegisterAttached<Control, InputElement, bool>("MoveCaretToEnd");

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

    /// <summary>
    /// Target 目标内部的 TextBox 控件
    /// </summary>
    private TextBox? _targetInnerText;

    static NumPad()
    {
        AttachProperty.Changed.AddClassHandler<InputElement, bool>(OnAttachNumPad);
    }

    public InputElement? Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public bool NumMode
    {
        get => GetValue(NumModeProperty);
        set => SetValue(NumModeProperty, value);
    }

    public static void SetAttach(InputElement obj, bool value) => obj.SetValue(AttachProperty, value);
    public static bool GetAttach(InputElement obj) => obj.GetValue(AttachProperty);

    /// <summary>
    /// 设置MoveCaretToEnd附加属性的值
    /// </summary>
    /// <param name="element">要设置属性的Avalonia对象</param>
    /// <param name="value">布尔值，true表示启用光标移动到末尾功能，false表示禁用</param>
    public static void SetMoveCaretToEnd(AvaloniaObject element, bool value)
    {
        element.SetValue(MoveCaretToEndProperty, value);
    }

    /// <summary>
    /// 获取MoveCaretToEnd附加属性的值
    /// </summary>
    /// <param name="element">要获取属性的Avalonia对象</param>
    /// <returns>返回布尔值，true表示启用了光标移动到末尾功能，false表示未启用</returns>
    public static bool GetMoveCaretToEnd(AvaloniaObject element)
    {
        return element.GetValue(MoveCaretToEndProperty);
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
        if (sender is not InputElement element) return;
        var existing = OverlayDialog.Recall<NumPad>(null);
        if (existing is not null)
        {
            if (existing.Target is IPv4Box pv4Box)
            {
                pv4Box.IsTargetByNumPad = false; // 取消 IPv4Box 的 NumPad 输入模式
            }

            existing.Target = element;
            existing._targetInnerText = FindTextBoxInTarget(element);

            if (existing.Target is IPv4Box pv4Box2)
            {
                pv4Box2.IsTargetByNumPad = true;
            }

            return;
        }

        var numPad = new NumPad()
        {
            Target = element,
            _targetInnerText = FindTextBoxInTarget(element)
        };
        if (GetMoveCaretToEnd(element))
            MoveCaretToEnd(element);
        var options = BuildPositionedOptions(element as Control) ?? new OverlayDialogOptions()
        {
            Buttons = DialogButton.None,
            OnDialogControlClosed = (object? _, object? _) => { numPad.Target?.Focus(); }
        };
        OverlayDialog.Show(numPad, new object(), options: options);
    }

    /// <summary>
    /// 将光标移动到文本框的末尾位置
    /// </summary>
    /// <param name="element">输入元素，可以是TextBox或包含TextBox的容器</param>
    private static void MoveCaretToEnd(InputElement element)
    {
        TextBox? textBox = null;

        if (element is TextBox tb)
        {
            textBox = tb;
        }
        else
        {
            textBox = FindTextBoxInTarget(element);
        }

        if (textBox is null)
            return;

        // 确保在 UI 线程末尾执行，避免焦点竞争
        Dispatcher.UIThread.Post(() =>
        {
            var length = textBox.Text?.Length ?? 0;
            textBox.CaretIndex = length;
            textBox.SelectionStart = length;
            textBox.SelectionEnd = length;
        });
    }

    public void ProcessClick(object o)
    {
        if (Target is null || o is not NumPadButton b) return;
        var key = (b.NumMode ? b.NumKey : b.FunctionKey) ?? Key.None;

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

    private static OverlayDialogOptions? BuildPositionedOptions(Control? target)
    {
        var topLevel = TopLevel.GetTopLevel(target);
        if (topLevel is null)
        {
            return null;
        }

        var rect = GetTargetRect(target);

        var showBelow = CanShowBelow(rect, topLevel);

        // 默认：左对齐目标
        var horizontalOffset = rect.Left;

        // 判断右侧是否会溢出
        var overflowRight = horizontalOffset + estimatedNumPadWidth > topLevel.Bounds.Width;

        if (overflowRight)
        {
            // 改为右对齐目标
            horizontalOffset = rect.Right - estimatedNumPadWidth;

            // 3️⃣ 仍然溢出 → 贴右边界
            if (horizontalOffset < 0)
            {
                horizontalOffset = topLevel.Bounds.Width - estimatedNumPadWidth;
            }
        }

        // 最终兜底（防止负数）
        horizontalOffset = Math.Max(0, horizontalOffset);

        if (showBelow)
        {
            return new OverlayDialogOptions
            {
                Buttons = DialogButton.None,
                CanLightDismiss = true,

                HorizontalAnchor = HorizontalPosition.Left,
                VerticalAnchor = VerticalPosition.Top,

                HorizontalOffset = horizontalOffset,
                VerticalOffset = rect.Bottom,

                OnDialogControlClosed = (object? _, object? _) => target.Focus(),
                LightDismissFilter = (visual) => LightDismissFilter(target, visual)
            };
        }
        else
        {
            return new OverlayDialogOptions
            {
                Buttons = DialogButton.None,
                CanLightDismiss = true,

                HorizontalAnchor = HorizontalPosition.Left,
                VerticalAnchor = VerticalPosition.Bottom,

                HorizontalOffset = horizontalOffset,
                VerticalOffset = topLevel.Bounds.Height - rect.Top,

                OnDialogControlClosed = (object? _, object? _) => target.Focus(),
                LightDismissFilter = (visual) => LightDismissFilter(target, visual)
            };
        }
    }

    /// <summary>
    /// LightDismiss过滤器函数，用于判断是否是目标控件点击了，避免点击目标控件导致NumPad关闭
    /// </summary>
    /// <param name="target">目标控件，通常是触发NumPad显示的控件</param>
    /// <param name="visual">点击事件的可视化元素</param>
    /// <returns>
    /// 返回true表示点击发生在NumPad内部或目标控件内部，应该阻止关闭NumPad；
    /// 返回false表示点击发生在外部，允许关闭NumPad
    /// </returns>
    private static bool LightDismissFilter(Control? target, Visual? visual)
    {
        if (visual == null)
            return false;

        // 在 NumPad 内
        if (visual.FindAncestorOfType<NumPad>() != null)
            return true;

        // 在 target 内
        return target is Visual targetVisual &&
               (visual == targetVisual || targetVisual.IsVisualAncestorOf(visual));
    }

    private static Rect GetTargetRect(Control target)
    {
        var topLevel = TopLevel.GetTopLevel(target)!;
        var pt = target.TranslatePoint(new Point(0, 0), topLevel);

        return pt.HasValue ? new Rect(pt.Value, target.Bounds.Size) : default;
    }

    private static bool CanShowBelow(Rect targetRect, TopLevel topLevel)
    {
        var spaceBelow = topLevel.Bounds.Height - targetRect.Bottom;
        return spaceBelow >= estimatedNumPadHeight;
    }
}