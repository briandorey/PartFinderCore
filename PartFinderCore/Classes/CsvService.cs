using System.Text;

namespace PartFinderCore.Classes;

public interface ICsvService
{
    string GenerateCsv<T>(IEnumerable<T> items);
}

public class CsvService : ICsvService
{
    public string GenerateCsv<T>(IEnumerable<T> items)
    {
        var csv = new StringBuilder();
        var properties = typeof(T).GetProperties();

        // Add header row
        csv.AppendLine(string.Join(",", properties.Select(p => $"\"{p.Name}\"")));

        // Add data rows
        foreach (var item in items)
        {
            var values = properties.Select(p => $"\"{p.GetValue(item)?.ToString()?.Replace("\"", "\"\"")}\"");
            csv.AppendLine(string.Join(",", values));
        }

        return csv.ToString();
    }
}