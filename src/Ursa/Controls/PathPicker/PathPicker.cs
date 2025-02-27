using System.Text;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Logging;
using Avalonia.Platform.Storage;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(Name = PART_Button, Type = typeof(Button))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public class PathPicker : TemplatedControl
{
    public const string PART_Button = "PART_Button";
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

    public static readonly StyledProperty<string?> SelectedPathsTextProperty =
        AvaloniaProperty.Register<PathPicker, string?>(
            nameof(SelectedPathsText), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsOmitCommandOnCancelProperty =
        AvaloniaProperty.Register<PathPicker, bool>(
            nameof(IsOmitCommandOnCancel));

    public static readonly StyledProperty<bool> IsClearSelectionOnCancelProperty =
        AvaloniaProperty.Register<PathPicker, bool>(
            nameof(IsClearSelectionOnCancel));

    public bool IsClearSelectionOnCancel
    {
        get => GetValue(IsClearSelectionOnCancelProperty);
        set => SetValue(IsClearSelectionOnCancelProperty, value);
    }

    public bool IsOmitCommandOnCancel
    {
        get => GetValue(IsOmitCommandOnCancelProperty);
        set => SetValue(IsOmitCommandOnCancelProperty, value);
    }

    public string? SelectedPathsText
    {
        get => GetValue(SelectedPathsTextProperty);
        set => SetValue(SelectedPathsTextProperty, value);
    }

    private Button? _button;

    private IReadOnlyList<string> _selectedPaths = [];

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

    private bool _twoConvertLock;

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (_twoConvertLock) return;
        if (change.Property == SelectedPathsProperty)
        {
            _twoConvertLock = true;
            var stringBuilder = new StringBuilder();
            if (SelectedPaths.Count != 0)
            {
                stringBuilder.Append(SelectedPaths[0]);
                foreach (var item in SelectedPaths.Skip(1))
                {
                    stringBuilder.AppendLine(item);
                }
            }

            SelectedPathsText = stringBuilder.ToString();
            _twoConvertLock = false;
        }

        if (change.Property == SelectedPathsTextProperty)
        {
            _twoConvertLock = true;
            string[] separatedStrings = ["\r", "\n", "\r\n"];
            SelectedPaths = SelectedPathsText?.Split(separatedStrings, StringSplitOptions.RemoveEmptyEntries)
                                .Select(RemoveNewLine).ToArray()
                            ?? [];
            _twoConvertLock = false;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(LaunchPicker, _button);
        _button = e.NameScope.Find<Button>(PART_Button);
        Button.ClickEvent.AddHandler(LaunchPicker, _button);
    }


    private static string RemoveNewLine(string str)
    {
        return str.Replace("\r", "")
            .Replace("\n", "")
            .Replace("\r\n", "")
            .Replace(Environment.NewLine, "");
    }

    /**
     * FilePickerFileTypeName,Pattern,Pattern,Pattern...
     */
    private static FilePickerFileType ParseFilePickerType(string str)
    {
        return str switch
        {
            nameof(FilePickerFileTypes.All) => FilePickerFileTypes.All,
            nameof(FilePickerFileTypes.Pdf) => FilePickerFileTypes.Pdf,
            nameof(FilePickerFileTypes.ImageAll) => FilePickerFileTypes.ImageAll,
            nameof(FilePickerFileTypes.ImageJpg) => FilePickerFileTypes.ImageJpg,
            nameof(FilePickerFileTypes.ImagePng) => FilePickerFileTypes.ImagePng,
            nameof(FilePickerFileTypes.ImageWebp) => FilePickerFileTypes.ImageWebp,
            nameof(FilePickerFileTypes.TextPlain) => FilePickerFileTypes.TextPlain,
            _ => Parse()
        };

        FilePickerFileType Parse()
        {
            var list = str.Split(',');
            return new FilePickerFileType(list.First())
            {
                Patterns = list.Skip(1).Select(x => x.Trim()).ToArray()
            };
        }
    }

    /**
    * [ParseFilePickerTypeStr][ParseFilePickerTypeStr]...
    */
    private static IReadOnlyList<FilePickerFileType>? ParseFileTypes(string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return null;
        string[] separatedStrings = ["[", "][", "]"];
        var list = RemoveNewLine(str)
            .Replace(" ", string.Empty)
            .Split(separatedStrings, StringSplitOptions.RemoveEmptyEntries);
        return list.Select(ParseFilePickerType).ToArray();
    }

    private async void LaunchPicker(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (TopLevel.GetTopLevel(this)?.StorageProvider is not { } storageProvider) return;
            _button?.SetValue(IsEnabledProperty, false);
            switch (UsePickerType)
            {
                case UsePickerTypes.OpenFile:
                    FilePickerOpenOptions filePickerOpenOptions = new()
                    {
                        Title = Title,
                        AllowMultiple = AllowMultiple,
                        SuggestedStartLocation =
                            await storageProvider.TryGetFolderFromPathAsync(SuggestedStartPath),
                        FileTypeFilter = ParseFileTypes(FileFilter)
                    };
                    var resFiles = await storageProvider.OpenFilePickerAsync(filePickerOpenOptions);
                    UpdateSelectedPaths(resFiles.Select(x => x.TryGetLocalPath()).ToArray());
                    break;
                case UsePickerTypes.SaveFile:
                    FilePickerSaveOptions filePickerSaveOptions = new()
                    {
                        Title = Title,
                        SuggestedStartLocation =
                            await storageProvider.TryGetFolderFromPathAsync(SuggestedStartPath),
                        SuggestedFileName = SuggestedFileName,
                        FileTypeChoices = ParseFileTypes(FileFilter),
                        DefaultExtension = DefaultFileExtension
                    };

                    var path = (await storageProvider.SaveFilePickerAsync(filePickerSaveOptions))
                        ?.TryGetLocalPath();
                    UpdateSelectedPaths([path]);
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
                    UpdateSelectedPaths(resFolder.Select(x => x.TryGetLocalPath()).ToArray());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedPaths.Count == 0);
            if (SelectedPaths.Count != 0 || IsOmitCommandOnCancel is false)
            {
                Command?.Execute(SelectedPaths);
            }
        }
        catch (Exception exception)
        {
            Logger.TryGet(LogEventLevel.Error, LogArea.Control)?.Log(this, $"{exception}");
        }
        finally
        {
            _button?.SetValue(IsEnabledProperty, true);
        }
    }

    private void UpdateSelectedPaths(IReadOnlyList<string?> newList)
    {
        var nonNullList = newList.Where(x => x is not null).Select(x => x!).ToList();
        if (nonNullList.Count != 0 || IsClearSelectionOnCancel && nonNullList.Count == 0)
            SelectedPaths = nonNullList;
    }
}