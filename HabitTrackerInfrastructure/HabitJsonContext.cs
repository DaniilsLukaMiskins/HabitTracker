using System.Text.Json.Serialization;

//koda validacijai un kļudu labošanai, un faila izveidošanai bija iazmantots ChatGPT-6 Astra

namespace HabitTrackerInfrastructure;

// JSON iestatījumi un automātiska koda ģenerēšana
// ieradumu saraksta pārveidošanai uz JSON un atpakaļ.
[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)] // piemērs "Frequency" : "Diena"]
[JsonSerializable(typeof(List<HabitFileData>))]
internal partial class HabitJsonContext : JsonSerializerContext
{
}
