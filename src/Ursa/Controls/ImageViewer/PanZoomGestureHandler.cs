using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace Ursa.Controls;

/// <summary>
/// Detects single-finger pan and two-finger pinch gestures from raw pointer events.
///
/// <para>Usage: forward <c>PointerPressed</c>, <c>PointerMoved</c>, <c>PointerReleased</c>,
/// and <c>PointerCaptureLost</c> from the host control to the corresponding methods
/// on this class.  Subscribe to <see cref="Pan"/> and <see cref="PinchUpdated"/>
/// to receive gesture deltas.</para>
/// </summary>
public class PanZoomGestureHandler
{
    // ── Internal state ───────────────────────────────────────────────────────

    private readonly Dictionary<long, Point> _activePointers = new();
    private bool _isDragging;
    private Point _lastPointerPosition;

    // Pinch start snapshot
    private Point _pinchStartCenter;
    private double _pinchStartDistance;

    // ── Output events ────────────────────────────────────────────────────────

    /// <summary>Raised every frame during a single-finger drag.</summary>
    /// <param name="deltaX">Horizontal delta in device-independent pixels.</param>
    /// <param name="deltaY">Vertical delta in device-independent pixels.</param>
    public event Action<double, double>? Pan;

    /// <summary>Raised when a second finger is added and pinch begins.</summary>
    public event Action? PinchStarted;

    /// <summary>
    /// Raised every frame during a two-finger pinch.  All values are cumulative
    /// relative to the start of the pinch (no frame-to-frame drift).
    /// </summary>
    public event Action<PinchUpdateEventArgs>? PinchUpdated;

    /// <summary>Raised when pinch ends (one finger lifted).</summary>
    public event Action? PinchEnded;

    // ── Input forwarding ─────────────────────────────────────────────────────

    /// <summary>Forward pointer press events from the host control.</summary>
    public void PointerPressed(PointerPressedEventArgs e, Control? visual)
    {
        var point = e.GetCurrentPoint(visual);
        if (!point.Properties.IsLeftButtonPressed) return;

        var id = point.Pointer.Id;
        _activePointers[id] = point.Position;
        e.Pointer.Capture(visual);
        e.Handled = true;

        if (_activePointers.Count == 1)
        {
            _isDragging = true;
            _lastPointerPosition = point.Position;
        }
        else if (_activePointers.Count == 2)
        {
            _isDragging = false;
            BeginPinch();
            PinchStarted?.Invoke();
        }
    }

    /// <summary>Forward pointer move events from the host control.</summary>
    public void PointerMoved(PointerEventArgs e, Control? visual)
    {
        var point = e.GetCurrentPoint(visual);
        var id = point.Pointer.Id;

        if (!_activePointers.TryGetValue(id, out _)) return;
        _activePointers[id] = point.Position;

        if (_activePointers.Count == 1 && _isDragging)
        {
            Pan?.Invoke(
                point.Position.X - _lastPointerPosition.X,
                point.Position.Y - _lastPointerPosition.Y);
            _lastPointerPosition = point.Position;
        }
        else if (_activePointers.Count >= 2)
        {
            NotifyPinchUpdate();
        }
    }

    /// <summary>Forward pointer release events from the host control.</summary>
    public void PointerReleased(PointerReleasedEventArgs e, Control? visual)
    {
        var point = e.GetCurrentPoint(visual);
        var id = point.Pointer.Id;

        _activePointers.Remove(id);
        e.Pointer.Capture(null);

        if (_activePointers.Count == 0)
        {
            var wasPinching = !_isDragging;
            _isDragging = false;
            if (wasPinching)
                PinchEnded?.Invoke();
        }
        else if (_activePointers.Count == 1)
        {
            PinchEnded?.Invoke();
            _isDragging = true;
            _lastPointerPosition = _activePointers.Values.First();
        }
    }

    /// <summary>Forward pointer capture loss from the host control.</summary>
    public void PointerCaptureLost()
    {
        _activePointers.Clear();
        _isDragging = false;
    }

    /// <summary>Force-reset internal state (e.g. when image source changes).</summary>
    public void Complete()
    {
        _activePointers.Clear();
        _isDragging = false;
    }

    // ── Pinch helpers ────────────────────────────────────────────────────────

    private void BeginPinch()
    {
        var pts = _activePointers.Values.ToArray();
        _pinchStartCenter = new Point(
            (pts[0].X + pts[1].X) / 2,
            (pts[0].Y + pts[1].Y) / 2);
        _pinchStartDistance = Distance(pts[0], pts[1]);
    }

    private void NotifyPinchUpdate()
    {
        var pts = _activePointers.Values.ToArray();
        var center = new Point(
            (pts[0].X + pts[1].X) / 2,
            (pts[0].Y + pts[1].Y) / 2);
        var distance = Distance(pts[0], pts[1]);

        if (_pinchStartDistance <= 0) return;

        PinchUpdated?.Invoke(new PinchUpdateEventArgs(
            cumulativeScale: distance / _pinchStartDistance,
            cumulativeTranslationX: center.X - _pinchStartCenter.X,
            cumulativeTranslationY: center.Y - _pinchStartCenter.Y,
            centerX: center.X,
            centerY: center.Y));
    }

    private static double Distance(Point a, Point b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}

/// <summary>Carries cumulative pinch state since the gesture started.</summary>
public class PinchUpdateEventArgs : System.EventArgs
{
    /// <summary>Scale factor relative to pinch start (&gt;1 = zoom in).</summary>
    public double CumulativeScale { get; }

    /// <summary>Total horizontal translation since pinch start (DIPs).</summary>
    public double CumulativeTranslationX { get; }

    /// <summary>Total vertical translation since pinch start (DIPs).</summary>
    public double CumulativeTranslationY { get; }

    /// <summary>Current X position of the pinch centre in control coordinates.</summary>
    public double CenterX { get; }

    /// <summary>Current Y position of the pinch centre in control coordinates.</summary>
    public double CenterY { get; }

    public PinchUpdateEventArgs(
        double cumulativeScale,
        double cumulativeTranslationX,
        double cumulativeTranslationY,
        double centerX,
        double centerY)
    {
        CumulativeScale = cumulativeScale;
        CumulativeTranslationX = cumulativeTranslationX;
        CumulativeTranslationY = cumulativeTranslationY;
        CenterX = centerX;
        CenterY = centerY;
    }
}
