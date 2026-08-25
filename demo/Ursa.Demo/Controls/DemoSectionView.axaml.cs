using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Controls;

public partial class DemoSectionView : UserControl
{
    public static readonly StyledProperty<string?> HeaderProperty =
        AvaloniaProperty.Register<DemoSectionView, string?>(nameof(Header));

    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<DemoSectionView, string?>(nameof(Description));

    public static readonly StyledProperty<string?> AnchorIdProperty =
        AvaloniaProperty.Register<DemoSectionView, string?>(nameof(AnchorId));

    public static readonly StyledProperty<IEnumerable<DemoSectionCodeSnippetViewModel>?> CodeSnippetsProperty =
        AvaloniaProperty.Register<DemoSectionView, IEnumerable<DemoSectionCodeSnippetViewModel>?>(nameof(CodeSnippets));

    public static readonly StyledProperty<object?> DemoContentProperty =
        AvaloniaProperty.Register<DemoSectionView, object?>(nameof(DemoContent));

    public string? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public string? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public string? AnchorId
    {
        get => GetValue(AnchorIdProperty);
        set => SetValue(AnchorIdProperty, value);
    }

    public IEnumerable<DemoSectionCodeSnippetViewModel>? CodeSnippets
    {
        get => GetValue(CodeSnippetsProperty);
        set => SetValue(CodeSnippetsProperty, value);
    }

    public object? DemoContent
    {
        get => GetValue(DemoContentProperty);
        set => SetValue(DemoContentProperty, value);
    }

    public DemoSectionView()
    {
        CodeSnippets = [];
        InitializeComponent();
    }
}
