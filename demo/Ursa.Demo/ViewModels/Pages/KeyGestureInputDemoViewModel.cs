using System.Collections.Generic;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class KeyGestureInputDemoViewModel: ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "KeyGestureInput",
        Description = "KeyGestureInput captures keyboard shortcut gestures from user input.",
        Breadcrumbs = ["Buttons & Inputs", "KeyGestureInput"],
        Tags = ["KeyGestureInput", "Input", "HotKey"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/KeyGestureInputDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/KeyGestureInputDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public List<Key> AcceptableKeys { get; set; } = new List<Key>()
    {
        Key.A, Key.B, Key.C,
    };
}