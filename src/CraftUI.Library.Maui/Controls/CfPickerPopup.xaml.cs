using System.Windows.Input;
using CommunityToolkit.Maui;
using CraftUI.Library.Maui.Common;
using CraftUI.Library.Maui.Common.Models;
using CraftUI.Library.Maui.Popups;
using CraftUI.Library.Maui.Popups.Settings;

namespace CraftUI.Library.Maui.Controls;

public partial class CfPickerPopup
{
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(List<DisplayValueItem>), typeof(CfPickerPopup), defaultValue: null);
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(DisplayValueItem), typeof(CfPickerPopup), defaultValue: null, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(CfPickerPopup), defaultBindingMode: BindingMode.TwoWay);
    public static readonly BindableProperty SelectionChangedCommandProperty = BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(CfPickerPopup));
    public static readonly BindableProperty IsSearchVisibleProperty = BindableProperty.Create(nameof(IsSearchVisible), typeof(bool), typeof(CfPickerPopup), defaultBindingMode: BindingMode.OneWay);
   
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

    public CfPickerPopup()
    {
        InitializeComponent();
    }

    private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue) => ((CfPickerPopup) bindable).UpdateSelectedItemView();

    private void UpdateSelectedItemView()
    {
        Value = SelectedItem?.DisplayValue;
    }
    
    private async void OpenSelectionPopup_OnTapped(object? sender, TappedEventArgs e)
    {
        var popupService = Application.Current?.Handler?.MauiContext?.Services.GetService<IPopupService>();
        ArgumentNullException.ThrowIfNull(popupService);

        var queryAttributes = new Dictionary<string, object>
        {
            [nameof(CfCollectionSingleSelectionPopupViewModel.Title)] = Label ?? "",
            [nameof(CfCollectionSingleSelectionPopupViewModel.ItemsSource)] = ItemsSource ?? [],
            [nameof(CfCollectionSingleSelectionPopupViewModel.IsSearchVisible)] = IsSearchVisible
        };
        
        if (SelectedItem is not null)
        {
            queryAttributes[nameof(CfCollectionSingleSelectionPopupViewModel.SelectedItem)] = SelectedItem;
        }

        var popupResult = await popupService.ShowPopupAsync<CfCollectionSingleSelectionPopupViewModel, DisplayValueItem>(
            Shell.Current, 
            options: new BottomSelectionPopupOptionsSettings(),
            shellParameters: queryAttributes);

        if (popupResult is { WasDismissedByTappingOutsideOfPopup: false, Result: not null })
        {
            SelectedItem = popupResult.Result;
            SelectionChangedCommand?.Execute(SelectedItem);
            Value = popupResult.Result.DisplayValue;
        }
    }
}