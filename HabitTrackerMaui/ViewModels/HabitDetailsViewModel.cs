using System.Collections.ObjectModel;
using System.Security.Cryptography;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitTrackerDomain;
using HabitTrackerMaui.Views;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai
namespace HabitTrackerMaui.ViewModels;

public partial class HabitDetailsViewModel : BaseViewModel, IQueryAttributable
{
    private string _habitID = string.Empty;

    [ObservableProperty]
    private Habit? habit;

    [ObservableProperty]
    private bool canChange;

    [ObservableProperty]
    private string frequencyText = string.Empty;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private DateTime? selectedDate;

    [ObservableProperty]
    private string selectedDateStatus = string.Empty;

    [ObservableProperty]
    private string successMessage = string.Empty;

    public ObservableCollection<string> History { get; } = new ObservableCollection<string>();

    public HabitDetailsViewModel(IHabitRepository repository) : base(repository)
    {
        FrequencyText = StatusText = SelectedDateStatus = SuccessMessage = string.Empty;
        SelectedDate = DateTime.Today;
    }

    // Saņem atvērtā ieraduma ID.
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("HabitID", out object? value) && value is string id)
        {
            _habitID = id;
            SelectedDate = DateTime.Today;
        }
    }

    public override Task RefreshAsync() => LoadAsync();

    // Ielādē ieraduma datus.
    [RelayCommand]
    private async Task LoadAsync()
    {
        if (!BeginOperation()) return;
        CanChange = false;
        try { DisplayHabit(await GetCurrentUserHabitAsync(_habitID)); }
        catch (Exception exception)
        {
            Habit = null;
            History.Clear();
            ShowError(exception);
        }
        finally { IsBusy = false; }
    }

    // Sagatavo ieraduma datus attēlošanai.
    private void DisplayHabit(Habit habit)
    {
        Habit = habit;
        CanChange = !habit.isArchived;
        FrequencyText = FrequencyOption.GetTitle(habit.Frequency);
        StatusText = habit.isArchived ? "Arhivēts" : "Aktīvs";
        List<DateOnly> dates = new List<DateOnly>();
        foreach (HabitCompletion completion in habit.Completions)
            dates.Add(completion.Date);
        // Jaunākie izpildes datumi ir saraksta sākumā.
        dates.Sort();
        dates.Reverse();
        History.Clear();
        foreach (DateOnly date in dates)
            History.Add(date.ToString("dd.MM.yyyy"));
        if (dates.Count == 0)
            History.Add("Izpildes ierakstu vēl nav.");
        UpdateSelectedDateStatus();
    }

    partial void OnSelectedDateChanged(DateTime? value) => UpdateSelectedDateStatus();

    // Pārbauda izpildi izvēlētajā datumā.
    private void UpdateSelectedDateStatus()
    {
        if (SelectedDate == null)
            SelectedDateStatus = "Izvēlieties izpildes datumu.";
        else if (Habit?.IsCompletedOn(DateOnly.FromDateTime(SelectedDate.Value)) == true)
            SelectedDateStatus = "Šajā datumā jau izpildīts.";
        else
            SelectedDateStatus = "Šajā datumā vēl nav izpildīts.";
    }

    // Saglabā izpildi izvēlētajā datumā.
    [RelayCommand]
    private async Task MarkCompletedAsync()
    {
        if (!CanChange || !BeginOperation()) return;
        SuccessMessage = string.Empty;
        try
        {
            if (SelectedDate == null)
                throw new ArgumentException("Izvēlieties izpildes datumu.");
            // Maina atsevišķi ielasītu objektu. Kļūdas gadījumā ekrāna dati paliek nemainīti.
            Habit habit = await GetCurrentUserHabitAsync(_habitID);
            habit.MarkCompleted(RandomNumberGenerator.GetHexString(32), DateOnly.FromDateTime(SelectedDate.Value));
            await Repository.SaveAsync(habit);
            DisplayHabit(habit);
            SuccessMessage = "Izpilde saglabāta.";
        }
        catch (Exception exception) { ShowError(exception); }
        finally { IsBusy = false; }
    }

    // Atver ieraduma rediģēšanu.
    [RelayCommand]
    private async Task EditAsync()
    {
        if (!CanChange || !BeginOperation()) return;
        try
        {
            await Shell.Current.GoToAsync(nameof(HabitEditPage),
                new ShellNavigationQueryParameters { { "HabitID", _habitID } });
        }
        catch (Exception exception) { ShowError(exception); }
        finally { IsBusy = false; }
    }

    // Pēc apstiprinājuma arhivē ieradumu.
    [RelayCommand]
    private async Task ArchiveAsync()
    {
        if (!CanChange || !BeginOperation()) return;
        SuccessMessage = string.Empty;
        try
        {
            bool confirmed = await Shell.Current.DisplayAlertAsync("Arhivēt ieradumu?",
                "Ieradums pazudīs no aktīvo ieradumu saraksta. Izpildes vēsture tiks saglabāta.", "Arhivēt", "Atcelt");
            if (!confirmed) return;
            Habit habit = await GetCurrentUserHabitAsync(_habitID);
            habit.SetArchived(true);
            await Repository.SaveAsync(habit);
            DisplayHabit(habit);
            SuccessMessage = "Ieradums arhivēts.";
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception exception) { ShowError(exception); }
        finally { IsBusy = false; }
    }
}

