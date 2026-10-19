using HabitTrackerDomain;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai
namespace HabitTrackerMaui.ViewModels;

public sealed class FrequencyOption
{
    public HabitFrequency Value { get; }
    public string Title { get; }

    public FrequencyOption(HabitFrequency value)
    {
        Value = value;
        Title = GetTitle(value);
    }

    // Atkārtošanās biežuma nosaukums attēlošanai.
    public static string GetTitle(HabitFrequency frequency)
    {
        switch (frequency)
        {
            case HabitFrequency.Diena: return "Katru dienu";
            case HabitFrequency.Nedela: return "Katru nedēļu";
            case HabitFrequency.Menesis: return "Katru mēnesi";
            case HabitFrequency.Gads: return "Katru gadu";
            case HabitFrequency.DarbaDienas: return "Darba dienās";
            case HabitFrequency.Brivdienas: return "Brīvdienās";
            default: return "Nezināms biežums";
        }
    }
}
