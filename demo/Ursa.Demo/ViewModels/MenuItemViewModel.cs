using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Irihi.Dogma.Docs;

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
        MenuHeader = node.Page?.Title;
        Key = node.Metadata.Key;
        Status = node.Metadata.Tags.FirstOrDefault();
        Node = node;
    }
}
