using Ursa.Common;

namespace Ursa.Controls;

public record DialogOptions
{
    public bool ShowCloseButton { get; set; } = true;
    public string? Title { get; set; }
    public bool ExtendToClientArea { get; set; } = false;
    public DialogButton DefaultButtons { get; set; } = DialogButton.None;
}