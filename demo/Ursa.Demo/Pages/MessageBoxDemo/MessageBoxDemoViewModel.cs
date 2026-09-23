using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Irihi.Dogma.Controls;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.MessageBoxDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(MessageBoxDemo))]
public class MessageBoxDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "MessageBox";
    public const string Menu_Header = "Menu_Header_MessageBox";
    private const string IconAnchorId = "message-box-icon";
    private const string TitleAnchorId = "message-box-title";
    private const string ButtonCombinationsAnchorId = "message-box-button-combinations";
    private const string PresentationModesAnchorId = "message-box-presentation-modes";
    private const string StyleClassAnchorId = "message-box-style-class";
    private const string ObservableContentAnchorId = "message-box-observable-content";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_MessageBox,
        Description = LanguageManager.Instance.Page_Description_MessageBox,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_MessageBox)],
        Tags = ["MessageBox", "Dialog", "Alert"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/MessageBoxDemo/MessageBoxDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/MessageBoxDemo/MessageBoxDemoViewModel.cs",
        InlineXamlSupport = false,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    private const string DefaultMessage = "Welcome to Ursa Avalonia!";
    private string? _title;

    public ICommand DefaultMessageBoxCommand { get; set; }
    public ICommand ObservableDemoCommand { get; set; }
    public ICommand OkCommand { get; set; }
    public ICommand YesNoCommand { get; set; }
    public ICommand YesNoCancelCommand { get; set; }
    public ICommand OkCancelCommand { get; set; }
    public ICommand CompactMessageBoxCommand { get; set; }

    public DemoSectionViewModel IconSection { get; }
    public DemoSectionViewModel TitleSection { get; }
    public DemoSectionViewModel ButtonCombinationsSection { get; }
    public DemoSectionViewModel PresentationModesSection { get; }
    public DemoSectionViewModel StyleClassSection { get; }
    public DemoSectionViewModel ObservableContentSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_MessageBox_Section_Icon_Header,
            AnchorId = IconAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_MessageBox_Section_Title_Header,
            AnchorId = TitleAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_MessageBox_Section_Button_Combinations_Header,
            AnchorId = ButtonCombinationsAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_MessageBox_Section_Presentation_Modes_Header,
            AnchorId = PresentationModesAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_MessageBox_Section_Style_Class_Header,
            AnchorId = StyleClassAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_MessageBox_Section_Observable_Content_Header,
            AnchorId = ObservableContentAnchorId
        },
    ];
    
    public ObservableCollection<MessageBoxIcon> Icons { get; set; }
    
    private MessageBoxIcon _selectedIcon;
    public MessageBoxIcon SelectedIcon
    {
        get => _selectedIcon;
        set => SetProperty(ref _selectedIcon, value);
    }

    private MessageBoxResult _result;
    public MessageBoxResult Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    private bool _useTitle;

    public bool UseTitle
    {
        get => _useTitle;
        set
        {
            SetProperty(ref _useTitle, value);
            _title = value ? "Ursa MessageBox" : string.Empty;
        }
    }

    private bool _useOverlay;

    public bool UseOverlay
    {
        get => _useOverlay;
        set => SetProperty(ref _useOverlay, value);
    }

    public MessageBoxDemoViewModel()
    {
        DefaultMessageBoxCommand = new AsyncRelayCommand(OnDefaultMessageAsync);
        ObservableDemoCommand = new AsyncRelayCommand(OnObservableDemoAsync);
        OkCommand = new AsyncRelayCommand(OnOkAsync);
        YesNoCommand = new AsyncRelayCommand(OnYesNoAsync);
        YesNoCancelCommand = new AsyncRelayCommand(OnYesNoCancelAsync);
        OkCancelCommand = new AsyncRelayCommand(OnOkCancelAsync);
        CompactMessageBoxCommand = new AsyncRelayCommand(OnCompactMessageBoxAsync);
        Icons = new ObservableCollection<MessageBoxIcon>(
            Enum.GetValues<MessageBoxIcon>());
        SelectedIcon = MessageBoxIcon.None;

        IconSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_MessageBox_Section_Icon_Header,
            Descriptions = { LanguageManager.Instance.Page_MessageBox_Section_Icon_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = IconAnchorId
        };
        AddSnippet(IconSection, CodeLanguage.CSharp, LanguageManager.Instance.DemoSection_Tab_ViewModel, """
            var result = await MessageBox.ShowAsync(
                "Welcome to Ursa Avalonia!",
                icon: MessageBoxIcon.Information);
            """);

        TitleSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_MessageBox_Section_Title_Header,
            Descriptions = { LanguageManager.Instance.Page_MessageBox_Section_Title_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = TitleAnchorId
        };
        AddSnippet(TitleSection, CodeLanguage.CSharp, LanguageManager.Instance.DemoSection_Tab_ViewModel, """
            var result = await MessageBox.ShowAsync(
                "Welcome to Ursa Avalonia!",
                title: "Ursa MessageBox");
            """);

        ButtonCombinationsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_MessageBox_Section_Button_Combinations_Header,
            Descriptions = { LanguageManager.Instance.Page_MessageBox_Section_Button_Combinations_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ButtonCombinationsAnchorId
        };
        AddSnippet(ButtonCombinationsSection, CodeLanguage.CSharp, LanguageManager.Instance.DemoSection_Tab_ViewModel, """
            var result = await MessageBox.ShowAsync(
                "Continue with this operation?",
                icon: MessageBoxIcon.Question,
                button: MessageBoxButton.YesNoCancel);
            """);

        PresentationModesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_MessageBox_Section_Presentation_Modes_Header,
            Descriptions = { LanguageManager.Instance.Page_MessageBox_Section_Presentation_Modes_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = PresentationModesAnchorId
        };
        AddSnippet(PresentationModesSection, CodeLanguage.CSharp, LanguageManager.Instance.DemoSection_Tab_ViewModel, """
            var result = UseOverlay
                ? await OverlayMessageBox.ShowAsync(message, title, icon: SelectedIcon)
                : await MessageBox.ShowAsync(message, title, icon: SelectedIcon);
            """);

        StyleClassSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_MessageBox_Section_Style_Class_Header,
            Descriptions = { LanguageManager.Instance.Page_MessageBox_Section_Style_Class_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = StyleClassAnchorId
        };
        AddSnippet(StyleClassSection, CodeLanguage.Axaml, LanguageManager.Instance.DemoSection_Tab_Xaml, """
            <Style Selector="u|MessageBoxWindow.Compact, u|MessageBoxControl.Compact">
                <Setter Property="Padding" Value="24 12" />
            </Style>
            """);
        AddSnippet(StyleClassSection, CodeLanguage.CSharp, LanguageManager.Instance.DemoSection_Tab_ViewModel, """
            await OverlayMessageBox.ShowAsync(
                message,
                title,
                styleClass: "Compact");
            """);

        ObservableContentSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_MessageBox_Section_Observable_Content_Header,
            Descriptions = { LanguageManager.Instance.Page_MessageBox_Section_Observable_Content_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ObservableContentAnchorId
        };
        AddSnippet(ObservableContentSection, CodeLanguage.CSharp, LanguageManager.Instance.DemoSection_Tab_ViewModel, """
            await OverlayMessageBox.ShowAsync(
                messageSource,
                titleSource,
                icon: SelectedIcon);
            """);
    }

    private async Task OnDefaultMessageAsync()
    {
        await Show(MessageBoxButton.OK);
    }

    private async Task OnObservableDemoAsync()
    {
        // Demonstrate dynamic observable: title updates every second, 5 ticks total
        var titleObs = CreateIntervalObservable(TimeSpan.FromSeconds(1), 5, i => $"Observing... {i + 1}s");
        var messageObs = CreateReturnObservable(
            "This message is delivered via IObservable<string>.\nWatch the title change every second.");

        if (UseOverlay)
        {
            Result = await OverlayMessageBox.ShowAsync(messageObs, titleObs, icon: SelectedIcon);
        }
        else
        {
            Result = await MessageBox.ShowAsync(messageObs, titleObs, icon: SelectedIcon);
        }
    }
    
    private async Task OnOkAsync()
    {
        await Show(MessageBoxButton.OK);
    }
    
    private async Task OnYesNoAsync()
    {
        await Show(MessageBoxButton.YesNo);
    }
    
    private async Task OnYesNoCancelAsync()
    {
        await Show(MessageBoxButton.YesNoCancel);
    }

    private async Task OnOkCancelAsync()
    {
        await Show(MessageBoxButton.OKCancel);
    }

    private async Task OnCompactMessageBoxAsync()
    {
        await Show(MessageBoxButton.OK, "Compact");
    }

    private async Task Show(MessageBoxButton button, string? styleClass = null)
    {
        if (UseOverlay)
        {
            Result = await OverlayMessageBox.ShowAsync(DefaultMessage, _title, icon: SelectedIcon, button:button, styleClass: styleClass);
        }
        else
        {
            if (OperatingSystem.IsBrowser() || OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
            {
                await OverlayMessageBox.ShowAsync("Only overlay message box is supported on this platform.",
                    "Ursa MessageBox", button: MessageBoxButton.OK, icon: MessageBoxIcon.Error);
                return;
            }
            Result = await MessageBox.ShowAsync(DefaultMessage, _title, icon: SelectedIcon, button:button, styleClass: styleClass);
        }
    }

    private static void AddSnippet(
        DemoSectionViewModel section,
        CodeLanguage language,
        IObservable<string?> tabName,
        string code)
    {
        section.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = language,
            TabName = tabName,
            CodeSnippet = code
        });
    }

    #region Minimal Observable Helpers (no System.Reactive required)

    private static IObservable<T> CreateReturnObservable<T>(T value)
    {
        return new ReturnObservable<T>(value);
    }

    private static IObservable<T> CreateIntervalObservable<T>(TimeSpan interval, int count, Func<int, T> selector)
    {
        return new IntervalObservable<T>(interval, count, selector);
    }

    private sealed class ReturnObservable<T>(T value) : IObservable<T>
    {
        public IDisposable Subscribe(IObserver<T> observer)
        {
            observer.OnNext(value);
            observer.OnCompleted();
            return EmptyDisposable.Instance;
        }
    }

    private sealed class IntervalObservable<T> : IObservable<T>
    {
        private readonly TimeSpan _interval;
        private readonly int _count;
        private readonly Func<int, T> _selector;

        public IntervalObservable(TimeSpan interval, int count, Func<int, T> selector)
        {
            _interval = interval;
            _count = count;
            _selector = selector;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_count <= 0)
            {
                observer.OnCompleted();
                return EmptyDisposable.Instance;
            }

            var cts = new CancellationTokenSource();
            _ = RunAsync(observer, cts.Token);
            return new CancellationDisposable(cts);
        }

        private async Task RunAsync(IObserver<T> observer, CancellationToken ct)
        {
            try
            {
                for (int i = 0; i < _count; i++)
                {
                    await Task.Delay(_interval, ct);
                    if (ct.IsCancellationRequested) return;
                    observer.OnNext(_selector(i));
                }
                observer.OnCompleted();
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                observer.OnError(ex);
            }
        }
    }

    private sealed class EmptyDisposable : IDisposable
    {
        public static readonly EmptyDisposable Instance = new();
        public void Dispose() { }
    }

    private sealed class CancellationDisposable(CancellationTokenSource cts) : IDisposable
    {
        public void Dispose() => cts.Cancel();
    }

    #endregion
}
