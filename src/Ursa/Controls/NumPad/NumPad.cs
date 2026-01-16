using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Platform;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class NumPad : TemplatedControl
{
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

    private static void OnTargetGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (sender is not InputElement targetElement) return;

        var existing = OverlayDialog.Recall<NumPad>(null);
        if (existing is not null)
        {
            if (existing.Target is IPv4Box pv4Box)
            {
                pv4Box.IsTargetByNumPad = false; // 取消 IPv4Box 的 NumPad 输入模式
            }

            existing.Target = targetElement;
            existing._targetInnerText = FindTextBoxInTarget(targetElement);

            if (existing.Target is IPv4Box pv4Box2)
            {
                pv4Box2.IsTargetByNumPad = true;
            }

            return;
        }

        var numPad = new NumPad()
        {
            Target = targetElement,
            _targetInnerText = FindTextBoxInTarget(targetElement)
        };

        // 获取弹窗选项
        var options = GetPositionedDialogOptions(targetElement);
        OverlayDialog.Show(numPad, new object(), options: options);
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

    /// <summary>
    /// 获取根据目标控件位置调整的弹窗选项
    /// </summary>
    private static OverlayDialogOptions GetPositionedDialogOptions(InputElement targetElement)
    {
        var options = new OverlayDialogOptions()
        {
            Buttons = DialogButton.None,
            CanLightDismiss = true,
            OnDialogControlClosed = (object? ss, object? e) =>
            {
                if (ss is DialogControlBase { Content: NumPad numPad })
                {
                    numPad.Target?.Focus();
                }
            }
        };

        // 获取目标控件的屏幕位置和大小
        var targetScreenRect = GetTargetScreenRect(targetElement);
        if (targetScreenRect == null)
        {
            // 如果无法获取屏幕位置，使用默认设置
            return options;
        }

        Debug.WriteLine($"目标控件的屏幕位置和大小:{targetScreenRect}");
        // 获取屏幕信息
        var screen = GetScreenInfo(targetElement);
        if (screen == null)
        {
            return options;
        }

        // 估计 NumPad 的尺寸（单位：像素）
        const int estimatedNumPadWidth = 282; // 弹窗宽度（像素）
        const int estimatedNumPadHeight = 378; // 弹窗高度（像素）
        const int margin = 5; // 弹窗与目标控件之间的最小间距（像素）

        // 目标控件在屏幕上的位置
        var targetRect = targetScreenRect.Value;
        var targetBottom = targetRect.Bottom;
        var targetTop = targetRect.Y;
        var targetHeight = targetRect.Height;
        var targetLeft = targetRect.X;

        // 屏幕边界
        var screenBounds = screen.Bounds;
        var screenWorkingArea = screen.WorkingArea;

        // 优先使用工作区，如果没有工作区则使用整个屏幕边界
        var usableArea = screenWorkingArea is { Width: > 0, Height: > 0 }
            ? screenWorkingArea
            : screenBounds;

        Debug.WriteLine($"目标控件位置: Top={targetTop}, Bottom={targetBottom}, Height={targetHeight}");
        Debug.WriteLine($"屏幕可用区域: Top={usableArea.Y}, Bottom={usableArea.Bottom}, Height={usableArea.Height}");

        // 计算底部可用空间（需要考虑间距）
        var bottomSpace = usableArea.Bottom - targetBottom;
        var hasEnoughSpaceBelow = bottomSpace >= (estimatedNumPadHeight + margin);

        // 计算顶部可用空间（需要考虑间距）
        var topSpace = targetTop - usableArea.Y;
        var hasEnoughSpaceAbove = topSpace >= (estimatedNumPadHeight + margin);

        Debug.WriteLine($"计算底部可用空间:{bottomSpace}, 判断底部是否有足够空间:{hasEnoughSpaceBelow}");
        Debug.WriteLine($"计算顶部可用空间:{topSpace}, 判断顶部是否有足够空间:{hasEnoughSpaceAbove}");

        // 统一使用 Top 锚点
        options.VerticalAnchor = VerticalPosition.Top;
        options.HorizontalAnchor = HorizontalPosition.Left;

        // 计算水平偏移，确保弹窗在屏幕内
        var horizontalOffset = targetLeft;
        if (horizontalOffset + estimatedNumPadWidth > usableArea.Right)
        {
            horizontalOffset = usableArea.Right - estimatedNumPadWidth;
        }

        if (horizontalOffset < usableArea.X)
        {
            horizontalOffset = usableArea.X;
        }

        options.HorizontalOffset = horizontalOffset;

        // 决策逻辑：优先显示在下方
        if (hasEnoughSpaceBelow)
        {
            // 底部有足够空间，显示在下方
            // 弹窗顶部在目标控件底部下方 margin 像素处
            options.VerticalOffset = targetBottom + margin;
            Debug.WriteLine($"决策: 显示在下方，位置={options.VerticalOffset}");
        }
        else if (hasEnoughSpaceAbove)
        {
            // 底部空间不足，但顶部有足够空间，显示在上方
            // 弹窗顶部在目标控件顶部上方 (弹窗高度 + margin) 像素处
            options.VerticalOffset = targetTop - estimatedNumPadHeight - targetHeight - margin;
            Debug.WriteLine($"决策: 显示在上方，位置={options.VerticalOffset}");
        }
        else
        {
            // 上下都没有足够空间，需要智能调整
            Debug.WriteLine($"决策: 上下空间都不足，智能调整");

            // 比较哪个方向的空间更多
            if (bottomSpace >= topSpace)
            {
                // 底部空间相对较多，尝试显示在下方（可能部分在屏幕外）
                options.VerticalOffset = targetBottom + margin;

                // 如果超出屏幕底部，调整到屏幕底部
                if (options.VerticalOffset + estimatedNumPadHeight > usableArea.Bottom)
                {
                    options.VerticalOffset = usableArea.Bottom - estimatedNumPadHeight;
                    Debug.WriteLine($"调整: 显示在下方但调整到屏幕底部，位置={options.VerticalOffset}");
                }
                else
                {
                    Debug.WriteLine($"决策: 显示在下方（可能部分覆盖），位置={options.VerticalOffset}");
                }
            }
            else
            {
                // 顶部空间相对较多，尝试显示在上方（可能部分在屏幕外）
                options.VerticalOffset = targetTop - estimatedNumPadHeight - margin;

                // 如果超出屏幕顶部，调整到屏幕顶部
                if (options.VerticalOffset < usableArea.Y)
                {
                    options.VerticalOffset = usableArea.Y;
                    Debug.WriteLine($"调整: 显示在上方但调整到屏幕顶部，位置={options.VerticalOffset}");
                }
                else
                {
                    Debug.WriteLine($"决策: 显示在上方（可能部分覆盖），位置={options.VerticalOffset}");
                }
            }
        }

        // 最终验证和调整，确保位置在屏幕内
        options.VerticalOffset = Math.Max(usableArea.Y,
            Math.Min(options.VerticalOffset.Value, usableArea.Bottom - estimatedNumPadHeight));

        // 检查是否覆盖了目标控件
        var dialogTop = options.VerticalOffset.Value;
        var dialogBottom = dialogTop + estimatedNumPadHeight;
        var overlapsTarget = dialogTop < targetBottom && dialogBottom > targetTop;

        if (overlapsTarget)
        {
            Debug.WriteLine($"警告: 弹窗可能会覆盖目标控件，尝试调整位置");

            // 尝试调整到不覆盖的位置
            if (dialogTop < targetTop)
            {
                // 弹窗在目标上方，尝试向下移动
                var newTop = targetBottom + margin;
                if (newTop + estimatedNumPadHeight <= usableArea.Bottom)
                {
                    options.VerticalOffset = newTop;
                    Debug.WriteLine($"调整: 向下移动到目标下方，位置={options.VerticalOffset}");
                }
                else
                {
                    // 如果下方没有空间，尝试向上移动更多
                    var alternativeTop = targetTop - estimatedNumPadHeight - margin;
                    if (alternativeTop >= usableArea.Y)
                    {
                        options.VerticalOffset = alternativeTop;
                        Debug.WriteLine($"调整: 向上移动更多避免覆盖，位置={options.VerticalOffset}");
                    }
                }
            }
            else
            {
                // 弹窗在目标下方，尝试向上移动
                var newTop = targetTop - estimatedNumPadHeight - margin;
                if (newTop >= usableArea.Y)
                {
                    options.VerticalOffset = newTop;
                    Debug.WriteLine($"调整: 向上移动到目标上方，位置={options.VerticalOffset}");
                }
                else
                {
                    // 如果上方没有空间，尝试向下移动更多
                    var alternativeTop = targetBottom + margin;
                    if (alternativeTop + estimatedNumPadHeight <= usableArea.Bottom)
                    {
                        options.VerticalOffset = alternativeTop;
                        Debug.WriteLine($"调整: 向下移动更多避免覆盖，位置={options.VerticalOffset}");
                    }
                }
            }
        }

        // 最终边界检查
        options.VerticalOffset = Math.Max(usableArea.Y,
            Math.Min(options.VerticalOffset.Value, usableArea.Bottom - estimatedNumPadHeight));

        Debug.WriteLine($"最终位置: 水平={options.HorizontalOffset}, 垂直={options.VerticalOffset}, " +
                        $"弹窗范围: Top={options.VerticalOffset}, Bottom={options.VerticalOffset + estimatedNumPadHeight}");
        return options;
    }

    /// <summary>
    /// 获取目标控件在屏幕上的 PixelRect
    /// </summary>
    private static PixelRect? GetTargetScreenRect(InputElement targetElement)
    {
        if (targetElement is not Visual visual) return null;

        try
        {
            var topLevel = TopLevel.GetTopLevel(visual);
            if (topLevel == null) return null;

            // 获取控件相对于 TopLevel 的位置
            var transform = visual.TransformToVisual(topLevel);
            if (!transform.HasValue) return null;

            // 计算控件在 TopLevel 中的边界
            var bounds = new Rect(0, 0, visual.Bounds.Width, visual.Bounds.Height);
            var transformedBounds = bounds.TransformToAABB(transform.Value);

            // 转换为屏幕坐标
            var topLeftPoint = topLevel.PointToScreen(transformedBounds.TopLeft);
            var bottomRightPoint = topLevel.PointToScreen(transformedBounds.BottomRight);

            // 创建 PixelRect
            var topLeftPixel = new PixelPoint(
                (int)Math.Round((decimal)topLeftPoint.X, MidpointRounding.AwayFromZero),
                (int)Math.Round((decimal)topLeftPoint.Y, MidpointRounding.AwayFromZero));
            var bottomRightPixel = new PixelPoint(
                (int)Math.Round((decimal)bottomRightPoint.X, MidpointRounding.AwayFromZero),
                (int)Math.Round((decimal)bottomRightPoint.Y, MidpointRounding.AwayFromZero));

            return new PixelRect(topLeftPixel, bottomRightPixel);
        }
        catch (Exception)
        {
            // 如果转换失败，返回 null
            return null;
        }
    }

    /// <summary>
    /// 获取屏幕信息
    /// </summary>
    private static Screen? GetScreenInfo(InputElement targetElement)
    {
        if (targetElement is not Visual visual) return null;

        var topLevel = TopLevel.GetTopLevel(visual);
        if (topLevel == null) return null;

        try
        {
            // 尝试获取控件所在的屏幕
            if (topLevel.Screens?.ScreenFromVisual(visual) is { } screen)
            {
                return screen;
            }

            // 如果无法获取特定屏幕，返回主屏幕
            return topLevel.Screens?.All.FirstOrDefault(s => s.IsPrimary)
                   ?? topLevel.Screens?.All.FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }
}