using System.Windows.Input;
using CommunityToolkit.Maui;
using CraftUI.Library.Maui.Common.Models;
using CraftUI.Library.Maui.Popups;
using CraftUI.Library.Maui.Popups.Settings;

namespace CraftUI.Library.Maui.Controls;

public partial class CfPickerSingleSelection
{
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(List<DisplayValueItem>), typeof(CfPickerSingleSelection), defaultValue: null, propertyChanged: ItemsSourceChanged);
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(DisplayValueItem), typeof(CfPickerSingleSelection), defaultValue: null, defaultBindingMode: BindingMode.TwoWay);
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(CfPickerSingleSelection), defaultBindingMode: BindingMode.TwoWay);
    public static readonly BindableProperty SelectionChangedCommandProperty = BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(CfPickerSingleSelection));
    public static readonly BindableProperty IsSearchVisibleProperty = BindableProperty.Create(nameof(IsSearchVisible), typeof(bool), typeof(CfPickerSingleSelection), defaultBindingMode: BindingMode.OneWay);
    
    private bool _isPopupOpened;
   
    public List<DisplayValueItem>? ItemsSource
    {
        get => (List<DisplayValueItem>?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public DisplayValueItem? SelectedItem
    {
        get => (DisplayValueItem?)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    
    public string? Value
    {
        get => (string?)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public ICommand? SelectionChangedCommand
    {
        get => (ICommand?)GetValue(SelectionChangedCommandProperty);
        set => SetValue(SelectionChangedCommandProperty, value);
    }
    
    public bool IsSearchVisible
    {
        get => (bool)GetValue(IsSearchVisibleProperty);
        set => SetValue(IsSearchVisibleProperty, value);
    }

    public CfPickerSingleSelection()
    {
        InitializeComponent();
    }
    
    private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue) => ((CfPickerSingleSelection)bindable).UpdateItemsSourceView();
    
    public void UpdateItemsSourceView()
    {
        if (_isPopupOpened)
        {
            // The popup is open, close and open again to refresh the collection
            IPlatformApplication.Current?.Services.GetService<IPopupService>()?.ClosePopupAsync(Shell.Current);
            OpenSelectionPopup_OnTapped(sender: null, e: new TappedEventArgs(null));
        }
    }
    
    private async void OpenSelectionPopup_OnTapped(object? sender, TappedEventArgs e)
    {
        var popupService = IPlatformApplication.Current?.Services.GetService<IPopupService>();
        ArgumentNullException.ThrowIfNull(popupService);

        var queryAttributes = new Dictionary<string, object>
        {
            [nameof(CfCollectionSingleSelectionPopupViewModel.Title)] = Label ?? string.Empty,
            [nameof(CfCollectionSingleSelectionPopupViewModel.ItemsSource)] = ItemsSource ?? [],
            [nameof(CfCollectionSingleSelectionPopupViewModel.IsSearchVisible)] = IsSearchVisible
        };
        
        if (SelectedItem is not null)
        {
            queryAttributes[nameof(CfCollectionSingleSelectionPopupViewModel.SelectedItem)] = SelectedItem;
        }

        _isPopupOpened = true;

        var popupResult = await popupService
            .ShowPopupAsync<CfCollectionSingleSelectionPopupViewModel, DisplayValueItem>(
                Shell.Current,
                options: new BottomSelectionPopupOptionsSettings(),
                shellParameters: queryAttributes)
            .ConfigureAwait(false);

        if (popupResult is { WasDismissedByTappingOutsideOfPopup: false, Result: not null })
        {
            SelectedItem = popupResult.Result;
            SelectionChangedCommand?.Execute(SelectedItem);
            Value = popupResult.Result.DisplayValue;
        }
        
        _isPopupOpened = false;
    }
}