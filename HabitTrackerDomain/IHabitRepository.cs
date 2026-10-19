namespace HabitTrackerDomain;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra

// Nosaka metodes ieradumu ielādei un saglabāšanai.
// Metožu darbība ir īstenota klasē JsonHabitRepository.
public interface IHabitRepository
{
    // Asinhroni ielādē visus ieradumus un atgriež to sarakstu.
    Task<List<Habit>> GetAllAsync();

    // Asinhroni saglabā vienu ieradumu,
    // saglabājot arī pārējos ieradumus un citu lietotāju datus.
    Task SaveAsync(Habit habit);
}
