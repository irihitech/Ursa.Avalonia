using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.LogicalTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.FormTests.Dynamic_Item_Generation;

public class Test
{
    [AvaloniaFact]
    public void FormItem_Generation()
    {
        // Arrange
        var viewModel = new TestViewModel();
        viewModel.Items = new ObservableCollection<IFormElement>
        {
            new FormTextViewModel { Label = "First Name" },
            new FormTextViewModel { Label = "Last Name" },
            new FormAgeViewModel { Label = "Age" },
            new FormDateRangeViewModel { Label = "Date of Birth" }
        };
        var window = new TestWindow { DataContext = viewModel };

        // Act
        window.Show();

        var form = window.FindControl<Form>("Form");
        Assert.NotNull(form);
        var logicalChildren = form.GetLogicalChildren().ToList();
        Assert.True(logicalChildren.All(a=>a is FormItem));
        Assert.IsType<TextBox>(logicalChildren[0].LogicalChildren[0]);
        Assert.IsType<TextBox>(logicalChildren[1].LogicalChildren[0]);
        Assert.IsType<NumericUIntUpDown>(logicalChildren[2].LogicalChildren[0]);
        Assert.IsType<DateRangePicker>(logicalChildren[3].LogicalChildren[0]);
        Assert.Equal("First Name", FormItem.GetLabel((Control)logicalChildren[0]));
        Assert.Equal("Last Name", FormItem.GetLabel((Control)logicalChildren[1]));
        Assert.Equal("Age", FormItem.GetLabel((Control)logicalChildren[2]));
        Assert.Equal("Date of Birth", FormItem.GetLabel((Control)logicalChildren[3]));
        
        window.Close();
    }
    
    [AvaloniaFact]
    public void FormGroup_Generation()
    {
        // Arrange
        var viewModel = new TestViewModel();
        viewModel.Items = new ObservableCollection<IFormElement>
        {
            new FormGroupViewModel
            {
                Title = "Basic Information",
                Items = new ObservableCollection<IFromItemViewModel>
                {
                    new FormTextViewModel { Label = "First Name" },
                    new FormTextViewModel { Label = "Last Name" },
                    new FormAgeViewModel { Label = "Age" },
                    new FormDateRangeViewModel { Label = "Date of Birth" }
                }
            }
        };
        var window = new TestWindow { DataContext = viewModel };

        // Act
        window.Show();

        var form = window.FindControl<Form>("Form");
        Assert.NotNull(form);
        var logicalChildren = form.GetLogicalChildren().ToList();
        Assert.True(logicalChildren.All(a=>a is FormGroup));
        var formGroup = (FormGroup)logicalChildren[0];
        Assert.Equal("Basic Information", formGroup.Header);
        var formItems = formGroup.GetLogicalChildren().ToList();
        Assert.True(formItems.All(a=>a is FormItem));
        Assert.IsType<TextBox>(formItems[0].LogicalChildren[0]);
        Assert.IsType<TextBox>(formItems[1].LogicalChildren[0]);
        Assert.IsType<NumericUIntUpDown>(formItems[2].LogicalChildren[0]);
        Assert.IsType<DateRangePicker>(formItems[3].LogicalChildren[0]);
        Assert.Equal("First Name", FormItem.GetLabel((Control)formItems[0]));
        Assert.Equal("Last Name", FormItem.GetLabel((Control)formItems[1]));
        Assert.Equal("Age", FormItem.GetLabel((Control)formItems[2]));
        Assert.Equal("Date of Birth", FormItem.GetLabel((Control)formItems[3]));
        
        window.Close();
    }
    
    [AvaloniaFact]
    public void Mixture_Generation()
    {
        // Arrange
        var viewModel = new TestViewModel();
        viewModel.Items = new ObservableCollection<IFormElement>
        {
            new FormTextViewModel { Label = "First Name" },
            new FormGroupViewModel
            {
                Title = "Basic Information",
                Items = new ObservableCollection<IFromItemViewModel>
                {
                    new FormTextViewModel { Label = "Last Name" },
                    new FormAgeViewModel { Label = "Age" },
                    new FormDateRangeViewModel { Label = "Date of Birth" }
                }
            }
        };
        var window = new TestWindow { DataContext = viewModel };

        // Act
        window.Show();

        var form = window.FindControl<Form>("Form");
        Assert.NotNull(form);
        var logicalChildren = form.GetLogicalChildren().ToList();
        Assert.True(logicalChildren.All(a=>a is FormItem || a is FormGroup));
        Assert.IsType<TextBox>(logicalChildren[0].LogicalChildren[0]);
        var formGroup = (FormGroup)logicalChildren[1];
        Assert.Equal("Basic Information", formGroup.Header);
        var formItems = formGroup.GetLogicalChildren().ToList();
        Assert.True(formItems.All(a=>a is FormItem));
        Assert.IsType<TextBox>(formItems[0].LogicalChildren[0]);
        Assert.IsType<NumericUIntUpDown>(formItems[1].LogicalChildren[0]);
        Assert.IsType<DateRangePicker>(formItems[2].LogicalChildren[0]);
        
        window.Close();
    }
}