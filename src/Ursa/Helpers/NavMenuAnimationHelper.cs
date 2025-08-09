using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;
using Ursa.Controls;

namespace Ursa.Helpers;

public class NavMenuAnimationHelper(NavMenu control) : WHAnimationHelper(control, NavMenu.IsHorizontalCollapsedProperty)
{
    protected override Animation CreateAnimation(Size oldValue, Size newValue)
    {
        if (oldValue.Width > newValue.Width)
        {
            newValue = newValue.WithWidth(newValue.Width + 20);
        }

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
                        new Setter(Layoutable.WidthProperty, oldValue.Width)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1.0),
                    Setters =
                    {
                        new Setter(Layoutable.WidthProperty, newValue.Width)
                    }
                }
            }
        };
    }
}