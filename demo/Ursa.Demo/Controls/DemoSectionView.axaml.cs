using Avalonia;
using Avalonia.Controls;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Controls;

public partial class DemoSectionView : UserControl
{
    public static readonly StyledProperty<bool> HasSectionTagProperty =
        AvaloniaProperty.Register<DemoSectionView, bool>(nameof(HasSectionTag));

    public static readonly StyledProperty<string?> SectionTagDisplayTextProperty =
        AvaloniaProperty.Register<DemoSectionView, string?>(nameof(SectionTagDisplayText));

    public static readonly StyledProperty<DemoSectionViewModel?> SectionContextProperty =
        AvaloniaProperty.Register<DemoSectionView, DemoSectionViewModel?>(nameof(SectionContext));

    static DemoSectionView()
    {
        SectionContextProperty.Changed.AddClassHandler<DemoSectionView>((sender, e) =>
        {
            var oldContext = e.OldValue as DemoSectionViewModel;
            var newContext = e.NewValue as DemoSectionViewModel;
            sender.OnSectionContextChanged(oldContext, newContext);
        });
    }

    public DemoSectionViewModel? SectionContext
    {
        get => GetValue(SectionContextProperty);
        set => SetValue(SectionContextProperty, value);
    }

    public bool HasSectionTag
    {
        get => GetValue(HasSectionTagProperty);
        private set => SetValue(HasSectionTagProperty, value);
    }

    public string? SectionTagDisplayText
    {
        get => GetValue(SectionTagDisplayTextProperty);
        private set => SetValue(SectionTagDisplayTextProperty, value);
    }

    public DemoSectionView()
    {
        SectionContext = new DemoSectionViewModel();
        InitializeComponent();
        UpdateSectionTagDisplay(SectionContext.SectionTag);
    }

    private void OnSectionContextChanged(DemoSectionViewModel? oldContext, DemoSectionViewModel? newContext)
    {
        _ = oldContext;
        UpdateSectionTagDisplay(newContext?.SectionTag ?? DemoSectionTag.None);
    }

    private void UpdateSectionTagDisplay(DemoSectionTag sectionTag)
    {
        SectionTagDisplayText = sectionTag switch
        {
            DemoSectionTag.Function => "Function",
            DemoSectionTag.Style => "Style",
            DemoSectionTag.Others => "Others",
            _ => null
        };

        HasSectionTag = SectionTagDisplayText is not null;
    }
}
