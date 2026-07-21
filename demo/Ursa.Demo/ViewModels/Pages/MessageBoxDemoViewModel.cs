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

namespace Ursa.Demo.ViewModels;

public class MessageBoxDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_MessageBox,
        Description = LanguageManager.Instance.Page_Description_MessageBox,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_MessageBox)],
        Tags = ["MessageBox", "Dialog", "Alert"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/MessageBoxDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/MessageBoxDemoViewModel.cs",
        InlineXamlSupport = false,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    private readonly string _longMessage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

    private readonly string _shortMessage = "Welcome to Ursa Avalonia!";
    private string _message;
    private string? _title;

    public ICommand DefaultMessageBoxCommand { get; set; }
    public ICommand ObservableDemoCommand { get; set; }
    public ICommand OkCommand { get; set; }
    public ICommand YesNoCommand { get; set; }
    public ICommand YesNoCancelCommand { get; set; }
    public ICommand OkCancelCommand { get; set; }
    
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

    private bool _useLong;

    public bool UseLong
    {
        get => _useLong;
        set
        {
            SetProperty(ref _useLong, value);
            _message = value ? _longMessage : _shortMessage;
        }
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
        Icons = new ObservableCollection<MessageBoxIcon>(
            Enum.GetValues<MessageBoxIcon>());
        SelectedIcon = MessageBoxIcon.None;
        _message = _shortMessage;
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

    private async Task Show(MessageBoxButton button)
    {
        if (UseOverlay)
        {
            Result = await OverlayMessageBox.ShowAsync(_message, _title, icon: SelectedIcon, button:button);
        }
        else
        {
            if (OperatingSystem.IsBrowser() || OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
            {
                await OverlayMessageBox.ShowAsync("Only overlay message box is supported on this platform.",
                    "Ursa MessageBox", button: MessageBoxButton.OK, icon: MessageBoxIcon.Error);
                return;
            }
            Result = await MessageBox.ShowAsync(_message, _title, icon: SelectedIcon, button:button);
        }
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
