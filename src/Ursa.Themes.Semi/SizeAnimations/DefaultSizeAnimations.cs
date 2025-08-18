using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;
using Ursa.Helpers;

namespace Ursa.Themes.Semi.SizeAnimations;

public partial class DefaultSizeAnimations : ResourceDictionary
{
    public const string WidthAnimationGeneratorKey = "WidthAnimationGenerator";
    public const string HeightAnimationGeneratorKey = "HeightAnimationGenerator";
    public const string WidthHeightAnimationGeneratorKey = "WidthHeightAnimationGenerator";

    public DefaultSizeAnimations()
    {
        Add(WidthAnimationGeneratorKey, WidthAnimationGenerator);
        Add(HeightAnimationGeneratorKey, HeightAnimationGenerator);
        Add(WidthHeightAnimationGeneratorKey, WidthHeightAnimationGenerator);
    }

    private readonly SizeAnimationHelperAnimationGeneratorDelegate WidthAnimationGenerator =
        (_, oldDesiredSize, newDesiredSize) => new Animation
        {
            Duration = TimeSpan.FromMilliseconds(300),
            Easing = new CubicEaseInOut(),
            FillMode = FillMode.None,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0.0),
                    Setters =
                    {
                        new Setter(Layoutable.WidthProperty, oldDesiredSize.Width)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1.0),
                    Setters =
                    {
                        new Setter(Layoutable.WidthProperty, newDesiredSize.Width)
                    }
                }
            }
        };

    private readonly SizeAnimationHelperAnimationGeneratorDelegate HeightAnimationGenerator =
        (_, oldDesiredSize, newDesiredSize) => new Animation
        {
            Duration = TimeSpan.FromMilliseconds(300),
            Easing = new CubicEaseInOut(),
            FillMode = FillMode.None,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0.0),
                    Setters =
                    {
                        new Setter(Layoutable.HeightProperty, oldDesiredSize.Height)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1.0),
                    Setters =
                    {
                        new Setter(Layoutable.HeightProperty, newDesiredSize.Height)
                    }
                }
            }
        };

    private readonly SizeAnimationHelperAnimationGeneratorDelegate WidthHeightAnimationGenerator =
        (_, oldDesiredSize, newDesiredSize) => new Animation
        {
            Duration = TimeSpan.FromMilliseconds(300),
            Easing = new CubicEaseInOut(),
            FillMode = FillMode.None,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0.0),
                    Setters =
                    {
                        new Setter(Layoutable.WidthProperty, oldDesiredSize.Width),
                        new Setter(Layoutable.HeightProperty, oldDesiredSize.Height)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1.0),
                    Setters =
                    {
                        new Setter(Layoutable.WidthProperty, newDesiredSize.Width),
                        new Setter(Layoutable.HeightProperty, newDesiredSize.Height)
                    }
                }
            }
        };
}