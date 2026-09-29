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

        private string? _description;
        public string? Description {  // Ieraduma arpraksts
            get {  return _description; }
            set
            {
                _description = value;
                UpdatedTime = DateTime.Now;
                

            } 


        }
        public bool isArchived {  get; private set; } = false; // Vai ieradums ir arhivets

        public DateTime CreatedTime {  get; }
        public DateTime UpdatedTime { get; private set; }


        public Habit( string id, string userId, string name, string? description) //konstruktors
        {
            if(string.IsNullOrWhiteSpace(id))
            {

                throw new ArgumentNullException("Ieraduma ID nedrīkst būt tukšs");
                
            }
            ID = id;
            if (string.IsNullOrWhiteSpace(userId))
            {

                throw new ArgumentNullException("Lietotāja ID nedrīkst būt tukšs");

            }
            UserID = userId;
            if (string.IsNullOrWhiteSpace(name))
            {

                throw new ArgumentNullException("Ieraduma vards nedrīkst būt tukšs");

            }
            name = name.Trim();
            if (name.Length>100)
            {

                throw new ArgumentNullException("Ieraduma vards nedrīkst būt lielāks par 100 simboliem");

            }
            Name = name;
            Description= description;
            CreatedTime = DateTime.Now;
            UpdatedTime = CreatedTime;
        }


    }
}
