namespace CraftUI.Demo.Presentation.Pages.Controls.Pickers;

public partial class PickerSingleSelectionPage
{
    public PickerSingleSelectionPage(PickerPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}