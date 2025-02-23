using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Ursa.Controls;

public abstract class SplashWindow: Window
{
    public static readonly StyledProperty<TimeSpan?> CountDownProperty = AvaloniaProperty.Register<SplashWindow, TimeSpan?>(
        nameof(CountDown));

    public TimeSpan? CountDown
    {
        get => GetValue(CountDownProperty);
        set => SetValue(CountDownProperty, value);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (CountDown != null && CountDown != TimeSpan.Zero)
        {
            DispatcherTimer.RunOnce(Close, CountDown.Value);
        }
    }

    protected virtual Task<bool> CanClose() => Task.FromResult(true);
    protected abstract Task<Window> CreateNextWindow();
    
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
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
                {
                    lifetime.MainWindow = nextWindow;
                }
                nextWindow.Show();
                Close();
                return;
            }
        }
        base.OnClosing(e);
        
    }
}