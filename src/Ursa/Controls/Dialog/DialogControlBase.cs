using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Ursa.Common;
using Ursa.Controls.OverlayShared;
using Ursa.EventArgs;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_TitleArea, typeof(Panel))]
[PseudoClasses(PC_Modal)]
public abstract class DialogControlBase: OverlayFeedbackElement
{
    public const string PART_CloseButton = "PART_CloseButton";
    public const string PART_TitleArea = "PART_TitleArea";
    public const string PC_Modal = ":modal";
    
    internal HorizontalPosition HorizontalAnchor { get; set; } = HorizontalPosition.Center;
    internal VerticalPosition VerticalAnchor { get; set; } = VerticalPosition.Center;
    internal HorizontalPosition ActualHorizontalAnchor { get; set; }
    internal VerticalPosition ActualVerticalAnchor { get; set; }
    internal double? HorizontalOffset { get; set; }
    internal double? VerticalOffset { get; set; }
    internal double? HorizontalOffsetRatio { get; set; }
    internal double? VerticalOffsetRatio { get; set; }
    internal bool CanClickOnMaskToClose { get; set; }
    
    protected internal Button? _closeButton;
    private Panel? _titleArea;
    
    internal void SetAsModal(bool modal)
    {
        PseudoClasses.Set(PC_Modal, modal);
    }
    
    public static readonly RoutedEvent<DialogLayerChangeEventArgs> LayerChangedEvent = RoutedEvent.Register<CustomDialogControl, DialogLayerChangeEventArgs>(
        nameof(LayerChanged), RoutingStrategies.Bubble);
    public event EventHandler<DialogLayerChangeEventArgs> LayerChanged
    {
        add => AddHandler(LayerChangedEvent, value);
        remove => RemoveHandler(LayerChangedEvent, value);
    }

    public void UpdateLayer(object? o)
    {
        if (o is DialogLayerChangeType t)
        {
            RaiseEvent(new DialogLayerChangeEventArgs(LayerChangedEvent, t));
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _titleArea?.RemoveHandler(PointerMovedEvent, OnTitlePointerMove);
        _titleArea?.RemoveHandler(PointerPressedEvent, OnTitlePointerPressed);
        _titleArea?.RemoveHandler(PointerReleasedEvent, OnTitlePointerRelease);
        _titleArea = e.NameScope.Find<Panel>(PART_TitleArea);
        _titleArea?.AddHandler(PointerMovedEvent, OnTitlePointerMove, RoutingStrategies.Bubble);
        _titleArea?.AddHandler(PointerPressedEvent, OnTitlePointerPressed, RoutingStrategies.Bubble);
        _titleArea?.AddHandler(PointerReleasedEvent, OnTitlePointerRelease, RoutingStrategies.Bubble);
        EventHelper.UnregisterClickEvent(OnCloseButtonClick, _closeButton);
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        EventHelper.RegisterClickEvent(OnCloseButtonClick, _closeButton);
    }
    
    private void OnTitlePointerPressed(object sender, PointerPressedEventArgs e)
    {
        e.Source = this;
    }

    private void OnTitlePointerMove(object sender, PointerEventArgs e)
    {
        e.Source = this;
    }

    private void OnTitlePointerRelease(object sender, PointerReleasedEventArgs e)
    {
        e.Source = this;
    }
    
    private void OnCloseButtonClick(object sender, RoutedEventArgs args) => Close();
}