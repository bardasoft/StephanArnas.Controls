using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui;
using CraftUI.Library.Maui.Common.Models;
using CraftUI.Library.Maui.Popups;
using CraftUI.Library.Maui.Popups.Settings;

namespace CraftUI.Library.Maui.Controls;

public partial class CfPickerMultipleSelection
{
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList<DisplayValueItem>), typeof(CfPickerMultipleSelection), defaultBindingMode: BindingMode.OneWay);
    public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems), typeof(IList<DisplayValueItem>), typeof(CfPickerMultipleSelection), defaultBindingMode: BindingMode.TwoWay, propertyChanged: SelectedItemsPropertyChanged, coerceValue: CoerceSelectedItems, defaultValueCreator: DefaultValueCreator);
    public static readonly BindableProperty SelectionChangedCommandProperty = BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(CfPickerMultipleSelection));
    public static readonly BindableProperty IsSearchVisibleProperty = BindableProperty.Create(nameof(IsSearchVisible), typeof(bool), typeof(CfPickerMultipleSelection), defaultBindingMode: BindingMode.OneWay);
    
    private bool _isPopupOpened;
    
    public IList<DisplayValueItem>? ItemsSource
    {
        get => (IList<DisplayValueItem>?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public IList<DisplayValueItem>? SelectedItems
    {
        get => (IList<DisplayValueItem>?)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value); //new SelectionList(this, value));
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

    public CfPickerMultipleSelection()
    {
        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        ActionIconSource ??= "chevron_bottom.png";
        ActionIconCommand ??= new Command(() => OpenSelectionPopup_OnTapped(sender: null, e: new TappedEventArgs(null)));
    }
    
    private static void SelectedItemsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var selectableItemsView = (CfPickerMultipleSelection)bindable;
        var oldSelection = (IList<DisplayValueItem>)oldValue;
        var newSelection = (IList<DisplayValueItem>)newValue;

        selectableItemsView.SelectedItemsPropertyChanged(oldSelection, newSelection);
    }

    private static object CoerceSelectedItems(BindableObject bindable, object? value)
    {
        if (value == null)
        {
            return new SelectionList((CfPickerMultipleSelection)bindable);
        }

        if (value is SelectionList)
        {
            return value;
        }

        return new SelectionList((CfPickerMultipleSelection)bindable, value as IList<DisplayValueItem>);
    }

    private static IList<DisplayValueItem> DefaultValueCreator(BindableObject bindable)
    {
        return new SelectionList((CfPickerMultipleSelection)bindable);
    }

    internal void SelectedItemsPropertyChanged(IList<DisplayValueItem> oldSelection, IList<DisplayValueItem> newSelection)
    {
        OnPropertyChanged(SelectedItemsProperty.PropertyName);
        
        /*if (_isPopupOpened)
        {
            // The popup is open, close and open again to refresh the collection
            IPlatformApplication.Current?.Services.GetService<IPopupService>()?.ClosePopupAsync(Shell.Current);
            OpenSelectionPopup_OnTapped(sender: null, e: new TappedEventArgs(null));
        }*/
    }
    
    private async void OpenSelectionPopup_OnTapped(object? sender, TappedEventArgs e)
    {
        var popupService = IPlatformApplication.Current?.Services.GetService<IPopupService>();
        ArgumentNullException.ThrowIfNull(popupService);

        var queryAttributes = new Dictionary<string, object>
        {
            [nameof(CfCollectionMultiSelectionPopupViewModel.Title)] = Label ?? string.Empty,
            [nameof(CfCollectionMultiSelectionPopupViewModel.ItemsSource)] = ItemsSource ?? [],
            [nameof(CfCollectionMultiSelectionPopupViewModel.IsSearchVisible)] = IsSearchVisible
        };
        
        if (SelectedItems is not null)
        {
            queryAttributes[nameof(CfCollectionMultiSelectionPopupViewModel.SelectedItems)] = SelectedItems;
        }

        _isPopupOpened = true;
        
        var popupResult = await popupService
            .ShowPopupAsync<CfCollectionMultiSelectionPopupViewModel, IList<DisplayValueItem>>(
                Shell.Current,
                options: new BottomSelectionPopupOptionsSettings(),
                shellParameters: queryAttributes)
            .ConfigureAwait(false);

        if (popupResult is { Result: not null })
        {
            SelectedItems = popupResult.Result;
            SelectionChangedCommand?.Execute(SelectedItems);
        }

        _isPopupOpened = false;
    }
}