using Avalonia;
using Avalonia.Controls;
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
    }


    private void Numd_ValueChanged(object? sender, ValueChangedEventArgs<uint> e)
    {
        vm.ValueChangedUpdateText = $"ValueChanged,Old={e.OldValue} =>{e.NewValue}";
    }
}