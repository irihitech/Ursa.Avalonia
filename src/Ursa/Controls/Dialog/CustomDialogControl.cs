using Avalonia;
using Avalonia.Controls.Primitives;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class CustomDialogControl : DialogControlBase
{
    
    public static readonly StyledProperty<Thickness> ContentMarginProperty =
        AvaloniaProperty.Register<CustomDialogControl, Thickness>(
            nameof(ContentMargin),
            new Thickness(0)
        );
    public Thickness ContentMargin
    {
        get => GetValue(ContentMarginProperty);
        set => SetValue(ContentMarginProperty, value);
    }
    
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<CustomDialogControl, string>(
            nameof(ContentMargin),
            string.Empty
        );
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == TitleProperty)
        {
            ContentMargin = string.IsNullOrWhiteSpace(Title)
                ? new Thickness(0,0,0,0)
                : new Thickness(0,48,0,0); 
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var closeButtonVisible =  IsCloseButtonVisible ??DataContext is IDialogContext;
        IsHitTestVisibleProperty.SetValue(closeButtonVisible, _closeButton);
        if (!closeButtonVisible)
        {
            OpacityProperty.SetValue(0, _closeButton);
        }
    }

    public override void Close()
    {
        if (DataContext is IDialogContext context)
            context.Close();
        else
            OnElementClosing(this, null);
    }
}