using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Avalonia.Shared.Contracts;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class FormDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Form,
        Description = LanguageManager.Instance.Page_Description_Form,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Form)],
        Tags = ["Form", "Layout", "Label"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/FormDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/FormDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DataModel Model { get; set; }

    public FormDemoViewModel()
    {
        Model = new DataModel();
        FormGroups = new ObservableCollection<IFormElement>
        {
            new FormGroupViewModel
            {
                Title = "Basic Information",
                Items = new ObservableCollection<IFromItemViewModel>
                {
                    new FormTextViewModel { Label = "Name" },
                    new FormAgeViewModel { Label = "Age" },
                    new FormTextViewModel { Label = "Email" }
                }
            },
            new FormGroupViewModel
            {
                Title = "Education Information",
                Items = new ObservableCollection<IFromItemViewModel>
                {
                    new FormTextViewModel { Label = "College" },
                    new FormDateRangeViewModel { Label = "Study Time" }
                }
            },
            new FormTextViewModel(){ Label = "Other" }
        };
    }

    public ObservableCollection<IFormElement> FormGroups { get; set; }
}

public class DataModel : ObservableObject
{
    private DateTime _date;

    private string _email = string.Empty;
    private string _name = string.Empty;

    private double _number;

    public DataModel()
    {
        Date = DateTime.Today;
    }

    [MinLength(10)]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    [Range(0.0, 10.0)]
    public double Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    [EmailAddress]
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }
}

public interface IFormElement
{
    
}

public interface IFormGroupViewModel : IFormGroup, IFormElement
{
    public string? Title { get; set; }
    public ObservableCollection<IFromItemViewModel> Items { get; set; }
}

public interface IFromItemViewModel: IFormElement
{
    public string? Label { get; set; }
}

public partial class FormGroupViewModel : ObservableObject, IFormGroupViewModel
{
    [ObservableProperty] public partial string? Title { get; set; }
    public ObservableCollection<IFromItemViewModel> Items { get; set; } = [];
}

public partial class FormTextViewModel : ObservableObject, IFromItemViewModel
{
    [ObservableProperty] public partial string? Label { get; set; }
    [ObservableProperty] public partial string? Value { get; set; }
}

public partial class FormAgeViewModel : ObservableObject, IFromItemViewModel
{
    [ObservableProperty] public partial uint? Age { get; set; }
    [ObservableProperty] public partial string? Label { get; set; }
}

public partial class FormDateRangeViewModel : ObservableObject, IFromItemViewModel
{
    [ObservableProperty] public partial DateTime? End { get; set; }
    [ObservableProperty] public partial string? Label { get; set; }
    [ObservableProperty] public partial DateTime? Start { get; set; }
}