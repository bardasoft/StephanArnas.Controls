using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CraftUI.Library.Maui.Common;

public partial class ViewModelPopupBase : ObservableObject, IQueryAttributable
{
    protected readonly IPopupService PopupService;

    [ObservableProperty] 
    private string? _title;
    
    public ViewModelPopupBase(IPopupService popupService)
    {
        PopupService = popupService;
    }
    
    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(Title), out var title))
        {
            Title = title.ToString();
        }
        
        query.Clear();
    }
    
    [RelayCommand]
    private async Task Close()
    {
        await Task.Delay(100);
        await PopupService.ClosePopupAsync(Shell.Current);
    }
}