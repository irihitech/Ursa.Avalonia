using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(NumPad.PART_Seven, typeof(Button))]
public class NumPad: TemplatedControl
{
    public const string PART_Seven = "PART_Seven";
    private Button? _sevenButton;
    public static readonly StyledProperty<InputElement?> TargetProperty = AvaloniaProperty.Register<NumPad, InputElement?>(
        nameof(Target));

    public InputElement? Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public static readonly AttachedProperty<bool> AttachProperty =
        AvaloniaProperty.RegisterAttached<NumPad, InputElement, bool>("Attach");

    public static void SetAttach(InputElement obj, bool value) => obj.SetValue(AttachProperty, value);
    public static bool GetAttach(InputElement obj) => obj.GetValue(AttachProperty);

    static NumPad()
    {
        TargetProperty.Changed.AddClassHandler<NumPad, InputElement?>((n, args) => n.OnTargetChanged(args));
        AttachProperty.Changed.AddClassHandler<InputElement, bool>((input, args)=> OnAttachNumPad(input, args));
    }

    private static void OnAttachNumPad(InputElement input, AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.NewValue.Value)
        {
            GotFocusEvent.AddHandler(OnTargetGotFocus, input);
        }
        else
        {
            GotFocusEvent.RemoveHandler(OnTargetGotFocus, input);
        }
    }

    private void OnTargetChanged(AvaloniaPropertyChangedEventArgs<InputElement?> args)
    {
        //GotFocusEvent.RemoveHandler(OnTargetGotFocus, args.OldValue.Value);
        //GotFocusEvent.AddHandler(OnTargetGotFocus, args.NewValue.Value);
    }
    
    private static void OnTargetGotFocus(object sender, GotFocusEventArgs e)
    {
        if (sender is not InputElement) return;
        var existing = OverlayDialog.Recall<NumPad>(null);
        if (existing is not null)
        {
            existing.Target = sender as InputElement;
            return;
        }
        var numPad = new NumPad() { Target = sender as InputElement };
        OverlayDialog.ShowCustom(numPad, new object());
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnSevenButtonClick, _sevenButton);
        _sevenButton = e.NameScope.Find<Button>(PART_Seven);
        Button.ClickEvent.AddHandler(OnSevenButtonClick, _sevenButton);
    }

    private void OnSevenButtonClick(object sender, RoutedEventArgs e)
    {
        Target?.RaiseEvent(new TextInputEventArgs()
        {
            Source = this,
            RoutedEvent = TextInputEvent,
            Text = "7",
        });
    }
}