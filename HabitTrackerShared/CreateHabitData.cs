using System;
using System.Collections.Generic;
using System.Text;

namespace HabitTrackerShared
{
    public class CreateHabitData // Dati jauna ieraduma izveidei
    {
        public string UserID { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Frequency { get; set; } = "Diena";
    }
}
