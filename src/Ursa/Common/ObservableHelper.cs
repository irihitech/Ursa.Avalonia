using Avalonia.Reactive;

namespace Ursa.Common;

internal static class ObservableHelper
{
    public static IObservable<T> Skip<T>(this IObservable<T> source, int skipCount)
    {
        if (skipCount <= 0) throw new ArgumentException("Skip count must be bigger than zero", nameof(skipCount));

        return Create<T>(obs =>
        {
            var remaining = skipCount;
            return source.Subscribe(new AnonymousObserver<T>(
                input =>
                {
                    if (remaining <= 0)
                        obs.OnNext(input);
                    else
                        remaining--;
                }, obs.OnError, obs.OnCompleted));
        });
    }

    public static IObservable<TSource> Create<TSource>(Func<IObserver<TSource>, IDisposable> subscribe)
    {
        return new CreateWithDisposableObservable<TSource>(subscribe);
    }

    private sealed class CreateWithDisposableObservable<TSource>(Func<IObserver<TSource>, IDisposable> subscribe)
        : IObservable<TSource>
    {
        public IDisposable Subscribe(IObserver<TSource> observer)
        {
            return subscribe(observer);
        }
    }
}