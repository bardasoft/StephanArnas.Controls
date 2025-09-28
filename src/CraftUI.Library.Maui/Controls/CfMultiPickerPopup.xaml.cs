using System.Windows.Input;
using CommunityToolkit.Maui;
using CraftUI.Library.Maui.Common.Models;
using CraftUI.Library.Maui.Popups;
using CraftUI.Library.Maui.Popups.Settings;

namespace CraftUI.Library.Maui.Controls;

public partial class CfMultiPickerPopup
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(CfMultiPickerPopup));
    public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems), typeof(IList<DisplayValueItem>), typeof(CfMultiPickerPopup), defaultBindingMode: BindingMode.TwoWay, propertyChanged: SelectedItemsPropertyChanged, coerceValue: CoerceSelectedItems, defaultValueCreator: DefaultValueCreator);
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList<DisplayValueItem>), typeof(CfMultiPickerPopup), defaultBindingMode: BindingMode.OneWay);
    public static readonly BindableProperty SelectionChangedCommandProperty = BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(CfMultiPickerPopup));
    public static readonly BindableProperty IsSearchVisibleProperty = BindableProperty.Create(nameof(IsSearchVisible), typeof(bool), typeof(CfMultiPickerPopup), defaultBindingMode: BindingMode.OneWay);
    
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public IList<DisplayValueItem>? SelectedItems
    {
        get => (IList<DisplayValueItem>?)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value); //new SelectionList(this, value));
    }
    
    public IList<DisplayValueItem>? ItemsSource
    {
        get => (IList<DisplayValueItem>?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
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

    public CfMultiPickerPopup()
    {
        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        ActionIconSource ??= "chevron_bottom.png";
        //ActionIconCommand ??= new Command(() => OnTapped(null, EventArgs.Empty));
    }
        
    private static void SelectedItemsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var selectableItemsView = (CfMultiPickerPopup)bindable;
        var oldSelection = (IList<DisplayValueItem>)oldValue;
        var newSelection = (IList<DisplayValueItem>)newValue;

        selectableItemsView.SelectedItemsPropertyChanged(oldSelection, newSelection);
    }

    private static object CoerceSelectedItems(BindableObject bindable, object? value)
    {
        if (value == null)
        {
            return new SelectionList((CfMultiPickerPopup)bindable);
        }

        if (value is SelectionList)
        {
            return value;
        }

        return new SelectionList((CfMultiPickerPopup)bindable, value as IList<DisplayValueItem>);
    }

    private static IList<DisplayValueItem> DefaultValueCreator(BindableObject bindable)
    {
        return new SelectionList((CfMultiPickerPopup)bindable);
    }

    internal void SelectedItemsPropertyChanged(IList<DisplayValueItem> oldSelection, IList<DisplayValueItem> newSelection)
    {
        OnPropertyChanged(SelectedItemsProperty.PropertyName);
    }
    
    private async void OpenSelectionPopup_OnTapped(object? sender, TappedEventArgs e)
    {
        var popupService = Application.Current?.Handler?.MauiContext?.Services.GetService<IPopupService>();
        ArgumentNullException.ThrowIfNull(popupService);

        var queryAttributes = new Dictionary<string, object>
        {
            [nameof(CfCollectionMultiSelectionPopupViewModel.Title)] = Label ?? "",
            [nameof(CfCollectionMultiSelectionPopupViewModel.ItemsSource)] = ItemsSource ?? [],
            [nameof(CfCollectionMultiSelectionPopupViewModel.IsSearchVisible)] = IsSearchVisible
        };
        
        if (SelectedItems is not null)
        {
            queryAttributes[nameof(CfCollectionMultiSelectionPopupViewModel.SelectedItems)] = SelectedItems;
        }

        var popupResult = await popupService.ShowPopupAsync<CfCollectionMultiSelectionPopupViewModel, IList<DisplayValueItem>>(
            Shell.Current,
            options: new BottomSelectionPopupOptionsSettings(),
            shellParameters: queryAttributes);

        if (popupResult is { WasDismissedByTappingOutsideOfPopup: false, Result: not null })
        {
            SelectedItems = popupResult.Result;
            SelectionChangedCommand?.Execute(SelectedItems);
        }
    }
}