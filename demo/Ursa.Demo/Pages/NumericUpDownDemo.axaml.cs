using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using Ursa.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class NumericUpDownDemo : UserControl
{
    NumericUpDownDemoViewModel vm;
    public NumericUpDownDemo()
    {
        InitializeComponent();
        DataContext = vm = new NumericUpDownDemoViewModel();
        numd.ValueChanged += Numd_ValueChanged;
        numd.ReadRequested += Numd_ReadRequested;
    }

    Random random = new Random();
    private void Numd_ReadRequested(object? sender, RoutedEventArgs e)
    {
        Trace.WriteLine(e.Source);
        Trace.WriteLine(sender as NumericUIntUpDown);
        var val = (sender as NumericUIntUpDown).Value;
        var a = (uint)random.Next(0, 100);
        vm.ReadRequestedUpdateText = $"ReadRequested,Old={val}, ChangeTo={a}";
        vm.Value = a;
    }

    private void Numd_ValueChanged(object? sender, ValueChangedEventArgs<uint> e)
    {
        vm.ValueChangedUpdateText = $"ValueChanged,Old={e.OldValue}, New={e.NewValue}";
    }
}