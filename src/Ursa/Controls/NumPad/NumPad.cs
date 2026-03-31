using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.LogicalTree;
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

    private static void OnTargetGotFocus(object? sender, FocusChangedEventArgs e)
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
        var options = BuildPositionedOptions(element as Control) ?? new OverlayDialogOptions()
        {
            Buttons = DialogButton.None,
            OnDialogControlClosed = (object? _, object? _) => { numPad.Target?.Focus(); }
        };
        OverlayDialog.ShowStandard(numPad, new object(), options: options);
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

                OnDialogControlClosed = (object? _, object? _) => target.Focus()
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

                OnDialogControlClosed = (object? _, object? _) => target.Focus()
            };
        }
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