using System.Windows.Input;

namespace CraftUI.Library.Maui.Popups.Templates;

public partial class HeaderTemplatePopup
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(HeaderTemplatePopup));
    public static readonly BindableProperty CloseCommandProperty = BindableProperty.Create(nameof(CloseCommand), typeof(ICommand), typeof(HeaderTemplatePopup));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public ICommand? CloseCommand
    {
        get => (ICommand?)GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }

    public HeaderTemplatePopup()
    {
        InitializeComponent();
    }
}