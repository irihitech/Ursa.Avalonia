using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;

namespace Ursa.Controls;

[TemplatePart(PART_Image, typeof(Image))]
[TemplatePart(PART_Layer, typeof(VisualLayerManager))]
public class ImageViewer: TemplatedControl
{
    public const string PART_Image = "PART_Image";
    public const string PART_Layer = "PART_Layer";
    
    private Image _image = null!;
    private VisualLayerManager? _layer;
    private Point? _lastClickPoint;
    private Point? _lastReleasePoint;

    public static readonly StyledProperty<object?> OverlayerProperty = AvaloniaProperty.Register<ImageViewer, object?>(
        nameof(Overlayer));

    public object? Overlayer
    {
        get => GetValue(OverlayerProperty);
        set => SetValue(OverlayerProperty, value);
    }

    public static readonly StyledProperty<IImage?> SourceProperty = Image.SourceProperty.AddOwner<ImageViewer>();
    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    private double _scale  = 1;

    public static readonly DirectProperty<ImageViewer, double> ScaleProperty = AvaloniaProperty.RegisterDirect<ImageViewer, double>(
        nameof(Scale), o => o.Scale, unsetValue: 1);

    public double Scale
    {
        get => _scale;
        private set => SetAndRaise(ScaleProperty, ref _scale, value);
    }

    private double _translateX;

    public static readonly DirectProperty<ImageViewer, double> TranslateXProperty = AvaloniaProperty.RegisterDirect<ImageViewer, double>(
        nameof(TranslateX), o => o.TranslateX, unsetValue: 0);

    public double TranslateX
    {
        get => _translateX;
        private set => SetAndRaise(TranslateXProperty, ref _translateX, value);
    }

    private double _translateY;

    public static readonly DirectProperty<ImageViewer, double> TranslateYProperty = AvaloniaProperty.RegisterDirect<ImageViewer, double>(
        nameof(TranslateY), o => o.TranslateY);

    public double TranslateY
    {
        get => _translateY;
        private set => SetAndRaise(TranslateYProperty, ref _translateY, value);
    }
    

    static ImageViewer()
    {
        OverlayerProperty.Changed.AddClassHandler<ImageViewer>((o, e) => o.OnOverlayerChanged(e));
        SourceProperty.Changed.AddClassHandler<ImageViewer>((o, e) => o.OnSourceChanged(e));
    }
    
    private void OnOverlayerChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var control = args.GetNewValue<object?>();
        if (control is Control c)
        {
            AdornerLayer.SetAdorner(this, c);
        }
    }

    private void OnSourceChanged(AvaloniaPropertyChangedEventArgs args)
    {
        IImage image = args.GetNewValue<IImage>();
        Size size = image.Size;
        double width = this.Width;
        double height = this.Height;
        Scale = Math.Min(width/size.Width, height/size.Height);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _image = e.NameScope.Get<Image>(PART_Image);
        _layer = e.NameScope.Get<VisualLayerManager>(PART_Layer);
        Scale = 1;
        if (Overlayer is Control c)
        {
            AdornerLayer.SetAdorner(this, c);
        }
    }



    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        if(e.Delta.Y > 0)
        {
            Scale *= 1.1;
        }
        else
        {
            var scale = Scale;
            scale /= 1.1;
            if (scale < 0.1) scale = 0.1;
            Scale = scale;
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (e.Pointer.Captured == this && _lastClickPoint != null)
        {
            Point p = e.GetPosition(this);
            double deltaX = p.X - _lastClickPoint.Value.X;
            double deltaY = p.Y - _lastClickPoint.Value.Y;
            TranslateX = deltaX + (_lastReleasePoint?.X ?? 0);
            TranslateY = deltaY + (_lastReleasePoint?.Y ?? 0);
        }
        
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        e.Pointer.Capture(this);
        _lastClickPoint = e.GetPosition(this);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        e.Pointer.Capture(null);
        _lastReleasePoint = new Point(TranslateX, TranslateY);
    }
}