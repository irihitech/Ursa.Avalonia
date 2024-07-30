using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public class EnumSelectorDemoViewModel: ObservableObject
{
    public ObservableCollection<Type?> Types { get; set; }
    
    private Type? _selectedType;
    public Type? SelectedType
    {
        get => _selectedType;
        set
        {
            SetProperty(ref _selectedType, value);
            Value = null;
        }
    }

    private object? _value;
    public object? Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }
    
    public EnumSelectorDemoViewModel()
    {
        Types = new ObservableCollection<Type?>()
        {
            typeof(HorizontalAlignment),
            typeof(VerticalAlignment),
            typeof(Orientation),
            typeof(Dock),
            typeof(GridResizeDirection),
            typeof(DayOfWeek),
            typeof(FillMode),
            typeof(IterationType),
            typeof(BindingMode),
            typeof(BindingPriority),
            typeof(StandardCursorType),
            typeof(Key),
            typeof(KeyModifiers),
            typeof(RoutingStrategies),
            typeof(CustomEnum),
        };
    }
}

public enum CustomEnum
{
    [Description("是")]
    Yes,
    [Description("否")]
    No,
}