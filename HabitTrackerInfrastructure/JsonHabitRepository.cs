using System.Text.Json;
using HabitTrackerDomain;
using HabitTrackerTestData;

//koda validacijai un kļudu labošanai, un koda ģenerešanai bija iazmantots ChatGPT-6 Astra


namespace HabitTrackerInfrastructure;

public sealed class JsonHabitRepository : IHabitRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);

    public JsonHabitRepository(string filePath)
    {
        _filePath = Path.GetFullPath(filePath);
    }

    // Ielādē visus ieradumus.
    public async Task<List<Habit>> GetAllAsync()
    {
        await _fileLock.WaitAsync();

        try
        {
            return await ReadAsync();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    // Pievieno jaunu ieradumu vai saglabā esoša ieraduma izmaiņas.
    public async Task SaveAsync(Habit habit)
    {
        if (habit == null)
        {
            throw new ArgumentNullException(nameof(habit));
        }

        await _fileLock.WaitAsync();

        try
        {
            List<Habit> habits = await ReadAsync();
            int foundIndex = -1;

            for (int i = 0; i < habits.Count; i++)
            {
                if (habits[i].ID == habit.ID)
                {
                    if (habits[i].UserID != habit.UserID)
                    {
                        throw new HabitStorageException("Ieraduma ID jau pieder citam lietotājam.");
                    }

                    foundIndex = i;
                    break;
                }
            }

            if (foundIndex == -1)
            {
                habits.Add(habit);
            }
            else
            {
                habits[foundIndex] = habit;
            }

            await WriteAsync(habits);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    // Nolasa failu. Ja faila nav, izveido testa datus.
    private async Task<List<Habit>> ReadAsync()
    {
        string json;

        try
        {
            json = await File.ReadAllTextAsync(_filePath);
        }
        catch (FileNotFoundException)
        {
            return await CreateTestDataAsync();
        }
        catch (DirectoryNotFoundException)
        {
            return await CreateTestDataAsync();
        }

        List<HabitFileData>? records = JsonSerializer.Deserialize(json, HabitJsonContext.Default.ListHabitFileData);

        if (records == null)
        {
            throw new HabitStorageException("Failā nav derīga ieradumu saraksta.");
        }

        List<Habit> habits = new List<Habit>();

        foreach (HabitFileData record in records)
        {
            if (record == null)
            {
                throw new HabitStorageException("Failā ir tukšs ieraduma ieraksts.");
            }

            habits.Add(record.ToHabit());
        }

        return habits;
    }

    // Izveido un saglabā sākotnējos testa ieradumus.
    private async Task<List<Habit>> CreateTestDataAsync()
    {
        TestDataFactory factory = new TestDataFactory();
        List<Habit> habits = factory.CreateTestData();

        await WriteAsync(habits);

        return habits;
    }

    // Saglabā visu ieradumu sarakstu JSON failā.
    private async Task WriteAsync(List<Habit> habits)
    {
        List<HabitFileData> records = new List<HabitFileData>();

        foreach (Habit habit in habits)
        {
            records.Add(HabitFileData.FromHabit(habit));
        }

        string json = JsonSerializer.Serialize(records, HabitJsonContext.Default.ListHabitFileData);

        string? directory = Path.GetDirectoryName(_filePath);

        if (directory != null)
        {
            Directory.CreateDirectory(directory);
        }

        // Vispirms ieraksta īslaicīgu failu, pēc tam aizvieto saglabājumu.
        string temporaryPath = _filePath + ".tmp";

        await File.WriteAllTextAsync(temporaryPath, json);
        File.Move(temporaryPath, _filePath, overwrite: true);
    }
}