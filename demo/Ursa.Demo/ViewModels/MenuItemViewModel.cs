using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Docs;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class MenuItemViewModel: ViewModelBase
{
    public IObservable<string?>? MenuHeader { get; set; }
    public string? Key { get; set; }
    public string? Status { get; set; }
    public DocCategoryNode Node { get; }
    
    public bool IsSeparator { get; set; }
    public ObservableCollection<MenuItemViewModel> Children { get; set; } = new();

    [ObservableProperty] public partial bool IsVisible { get; set; } = true;
    
    public MenuItemViewModel(DocCategoryNode node)
    {
        var titleKey = node.Page?.Metadata.TitleKey;
        MenuHeader = titleKey is null? null: LanguageManager.Instance.GetObservable(titleKey);
        Key = node.Metadata.Key;
        Status = node.Metadata.Tags.FirstOrDefault();
        Node = node;
    }
}
