using System;
using System.Collections.Generic;
using System.Text;

namespace HabitTrackerShared
{
    public class UpdateHabitData // Dati ieraduma labošanai
    {
        public string ID { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Frequency { get; set; } = string.Empty;
        public bool isArchived { get; set; }
    }
}
