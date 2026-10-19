namespace HabitTrackerDomain;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
public class HabitStorageException : Exception // Kļūda, kas rodas, ja ieradumu datus nevar ielādēt vai saglabāt.
{
    public HabitStorageException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
