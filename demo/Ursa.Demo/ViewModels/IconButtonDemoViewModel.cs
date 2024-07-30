using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Common;

namespace Ursa.Demo.ViewModels;

public partial class IconButtonDemoViewModel : ObservableObject
{
    [ObservableProperty] private Position _selectedPosition = Position.Top;

    public ObservableCollection<Position> Positions =>
    [
        Position.Left,
        Position.Top,
        Position.Right,
        Position.Bottom,
    ];
}