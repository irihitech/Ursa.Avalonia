using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

public class ControlClassesInput: TemplatedControl
{
    LinkedList<List<string>> _history = new();
    LinkedList<List<string>> _undoHistory = new();
    
    public int CountOfHistoricalRecord { get; set; } = 10;
    
    public static readonly StyledProperty<Control?> TargetProperty = AvaloniaProperty.Register<ControlClassesInput, Control?>(
        nameof(Target));

    public Control? Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }
    
    public static readonly StyledProperty<ObservableCollection<string>?> TargetClassesProperty = AvaloniaProperty.Register<ControlClassesInput, ObservableCollection<string>?>(
        nameof(TargetClasses));
    
    public ObservableCollection<string>? TargetClasses
    {
        get => GetValue(TargetClassesProperty);
        set => SetValue(TargetClassesProperty, value);
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
        TargetClasses.CollectionChanged += InccOnCollectionChanged;
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

    private static readonly char[] _separators = {' ', '\t', '\n', '\r'};
    
    private void OnClassesChanged(AvaloniaPropertyChangedEventArgs<ObservableCollection<string>?> args)
    {
        var newValue = args.NewValue.Value;
        if (newValue is null)
        {
            SaveHistory(new List<string>());
            return;
        }
        else
        {
            var classes = newValue.Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().ToList();
            SaveHistory(classes);
            if (newValue is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged+=InccOnCollectionChanged;
            }
            return;
        }
    }

    private void InccOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        SaveHistory(TargetClasses?.ToList() ?? new List<string>());
    }

    private void SaveHistory(List<string> strings)
    {
        _history.AddLast(strings);
        if (_history.Count > CountOfHistoricalRecord)
        {
            _history.RemoveFirst();
        }
        SetClassesToTarget();
    }

    private void SetClassesToTarget()
    {
        List<string> strings;
        if (_history.Count == 0)
        {
            strings = new List<string>();
        }
        else
        {
            strings = _history.Last.Value;
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
        _history.RemoveLast();
        _undoHistory.AddFirst(node);
        SetCurrentValue(TargetClassesProperty, new AvaloniaList<string>(node.Value));
        SetClassesToTarget();
    }

    public void Redo()
    {
        var node = _undoHistory.First;
        _undoHistory.RemoveFirst();
        _history.AddLast(node);
        SetCurrentValue(TargetClassesProperty, new AvaloniaList<string>(node.Value));
        SetClassesToTarget();
    }

    public void Clear()
    {
        SaveHistory(new List<string>());
    }
}