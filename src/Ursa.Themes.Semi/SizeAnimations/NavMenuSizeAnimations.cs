using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;
using Ursa.Helpers;

namespace Ursa.Themes.Semi.SizeAnimations;

public class NavMenuSizeAnimations : ResourceDictionary
{
    public const string NavMenuWidthAnimationGeneratorKey = "NavMenuWidthAnimationGenerator";

    public NavMenuSizeAnimations()
    {
        Add(NavMenuWidthAnimationGeneratorKey, NavMenuWidthAnimationGenerator);
    }

    private readonly SizeAnimationHelperAnimationGeneratorDelegate NavMenuWidthAnimationGenerator =
        (_, oldDesiredSize, newDesiredSize) =>
        {
            if (oldDesiredSize.Width > newDesiredSize.Width)
                newDesiredSize = newDesiredSize.WithWidth(newDesiredSize.Width + 20);
            return new Animation
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
        };
}