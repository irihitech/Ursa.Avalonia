using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Irihi.Avalonia.Shared.Common;

namespace Ursa.Controls;

[TemplatePart(Name = "PART_Button", Type = typeof(Button))]
public class PathPicker : TemplatedControl
{
    public static readonly StyledProperty<string?> SelectedPathProperty =
        AvaloniaProperty.Register<PathPicker, string?>(
            nameof(SelectedPath), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true,
            validate: x => string.IsNullOrWhiteSpace(x) || File.Exists(x) || Directory.Exists(x));


    public static readonly StyledProperty<string> SuggestedStartPathProperty =
        AvaloniaProperty.Register<PathPicker, string>(
            nameof(SuggestedStartPath), string.Empty);

    public static readonly StyledProperty<UsePickerTypes> UsePickerTypeProperty =
        AvaloniaProperty.Register<PathPicker, UsePickerTypes>(
            nameof(UsePickerType));

    public static readonly StyledProperty<string> SuggestedFileNameProperty =
        AvaloniaProperty.Register<PathPicker, string>(
            nameof(SuggestedFileName), string.Empty);

    public static readonly StyledProperty<string> FileFilterProperty = AvaloniaProperty.Register<PathPicker, string>(
        nameof(FileFilter), string.Empty);

    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<PathPicker, string>(
        nameof(Title), string.Empty);

    public static readonly StyledProperty<string> DefaultFileExtensionProperty =
        AvaloniaProperty.Register<PathPicker, string>(
            nameof(DefaultFileExtension), string.Empty);

    public static readonly DirectProperty<PathPicker, IReadOnlyList<string>> SelectedPathsProperty =
        AvaloniaProperty.RegisterDirect<PathPicker, IReadOnlyList<string>>(
            nameof(SelectedPaths), o => o.SelectedPaths, (o, v) => o.SelectedPaths = v);

    public static readonly StyledProperty<ICommand?> CommandProperty = AvaloniaProperty.Register<PathPicker, ICommand?>(
        nameof(Command));

    public static readonly StyledProperty<bool> AllowMultipleProperty = AvaloniaProperty.Register<PathPicker, bool>(
        nameof(AllowMultiple));

    private Button? _button;

    private IReadOnlyList<string> _selectedPaths = [];

    public PathPicker()
    {
        KeyBindings.Add(new KeyBinding
        {
            Command = new IRIHI_CommandBase(() =>
            {
                if (!SelectedPathProperty.ValidateValue!.Invoke(SelectedPath)) return;
                SelectedPaths = string.IsNullOrWhiteSpace(SelectedPath) ? Array.Empty<string>() : [SelectedPath!];
                Command?.Execute(Task.FromResult(SelectedPaths));
            }),
            Gesture = new KeyGesture(Key.Enter)
        });
    }

    public bool AllowMultiple
    {
        get => GetValue(AllowMultipleProperty);
        set => SetValue(AllowMultipleProperty, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public IReadOnlyList<string> SelectedPaths
    {
        get => _selectedPaths;
        private set => SetAndRaise(SelectedPathsProperty, ref _selectedPaths, value);
    }

    public string SuggestedFileName
    {
        get => GetValue(SuggestedFileNameProperty);
        set => SetValue(SuggestedFileNameProperty, value);
    }

    public string DefaultFileExtension
    {
        get => GetValue(DefaultFileExtensionProperty);
        set => SetValue(DefaultFileExtensionProperty, value);
    }


    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string FileFilter
    {
        get => GetValue(FileFilterProperty);
        set => SetValue(FileFilterProperty, value);
    }

    public UsePickerTypes UsePickerType
    {
        get => GetValue(UsePickerTypeProperty);
        set => SetValue(UsePickerTypeProperty, value);
    }

    public string SuggestedStartPath
    {
        get => GetValue(SuggestedStartPathProperty);
        set => SetValue(SuggestedStartPathProperty, value);
    }

    public string? SelectedPath
    {
        get => GetValue(SelectedPathProperty);
        set => SetValue(SelectedPathProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedPathsProperty)
            SelectedPath = SelectedPaths.Count > 0 ? SelectedPaths[0] : string.Empty;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _button = e.NameScope.Find<Button>("PART_Button");
        _button!.Click += LaunchPicker;
    }

    private void LaunchPicker(object? sender, RoutedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this)?.StorageProvider is not { } storageProvider) return;

        Task<IReadOnlyList<string>> task = Task.Run(async () =>
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                switch (UsePickerType)
                {
                    case UsePickerTypes.OpenFile:
                        FilePickerOpenOptions filePickerOpenOptions = new()
                        {
                            Title = Title,
                            AllowMultiple = AllowMultiple,
                            SuggestedStartLocation =
                                await storageProvider.TryGetFolderFromPathAsync(SuggestedStartPath),
                            FileTypeFilter = FileFilter?.Split(',')
                                .Select(x => new FilePickerFileType(x) { Patterns = [x] }).ToList()
                        };
                        var resFiles = await storageProvider.OpenFilePickerAsync(filePickerOpenOptions);
                        SelectedPaths = resFiles.Select(x => x.TryGetLocalPath()).ToArray()!;
                        break;
                    case UsePickerTypes.SaveFile:
                        FilePickerSaveOptions filePickerSaveOptions = new()
                        {
                            Title = Title,
                            SuggestedStartLocation =
                                await storageProvider.TryGetFolderFromPathAsync(SuggestedStartPath),
                            SuggestedFileName = SuggestedFileName,
                            FileTypeChoices = FileFilter?.Split(',')
                                .Select(x => new FilePickerFileType(x) { Patterns = [x] }).ToList(),
                            DefaultExtension = DefaultFileExtension
                        };

                        var path = (await storageProvider.SaveFilePickerAsync(filePickerSaveOptions))
                            ?.TryGetLocalPath();
                        SelectedPaths = string.IsNullOrEmpty(path)
                            ? Array.Empty<string>()
                            : [path!];
                        break;
                    case UsePickerTypes.OpenFolder:
                        FolderPickerOpenOptions folderPickerOpenOptions = new()
                        {
                            Title = Title,
                            AllowMultiple = AllowMultiple,
                            SuggestedStartLocation =
                                await storageProvider.TryGetFolderFromPathAsync(SuggestedStartPath),
                            SuggestedFileName = SuggestedFileName
                        };
                        var resFolder = await storageProvider.OpenFolderPickerAsync(folderPickerOpenOptions);
                        SelectedPaths = resFolder.Select(x => x.TryGetLocalPath()).ToArray()!;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });

            return await Dispatcher.UIThread.InvokeAsync(() => SelectedPaths);
        });
        _button!.CommandParameter = task;
    }
}