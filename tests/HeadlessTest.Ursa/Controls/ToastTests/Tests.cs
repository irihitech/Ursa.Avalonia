using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using UrsaToast = Ursa.Controls.Toast;
using UrsaToastCard = Ursa.Controls.ToastCard;
using UrsaWindowToastManager = Ursa.Controls.WindowToastManager;

namespace HeadlessTest.Ursa.Controls.ToastTests;

public class Tests
{
    [AvaloniaFact]
    public void CloseAll_Should_Mark_All_Toasts_As_Closing()
    {
        var window = new Window();
        var manager = new UrsaWindowToastManager(window);
        window.Show();

        var toast1 = new UrsaToast("Content1", expiration: TimeSpan.Zero);
        var toast2 = new UrsaToast("Content2", expiration: TimeSpan.Zero);

        manager.Show(toast1);
        manager.Show(toast2);

        Dispatcher.UIThread.RunJobs();

        var cards = window.GetVisualDescendants().OfType<UrsaToastCard>().ToList();
        Assert.Equal(2, cards.Count);
        Assert.All(cards, c => Assert.False(c.IsClosing));

        manager.CloseAll();

        Assert.All(cards, c => Assert.True(c.IsClosing));
    }

    [AvaloniaFact]
    public void Close_Should_Mark_Specific_Toast_As_Closing()
    {
        var window = new Window();
        var manager = new UrsaWindowToastManager(window);
        window.Show();

        var toast1 = new UrsaToast("Content1", expiration: TimeSpan.Zero);
        var toast2 = new UrsaToast("Content2", expiration: TimeSpan.Zero);

        manager.Show(toast1);
        manager.Show(toast2);

        Dispatcher.UIThread.RunJobs();

        var cards = window.GetVisualDescendants().OfType<UrsaToastCard>().ToList();
        Assert.Equal(2, cards.Count);

        manager.Close(toast1);

        var card1 = cards.FirstOrDefault(c => c.Content == toast1);
        var card2 = cards.FirstOrDefault(c => c.Content == toast2);

        Assert.NotNull(card1);
        Assert.NotNull(card2);
        Assert.True(card1.IsClosing);
        Assert.False(card2.IsClosing);
    }

    [AvaloniaFact]
    public void Close_With_Nonexistent_Toast_Should_Not_Throw()
    {
        var window = new Window();
        var manager = new UrsaWindowToastManager(window);
        window.Show();

        var toast = new UrsaToast("Content", expiration: TimeSpan.Zero);

        // Should not throw when toast was never shown
        manager.Close(toast);

        window.Close();
    }

    [AvaloniaFact]
    public void CloseAll_With_No_Toasts_Should_Not_Throw()
    {
        var window = new Window();
        var manager = new UrsaWindowToastManager(window);
        window.Show();

        // Should not throw when no toasts are shown
        manager.CloseAll();

        window.Close();
    }
}
