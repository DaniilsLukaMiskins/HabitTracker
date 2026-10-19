using HabitTrackerDomain;

namespace HabitTrackerMaui.ViewModels;

// Saraksta rinda tiek izveidota no jauna pēc datu izmaiņām.
public sealed class HabitListItem
{
    public string ID { get; }
    public string Name { get; }
    public string FrequencyText { get; }
    public string TodayStatus { get; }

    public HabitListItem(Habit habit, DateOnly today)
    {
        ID = habit.ID;
        Name = habit.Name;
        FrequencyText = FrequencyOption.GetTitle(habit.Frequency);
        TodayStatus = habit.IsCompletedOn(today) ? "Šodien izpildīts" : "Šodien vēl nav izpildīts";
    }
}
