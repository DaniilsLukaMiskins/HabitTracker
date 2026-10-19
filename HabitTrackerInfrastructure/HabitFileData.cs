using HabitTrackerDomain;

namespace HabitTrackerInfrastructure;
//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra

// Faila modelis ir atdalīts no domēna. Trūkstoši lauki tiek uzskatīti par kļūdu.
internal sealed class HabitFileData
{
    public required string ID { get; set; }
    public required string UserID { get; set; }
    public required string Name { get; set; }
    public required string? Description { get; set; }
    public required HabitFrequency Frequency { get; set; }
    public required DateTime CreatedTime { get; set; }
    public required DateTime UpdatedTime { get; set; }
    public required bool IsArchived { get; set; }
    public required List<HabitCompletionFileData> Completions { get; set; }

    public Habit ToHabit() // No faila datiem atjauno ieradumu un tā izpildes vēsturi.
    {
        if (Completions == null)
        { throw new ArgumentException("Ieraduma izpildes vēsture nav norādīta."); }

        List<HabitCompletion> completions = new List<HabitCompletion>();

        foreach (HabitCompletionFileData completion in Completions)
        {
            if (completion == null)
            { throw new ArgumentException("Izpildes ieraksts nedrīkst būt tukšs."); }
            completions.Add(new HabitCompletion(completion.ID, completion.HabitID, completion.Date));
        }
        return Habit.Restore(ID, UserID, Name, Description, Frequency,
            CreatedTime, UpdatedTime, IsArchived, completions);
    }

    public static HabitFileData FromHabit(Habit habit) // Pārveido ieradumu par datiem, kurus var saglabāt JSON failā.
    {
        List<HabitCompletionFileData> completions = new List<HabitCompletionFileData>();
        foreach (HabitCompletion completion in habit.Completions)
        {
            completions.Add(new HabitCompletionFileData
            {
                ID = completion.ID,
                HabitID = completion.HabitID,
                Date = completion.Date
            });
        }
        return new HabitFileData
        {
            ID = habit.ID,
            UserID = habit.UserID,
            Name = habit.Name,
            Description = habit.Description,
            Frequency = habit.Frequency,
            CreatedTime = habit.CreatedTime,
            UpdatedTime = habit.UpdatedTime,
            IsArchived = habit.isArchived,
            Completions = completions
        };
    }
}

internal sealed class HabitCompletionFileData
{
    public required string ID { get; set; }
    public required string HabitID { get; set; }
    public required DateOnly Date { get; set; }
}
