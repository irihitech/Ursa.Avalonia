using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;
using Ursa.Fluent2.Demo;
using Ursa.Fluent2.Demo.Pages;
using Xunit;

namespace Ursa.Themes.Fluent2.Tests;

public class CustomDialogDemoTests
{
    [AvaloniaTheory]
    [InlineData(false, "CancelButton", DialogResult.Cancel)]
    [InlineData(true, "DoneButton", DialogResult.OK)]
    [InlineData(false, CustomDialogWindow.PART_CloseButton, DialogResult.Cancel)]
    public void Custom_window_sample_has_one_header_and_working_close_actions(
        bool dark, string closeButtonName, DialogResult expectedResult)
    {
        var owner = new MainWindow { RequestedThemeVariant = dark ? ThemeVariant.Dark : ThemeVariant.Light };
        CustomDialogWindow? dialog = null;
        try
        {
            owner.Show();
            var model = owner.ViewModel;
            model.SelectedItem = model.Categories.SelectMany(category => category.Children)
                .Single(item => item.Page?.Id == "Dialog");
            owner.UpdateLayout();
            var launch = owner.GetVisualDescendants().OfType<Button>()
                .Single(button => Equals(button.Tag, "CustomDialogWindow"));
            launch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Dispatcher.UIThread.RunJobs();
            dialog = Assert.IsType<CustomDialogWindow>(Assert.Single(owner.OwnedWindows));
            dialog.UpdateLayout();

            var content = Assert.IsType<CustomDialogSample>(dialog.Content);
            Assert.True(string.IsNullOrEmpty(dialog.Title));
            Assert.Equal(SizeToContent.WidthAndHeight, dialog.SizeToContent);
            Assert.Same(owner, dialog.Owner);
            var heading = content.FindControl<TextBlock>("DialogHeading")!;
            var input = content.FindControl<TextBox>("ProjectName")!;
            var close = dialog.GetVisualDescendants().OfType<Button>()
                .Single(button => button.Name == CustomDialogWindow.PART_CloseButton);
            var headingBounds = GetBounds(heading, dialog);
            var inputBounds = GetBounds(input, dialog);
            var closeBounds = GetBounds(close, dialog);
            Assert.False(headingBounds.Intersects(closeBounds));
            Assert.True(inputBounds.Top > headingBounds.Bottom);
            Assert.True(dialog.Bounds.Height > 0);
            Assert.True(dialog.Bounds.Height < 320);

            input.Text = "Updated project";
            Assert.Equal("Updated project", input.Text);
            var context = Assert.IsType<CustomDialogSampleViewModel>(dialog.DataContext);
            object? result = null;
            context.RequestClose += (_, value) => result = value;
            var closed = false;
            dialog.Closed += (_, _) => closed = true;
            var action = dialog.GetVisualDescendants().OfType<Button>()
                .Single(button => button.Name == closeButtonName);
            action.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Dispatcher.UIThread.RunJobs();
            Assert.True(closed);
            Assert.Equal(expectedResult, result);
            Assert.Empty(owner.OwnedWindows);
        }
        finally
        {
            dialog?.Close();
            owner.Close();
        }
    }

    private static Rect GetBounds(Control control, Visual root) =>
        new(control.TranslatePoint(default, root)!.Value, control.Bounds.Size);
}
