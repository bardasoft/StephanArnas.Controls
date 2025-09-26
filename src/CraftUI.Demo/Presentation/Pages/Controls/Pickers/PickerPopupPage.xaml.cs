namespace CraftUI.Demo.Presentation.Pages.Controls.Pickers;

public partial class PickerPopupPage
{
    public PickerPopupPage(PickerPage2ViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}