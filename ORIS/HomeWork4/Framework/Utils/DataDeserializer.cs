using System.Text.Json;
using http_server.Framework.Configuration;

namespace http_server.Framework.Utils;

public class DataDeserializer
{
    private static Setting _prefix = JsonSerialize();

    public static Setting Prefix => _prefix;

    private static Setting JsonSerialize()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "server.json");
        try
        {

            return JsonSerializer.Deserialize<Setting>(File.ReadAllText(filePath))
                      ?? new Setting();
        }
        catch (FileNotFoundException)
        {
            string jsonText = JsonSerializer.Serialize(new Setting());
            File.WriteAllText(filePath, jsonText);
            return new Setting();
        }
        catch (JsonException)
        {
            Console.WriteLine($"Файл имеет ошибки. Перепроверьте файл, и попробуй снова");
            return new Setting();
        }
    }

    private DataDeserializer() { }
}