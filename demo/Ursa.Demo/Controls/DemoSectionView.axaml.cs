using Avalonia;
using Avalonia.Controls;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Controls;

public partial class DemoSectionView : UserControl
{
    public static readonly StyledProperty<DemoSectionViewModel?> SectionContextProperty =
        AvaloniaProperty.Register<DemoSectionView, DemoSectionViewModel?>(nameof(SectionContext));

    public static readonly StyledProperty<object?> DemoContentProperty =
        AvaloniaProperty.Register<DemoSectionView, object?>(nameof(DemoContent));

    public DemoSectionViewModel? SectionContext
    {
        get => GetValue(SectionContextProperty);
        set => SetValue(SectionContextProperty, value);
    }

    public object? DemoContent
    {
        get => GetValue(DemoContentProperty);
        set => SetValue(DemoContentProperty, value);
    }

    public DemoSectionView()
    {
        SectionContext = new DemoSectionViewModel();
        InitializeComponent();
    }
}
