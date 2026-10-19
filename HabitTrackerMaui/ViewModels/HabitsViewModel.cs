using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitTrackerDomain;
using HabitTrackerMaui.Views;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai
namespace HabitTrackerMaui.ViewModels;

public partial class HabitsViewModel : BaseViewModel
{
    public ObservableCollection<HabitListItem> Habits { get; } = new ObservableCollection<HabitListItem>();

    [ObservableProperty]
    private bool isLoaded;

    [ObservableProperty]
    private string emptyMessage = string.Empty;

    [ObservableProperty]
    private string todayText = string.Empty;

    public DateOnly LoadedDate { get; private set; }

    public HabitsViewModel(IHabitRepository repository) : base(repository)
    {
        EmptyMessage = "Ielādē ieradumus…";
        TodayText = string.Empty;
    }

    public override Task RefreshAsync() => LoadAsync();

    // Ielādē pašreizējā lietotāja aktīvos ieradumus.
    [RelayCommand]
    private async Task LoadAsync()
    {
        if (!BeginOperation()) return;
        IsLoaded = false;
        try
        {
            List<Habit> habits = await Repository.GetAllAsync();
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            Habits.Clear();
            foreach (Habit habit in habits)
            {
                if (habit.UserID == CurrentUserID && !habit.isArchived)
                    Habits.Add(new HabitListItem(habit, today));
            }
            LoadedDate = today;
            TodayText = $"Šodien: {today:dd.MM.yyyy}";
            EmptyMessage = "Aktīvu ieradumu nav. Pievienojiet savu pirmo ieradumu.";
            IsLoaded = true;
        }
        catch (Exception exception)
        {
            Habits.Clear();
            EmptyMessage = "Dati nav pieejami. Novērsiet kļūdu un nospiediet “Atjaunot”.";
            ShowError(exception);
        }
        finally { IsBusy = false; }
    }

    // Atver jauna ieraduma formu.
    [RelayCommand]
    private async Task CreateAsync()
    {
        if (!IsLoaded || !BeginOperation()) return;
        try { await Shell.Current.GoToAsync(nameof(HabitEditPage)); }
        catch (Exception exception) { ShowError(exception); }
        finally { IsBusy = false; }
    }

    // Atver izvēlētā ieraduma informāciju.
    [RelayCommand]
    private async Task OpenDetailsAsync(HabitListItem? item)
    {
        if (item == null || !IsLoaded || !BeginOperation()) return;
        try
        {
            await Shell.Current.GoToAsync(nameof(HabitDetailsPage),
                new ShellNavigationQueryParameters { { "HabitID", item.ID } });
        }
        catch (Exception exception) { ShowError(exception); }
        finally { IsBusy = false; }
    }
}

