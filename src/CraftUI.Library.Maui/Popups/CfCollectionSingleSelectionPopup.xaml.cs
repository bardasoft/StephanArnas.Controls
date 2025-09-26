namespace CraftUI.Library.Maui.Popups;

public partial class CfCollectionSingleSelectionPopup
{
    public CfCollectionSingleSelectionPopup(CfCollectionSingleSelectionPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}