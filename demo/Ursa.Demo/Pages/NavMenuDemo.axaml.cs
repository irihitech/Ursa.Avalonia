using System;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Styling;
using Ursa.Controls;
using Ursa.Helpers;

namespace Ursa.Demo.Pages;

public partial class NavMenuDemo : UserControl
{
    public NavMenuDemo()
    {
        InitializeComponent();
    }

    public static SizeAnimationHelperAnimationGeneratorDelegate NavMenuAnimation { get; } =
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
                            new Setter(WidthProperty, oldDesiredSize.Width),
                            new Setter(HeightProperty, oldDesiredSize.Height)
                        }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1.0),
                        Setters =
                        {
                            new Setter(WidthProperty, newDesiredSize.Width),
                            new Setter(HeightProperty, newDesiredSize.Height)
                        }
                    }
                }
            };
        };
}