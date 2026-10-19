using Microsoft.Extensions.Logging;

using HabitTrackerDomain;
using HabitTrackerInfrastructure;
using HabitTrackerMaui.ViewModels;
using HabitTrackerMaui.Views;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra


namespace HabitTrackerMaui
{
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            string filePath = Path.Combine(FileSystem.AppDataDirectory, "habits.json"); // Ceļš līdz ieradumu JSON failam lietotnes datu mapē.
            builder.Services.AddSingleton<IHabitRepository>(new JsonHabitRepository(filePath)); // Reģistrē vienu kopīgu JSON glabātuvi darbam ar ieradumiem.

            // Galvenā saraksta loģikai, lapai un navigācijai atkārtoti izmanto tos pašus objektus.
            builder.Services.AddSingleton<HabitsViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<AppShell>();

            // Detaļu lapai un tās loģikai katrā pieprasījumā izveido jaunus objektus.
            builder.Services.AddTransient<HabitDetailsViewModel>();
            builder.Services.AddTransient<HabitDetailsPage>();

            // Izveides un rediģēšanas lapai un tās loģikai katrā pieprasījumā izveido jaunus objektus.
            builder.Services.AddTransient<HabitEditViewModel>();
            builder.Services.AddTransient<HabitEditPage>();

            return builder.Build();
        }
    }
}
