using System;
using Avalonia;
using Avalonia.Controls;
using Ursa.Demo.Localizations;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Controls;

public partial class DemoSectionView : UserControl
{
    public static readonly StyledProperty<IObservable<string?>?> SectionTagDisplayTextProperty =
        AvaloniaProperty.Register<DemoSectionView, IObservable<string?>?>(nameof(SectionTagDisplayText));

    public static readonly StyledProperty<DemoSectionViewModel?> SectionContextProperty =
        AvaloniaProperty.Register<DemoSectionView, DemoSectionViewModel?>(nameof(SectionContext));

    static DemoSectionView()
    {
        SectionContextProperty.Changed.AddClassHandler<DemoSectionView>((sender, e) =>
        {
            var sectionContext = e.NewValue as DemoSectionViewModel;
            sender.UpdateSectionTagDisplay(sectionContext?.SectionTag ?? DemoSectionTag.None);
        });
    }

    public DemoSectionViewModel? SectionContext
    {
        get => GetValue(SectionContextProperty);
        set => SetValue(SectionContextProperty, value);
    }

    public IObservable<string?>? SectionTagDisplayText
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

    private void UpdateSectionTagDisplay(DemoSectionTag sectionTag)
    {
        SectionTagDisplayText = sectionTag switch
        {
            DemoSectionTag.Function => LanguageManager.Instance.DemoSection_Tag_Function,
            DemoSectionTag.Style => LanguageManager.Instance.DemoSection_Tag_Style,
            DemoSectionTag.Others => LanguageManager.Instance.DemoSection_Tag_Others,
            _ => null
        };
    }
}
