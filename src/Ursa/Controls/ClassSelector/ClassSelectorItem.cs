using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class ClassSelectorItem : ContentControl
{
    private static readonly Point InvalidPoint = new(double.NaN, double.NaN);
    private Point _pointerDownPoint = InvalidPoint;
    private bool _isUpdatingSelection;
    private object? _displayContent;

    public static readonly StyledProperty<string?> ClassNameProperty =
        AvaloniaProperty.Register<ClassSelectorItem, string?>(nameof(ClassName));

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<ClassSelectorItem, bool>(nameof(IsSelected));

    public static readonly DirectProperty<ClassSelectorItem, object?> DisplayContentProperty =
        AvaloniaProperty.RegisterDirect<ClassSelectorItem, object?>(
            nameof(DisplayContent),
            item => item.DisplayContent);

    static ClassSelectorItem()
    {
        IsSelectedProperty.AffectsPseudoClass<ClassSelectorItem>(PseudoClassName.PC_Selected);
        IsSelectedProperty.Changed.AddClassHandler<ClassSelectorItem, bool>((item, args) =>
            item.OnSelectionChanged(args.NewValue.Value));
        ClassNameProperty.Changed.AddClassHandler<ClassSelectorItem>((item, _) => item.UpdateDisplayContent());
        ContentProperty.Changed.AddClassHandler<ClassSelectorItem>((item, _) => item.UpdateDisplayContent());
        PressedMixin.Attach<ClassSelectorItem>();
        FocusableProperty.OverrideDefaultValue<ClassSelectorItem>(true);
    }

    public string? ClassName
    {
        get => GetValue(ClassNameProperty);
        set => SetValue(ClassNameProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public object? DisplayContent
    {
        get => _displayContent;
        private set => SetAndRaise(DisplayContentProperty, ref _displayContent, value);
    }

    internal string? GetClassName()
    {
        return ClassName ?? DataContext?.ToString() ?? Content?.ToString();
    }

    internal void SetSelectionFromSelector(bool isSelected)
    {
        _isUpdatingSelection = true;
        SetCurrentValue(IsSelectedProperty, isSelected);
        _isUpdatingSelection = false;
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);
        UpdateDisplayContent();
        var selector = this.FindLogicalAncestorOfType<ClassSelector>();
        if (IsSelected || selector?.SelectedClasses?.Contains(GetClassName()) == true)
        {
            selector?.UpdateSelection(this, true);
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        _pointerDownPoint = e.GetPosition(this);
        if (e.Handled)
        {
            return;
        }

        var point = e.GetCurrentPoint(this);
        if (point.Properties.PointerUpdateKind is PointerUpdateKind.LeftButtonPressed
            or PointerUpdateKind.RightButtonPressed)
        {
            if (point.Pointer.Type == PointerType.Mouse)
            {
                SetCurrentValue(IsSelectedProperty, !IsSelected);
                e.Handled = true;
            }
            else
            {
                _pointerDownPoint = point.Position;
            }
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (e.Handled || double.IsNaN(_pointerDownPoint.X) ||
            e.InitialPressMouseButton is not (MouseButton.Left or MouseButton.Right))
        {
            return;
        }

        var point = e.GetCurrentPoint(this);
        if (new Rect(Bounds.Size).ContainsExclusive(point.Position) && e.Pointer.Type == PointerType.Touch)
        {
            SetCurrentValue(IsSelectedProperty, !IsSelected);
            e.Handled = true;
        }
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new ListItemAutomationPeer(this);
    }

    private void OnSelectionChanged(bool isSelected)
    {
        if (_isUpdatingSelection)
        {
            return;
        }

        this.FindLogicalAncestorOfType<ClassSelector>()?.UpdateSelection(this, isSelected);
    }

    private void UpdateDisplayContent()
    {
        DisplayContent = Content switch
        {
            null => ClassName,
            string text when string.IsNullOrWhiteSpace(text) => ClassName,
            _ => Content
        };
    }
}
