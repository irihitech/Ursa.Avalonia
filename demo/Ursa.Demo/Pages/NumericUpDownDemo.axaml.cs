using Avalonia.Controls;
using System.Diagnostics;
using Ursa.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class NumericUpDownDemo : UserControl
{
    public NumericUpDownDemo()
    {
        InitializeComponent();
        DataContext = new NumericUpDownDemoViewModel();
        numd.ValueChanged += Numd_ValueChanged;
    }

    private void Numd_ValueChanged(object? sender, ValueChangedEventArgs<uint> e)
    {
        if (sender is NumericIntUpDown i)
        {
            Trace.WriteLine($"{i.Name} {e.OldValue} {e.NewValue}");
        }
    }
}