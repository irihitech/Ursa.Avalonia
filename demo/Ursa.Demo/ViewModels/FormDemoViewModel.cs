using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class FormDemoViewModel: ObservableObject
{
    [ObservableProperty] private DataModel _model;

    public FormDemoViewModel()
    {
        Model = new DataModel();
    }
}

public partial class DataModel : ObservableObject
{
    private string _name;

    [MinLength(10)]
    public string Name
    {
        get=>_name;
        set => SetProperty(ref _name, value);
    }

    private double _number;
    [Range(0.0, 10.0)]
    public double Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }
    
    private string _email;
    
    [EmailAddress]
    public string Email
    {
        get=>_email;
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