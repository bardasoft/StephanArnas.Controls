using System.Windows.Input;

namespace CraftUI.Library.Maui.Controls;

public partial class CfSearch
{
    public static readonly BindableProperty SearchProperty = BindableProperty.Create(nameof(Search), typeof(string), typeof(CfSearch), defaultBindingMode: BindingMode.TwoWay);
    public static readonly BindableProperty SearchUpdatedCommandProperty = BindableProperty.Create(nameof(SearchUpdatedCommand), typeof(ICommand), typeof(CfSearch));
    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CfSearch), defaultBindingMode: BindingMode.OneWay);

    public string? Search
    {
        get => (string?)GetValue(SearchProperty);
        set => SetValue(SearchProperty, value);
    }
    
    public ICommand? SearchUpdatedCommand
    {
        get => (ICommand?)GetValue(SearchUpdatedCommandProperty);
        set => SetValue(SearchUpdatedCommandProperty, value);
    }
    
    public string? Placeholder
    {
        get => (string?)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    
    public CfSearch()
    {
        InitializeComponent();
    }

    private void SearchImage_OnTapped(object? sender, TappedEventArgs e)
    {
        SearchEntry.Focus();
    }

    private void SearchEntry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        SearchUpdatedCommand?.Execute(e.NewTextValue);
        SearchEntry.Focus();
    }
}