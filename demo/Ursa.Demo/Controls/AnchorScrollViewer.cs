using System;
using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Controls.Primitives;
using Avalonia.Reactive;

namespace Ursa.Demo.Controls;

/// <summary>
/// A demo ScrollViewer with an integrated Anchor navigation panel on the right side.
/// </summary>
public class AnchorScrollViewer : ScrollViewer
{
    private ScrollContentPresenter? _contentPresenter;
    private IDisposable? _contentPresenterBoundsSubscription;

    public static readonly StyledProperty<IEnumerable?> AnchorItemsProperty =
        AvaloniaProperty.Register<AnchorScrollViewer, IEnumerable?>(
            nameof(AnchorItems));

    public static readonly StyledProperty<IDataTemplate?> AnchorItemTemplateProperty =
        AvaloniaProperty.Register<AnchorScrollViewer, IDataTemplate?>(
            nameof(AnchorItemTemplate));

    public static readonly StyledProperty<double> AnchorTopOffsetProperty =
        AvaloniaProperty.Register<AnchorScrollViewer, double>(
            nameof(AnchorTopOffset), 0.0);

    public static readonly StyledProperty<double> MaxContentWidthProperty =
        AvaloniaProperty.Register<AnchorScrollViewer, double>(
            nameof(MaxContentWidth), double.PositiveInfinity);

    public static readonly StyledProperty<double> AnchorWidthProperty =
        AvaloniaProperty.Register<AnchorScrollViewer, double>(
            nameof(AnchorWidth), 240);

    public IEnumerable? AnchorItems
    {
        get => GetValue(AnchorItemsProperty);
        set => SetValue(AnchorItemsProperty, value);
    }

    public IDataTemplate? AnchorItemTemplate
    {
        get => GetValue(AnchorItemTemplateProperty);
        set => SetValue(AnchorItemTemplateProperty, value);
    }

    public double AnchorTopOffset
    {
        get => GetValue(AnchorTopOffsetProperty);
        set => SetValue(AnchorTopOffsetProperty, value);
    }

    public double MaxContentWidth
    {
        get => GetValue(MaxContentWidthProperty);
        set => SetValue(MaxContentWidthProperty, value);
    }

    public double AnchorWidth
    {
        get => GetValue(AnchorWidthProperty);
        set => SetValue(AnchorWidthProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PaddingProperty || change.Property == MaxContentWidthProperty)
        {
            UpdateContentPresenterPadding();
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _contentPresenterBoundsSubscription?.Dispose();
        _contentPresenter = e.NameScope.Find<ScrollContentPresenter>("PART_ContentPresenter");
        _contentPresenterBoundsSubscription = _contentPresenter?.GetObservable(BoundsProperty)
            .Subscribe(new AnonymousObserver<Rect>(_ => UpdateContentPresenterPadding()));

        UpdateContentPresenterPadding();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _contentPresenterBoundsSubscription?.Dispose();
        _contentPresenterBoundsSubscription = null;
        _contentPresenter = null;
    }

    private void UpdateContentPresenterPadding()
    {
        if (_contentPresenter is null)
            return;

        var padding = Padding;
        var width = _contentPresenter.Bounds.Width;

        if (double.IsNaN(width) || double.IsInfinity(MaxContentWidth) || MaxContentWidth <= 0)
        {
            _contentPresenter.Padding = padding;
            return;
        }

        var horizontalPadding = Math.Max((width - MaxContentWidth) / 2, 0);
        _contentPresenter.Padding = new Thickness(
            padding.Left + horizontalPadding,
            padding.Top,
            padding.Right + horizontalPadding,
            padding.Bottom);
    }
}
