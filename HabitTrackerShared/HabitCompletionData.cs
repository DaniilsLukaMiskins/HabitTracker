using System;
using System.Collections.Generic;
using System.Text;

namespace HabitTrackerShared
{
    public class HabitCompletionData // Ieraduma izpildes dati attēlošanai
    {
        public string ID { get; set; } = string.Empty;
        public string HabitID { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
    }
}
