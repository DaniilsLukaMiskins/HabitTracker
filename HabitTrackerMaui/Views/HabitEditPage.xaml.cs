using HabitTrackerMaui.ViewModels;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai
namespace HabitTrackerMaui.Views;

public partial class HabitEditPage : ContentPage
{
    private readonly HabitEditViewModel _viewModel;

    public HabitEditPage(HabitEditViewModel viewModel)
    {
        InitializeComponent();
        // Saista formu ar rediģēšanas datiem un komandām.
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    // Sagatavo formas datus, atverot lapu.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.RefreshAsync();
    }

    // Neļauj atgriezties ar pogu, kamēr notiek darbība.
    protected override bool OnBackButtonPressed()
    {
        return _viewModel.IsBusy || base.OnBackButtonPressed();
    }
}
