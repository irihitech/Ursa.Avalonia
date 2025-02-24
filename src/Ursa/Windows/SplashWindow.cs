using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

public abstract class SplashWindow: Window
{
    protected override Type StyleKeyOverride => typeof(SplashWindow);

    public static readonly StyledProperty<TimeSpan?> CountDownProperty = AvaloniaProperty.Register<SplashWindow, TimeSpan?>(
        nameof(CountDown));

    public TimeSpan? CountDown
    {
        get => GetValue(CountDownProperty);
        set => SetValue(CountDownProperty, value);
    }
    
    static SplashWindow()
    {
        DataContextProperty.Changed.AddClassHandler<SplashWindow, object?>((window, e) =>
            window.OnDataContextChange(e));
    }
    
    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogContext oldContext) oldContext.RequestClose -= OnContextRequestClose;

        if (args.NewValue.Value is IDialogContext newContext) newContext.RequestClose += OnContextRequestClose;
    }
    
    private void OnContextRequestClose(object? sender, object? args)
    {
        DialogResult = args;
        Close();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (CountDown != null && CountDown != TimeSpan.Zero)
        {
            DispatcherTimer.RunOnce(Close, CountDown.Value);
        }
    }
    
    protected object? DialogResult { get; private set; }

    protected virtual Task<bool> CanClose() => Task.FromResult(true);
    protected abstract Task<Window?> CreateNextWindow();
    
    private bool _canClose;
    
    protected override sealed async void OnClosing(WindowClosingEventArgs e)
    {
        VerifyAccess();
        if (!_canClose)
        {
            e.Cancel = true;
            _canClose = await CanClose();
            if (_canClose)
            {
                var nextWindow = await CreateNextWindow();
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime && nextWindow is not null)
                {
                    lifetime.MainWindow = nextWindow;
                }
                nextWindow?.Show();
                Close();
                if (DataContext is IDialogContext idc)
                {
                    // unregister in advance in case developer try to raise event again. 
                    idc.RequestClose -= OnContextRequestClose;
                    idc.Close();
                }
                return;
            }
        }
        base.OnClosing(e);
        
    }
}