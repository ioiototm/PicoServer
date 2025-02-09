using System.IO;
using Newtonsoft.Json;

public static class ConfigLoader
{
    public static AppConfig LoadConfig(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<AppConfig>(json);
    }
}
