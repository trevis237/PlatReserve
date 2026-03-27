using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using PlatReserve.ViewModels;
using PlatReserve.Views;

namespace PlatReserve
{
    public static class MauiProgramExtensions
    {
        public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
        {
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();

            // Cela dit à MAUI : "Si quelqu'un demande IRealmFactory, donne-lui toujours la même instance de RealmFactory" (Singleton)
            builder.Services.AddSingleton<PlatReserve.Services.IRealmService,
                PlatReserve.Services.RealmService>();

            // On enregistre le ViewModel (Transient = crée une nouvelle instance à chaque fois qu'on ouvre une page)
            builder.Services.AddTransient<PlatReserve.ViewModels.ReservationViewModel>();

            builder.Services.AddTransient<PlatReserve.Views.ReservationPage>();
            builder.Services.AddSingleton<PlatReserve.Services.AuthService>();
            builder.Services.AddTransient<PlatReserve.Views.LoginPage>();
            builder.Services.AddTransient<PlatReserve.ViewModels.AdminViewModel>();
            builder.Services.AddTransient<PlatReserve.Views.AdminPage>();
            builder.Services.AddTransient<PlatReserve.ViewModels.VoyageurViewModel>();
            builder.Services.AddTransient<PlatReserve.Views.VoyageurDashboardPage>();
            builder.Services.AddTransient<PlatReserve.Views.SignupPage>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SignupViewModel>();
            builder.Services.AddTransient<BilletDetailViewModel>();
            builder.Services.AddTransient<BilletDetailPage>();
#endif

            return builder;
        }
    }
}
