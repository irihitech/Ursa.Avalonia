using Avalonia;
using Avalonia.Controls;
using Ursa.Common;

namespace Ursa.Controls;

public class Form: ItemsControl
{
    #region Attached Properties

    public static readonly AttachedProperty<object?> LabelProperty =
        AvaloniaProperty.RegisterAttached<Form, Control, object?>("Label");
    public static void SetLabel(Control obj, object? value) => obj.SetValue(LabelProperty, value);
    public static object? GetLabel(Control obj) => obj.GetValue(LabelProperty);
    

    public static readonly AttachedProperty<bool> IsRequiredProperty =
        AvaloniaProperty.RegisterAttached<Form, Control, bool>("IsRequired");
    public static void SetIsRequired(Control obj, bool value) => obj.SetValue(IsRequiredProperty, value);
    public static bool GetIsRequired(Control obj) => obj.GetValue(IsRequiredProperty);

    public static readonly AttachedProperty<GridLength> LabelWidthProperty =
        AvaloniaProperty.RegisterAttached<Form, Control, GridLength>("LabelWidth");

    public static void SetLabelWidth(Control obj, GridLength value) => obj.SetValue(LabelWidthProperty, value);
    public static GridLength GetLabelWidth(Control obj) => obj.GetValue(LabelWidthProperty);

    #endregion
    

    public static readonly StyledProperty<Position> LabelPositionProperty = AvaloniaProperty.Register<Form, Position>(
        nameof(LabelPosition));
    /// <summary>
    /// Only Left and Top work.
    /// </summary>
    public Position LabelPosition
    {
        get => GetValue(LabelPositionProperty);
        set => SetValue(LabelPositionProperty, value);
    }

    public static readonly StyledProperty<Position> LabelAlignmentProperty = AvaloniaProperty.Register<Form, Position>(
        nameof(LabelAlignment));
    /// <summary>
    /// Only Left and Right work. 
    /// </summary>
    public Position LabelAlignment
    {
        get => GetValue(LabelAlignmentProperty);
        set => SetValue(LabelAlignmentProperty, value);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is FormItem or FormGroup;
    }
    
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        if (item is not Control control) return new FormItem();
        return new FormItem()
        {
            Content = control,
            [!LabelProperty] = control.GetObservable(Form.LabelProperty).ToBinding(),
            [!IsRequiredProperty] = control.GetObservable(Form.IsRequiredProperty).ToBinding(),
        };
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is FormGroup group)
        {
            if (!group.IsSet(FormGroup.LabelWidthProperty))
            {
                group[!LabelWidthProperty] = this.GetObservable(LabelWidthProperty).ToBinding();
            }
        }
        else if (container is FormItem formItem)
        {
            if(!formItem.IsSet(FormItem.LabelWidthProperty))
            {
                formItem[!LabelWidthProperty] = this.GetObservable(LabelWidthProperty).ToBinding();
            }
        }
    }
}