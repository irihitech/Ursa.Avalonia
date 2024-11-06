using Avalonia.Headless.XUnit;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.UrsaWindowTests;

public class UrsaWindowTest
{
    [AvaloniaFact]
    public void Default_UrsaWindow_Closing_Is_Called_Once()
    {
        var ursaWindow = new UrsaWindow();
        ursaWindow.Show();
        int count = 0;
        ursaWindow.Closing += (_, _) => count++;
        ursaWindow.Close();
        Assert.Equal(1, count);
    }

    [AvaloniaFact]
    public void Inferenced_Window_Closing_Called_Once_When_Yes()
    {
        var window = new UrsaWindowWithCloseInference();
        window.Show();
        int count = 0;
        window.Closing += (_, _) => count++;
        window.Close();
        window.DialogViewModel.CloseYes();
        Assert.Equal(1, count);
    }

    [AvaloniaFact]
    public void Inferenced_Window_Closing_Called_Once_When_No()
    {
        var window = new UrsaWindowWithCloseInference();
        window.Show();
        int count = 0;
        window.Closing += (_, _) => count++;
        window.Close();
        window.DialogViewModel.Close();
        Assert.Equal(1, count);
    }
    
    
}