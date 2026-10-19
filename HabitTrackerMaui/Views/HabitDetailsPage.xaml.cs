using HabitTrackerMaui.ViewModels;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai
namespace HabitTrackerMaui.Views;

public partial class HabitDetailsPage : ContentPage
{
    private readonly HabitDetailsViewModel _viewModel;

    public HabitDetailsPage(HabitDetailsViewModel viewModel)
    {
        InitializeComponent();
        // Saista lapu ar ieraduma datiem un komandām.
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    // Atjauno ieraduma informāciju, atverot lapu.
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
