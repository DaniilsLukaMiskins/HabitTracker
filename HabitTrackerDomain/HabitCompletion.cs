using System;
using System.Collections.Generic;
using System.Text;


//koda validacijai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai 

namespace HabitTrackerDomain
{
    public class HabitCompletion
    {
        public string ID { get; } // Izpildes ieraksta ID
        public string HabitID { get; } // Izpildītā ieraduma ID
        public DateOnly Date { get; } // Izpildes datums

        public HabitCompletion(string id, string habitId, DateOnly date)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Izpildes ieraksta ID nedrīkst būt tukšs");
            }

            if (string.IsNullOrWhiteSpace(habitId))
            {
                throw new ArgumentException("Ieraduma ID nedrīkst būt tukšs");
            }

            ID = id;
            HabitID = habitId;
            Date = date;
        }
    }
}
