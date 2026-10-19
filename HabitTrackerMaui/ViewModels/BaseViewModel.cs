using CommunityToolkit.Mvvm.ComponentModel;
using HabitTrackerDomain;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai
namespace HabitTrackerMaui.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    protected const string CurrentUserID = "user-1";
    protected readonly IHabitRepository Repository;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string errorMessage = string.Empty;

    public bool IsNotBusy => !IsBusy;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    protected BaseViewModel(IHabitRepository repository)
    {
        Repository = repository;
        ErrorMessage = string.Empty;
    }

    // Atjauno lapas datus.
    public abstract Task RefreshAsync();

    // Neļauj sākt otru darbību, kamēr iepriekšējā nav pabeigta.
    protected bool BeginOperation()
    {
        if (IsBusy)
            return false;
        IsBusy = true;
        ErrorMessage = string.Empty;
        return true;
    }

    // Sagatavo lietotājam saprotamu kļūdas ziņojumu.
    protected void ShowError(Exception exception)
    {
        System.Diagnostics.Debug.WriteLine(exception);
        if (exception is HabitStorageException or ArgumentException or InvalidOperationException)
            ErrorMessage = exception.Message;
        else
            ErrorMessage = "Darbību neizdevās pabeigt. Lūdzu, mēģiniet vēlreiz.";
    }

    // Meklē ieradumu tikai pašreizējā lietotāja datos.
    protected async Task<Habit> GetCurrentUserHabitAsync(string id)
    {
        List<Habit> habits = await Repository.GetAllAsync();
        foreach (Habit habit in habits)
        {
            if (habit.ID == id && habit.UserID == CurrentUserID)
                return habit;
        }
        throw new InvalidOperationException("Ieradums nav atrasts šim lietotājam.");
    }
}

