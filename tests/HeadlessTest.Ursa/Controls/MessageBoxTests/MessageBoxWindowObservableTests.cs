using Avalonia.Headless.XUnit;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.MessageBoxTests;

#region Observable Helpers

internal sealed class ReturnObservable(string value) : IObservable<string>
{
    public IDisposable Subscribe(IObserver<string> observer)
    {
        observer.OnNext(value);
        observer.OnCompleted();
        return new EmptyDisposable();
    }
}

internal sealed class TestObservable : IObservable<string>
{
    public int SubscribeCount { get; private set; }
    public IObserver<string>? CurrentObserver { get; private set; }

    public IDisposable Subscribe(IObserver<string> observer)
    {
        SubscribeCount++;
        CurrentObserver = observer;
        return new UnsubscribeAction(() =>
        {
            if (ReferenceEquals(CurrentObserver, observer))
                CurrentObserver = null;
        });
    }
}

internal sealed class EmptyDisposable : IDisposable
{
    public void Dispose() { }
}

internal sealed class UnsubscribeAction(Action onDispose) : IDisposable
{
    public void Dispose() => onDispose();
}

#endregion

public class MessageBoxWindowObservableTests
{
    [AvaloniaFact]
    public void Window_TitleSource_ReceivesValue_OnSubscribe()
    {
        var window = new MessageBoxWindow(MessageBoxButton.OK);
        var obs = new ReturnObservable("Test Title");

        window.TitleSource = obs;

        Assert.Equal("Test Title", window.Title);
    }

    [AvaloniaFact]
    public void Window_ContentSource_ReceivesValue_OnSubscribe()
    {
        var window = new MessageBoxWindow(MessageBoxButton.OK);
        var obs = new ReturnObservable("Test Content");

        window.ContentSource = obs;

        Assert.Equal("Test Content", window.Content);
    }

    [AvaloniaFact]
    public void Window_TitleSource_Null_DoesNotThrow()
    {
        var window = new MessageBoxWindow(MessageBoxButton.OK);
        window.Title = "Original Title";

        window.TitleSource = null;

        Assert.Equal("Original Title", window.Title);
    }

    [AvaloniaFact]
    public void Window_ContentSource_Null_DoesNotThrow()
    {
        var window = new MessageBoxWindow(MessageBoxButton.OK);
        window.Content = "Original Content";

        window.ContentSource = null;

        Assert.Equal("Original Content", window.Content);
    }

    [AvaloniaFact]
    public void Window_TitleSource_ChangingObservable_DisposesPrevious()
    {
        var window = new MessageBoxWindow(MessageBoxButton.OK);
        var first = new TestObservable();
        var second = new ReturnObservable("Second Title");

        window.TitleSource = first;
        Assert.Equal(1, first.SubscribeCount);

        window.TitleSource = second;
        Assert.Null(first.CurrentObserver);
        Assert.Equal("Second Title", window.Title);
    }

    [AvaloniaFact]
    public void Window_ContentSource_ChangingObservable_DisposesPrevious()
    {
        var window = new MessageBoxWindow(MessageBoxButton.OK);
        var first = new TestObservable();
        var second = new ReturnObservable("Second Content");

        window.ContentSource = first;
        Assert.Equal(1, first.SubscribeCount);

        window.ContentSource = second;
        Assert.Null(first.CurrentObserver);
        Assert.Equal("Second Content", window.Content);
    }

    [AvaloniaFact]
    public void Window_TitleSource_ObservableEmitsMultipleValues()
    {
        var window = new MessageBoxWindow(MessageBoxButton.OK);
        var obs = new TestObservable();

        window.TitleSource = obs;
        Assert.Equal(1, obs.SubscribeCount);

        obs.CurrentObserver?.OnNext("First");
        Assert.Equal("First", window.Title);

        obs.CurrentObserver?.OnNext("Second");
        Assert.Equal("Second", window.Title);
    }

    [AvaloniaFact]
    public void Window_ContentSource_ObservableEmitsMultipleValues()
    {
        var window = new MessageBoxWindow(MessageBoxButton.OK);
        var obs = new TestObservable();

        window.ContentSource = obs;
        Assert.Equal(1, obs.SubscribeCount);

        obs.CurrentObserver?.OnNext("First");
        Assert.Equal("First", window.Content);

        obs.CurrentObserver?.OnNext("Second");
        Assert.Equal("Second", window.Content);
    }

    [AvaloniaFact]
    public void Window_OnClosed_DisposesSubscriptions()
    {
        var window = new MessageBoxWindow(MessageBoxButton.OK);
        var titleObs = new TestObservable();
        var contentObs = new TestObservable();

        window.TitleSource = titleObs;
        window.ContentSource = contentObs;

        Assert.NotNull(titleObs.CurrentObserver);
        Assert.NotNull(contentObs.CurrentObserver);

        window.Close(MessageBoxResult.OK);

        Assert.Null(titleObs.CurrentObserver);
        Assert.Null(contentObs.CurrentObserver);
    }
}
