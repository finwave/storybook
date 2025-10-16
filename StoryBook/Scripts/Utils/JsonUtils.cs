namespace StoryBook.Scripts;

using System.Text.Json;

public class JsonUtils
{
    public static T[]? FromJson<T>(string json)
    {
        return JsonSerializer.Deserialize<T[]>(json);
    }

    public static string ToJson<T>(T[] array)
    {
        return JsonSerializer.Serialize(array);
    }

    public static string ToJson<T>(List<T> list)
    {
        return JsonSerializer.Serialize(list);
    }
}