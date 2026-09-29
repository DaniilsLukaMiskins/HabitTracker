using System;
using System.Collections.Generic;
using System.Text;

namespace HabitTrackerShared
{
    public class MarkHabitCompletedData // Dati ieraduma izpildes atzīmēšanai
    {
        public string HabitID { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
    }
}
