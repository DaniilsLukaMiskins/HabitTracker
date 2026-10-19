using Microsoft.Extensions.DependencyInjection;
using HabitTrackerMaui.ViewModels;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai
namespace HabitTrackerMaui
{
    public partial class App : Application
    {
        private readonly IServiceProvider _services;

        public App(IServiceProvider services)
        {
            InitializeComponent();
            _services = services;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Galveno logu izveido ar reģistrēto navigāciju.
            Window window = new Window(_services.GetRequiredService<AppShell>());
            window.Resumed += OnWindowResumed;
            return window;
        }

        // Atjauno atvērtās lapas datus pēc atgriešanās lietotnē.
        private async void OnWindowResumed(object? sender, EventArgs e)
        {
            if (Shell.Current?.CurrentPage?.BindingContext is BaseViewModel viewModel)
            {
                await viewModel.RefreshAsync();
            }
        }
    }
}