using CraftUI.Demo.Infrastructure;
using CraftUI.Library.Maui;
using CraftUI.Demo.Services;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using CraftUI.Demo.Presentation.Common;
using CraftUI.Demo.Presentation.Pages.Controls;
using CraftUI.Demo.Presentation.Pages.Controls.Buttons;
using CraftUI.Demo.Presentation.Pages.Controls.DatePickers;
using CraftUI.Demo.Presentation.Pages.Controls.Entries;
using CraftUI.Demo.Presentation.Pages.Controls.Pickers;
using CraftUI.Demo.Presentation.Pages.Controls.ProgressBars;
using CraftUI.Demo.Presentation.Pages.Settings;
using CraftUI.Demo.Presentation.Pages.UseCases;
using Microsoft.Maui.Controls.Shapes;

namespace CraftUI.Demo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseCraftUi()
            .UseMauiCommunityToolkit(ConfigurePopup)
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services
            .AddInfrastructure()
            .AddServices();

        builder.Services
            .AddTransient<UseCasesList, UseCasesListViewModel>()
            .AddTransient<ControlsList, ControlsListViewModel>()
            .AddTransient<SettingsPage, SettingsPageViewModel>()
            .AddTransientWithShellRoute<ButtonPage, ButtonPageViewModel>(RouteConstants.ButtonPage)
            .AddTransientWithShellRoute<EntryPage, EntryPageViewModel>(RouteConstants.EntryPage)
            .AddTransientWithShellRoute<DatePickerPage, DatePickerPageViewModel>(RouteConstants.DatePickerPage)
            .AddTransientWithShellRoute<PickerPage, PickerPageViewModel>(RouteConstants.PickerPage)
            .AddTransientWithShellRoute<PickerPopupPage, PickerPageViewModel>(RouteConstants.PickerPopupPage)
            .AddTransientWithShellRoute<MultiPickerPopupPage, PickerPageViewModel>(RouteConstants.MultiPickerPopupPage)
            .AddTransientWithShellRoute<ProgressBarPage, ProgressBarPageViewModel>(RouteConstants.ProgressBarPage);
        
        return builder.Build();
    }

    private static void ConfigurePopup(Options options)
    {
        options.SetPopupDefaults(new DefaultPopupSettings
        {
            CanBeDismissedByTappingOutsideOfPopup = true,
            Margin = 0,
            Padding = 0
        });

        options.SetPopupOptionsDefaults(new DefaultPopupOptionsSettings
        {
            CanBeDismissedByTappingOutsideOfPopup = true,
            Shape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(8),
                StrokeThickness = 0
            }
        });
    }
}
