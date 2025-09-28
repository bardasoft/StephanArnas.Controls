using System.Windows.Input;
using CraftUI.Library.Maui.Common.Extensions;
using CraftUI.Library.Maui.Common.Models;

namespace CraftUI.Library.Maui.Controls;

public partial class CfPicker
{
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList<DisplayValueItem>), typeof(CfPicker), propertyChanged: ItemsSourceChanged);
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(DisplayValueItem), typeof(CfPicker), defaultValue: null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);
    public static readonly BindableProperty SelectionChangedCommandProperty = BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(CfPicker));

    public IList<DisplayValueItem>? ItemsSource
    {
        get => (IList<DisplayValueItem>?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public DisplayValueItem? SelectedItem
    {
        get => (DisplayValueItem?)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    
    public ICommand? SelectionChangedCommand
    {
        get => (ICommand?)GetValue(SelectionChangedCommandProperty);
        set => SetValue(SelectionChangedCommandProperty, value);
    }
    
    public CfPicker()
    {
        InitializeComponent();
        
        Element.SetVisualElementBinding();
        Element.BindingContext = this;
    }
    
    private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue) => ((CfPicker)bindable).UpdateItemsSourceView();
    private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue) => ((CfPicker)bindable).OnSelectedItemChanged();

    public void UpdateItemsSourceView()
    {
#if ANDROID
        var wasOpen = Element.IsFocused;
        if (wasOpen)
        {
            // Rebuild the dialog content with the new ItemsSource.
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    Element.Unfocus();
                    await Task.Delay(50); // small delay to let the platform tear down the popup
                    Element.Focus();      // re-open with fresh data
                }
                catch
                {
                    // Ignore if anything goes wrong, at least ItemsSource is updated
                }
            });
        }
#endif
    }
    
    private void OnSelectedItemChanged()
    {
        SelectionChangedCommand?.Execute(null);
    }
}