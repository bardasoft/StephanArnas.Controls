using System.Windows.Input;
using CommunityToolkit.Maui;
using CraftUI.Library.Maui.Common.Models;
using CraftUI.Library.Maui.Popups;
using CraftUI.Library.Maui.Popups.Settings;

namespace CraftUI.Library.Maui.Controls;

public partial class CfPickerMultipleSelection
{
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList<DisplayValueItem>), typeof(CfPickerMultipleSelection), defaultBindingMode: BindingMode.OneWay, propertyChanged: ItemsSourceChanged);
    public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems), typeof(IList<DisplayValueItem>), typeof(CfPickerMultipleSelection), defaultBindingMode: BindingMode.TwoWay, propertyChanged: SelectedItemsPropertyChanged);
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
    
    private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue) => ((CfPickerMultipleSelection)bindable).UpdateItemsSourceView();
    
    public void UpdateItemsSourceView()
    {
        if (_isPopupOpened)
        {
            // The popup is open, close and open again to refresh the collection
            IPlatformApplication.Current?.Services.GetService<IPopupService>()?.ClosePopupAsync(Shell.Current);
            OpenSelectionPopup_OnTapped(sender: null, e: new TappedEventArgs(null));
        }
    }
    
    private static void SelectedItemsPropertyChanged(BindableObject bindable, object oldValue, object newValue) => ((CfPickerMultipleSelection)bindable).UpdateSelectedItemsView();
    
    public void UpdateSelectedItemsView()
    {
        OnPropertyChanged(nameof(SelectedItems));
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