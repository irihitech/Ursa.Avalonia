using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

public class ControlClassesInput: TemplatedControl
{
    private readonly LinkedList<List<string>> _history = [];
    private readonly LinkedList<List<string>> _undoHistory = [];
    private bool _disableHistory;
    
    public int CountOfHistoricalRecord { get; set; } = 10;
    
    public static readonly StyledProperty<Control?> TargetProperty = AvaloniaProperty.Register<ControlClassesInput, Control?>(
        nameof(Target));

    public Control? Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public static readonly StyledProperty<string> SeparatorProperty =
        TagInput.SeparatorProperty.AddOwner<ControlClassesInput>();

    public string Separator
    {
        get => GetValue(SeparatorProperty);
        set => SetValue(SeparatorProperty, value);
    }


    private ObservableCollection<string>? _targetClasses;

    internal static readonly DirectProperty<ControlClassesInput, ObservableCollection<string>?> TargetClassesProperty = AvaloniaProperty.RegisterDirect<ControlClassesInput, ObservableCollection<string>?>(
        nameof(TargetClasses), o => o.TargetClasses, (o, v) => o.TargetClasses = v);

    public ObservableCollection<string>? TargetClasses
    {
        get => _targetClasses;
        set => SetAndRaise(TargetClassesProperty, ref _targetClasses, value);
    }

    public static readonly AttachedProperty<ControlClassesInput?> SourceProperty =
        AvaloniaProperty.RegisterAttached<ControlClassesInput, StyledElement, ControlClassesInput?>("Source");

    public static void SetSource(StyledElement obj, ControlClassesInput value) => obj.SetValue(SourceProperty, value);
    public static ControlClassesInput? GetSource(StyledElement obj) => obj.GetValue(SourceProperty);

    static ControlClassesInput()
    {
        TargetClassesProperty.Changed.AddClassHandler<ControlClassesInput, ObservableCollection<string>?>((o,e)=>o.OnClassesChanged(e));
        SourceProperty.Changed.AddClassHandler<StyledElement, ControlClassesInput?>(HandleSourceChange);
    }

    public ControlClassesInput()
    {
        TargetClasses = new ObservableCollection<string>();
    }

    private List<StyledElement> _targets = new();
    
    private static void HandleSourceChange(StyledElement arg1, AvaloniaPropertyChangedEventArgs<ControlClassesInput?> arg2)
    {
        var newControl = arg2.NewValue.Value;
        if (newControl is null) return;
        newControl._targets.Add(arg1);
        var oldControl = arg2.OldValue.Value;
        if (oldControl is not null)
        {
            newControl._targets.Remove(oldControl);
        }
    }
    
    private void OnClassesChanged(AvaloniaPropertyChangedEventArgs<ObservableCollection<string>?> args)
    {
        var newValue = args.NewValue.Value;
        if (newValue is null)
        {
            SaveHistory(new List<string>(), true);
        }
        else
        {
            var classes = newValue.Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().ToList();
            SaveHistory(classes, true);
            if (newValue is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged+=InccOnCollectionChanged;
            }
        }
    }

    private void InccOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if(_disableHistory) return;
        SaveHistory(TargetClasses?.ToList() ?? new List<string>(), true);
    }

    private void SaveHistory(List<string> strings, bool fromInput)
    {
        _history.AddLast(strings);
        _undoHistory.Clear();
        if (_history.Count > CountOfHistoricalRecord)
        {
            _history.RemoveFirst();
        }
        SetClassesToTarget(fromInput);
    }

    private void SetClassesToTarget(bool fromInput)
    {
        List<string> strings;
        if (_history.Count == 0)
        {
            strings = new List<string>();
        }
        else
        {
            strings = _history.Last?.Value ?? [];
        }

        if (!fromInput)
        {
            SetCurrentValue(TargetClassesProperty, new ObservableCollection<string>(strings));
        }
        if (Target is not null)
        {
            Target.Classes.Replace(strings);
        }
        foreach (var target in _targets)
        {
            target.Classes.Replace(strings);
        }
    }

    public void UnDo()
    {
        var node = _history.Last;
        if (node is null) return;
        _history.RemoveLast();
        _undoHistory.AddFirst(node.Value);
        _disableHistory = true;
        TargetClasses?.Clear();
        foreach (var value in node.Value)
        {
            TargetClasses?.Add(value);
        }
        _disableHistory = false;
        SetClassesToTarget(false);
    }

    public void Redo()
    {
        var node = _undoHistory.First;
        if (node is null) return;
        _undoHistory.RemoveFirst();
        _history.AddLast(node);
        _disableHistory = true;
        TargetClasses?.Clear();
        foreach (var value in node.Value)
        {
            TargetClasses?.Add(value);
        }
        _disableHistory = false;
        SetClassesToTarget(false);
    }

    public void Clear()
    {
        SaveHistory([], false);
    }
}