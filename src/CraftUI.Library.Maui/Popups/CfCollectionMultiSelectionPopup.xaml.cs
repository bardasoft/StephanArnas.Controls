namespace CraftUI.Library.Maui.Popups;

public partial class CfCollectionMultiSelectionPopup
{
    public CfCollectionMultiSelectionPopup(CfCollectionMultiSelectionPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}