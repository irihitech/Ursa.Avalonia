using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Metadata;
using Avalonia.Styling;

namespace Ursa.Controls;

public class AspectRatioLayout : TransitioningContentControl
{
    public static readonly StyledProperty<List<AspectRatioLayoutItem>> ItemsProperty =
        AvaloniaProperty.Register<AspectRatioLayout, List<AspectRatioLayoutItem>>(
            nameof(Items));

    public static readonly StyledProperty<double> AspectRatioToleranceProperty =
        AvaloniaProperty.Register<AspectRatioLayout, double>(
            nameof(AspectRatioTolerance), 0.2);

    private AspectRatioMode _currentAspectRatioMode;

    public static readonly DirectProperty<AspectRatioLayout, AspectRatioMode> CurrentAspectRatioModeProperty =
        AvaloniaProperty.RegisterDirect<AspectRatioLayout, AspectRatioMode>(
            nameof(CurrentAspectRatioMode), o => o.CurrentAspectRatioMode);

    private readonly Queue<bool> _history = new();

    static AspectRatioLayout()
    {
        PCrossFade pCrossFade = new()
        {
            Duration = TimeSpan.FromSeconds(0.55),
            FadeInEasing = new QuadraticEaseInOut(),
            FadeOutEasing = new QuadraticEaseInOut()
        };
        PageTransitionProperty.OverrideDefaultValue<AspectRatioLayout>(pCrossFade);
    }

    public AspectRatioLayout()
    {
        Items = new List<AspectRatioLayoutItem>();
    }

    public AspectRatioMode CurrentAspectRatioMode
    {
        get => GetValue(CurrentAspectRatioModeProperty);
        set => SetAndRaise(CurrentAspectRatioModeProperty, ref _currentAspectRatioMode, value);
    }

    public static readonly StyledProperty<double> AspectRatioValueProperty =
        AvaloniaProperty.Register<AspectRatioLayout, double>(
            nameof(AspectRatioValue));

    public double AspectRatioValue
    {
        get => GetValue(AspectRatioValueProperty);
        set => SetValue(AspectRatioValueProperty, value);
    }

    protected override Type StyleKeyOverride => typeof(TransitioningContentControl);

    [Content]
    public List<AspectRatioLayoutItem> Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public double AspectRatioTolerance
    {
        get => GetValue(AspectRatioToleranceProperty);
        set => SetValue(AspectRatioToleranceProperty, value);
    }

    private void UpdateHistory(bool value)
    {
        _history.Enqueue(value);
        while (_history.Count > 3)
            _history.Dequeue();
    }

    private bool IsRightChanges()
    {
        //if (_history.Count < 3) return false;
        return _history.All(x => x) || _history.All(x => !x);
    }

    private double GetAspectRatio(Rect rect)
    {
        return Math.Round(Math.Truncate(Math.Abs(rect.Width)) / Math.Truncate(Math.Abs(rect.Height)), 3);
    }

    private AspectRatioMode GetScaleMode(Rect rect)
    {
        var scale = GetAspectRatio(rect);
        var absA = Math.Abs(AspectRatioTolerance);
        var h = 1d + absA;
        var v = 1d - absA;
        if (scale >= h) return AspectRatioMode.HorizontalRectangle;
        if (v < scale && scale < h) return AspectRatioMode.Square;
        if (scale <= v) return AspectRatioMode.VerticalRectangle;
        return AspectRatioMode.None;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ItemsProperty ||
            change.Property == AspectRatioToleranceProperty ||
            change.Property == BoundsProperty)
        {
            if (change.Property == BoundsProperty)
            {
                var o = (Rect)change.OldValue!;
                var n = (Rect)change.NewValue!;
                UpdateHistory(GetAspectRatio(o) <= GetAspectRatio(n));
                if (!IsRightChanges()) return;
                CurrentAspectRatioMode = GetScaleMode(n);
            }

            AspectRatioValue = GetAspectRatio(Bounds);
            var c =
                Items
                    .Where(x => x.IsUseAspectRatioRange)
                    .FirstOrDefault(x =>
                        x.StartAspectRatioValue <= AspectRatioValue
                        && AspectRatioValue <= x.EndAspectRatioValue);

            c ??= Items.FirstOrDefault(x => x.AcceptAspectRatioMode == GetScaleMode(Bounds));
            if (c == null)
            {
                if (Items.Count == 0) return;
                c = Items.First();
            }

            Content = c;
        }
    }

    private class PCrossFade : IPageTransition
    {
        private readonly Animation _fadeInAnimation;
        private readonly Animation _fadeOutAnimation;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PCrossFade" /> class.
        /// </summary>
        public PCrossFade()
            : this(TimeSpan.Zero)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PCrossFade" /> class.
        /// </summary>
        /// <param name="duration">The duration of the animation.</param>
        public PCrossFade(TimeSpan duration)
        {
            _fadeOutAnimation = new Animation
            {
                Children =
                {
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = OpacityProperty,
                                Value = 1d
                            }
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = OpacityProperty,
                                Value = 0d
                            }
                        },
                        Cue = new Cue(1d)
                    }
                }
            };
            _fadeInAnimation = new Animation
            {
                Children =
                {
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = OpacityProperty,
                                Value = 0d
                            }
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = OpacityProperty,
                                Value = 1d
                            }
                        },
                        Cue = new Cue(1d)
                    }
                }
            };
            _fadeInAnimation.FillMode = FillMode.Both;
            _fadeOutAnimation.FillMode = FillMode.Both;
            _fadeOutAnimation.Duration = _fadeInAnimation.Duration = duration;
        }

        /// <summary>
        ///     Gets the duration of the animation.
        /// </summary>
        public TimeSpan Duration
        {
            get => _fadeOutAnimation.Duration;
            set => _fadeOutAnimation.Duration = _fadeInAnimation.Duration = value;
        }

        /// <summary>
        ///     Gets or sets element entrance easing.
        /// </summary>
        public Easing FadeInEasing
        {
            get => _fadeInAnimation.Easing;
            set => _fadeInAnimation.Easing = value;
        }

        /// <summary>
        ///     Gets or sets element exit easing.
        /// </summary>
        public Easing FadeOutEasing
        {
            get => _fadeOutAnimation.Easing;
            set => _fadeOutAnimation.Easing = value;
        }

        /// <summary>
        ///     Starts the animation.
        /// </summary>
        /// <param name="from">
        ///     The control that is being transitioned away from. May be null.
        /// </param>
        /// <param name="to">
        ///     The control that is being transitioned to. May be null.
        /// </param>
        /// <param name="forward">
        ///     Unused for cross-fades.
        /// </param>
        /// <param name="cancellationToken">allowed cancel transition</param>
        /// <returns>
        ///     A <see cref="Task" /> that tracks the progress of the animation.
        /// </returns>
        Task IPageTransition.Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
        {
            return Start(from, to, cancellationToken);
        }

        /// <inheritdoc cref="Start(Visual, Visual, CancellationToken)" />
        public async Task Start(Visual? from, Visual? to, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            var tasks = new List<Task>();

            if (from != null) tasks.Add(_fadeOutAnimation.RunAsync(from, cancellationToken));

            if (to != null)
            {
                to.IsVisible = true;
                tasks.Add(_fadeInAnimation.RunAsync(to, cancellationToken));
            }

            await Task.WhenAll(tasks);

            if (from != null && !cancellationToken.IsCancellationRequested) from.IsVisible = false;
        }
    }
}