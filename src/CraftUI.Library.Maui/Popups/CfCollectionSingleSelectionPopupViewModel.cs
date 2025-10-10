using System.Collections.ObjectModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CraftUI.Library.Maui.Common;
using CraftUI.Library.Maui.Common.Models;

namespace CraftUI.Library.Maui.Popups;

public partial class CfCollectionSingleSelectionPopupViewModel(IPopupService popupService) : ViewModelPopupBase(popupService)
{
    private IList<DisplayValueItem>? _items;
    private bool _isInitialized;
    
    [ObservableProperty] 
    private DisplayValueItem? _selectedItem;

    [ObservableProperty] 
    private IList<DisplayValueItem>? _itemsSource;

    [ObservableProperty] 
    private string? _search;

    [ObservableProperty] 
    private bool _isSearchVisible;

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _isInitialized = true;
        
        if (query.TryGetValue(nameof(SelectedItem), out var selectedItemObject) && selectedItemObject is DisplayValueItem selectedItem)
        {
            SelectedItem = selectedItem;
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
    private async Task ItemSelected()
    {
        if (!_isInitialized)
        {
            await Task.Delay(50);
            await PopupService.ClosePopupAsync(Shell.Current, SelectedItem).ConfigureAwait(false);
        }
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
}