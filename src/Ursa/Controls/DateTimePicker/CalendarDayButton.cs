using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[PseudoClasses(PseudoClassName.PC_Pressed, PseudoClassName.PC_Selected, 
    PC_StartDate, PC_EndDate, PC_PreviewStartDate, PC_PreviewEndDate, PC_InRange, PC_Today, PC_Blackout)]
public class CalendarDayButton: ContentControl
{
    public const string PC_StartDate = ":start-date";
    public const string PC_EndDate = ":end-date";
    public const string PC_PreviewStartDate = ":preview-start-date";
    public const string PC_PreviewEndDate = ":preview-end-date";
    public const string PC_InRange = ":in-range";
    public const string PC_Today = ":today";
    public const string PC_Blackout = ":blackout";

    private bool _isToday;
    public bool IsToday
    {
        get => _isToday;
        set
        {
            _isToday = value;
            PseudoClasses.Set(PC_Today, value);
        }
    }
    
    private bool _isStartDate;
    public bool IsStartDate
    {
        get => _isStartDate;
        set
        {
            _isStartDate = value;
            PseudoClasses.Set(PC_StartDate, value);
        }
    }
    
    private bool _isEndDate;
    public bool IsEndDate
    {
        get => _isEndDate;
        set
        {
            _isEndDate = value;
            PseudoClasses.Set(PC_EndDate, value);
        }
    }
    
    private bool _isPreviewStartDate;
    public bool IsPreviewStartDate
    {
        get => _isPreviewStartDate;
        set
        {
            _isPreviewStartDate = value;
            PseudoClasses.Set(PC_PreviewStartDate, value);
        }
    }
    
    private bool _isPreviewEndDate;
    public bool IsPreviewEndDate
    {
        get => _isPreviewEndDate;
        set
        {
            _isPreviewEndDate = value;
            PseudoClasses.Set(PC_PreviewEndDate, value);
        }
    }
    
    private bool _isInRange;
    public bool IsInRange
    {
        get => _isInRange;
        set
        {
            _isInRange = value;
            PseudoClasses.Set(PC_InRange, value);
        }
    }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            PseudoClasses.Set(PseudoClassName.PC_Selected, value);
        }
    }
    
    private bool _isBlackout;
    public bool IsBlackout
    {
        get => _isBlackout;
        set
        {
            _isBlackout = value;
            PseudoClasses.Set(PC_Blackout, value);
        }
    }

    static CalendarDayButton()
    {
        PressedMixin.Attach<CalendarDayButton>();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        PseudoClasses.Set(PseudoClassName.PC_Disabled, IsEnabled);
        PseudoClasses.Set(PC_Today, IsToday);
        PseudoClasses.Set(PC_StartDate, IsStartDate);
        PseudoClasses.Set(PC_EndDate, IsEndDate);
        PseudoClasses.Set(PC_PreviewStartDate, IsPreviewStartDate);
        PseudoClasses.Set(PC_PreviewEndDate, IsPreviewEndDate);
        PseudoClasses.Set(PC_InRange, IsInRange);
        PseudoClasses.Set(PseudoClassName.PC_Selected, IsSelected);
        
    }
}