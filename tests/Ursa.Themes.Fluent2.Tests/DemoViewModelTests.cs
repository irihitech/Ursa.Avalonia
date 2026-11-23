using Ursa.Fluent2.Demo;
using Ursa.Fluent2.Demo.Services;
using Ursa.Fluent2.Demo.ViewModels;
using Ursa.Controls;
using Xunit;

namespace Ursa.Themes.Fluent2.Tests;

public class DemoViewModelTests
{
    [Fact]
    public async Task Sample_command_delegates_to_the_interaction_service_and_tracks_execution()
    {
        var service = new DemoInteractionStub();
        var viewModel = new DemoPageViewModel("Dialog", service);
        var execution = viewModel.OpenSampleCommand.ExecuteAsync("ModalDialog");

        Assert.Equal("ModalDialog", service.SampleName);
        Assert.True(viewModel.OpenSampleCommand.IsRunning);
        Assert.False(viewModel.OpenSampleCommand.CanExecute("Dialog"));
        service.Completion.SetResult();
        await execution;
        Assert.False(viewModel.OpenSampleCommand.IsRunning);
        Assert.True(viewModel.OpenSampleCommand.CanExecute("Dialog"));
    }

    [Fact]
    public void Custom_dialog_notifies_project_changes_and_returns_command_results()
    {
        var viewModel = new CustomDialogSampleViewModel();
        var changes = new List<string?>();
        viewModel.PropertyChanged += (_, e) => changes.Add(e.PropertyName);
        var results = new List<object?>();
        viewModel.RequestClose += (_, result) => results.Add(result);

        viewModel.ProjectName = "New project";
        viewModel.ConfirmCommand.Execute(null);
        viewModel.CancelCommand.Execute(null);

        Assert.Equal([nameof(viewModel.ProjectName)], changes);
        Assert.Equal([DialogResult.OK, DialogResult.Cancel], results);
    }

    private sealed class DemoInteractionStub : IDemoInteractionService
    {
        public string? SampleName { get; private set; }
        public TaskCompletionSource Completion { get; } = new();

        public Task OpenSampleAsync(string name)
        {
            SampleName = name;
            return Completion.Task;
        }
    }
}
