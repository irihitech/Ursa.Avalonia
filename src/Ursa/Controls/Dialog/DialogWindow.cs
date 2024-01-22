using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
public class DialogWindow: Window
{
    public const string PART_CloseButton = "PART_CloseButton";
    
    protected override Type StyleKeyOverride { get; } = typeof(DialogWindow);

    private Button? _closeButton;

    protected override void OnDataContextBeginUpdate()
    {
        base.OnDataContextBeginUpdate();
        if (DataContext is IDialogContext context)
        {
            context.Closed-= OnDialogClose;
        }
    }

    protected override void OnDataContextEndUpdate()
    {
        base.OnDataContextEndUpdate();
        if (DataContext is IDialogContext context)
        {
            context.Closed += OnDialogClose;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        
        if (_closeButton != null)
        {
            _closeButton.Click += OnDefaultClose;
        }
    }

    private void OnDialogClose(object? sender, object? args)
    {
        Close(args);
    }

    private void OnDefaultClose(object sender, RoutedEventArgs args)
    {
        if (DataContext is IDialogContext context)
        {
            Close(context.DefaultCloseResult);
        }
        else
        {
            Close(null);
        }
    }
}