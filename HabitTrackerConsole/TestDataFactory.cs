using HabitTrackerDomain;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai 
// Kods izdarīts ar https://github.com/ElinaKalninaLU/UzdevumuParvaldnieks/blob/95fc84bf5b0742081aa4699c55657ce2da436d4c/UzdevumuParvaldnieksTestData/TestDataFactoryList.cs piemēra palīdzību



namespace HabitTrackerConsole
{
    public class TestDataFactory
    {
        // 32 nejaušas heksadecimālas rakstzīmes — 128 biti. Lai ieradumu ID nebūtu vienādi
        // https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator.gethexstring
        public string CreateID()
        {
            return RandomNumberGenerator.GetHexString(32);
        }

        public List<Habit> CreateTestData()
        {
            List<Habit> habits = new List<Habit>();

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            string firstUserID = "user-1"; //2 lietotāju ID
            string secondUserID = "user-2";

            // Pirmā lietotāja ieradumi
            Habit reading = new Habit(CreateID(), firstUserID, "Lasīt grāmatu", "Lasīt vismaz 20 minūtes", HabitFrequency.Diena); // Saraksts no ieradumiem

            reading.MarkCompleted(CreateID(), today.AddDays(-1)); // Kā arī bija izpildīts vakar
            reading.MarkCompleted(CreateID(), today);

            habits.Add(reading);

            Habit exercise = new Habit( CreateID(), firstUserID,  "Vingrot", "Veikt 30 minūšu treniņu", HabitFrequency.Nedela);

            exercise.MarkCompleted(CreateID(), today.AddDays(-2));

            habits.Add(exercise);

            // Otrā lietotāja ieradumi
            Habit water = new Habit( CreateID(), secondUserID, "Dzert ūdeni", "Regulāri dzert ūdeni dienas laikā", HabitFrequency.Diena);

            water.MarkCompleted(CreateID(), today);

            habits.Add(water);

            Habit diary = new Habit( CreateID(), secondUserID, "Rakstīt dienasgrāmatu", "Pierakstīt dienas notikumus", HabitFrequency.Diena);

            diary.MarkCompleted(CreateID(), today.AddDays(-3));
            diary.SetArchived(true);

            habits.Add(diary);

            return habits;
        }
    }
}
