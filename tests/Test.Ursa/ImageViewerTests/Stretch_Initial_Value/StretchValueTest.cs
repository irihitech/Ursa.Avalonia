using Avalonia.Controls;
using Avalonia.Media;
using Ursa.Controls;

namespace Test.Ursa.Controls.ImageViewerTests.Stretch_Initial_Value;

public class StretchValueTest
{
    [Fact]
    public void Stretch_Initial_Value_Does_Not_Crash()
    {
        ImageViewer iv = new ImageViewer()
        {
            Stretch = Stretch.UniformToFill
        };
    }
}