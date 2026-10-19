using System.Security.Cryptography;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitTrackerDomain;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai
namespace HabitTrackerMaui.ViewModels;

public partial class HabitEditViewModel : BaseViewModel, IQueryAttributable
{
    private string? _habitID;
    private bool _formLoaded;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string? description;

    [ObservableProperty]
    private FrequencyOption? selectedFrequency;

    [ObservableProperty]
    private bool canSave;

    public List<FrequencyOption> Frequencies { get; } = new List<FrequencyOption>();

    public HabitEditViewModel(IHabitRepository repository) : base(repository)
    {
        Title = "Jauns ieradums";
        Name = string.Empty;
        // Sagatavo atkārtošanās biežuma izvēles.
        foreach (HabitFrequency frequency in Enum.GetValues<HabitFrequency>())
            Frequencies.Add(new FrequencyOption(frequency));
        SelectedFrequency = Frequencies[0];
    }

    // Saņem rediģējamā ieraduma ID.
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("HabitID", out object? value) && value is string id)
        {
            _habitID = id;
            Title = "Rediģēt ieradumu";
        }
    }

    public override Task RefreshAsync() => LoadAsync();

    [RelayCommand]
    private async Task LoadAsync()
    {
        // Atgriešanās no fona nedrīkst izdzēst vēl nesaglabātos formas laukus.
        if (_formLoaded || !BeginOperation()) return;
        try
        {
            if (_habitID != null)
            {
                Habit habit = await GetCurrentUserHabitAsync(_habitID);
                if (habit.isArchived)
                    throw new InvalidOperationException("Arhivētu ieradumu nevar rediģēt.");
                Name = habit.Name;
                Description = habit.Description;
                foreach (FrequencyOption option in Frequencies)
                {
                    if (option.Value == habit.Frequency)
                    {
                        SelectedFrequency = option;
                        break;
                    }
                }
            }
            else
            {
                // Arī jaunu ieradumu drīkst saglabāt tikai pēc sekmīgas faila ielādes.
                await Repository.GetAllAsync();
            }
            _formLoaded = true;
            CanSave = true;
        }
        catch (Exception exception) { ShowError(exception); }
        finally { IsBusy = false; }
    }

    // Saglabā jaunu ieradumu vai esošā ieraduma izmaiņas.
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!CanSave || !BeginOperation()) return;
        try
        {
            if (SelectedFrequency == null)
                throw new ArgumentException("Izvēlieties atkārtošanās biežumu.");
            Habit habit;
            if (_habitID == null)
            {
                habit = new Habit(RandomNumberGenerator.GetHexString(32), CurrentUserID,
                    Name, Description, SelectedFrequency.Value);
            }
            else
            {
                habit = await GetCurrentUserHabitAsync(_habitID);
                if (habit.isArchived)
                    throw new InvalidOperationException("Arhivētu ieradumu nevar rediģēt.");
                habit.Update(Name, Description, SelectedFrequency.Value);
            }
            await Repository.SaveAsync(habit);
            // Ja navigācija neizdodas, atkārtota poga neveidos otru ieradumu.
            _habitID = habit.ID;
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception exception) { ShowError(exception); }
        finally { IsBusy = false; }
    }

    // Atgriežas iepriekšējā lapā bez saglabāšanas.
    [RelayCommand]
    private async Task CancelAsync()
    {
        if (!BeginOperation()) return;
        try { await Shell.Current.GoToAsync(".."); }
        catch (Exception exception) { ShowError(exception); }
        finally { IsBusy = false; }
    }
}

