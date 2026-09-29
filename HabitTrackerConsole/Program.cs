using System;
using System.Collections.Generic;
using System.Text;
using HabitTrackerConsole;
using HabitTrackerDomain;

//koda validacijai un kļudu labošanai bija iazmantots ChatGPT-6 Astra
//https://learn.microsoft.com/en-us saite bija izmantota metožu atrašanai 
// Kods izdarīts ar https://github.com/ElinaKalninaLU/UzdevumuParvaldnieks/blob/95fc84bf5b0742081aa4699c55657ce2da436d4c/UzdevumuParvaldnieksKonsole/Program.cs piemēra palīdzību


Console.OutputEncoding = Encoding.UTF8; // Lai stradātu garumzīmes

// Testa datu izveide
TestDataFactory factory = new TestDataFactory();
List<Habit> habits = factory.CreateTestData();

DateOnly today = DateOnly.FromDateTime(DateTime.Now);

// Kopējais ieradumu skaits
Console.WriteLine($"Kopējais ieradumu skaits: {habits.Count}"); // Jābūt atbilde 4

// Lietotāju identifikatori bez atkārtojumiem
List<string> userIDs = new List<string>();

foreach (Habit habit in habits)
{
    if (!userIDs.Contains(habit.UserID))
    {
        userIDs.Add(habit.UserID);
    }
}

// Katra lietotāja ieradumi un to statuss
foreach (string userID in userIDs)
{
    Console.WriteLine();
    Console.WriteLine($"Lietotājs: {userID}");

    foreach (Habit habit in habits)
    {
        if (habit.UserID == userID)
        {
            string status = habit.isArchived ? "arhivēts" : "aktīvs";

            Console.WriteLine($"\t - {habit.Name} | {status}");
        }
    }
}

// Šodien izpildītie ieradumi
Console.WriteLine();
Console.WriteLine("Šodien izpildītie ieradumi:");

foreach (Habit habit in habits)
{
    foreach (HabitCompletion completion in habit.Completions)
    {
        if (completion.Date == today)
        {
            Console.WriteLine(
                $"\t - {habit.Name} | Lietotājs: {habit.UserID}");
        }
    }
}

// Domēna metodes izsaukums: nosaukuma maiņa
Console.WriteLine();
Console.WriteLine("Nosaukuma maiņas pārbaude:");

Habit firstHabit = habits[0];

Console.WriteLine($"Pirms: {firstHabit.Name}");

firstHabit.Update(
    "Lasīt katru dienu",
    firstHabit.Description,
    firstHabit.Frequency);

Console.WriteLine($"Pēc: {firstHabit.Name}"); // jābut "Lasīt katru dienu"

// Arhivēta ieraduma izpildes aizlieguma pārbaude
Console.WriteLine();
Console.WriteLine("Arhivēta ieraduma pārbaude:");

foreach (Habit habit in habits)
{
    if (habit.isArchived) //jābūt "Arhivētu ieradumu nevar atzīmēt kā izpildītu."
    {
        try
        {
            habit.MarkCompleted(factory.CreateID(), today);
        }
        catch (InvalidOperationException exception)
        {
            Console.WriteLine(exception.Message);
        }

        break;
    }
}