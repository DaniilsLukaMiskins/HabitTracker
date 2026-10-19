using HabitTrackerMaui.ViewModels;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai
namespace HabitTrackerMaui
{
    public partial class MainPage : ContentPage
    {
        private readonly HabitsViewModel _viewModel;
        private readonly IDispatcherTimer _dateTimer;

        public MainPage(HabitsViewModel viewModel)
        {
            InitializeComponent();
            // Saista lapu ar ieradumu saraksta datiem.
            _viewModel = viewModel;
            BindingContext = viewModel;
            _dateTimer = Dispatcher.CreateTimer();
            _dateTimer.Interval = TimeSpan.FromMinutes(1);
            _dateTimer.Tick += OnDateTimerTick;
        }

        // Atjauno sarakstu, atverot lapu.
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.RefreshAsync();
            _dateTimer.Start();
        }

        // Aptur datuma pārbaudi, aizejot no lapas.
        protected override void OnDisappearing()
        {
            _dateTimer.Stop();
            base.OnDisappearing();
        }

        // Atjauno šodienas statusu pēc datuma maiņas.
        private async void OnDateTimerTick(object? sender, EventArgs e)
        {
            if (_viewModel.IsLoaded && _viewModel.LoadedDate != DateOnly.FromDateTime(DateTime.Today))
            {
                await _viewModel.RefreshAsync();
            }
        }
    }
}
