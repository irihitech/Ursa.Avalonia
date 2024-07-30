using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Common;

namespace Ursa.Demo.ViewModels;

public partial class FormDemoViewModel : ObservableObject
{
    [ObservableProperty] private DataModel _model;
    [ObservableProperty] private Position _selectedPosition = Position.Top;
    [ObservableProperty] private HorizontalAlignment _selectedHorizontalAlignment = HorizontalAlignment.Left;

    public ObservableCollection<Position> Positions =>
    [
        Position.Left,
        Position.Top,
        Position.Right,
        Position.Bottom,
    ];

    public ObservableCollection<HorizontalAlignment> HorizontalAlignments =>
    [
        HorizontalAlignment.Stretch,
        HorizontalAlignment.Left,
        HorizontalAlignment.Center,
        HorizontalAlignment.Right,
    ];

    public FormDemoViewModel()
    {
        Model = new DataModel();
    }
}

public partial class DataModel : ObservableObject
{
    private string _name = string.Empty;

    [MinLength(10)]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private double _number;

    [Range(0.0, 10.0)]
    public double Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    private string _email = string.Empty;

    [EmailAddress]
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private DateTime _date;

    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    public DataModel()
    {
        Date = DateTime.Today;
    }
}