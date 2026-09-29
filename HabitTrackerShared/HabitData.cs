using System;
using System.Collections.Generic;
using System.Text;


//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai 

namespace HabitTrackerShared
{
    public class HabitData // Ieraduma dati attēlošanai
    {
        public string ID { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool isArchived { get; set; }
        public string Frequency { get; set; } = string.Empty;
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

    }
}
