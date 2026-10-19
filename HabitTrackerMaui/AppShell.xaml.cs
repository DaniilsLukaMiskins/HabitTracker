using HabitTrackerMaui.Views;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai
namespace HabitTrackerMaui
{
    public partial class AppShell : Shell
    {
        public AppShell(MainPage mainPage)
        {
            InitializeComponent();
            // Par sākuma lapu izmanto ieradumu sarakstu.
            HabitsContent.Content = mainPage;
            // Reģistrē ceļus uz ieraduma detaļām un rediģēšanu.
            Routing.RegisterRoute(nameof(HabitDetailsPage), typeof(HabitDetailsPage));
            Routing.RegisterRoute(nameof(HabitEditPage), typeof(HabitEditPage));
        }
    }
}
