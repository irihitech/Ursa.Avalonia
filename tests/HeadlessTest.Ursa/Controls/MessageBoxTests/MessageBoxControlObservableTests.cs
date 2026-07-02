using Avalonia.Headless.XUnit;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.MessageBoxTests;

public class MessageBoxControlObservableTests
{
    [AvaloniaFact]
    public void Control_TitleSource_ReceivesValue_OnSubscribe()
    {
        var control = new MessageBoxControl();
        var obs = new ReturnObservable("Test Title");

        control.TitleSource = obs;

        Assert.Equal("Test Title", control.Title);
    }

    [AvaloniaFact]
    public void Control_ContentSource_ReceivesValue_OnSubscribe()
    {
        var control = new MessageBoxControl();
        var obs = new ReturnObservable("Test Content");

        control.ContentSource = obs;

        Assert.Equal("Test Content", control.Content);
    }

    [AvaloniaFact]
    public void Control_TitleSource_Null_DoesNotThrow()
    {
        var control = new MessageBoxControl();
        control.Title = "Original Title";

        control.TitleSource = null;

        Assert.Equal("Original Title", control.Title);
    }

    [AvaloniaFact]
    public void Control_ContentSource_Null_DoesNotThrow()
    {
        var control = new MessageBoxControl();
        control.Content = "Original Content";

        control.ContentSource = null;

        Assert.Equal("Original Content", control.Content);
    }

    [AvaloniaFact]
    public void Control_TitleSource_ChangingObservable_DisposesPrevious()
    {
        var control = new MessageBoxControl();
        var first = new TestObservable();
        var second = new ReturnObservable("Second Title");

        control.TitleSource = first;
        Assert.Equal(1, first.SubscribeCount);

        control.TitleSource = second;
        Assert.Null(first.CurrentObserver);
        Assert.Equal("Second Title", control.Title);
    }

    [AvaloniaFact]
    public void Control_TitleSource_ObservableEmitsMultipleValues()
    {
        var control = new MessageBoxControl();
        var obs = new TestObservable();

        control.TitleSource = obs;
        Assert.Equal(1, obs.SubscribeCount);

        obs.CurrentObserver?.OnNext("First");
        Assert.Equal("First", control.Title);

        obs.CurrentObserver?.OnNext("Second");
        Assert.Equal("Second", control.Title);
    }

    [AvaloniaFact]
    public void Control_ContentSource_ObservableEmitsMultipleValues()
    {
        var control = new MessageBoxControl();
        var obs = new TestObservable();

        control.ContentSource = obs;
        Assert.Equal(1, obs.SubscribeCount);

        obs.CurrentObserver?.OnNext("First");
        Assert.Equal("First", control.Content);

        obs.CurrentObserver?.OnNext("Second");
        Assert.Equal("Second", control.Content);
    }

    [AvaloniaFact]
    public void Control_Property_Defaults_Before_ObservableSet()
    {
        var control = new MessageBoxControl();

        Assert.Null(control.TitleSource);
        Assert.Null(control.ContentSource);
        Assert.Null(control.Title);
        Assert.Null(control.Content);
    }

    [AvaloniaFact]
    public void Control_NullSource_DisposesSubscription()
    {
        var control = new MessageBoxControl();
        var titleObs = new TestObservable();
        var contentObs = new TestObservable();

        control.TitleSource = titleObs;
        control.ContentSource = contentObs;

        Assert.NotNull(titleObs.CurrentObserver);
        Assert.NotNull(contentObs.CurrentObserver);

        control.TitleSource = null;
        control.ContentSource = null;

        Assert.Null(titleObs.CurrentObserver);
        Assert.Null(contentObs.CurrentObserver);
    }
}
