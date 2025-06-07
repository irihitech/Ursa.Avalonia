using System.Diagnostics;
using Avalonia.Controls;
using Ursa.Controls;

namespace Ursa.Demo.Pages;

public partial class NumericUpDownDemo : UserControl
{
    public NumericUpDownDemo()
    {
        InitializeComponent();
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