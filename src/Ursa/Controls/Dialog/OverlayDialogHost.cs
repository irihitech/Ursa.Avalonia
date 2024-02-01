using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Utilities;

namespace Ursa.Controls;

public class OverlayDialogHost : Canvas
{
    private readonly List<DialogControl> _dialogs = new();
    private readonly List<DialogControl> _modalDialogs = new(); 
    private readonly List<Border> _masks = new();

    public string? HostId { get; set; }

    private Point _lastPoint;

    public static readonly StyledProperty<IBrush?> OverlayMaskBrushProperty =
        AvaloniaProperty.Register<OverlayDialogHost, IBrush?>(
            nameof(OverlayMaskBrush));

    public IBrush? OverlayMaskBrush
    {
        get => GetValue(OverlayMaskBrushProperty);
        set => SetValue(OverlayMaskBrushProperty, value);
    }

    private Border CreateOverlayMask() => new()
    {
        HorizontalAlignment = HorizontalAlignment.Stretch,
        VerticalAlignment = VerticalAlignment.Stretch,
        Width = this.Bounds.Width,
        Height = this.Bounds.Height,
        [!BackgroundProperty] = this[!OverlayMaskBrushProperty],
        IsVisible = true,
    };

    protected sealed override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        OverlayDialogManager.RegisterHost(this, HostId);
    }

    protected sealed override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        for (int i = 0; i < _masks.Count; i++)
        {
            _masks[i].Width = this.Bounds.Width;
            _masks[i].Height = this.Bounds.Height;
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        OverlayDialogManager.UnregisterHost(HostId);
        base.OnDetachedFromVisualTree(e);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (e.Source is DialogControl item)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                var p = e.GetPosition(this);
                var left = p.X - _lastPoint.X;
                var top = p.Y - _lastPoint.Y;
                left = MathUtilities.Clamp(left, 0, Bounds.Width - item.Bounds.Width);
                top = MathUtilities.Clamp(top, 0, Bounds.Height - item.Bounds.Height);
                Canvas.SetLeft(item, left);
                Canvas.SetTop(item, top);
            }
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        // base.OnPointerPressed(e);
        if (e.Source is DialogControl item)
        {
            _lastPoint = e.GetPosition(item);
        }
    }

    internal void AddDialog(DialogControl control)
    {
        this.Children.Add(control);
        _dialogs.Add(control);
        control.Measure(this.Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        SetToCenter(control);
        control.DialogControlClosing += OnDialogControlClosing;
        control.LayerChanged += OnDialogLayerChanged;
        ResetZIndices();
    }

    private void OnDialogControlClosing(object sender, object? e)
    {
        if (sender is DialogControl control)
        {
            this.Children.Remove(control);
            control.DialogControlClosing -= OnDialogControlClosing;
            control.LayerChanged -= OnDialogLayerChanged;
            if (_dialogs.Contains(control))
            {
                _dialogs.Remove(control);
            }
            else if (_modalDialogs.Contains(control))
            {
                _modalDialogs.Remove(control);
                if (_masks.Count > 0)
                {
                    var last = _masks.Last();
                    this.Children.Remove(last);
                    _masks.Remove(last);
                    if (_masks.Count > 0)
                    {
                        _masks.Last().IsVisible = true;
                    }
                }
            }
            ResetZIndices();
        }
    }

    /// <summary>
    ///  Add a dialog as a modal dialog to the host
    /// </summary>
    /// <param name="control"></param>
    internal void AddModalDialog(DialogControl control)
    {
        var mask = CreateOverlayMask();
        _masks.Add(mask);
        _modalDialogs.Add(control);
        control.SetAsModal(true);
        for (int i = 0; i < _masks.Count-1; i++)
        {
            _masks[i].Opacity = 0.5;
        }
        ResetZIndices();
        this.Children.Add(mask);
        this.Children.Add(control);
        control.Measure(this.Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        SetToCenter(control);
        control.DialogControlClosing += OnDialogControlClosing;
        control.LayerChanged += OnDialogLayerChanged;
    }

    // Handle dialog layer change event
    private void OnDialogLayerChanged(object sender, DialogLayerChangeEventArgs e)
    {
        if (sender is not DialogControl control)
            return;
        if (!_dialogs.Contains(control))
            return;
        int index = _dialogs.IndexOf(control);
        _dialogs.Remove(control);
        int newIndex = index;
        switch (e.ChangeType)
        {
            case DialogLayerChangeType.BringForward:
                newIndex = MathUtilities.Clamp(index + 1, 0, _dialogs.Count);
                break;
            case DialogLayerChangeType.SendBackward:
                newIndex = MathUtilities.Clamp(index - 1, 0, _dialogs.Count);
                break;
            case DialogLayerChangeType.BringToFront:
                newIndex = _dialogs.Count;
                break;
            case DialogLayerChangeType.SendToBack:
                newIndex = 0;
                break;
        }

        _dialogs.Insert(newIndex, control);
        for (int i = 0; i < _dialogs.Count; i++)
        {
            _dialogs[i].ZIndex = i;
        }

        for (int i = 0; i < _masks.Count * 2; i += 2) 
        {
            _masks[i].ZIndex = _dialogs.Count + i;
            _modalDialogs[i].ZIndex = _dialogs.Count + i + 1;
        }
        
    }

    private void ResetZIndices()
    {
        int index = 0;
        for ( int i = 0; i< _dialogs.Count; i++)
        {
            _dialogs[i].ZIndex = index;
            index++;
        }
        for(int i = 0; i< _masks.Count; i++)
        {
            _masks[i].ZIndex = index;
            index++;
            _modalDialogs[i].ZIndex = index;
            index++;
        }
    }

    private void SetToCenter(DialogControl? control)
    {
        // return;
        if (control is null) return;
        double left = (this.Bounds.Width - control.Bounds.Width) / 2;
        double top = (this.Bounds.Height - control.Bounds.Height) / 2;
        left = MathUtilities.Clamp(left, 0, Bounds.Width);
        top = MathUtilities.Clamp(top, 0, Bounds.Height);
        Canvas.SetLeft(control, left);
        Canvas.SetTop(control, top);
    }

    internal IDataTemplate? GetDataTemplate(object? o)
    {
        if (o is null) return null;
        IDataTemplate? result = null;
        var templates = this.DataTemplates.ToList();
        result = templates.FirstOrDefault(a => a.Match(o));
        if (result != null) return result;
        var resources = this.Resources.Where(a => a.Value is IDataTemplate).Select(a => a.Value)
            .OfType<IDataTemplate>();
        result = resources.FirstOrDefault(a => a.Match(o));
        return result;
    }
}