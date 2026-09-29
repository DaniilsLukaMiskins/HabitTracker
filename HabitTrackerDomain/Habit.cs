using System;
using System.Collections.Generic;
using System.Text;


//koda validacijai bija iazmantots ChatGPT-6 Astra
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


        public Habit( string id, string userId, string name, string? description, HabitFrequency frequency = HabitFrequency.Diena) //konstruktors
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
            CreatedTime = DateTime.Now;
            UpdatedTime = CreatedTime;
            Frequency = frequency;
        }


    }
}
