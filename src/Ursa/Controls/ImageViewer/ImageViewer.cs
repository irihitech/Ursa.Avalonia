using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Ursa.Controls;

[TemplatePart(PART_Image, typeof(Image))]
[TemplatePart(PART_Layer, typeof(VisualLayerManager))]
[PseudoClasses(PC_Moving)]
public class ImageViewer: TemplatedControl
{
    public const string PART_Image = "PART_Image";
    public const string PART_Layer = "PART_Layer";
    public const string PC_Moving = ":moving";
    
    private Image? _image = null!;
    private VisualLayerManager? _layer;
    private Point? _lastClickPoint;
    private Point? _lastlocation;

    public static readonly StyledProperty<Control?> OverlayerProperty = AvaloniaProperty.Register<ImageViewer, Control?>(
        nameof(Overlayer));

    public Control? Overlayer
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
        nameof(Scale), o => o.Scale, (o,v)=> o.Scale = v, unsetValue: 1);

    public double Scale
    {
        get => _scale;
        set => SetAndRaise(ScaleProperty, ref _scale, value);
    }

    private double _translateX;

    public static readonly DirectProperty<ImageViewer, double> TranslateXProperty = AvaloniaProperty.RegisterDirect<ImageViewer, double>(
        nameof(TranslateX), o => o.TranslateX, (o,v)=>o.TranslateX = v, unsetValue: 0);

    public double TranslateX
    {
        get => _translateX;
        set => SetAndRaise(TranslateXProperty, ref _translateX, value);
    }

    private double _translateY;

    public static readonly DirectProperty<ImageViewer, double> TranslateYProperty =
        AvaloniaProperty.RegisterDirect<ImageViewer, double>(
            nameof(TranslateY), o => o.TranslateY, (o, v) => o.TranslateY = v, unsetValue: 0);

    public double TranslateY
    {
        get => _translateY;
        set => SetAndRaise(TranslateYProperty, ref _translateY, value);
    }
    

    static ImageViewer()
    {
        OverlayerProperty.Changed.AddClassHandler<ImageViewer>((o, e) => o.OnOverlayerChanged(e));
        SourceProperty.Changed.AddClassHandler<ImageViewer>((o, e) => o.OnSourceChanged(e));
        TranslateXProperty.Changed.AddClassHandler<ImageViewer>((o,e)=>o.OnTranslateXChanged(e));
        TranslateYProperty.Changed.AddClassHandler<ImageViewer>((o, e) => o.OnTranslateYChanged(e));
    }

    private void OnTranslateYChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (PseudoClasses.Contains(PC_Moving)) return;
        var newValue = args.GetNewValue<double>();
        if (_lastlocation is not null)
        {
            _lastlocation = _lastlocation.Value.WithY(newValue);
        }
        else
        {
            _lastlocation = new Point(0, newValue);
        }
    }

    private void OnTranslateXChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (PseudoClasses.Contains(PC_Moving)) return;
        var newValue = args.GetNewValue<double>();
        if (_lastlocation is not null)
        {
            _lastlocation = _lastlocation.Value.WithX(newValue);
        }
        else
        {
            _lastlocation = new Point(newValue, 0);
        }
    }

    private void OnOverlayerChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var control = args.GetNewValue<Control?>();
        if (control is { } c)
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
        if (_image is not null)
        {
            _image.Width = size.Width;
            _image.Height = size.Height;
        }
        Scale = Math.Max(width/size.Width, height/size.Height);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _image = e.NameScope.Get<Image>(PART_Image);
        _layer = e.NameScope.Get<VisualLayerManager>(PART_Layer);
        if (Source is { } i)
        {
            Size size = i.Size;
            double width = this.Width;
            double height = this.Height;
            _image.Width = size.Width;
            _image.Height = size.Height;
            Scale = Math.Max(width/size.Width, height/size.Height);
        }
        if (Overlayer is { } c)
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
            PseudoClasses.Set(PC_Moving, true);
            Point p = e.GetPosition(this);
            double deltaX = p.X - _lastClickPoint.Value.X;
            double deltaY = p.Y - _lastClickPoint.Value.Y;
            TranslateX = deltaX + (_lastlocation?.X ?? 0);
            TranslateY = deltaY + (_lastlocation?.Y ?? 0);
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
        _lastlocation = new Point(TranslateX, TranslateY);
        PseudoClasses.Set(PC_Moving, false);
    }
}