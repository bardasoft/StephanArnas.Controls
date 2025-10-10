using CommunityToolkit.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CraftUI.Library.Maui.Controls.ProgressBars;
using CraftUI.Library.Maui.Popups;
using Microsoft.Maui.Handlers;

namespace CraftUI.Library.Maui;

public static class DependencyInjection
{
    public static MauiAppBuilder UseCraftUi(this MauiAppBuilder builder)
    {
        builder.UseSkiaSharp();

        builder.Services
                .AddTransientPopup<CfCollectionSingleSelectionPopup, CfCollectionSingleSelectionPopupViewModel>()
                .AddTransientPopup<CfCollectionMultiSelectionPopup, CfCollectionMultiSelectionPopupViewModel>();
        
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler<ProgressBar, CfProgressBarHandler>();
        });

        ApplyHandlerStyles();
        
        return builder;
    }
    
    private static void ApplyHandlerStyles()
    {
        EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
        {
#if ANDROID
        // Remove the underline from the EditText
        handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
        });
        
        EntryHandler.Mapper.AppendToMapping("SetUpEntry", (handler, _) =>
        {
#if ANDROID

#elif IOS || MACCATALYST
        // Remove outline from the UITextField
        handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS

#endif
        });
        
        PickerHandler.Mapper.AppendToMapping("NoUnderline", (handler, _) =>
        {
#if ANDROID
        // Remove the underline from the Spinner (Picker)
        handler.PlatformView.Background = null;
#endif
        });

        PickerHandler.Mapper.AppendToMapping("SetUpPicker", (handler, _) =>
        {
#if ANDROID
        // Set the background to transparent
        handler.PlatformView.Background = null;
#elif IOS || MACCATALYST
        // Remove border for the UITextField (Picker)
        if (handler.PlatformView is UIKit.UITextField textField)
        {
            textField.BorderStyle = UIKit.UITextBorderStyle.None;
        }
#elif WINDOWS

#endif
        });
        
        DatePickerHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.Background = null;
#elif IOS || MACCATALYST
            handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            //handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
        });
    }
}