using CommunityToolkit.Maui;
using Microsoft.Maui.Controls.Shapes;

namespace CraftUI.Library.Maui.Popups.Settings;

public class BottomSelectionPopupOptionsSettings : IPopupOptions
{
    public bool CanBeDismissedByTappingOutsideOfPopup { get; } = true;

    public Action? OnTappingOutsideOfPopup { get; } = null;

    public Color PageOverlayColor { get; } = Colors.Black.WithAlpha(0.3f);

    public Shape? Shape { get; } = new RoundRectangle
    {
        CornerRadius = new CornerRadius(0),
        StrokeThickness = 0
    };

    public Shadow? Shadow { get; } = new()
    {
        Brush = Colors.Black,
        Offset = new Point(20, 20),
        Radius = 40,
        Opacity = 0.8f
    };
}