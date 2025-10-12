using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

/// <summary>
/// LabeledContentControl is almost identical to HeaderedContentControl, but uses "Label" terminology instead of "Header".
/// This is to provide better semantic meaning of paired label and content.
/// Label part is recommended to be Label control for accessibility.
/// </summary>
[TemplatePart(PART_Label, typeof(Label))]
public abstract class LabeledContentControl: ContentControl
{
    public const string PART_Label = "PART_Label";
    
    public static readonly StyledProperty<object?> LabelProperty = AvaloniaProperty.Register<LabeledContentControl, object?>(
        nameof(Label));

    public object? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> LabelTemplateProperty = AvaloniaProperty.Register<LabeledContentControl, IDataTemplate?>(
        nameof(LabelTemplate));

    public IDataTemplate? LabelTemplate
    {
        get => GetValue(LabelTemplateProperty);
        set => SetValue(LabelTemplateProperty, value);
    }
    
    public Label? LabelHost
    {
        get;
        private set;
    }

    static LabeledContentControl()
    {
        LabelProperty.Changed.AddClassHandler<LabeledContentControl>((x, e) => x.LabelChanged(e));
    }

    private void LabelChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ILogical oldChild)
        {
            LogicalChildren.Remove(oldChild);
        }
        if (e.NewValue is ILogical newChild)
        {
            LogicalChildren.Add(newChild);
        }
        HookLabelToContent();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        LabelHost = e.NameScope.Find<Label>(PART_Label);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        HookLabelToContent();
    }
    
    protected virtual void HookLabelToContent()
    {
        if (LabelHost is null) return;
        // Set it directly if content is a control, this is faster than looking up logical tree. 
        if (Content is InputElement input)
        {
            LabelHost.Target = input;
        }
        else
        {
            var focusable = 
                Presenter?.GetSelfAndLogicalDescendants()
                    .OfType<InputElement>()
                    .FirstOrDefault(a => a.Focusable);
            if (focusable is not null)
            {
                LabelHost.Target = focusable;
            }
        }
    }
    
}