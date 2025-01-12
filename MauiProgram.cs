//namespace Todo;

//public static class MauiProgram
//{
//    public static MauiApp CreateMauiApp()
//    {
//        var builder = MauiApp.CreateBuilder();
//        builder
//            .UseMauiApp<App>()
//            .ConfigureFonts(fonts =>
//            {
//                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
//            });

//        builder.Services.AddMauiBlazorWebView();
//#if DEBUG
//        builder.Services.AddBlazorWebViewDeveloperTools();
//#endif

//        return builder.Build();
//    }
//}

using Todo.Data;
namespace Todo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Add Blazor WebView
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddSingleton<GlobalState>();



#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        // Register GlobalState as a scoped service
        builder.Services.AddScoped<GlobalState>();

        return builder.Build();
    }
}
