using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HeadlessTest.Ursa.Controls.VirtualizingUniformGridTests;


public partial class TestViewModel: ObservableObject
{
    /// <summary>
    /// A large collection of items to demonstrate virtualisation.
    /// </summary>
    public ObservableCollection<GridItem> Items { get; } = new();

    [ObservableProperty]
    private int _columns = 4;

    public TestViewModel()
    {
        for (int i = 0; i < 10_000; i++)
        {
            Items.Add(new GridItem
            {
                Index = i,
                Label = $"Item {i:N0}",
                Color = GetColorForIndex(i)
            });
        }
    }

    private static string GetColorForIndex(int index)
    {
        // Cycle through a set of nice colours.
        var colors = new[] { "#E57373", "#81C784", "#64B5F6", "#FFB74D",
            "#BA68C8", "#4DB6AC", "#FFF176", "#A1887F" };
        return colors[index % colors.Length];
    }
}

/// <summary>
/// A simple data item displayed in the virtualised grid.
/// </summary>
public partial class GridItem : ObservableObject
{
    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private string _label = string.Empty;

    [ObservableProperty]
    private string _color = "#E0E0E0";
}
