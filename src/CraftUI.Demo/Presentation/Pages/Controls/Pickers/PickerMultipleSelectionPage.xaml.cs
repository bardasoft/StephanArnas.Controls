namespace CraftUI.Demo.Presentation.Pages.Controls.Pickers;

public partial class PickerMultipleSelectionPage
{
    public PickerMultipleSelectionPage(PickerPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}