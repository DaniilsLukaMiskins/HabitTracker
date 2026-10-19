using System;
using System.Collections.Generic;
using System.Text;


//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai 

namespace HabitTrackerDomain
{
    public class Habit // Ieraduma klase
    {
        public string ID { get; } // Ieraduma ID
        public string UserID{  get; } //Lietotaja ID
        public string Name { get; private set; } // Ieraduma vards

        private string? _description;// Ieraduma apraksts
        public string? Description {  // Ieraduma arpraksta ierakstīšana
            get {  return _description; }
            set
            {
                _description = value;
                UpdatedTime = DateTime.Now;
                

            } 


        }
        public bool isArchived {  get; private set; } = false; // Vai ieradums ir arhivets

        public HabitFrequency Frequency { get; private set; } //Ieraduma atkārtošanas biežums

        public DateTime CreatedTime {  get; }
        public DateTime UpdatedTime { get; private set; }

        private List<HabitCompletion> _completions = new List<HabitCompletion>(); // Ieraduma izpildes ieraksti

        public IReadOnlyList<HabitCompletion> Completions
        {
            get { return _completions.AsReadOnly(); }
        }


        public Habit( string id, string userId, string name, string? description, HabitFrequency frequency = HabitFrequency.Diena, DateTime? createdTime = null) //konstruktors
        {
            if(string.IsNullOrWhiteSpace(id))
            {

                throw new ArgumentException("Ieraduma ID nedrīkst būt tukšs");
                
            }
            ID = id;
            if (string.IsNullOrWhiteSpace(userId))
            {

                throw new ArgumentException("Lietotāja ID nedrīkst būt tukšs");

            }
            UserID = userId;
            if (string.IsNullOrWhiteSpace(name))
            {

                throw new ArgumentException("Ieraduma vards nedrīkst būt tukšs");

            }
            name = name.Trim();
            if (name.Length>100)
            {

                throw new ArgumentException("Ieraduma vards nedrīkst būt lielāks par 100 simboliem");

            }
            Name = name;
            Description= description;
            CreatedTime = createdTime ?? DateTime.Now;
            UpdatedTime = CreatedTime;
            Frequency = frequency;
        }

        public void MarkCompleted(string completionId, DateOnly date) // Atzīmēt izpildītus ieradumus
        {
            if (isArchived)
            {
                throw new InvalidOperationException("Arhivētu ieradumu nevar atzīmēt kā izpildītu.");
            }

            foreach (HabitCompletion completion in _completions)
            {
                if (completion.Date == date)
                {
                    throw new InvalidOperationException("Šajā datumā ieradums jau ir atzīmēts kā izpildīts.");
                }
            }

            HabitCompletion newCompletion = new HabitCompletion(completionId, ID, date);

            _completions.Add(newCompletion);

            UpdatedTime = DateTime.Now;
        }

        public void SetArchived(bool archived)//Arhivēt ieradumu
        {
            isArchived = archived;
            UpdatedTime = DateTime.Now;
        }

        public void Update(string name, string? description, HabitFrequency frequency) // atjaunināt informāciju
        {
             if (string.IsNullOrWhiteSpace(name))
             {
                  throw new ArgumentException( "Ieraduma vards nedrīkst būt tukšs");
             }

             name = name.Trim();

             if (name.Length > 100)
             {
                  throw new ArgumentException( "Ieraduma vards nedrīkst būt lielāks par 100 simboliem");
             }

             Name = name;
             Description = description;
             Frequency = frequency;

             UpdatedTime = DateTime.Now;
        }


    
        public static Habit Restore(string id, string userId, string name, string? description,
            HabitFrequency frequency, DateTime createdTime, DateTime updatedTime,   // Atjauno ieradumu no saglabātajiem datiem, saglabājot tā ID,
            bool archived, IEnumerable<HabitCompletion> completions)                // izveides un labošanas laiku, arhīva statusu un izpildes vēsturi.
        {
            if (!Enum.IsDefined(frequency))
            {
                throw new ArgumentException("Saglabātais ieraduma biežums nav derīgs.");
            }
            if (completions == null)
            {
                throw new ArgumentNullException("completions", "Izpildes ierakstu saraksts nedrīkst būt null.");
            }

            Habit habit = new Habit(id, userId, name, description, frequency, createdTime);
            foreach (HabitCompletion completion in completions)
            {
                if (completion == null || completion.HabitID != id)
                {
                    throw new ArgumentException("Izpildes ieraksts nepieder šim ieradumam.");
                }

                // Izmanto esošās ID un atkārtota datuma pārbaudes.
                habit.MarkCompleted(completion.ID, completion.Date);
            }

            // Arhīvu un saglabāto laiku piešķir pēc vēstures atjaunošanas.
            habit.isArchived = archived;
            habit.UpdatedTime = updatedTime;
            return habit;
        }

        public bool IsCompletedOn(DateOnly date) // Pārbauda, vai ieradums ir izpildīts norādītajā datumā.
        {
            foreach (HabitCompletion completion in _completions)
            {
                if (completion.Date == date)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
