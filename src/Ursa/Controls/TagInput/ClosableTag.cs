using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(PathIcon))]
public class ClosableTag : ContentControl
{
    public const string PART_CloseButton = "PART_CloseButton";
    private PathIcon? _icon;

    public static readonly StyledProperty<ICommand?> CommandProperty = AvaloniaProperty.Register<ClosableTag, ICommand?>(
        nameof(Command));

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_icon is not null)
        {
            _icon.PointerReleased -= OnPointerReleased;
        }

        _icon = e.NameScope.Find<PathIcon>(PART_CloseButton);
        if (_icon is not null)
        {
            _icon.PointerReleased += OnPointerReleased;
        }
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs args)
    {
        if (Command is not null && Command.CanExecute(null))
        {
            Command.Execute(this);
        }
    }
}