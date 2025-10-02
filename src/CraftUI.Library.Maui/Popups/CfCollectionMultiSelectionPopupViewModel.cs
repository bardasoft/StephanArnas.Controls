using System.Collections.ObjectModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CraftUI.Library.Maui.Common;
using CraftUI.Library.Maui.Common.Models;

namespace CraftUI.Library.Maui.Popups;

public partial class CfCollectionMultiSelectionPopupViewModel(IPopupService popupService) : ViewModelPopupBase(popupService)
{
    private IList<DisplayValueItem>? _items;
    private bool _isInitialized;
    
    private ObservableCollection<object> _selectedItemsInternal = [];
    
    public ObservableCollection<object> SelectedItemsInternal
    {
        get => _selectedItemsInternal;
        set => SetProperty(ref _selectedItemsInternal, value);
    }
    
    public IList<DisplayValueItem>? SelectedItems
    {
        get => _selectedItemsInternal.OfType<DisplayValueItem>().ToList();
        set
        {
            // On réinitialise la collection interne avec les nouveaux éléments
            _selectedItemsInternal = value != null
                ? new ObservableCollection<object>(value.Cast<object>())
                : new ObservableCollection<object>();

            OnPropertyChanged(); // notifie si tu es en MVVM
        }
    }
    
    [ObservableProperty] 
    private IList<DisplayValueItem>? _itemsSource;

    [ObservableProperty] 
    private string? _search;

    [ObservableProperty] 
    private bool _isSearchVisible;

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _isInitialized = true;
        
        if (query.TryGetValue(nameof(SelectedItems), out var selectedItemsObject) && selectedItemsObject is IList<DisplayValueItem> selectedItems)
        {
            foreach (var selectedItem in selectedItems)
            {
                SelectedItemsInternal.Add(selectedItem);
            }
        }
        
        if (query.TryGetValue(nameof(ItemsSource), out var itemsSourceObject))
        {
            ItemsSource = itemsSourceObject switch
            {
                ObservableCollection<DisplayValueItem> itemsSourceAsObservableCollection => itemsSourceAsObservableCollection,
                List<DisplayValueItem> itemsAsSourceList => new ObservableCollection<DisplayValueItem>(itemsAsSourceList),
                IList<DisplayValueItem> itemsAsSourceList => new ObservableCollection<DisplayValueItem>(itemsAsSourceList),
                IEnumerable<DisplayValueItem> itemsAsSourceEnumerable => new ObservableCollection<DisplayValueItem>(itemsAsSourceEnumerable),
                _ => ItemsSource
            };
            _items = ItemsSource ?? new List<DisplayValueItem>();
        }
        
        if (query.TryGetValue(nameof(IsSearchVisible), out var isSearchVisibleObject) && isSearchVisibleObject is bool isSearchVisible)
        {
            IsSearchVisible = isSearchVisible;
        }

        base.ApplyQueryAttributes(query);
        
        _isInitialized = false;
    }

    [RelayCommand]
    private void SearchUpdated()
    {
        if (!_isInitialized && _items is not null)
        {
            if (string.IsNullOrEmpty(Search))
            {
                ItemsSource = _items;
            }
            else
            {
                ItemsSource = _items
                    .Where(x => x.DisplayValue.Contains(Search, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
            }
        }
    }

    [RelayCommand]
    private async Task ItemSelected()
    {
        if (!_isInitialized)
        {
            await PopupService.ClosePopupAsync(Shell.Current, SelectedItems).ConfigureAwait(false);
        }
    }
}