using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Selection;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_Indicator, typeof(Control))]
public class SelectionList: SelectingItemsControl
{
    public const string PART_Indicator = "PART_Indicator";
    private static readonly FuncTemplate<Panel?> DefaultPanel = new(() => new StackPanel());
    
    private Control? _indicator;
    private ImplicitAnimationCollection? _implicitAnimations;
    
    static SelectionList()
    {
        SelectionModeProperty.OverrideMetadata<SelectionList>(
            new StyledPropertyMetadata<SelectionMode>(
            defaultValue: SelectionMode.Single, 
            coerce: (o, mode) => SelectionMode.Single)
            );
        SelectedItemProperty.Changed.AddClassHandler<SelectionList, object?>((list, args) =>
            list.OnSelectedItemChanged(args));
    }

    private void OnSelectedItemChanged(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        var newValue = args.NewValue.Value;
        if (newValue is null)
        {
            OpacityProperty.SetValue(0d, _indicator);
            return;
        }
        var container = ContainerFromItem(newValue);
        if (container is null)
        {
            OpacityProperty.SetValue(0d, _indicator);
            return;
        }
        OpacityProperty.SetValue(1d, _indicator);
        InvalidateMeasure();
        InvalidateArrange();
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var size = base.ArrangeOverride(finalSize);
        if(_indicator is not null && SelectedItem is not null)
        {
            var container = ContainerFromItem(SelectedItem);
            if (container is null) return size;
            _indicator.Arrange(container.Bounds);
        }
        return size;
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<SelectionListItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new SelectionListItem();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _indicator = e.NameScope.Find<Control>(PART_Indicator);
        _indicator?.Arrange(new Rect());
        if (_indicator is not null)
        {
            _indicator.Opacity = 0;
            SetUpAnimation();
            if (ElementComposition.GetElementVisual(_indicator) is { } v)
            {
               v.ImplicitAnimations = _implicitAnimations;
            }
            _indicator.SizeChanged += OnIndicatorSizeChanged;
        }
    }

    private void OnIndicatorSizeChanged(object sender, SizeChangedEventArgs e)
    {
        
    }

    internal void SelectByIndex(int index)
    {
        using var operation = Selection.BatchUpdate();
        Selection.Clear();
        Selection.Select(index);
    }
    
    private void SetUpAnimation()
    {
        var compositor = ElementComposition.GetElementVisual(this)!.Compositor;
        var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
        offsetAnimation.Target = "Offset";
        offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
        offsetAnimation.Duration = TimeSpan.FromSeconds(0.3);
        var sizeAnimation = compositor.CreateVector2KeyFrameAnimation();
        sizeAnimation.Target = "Size";
        sizeAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
        sizeAnimation.Duration = TimeSpan.FromSeconds(0.3);
        var opacityAnimation = compositor.CreateScalarKeyFrameAnimation();
        opacityAnimation.Target = nameof(CompositionVisual.Opacity);
        opacityAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
        opacityAnimation.Duration = TimeSpan.FromSeconds(0.3);

        _implicitAnimations = compositor.CreateImplicitAnimationCollection();
        _implicitAnimations["Offset"] = offsetAnimation;
        _implicitAnimations["Size"] = sizeAnimation;
        _implicitAnimations["Opacity"] = opacityAnimation;
    }
    
}