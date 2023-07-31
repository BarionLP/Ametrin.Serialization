using System.Text.Json;

namespace Ametrin.Serialization;
public static class JsonExtensions {
    public static readonly JsonSerializerOptions DefaultOptions = new() { WriteIndented = true, IncludeFields = true };
    static JsonExtensions() {
        DefaultOptions.Converters.Add(new DirectoryInfoJsonConverter());
        DefaultOptions.Converters.Add(new FileInfoJsonConverter());
    }

    public static string ConvertToJson<T>(this T data, JsonSerializerOptions? options = null) => JsonSerializer.Serialize(data, options ?? DefaultOptions);
    public static JsonElement ConvertToJsonElement<T>(this T data, JsonSerializerOptions? options = null) => JsonSerializer.SerializeToElement(data, data!.GetType(), options ?? DefaultOptions);

    public static Task<string> ConvertToJsonAsync<T>(this T data, JsonSerializerOptions? options = null) => Task.Run(()=> data.ConvertToJson(options));
    public static Task<JsonElement> ConvertToJsonElementAsync<T>(this T data, JsonSerializerOptions? options = null) => Task.Run(()=> data.ConvertToJsonElement(options));
    
    
    public static void WriteToJsonFile<T>(this T data, string path, JsonSerializerOptions? options = null) {
        File.WriteAllText(path, data.ConvertToJson(options ?? DefaultOptions));
    }

    public static async Task WriteToJsonFileAsync<T>(this T data, string path, JsonSerializerOptions? options = null) {
        using var stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, data, options ?? DefaultOptions);
        await stream.DisposeAsync();
    }

    public static T? ReadFromJsonFile<T>(string path) {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json);
    }

    public static async Task<T?> ReadFromJsonFileAsync<T>(string path) {
        using FileStream stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<T>(stream);
    }

    public static void WriteToJsonFile<T>(this T data, FileInfo fileInfo, JsonSerializerOptions? options = null) => data.WriteToJsonFile(fileInfo.FullName, options);
    public static Task WriteToJsonFileAsync<T>(this T data, FileInfo fileInfo, JsonSerializerOptions? options = null) => data.WriteToJsonFileAsync(fileInfo.FullName, options);
    public static T? ReadFromJsonFile<T>(FileInfo fileInfo) => ReadFromJsonFile<T>(fileInfo.FullName);
    public static Task<T?> ReadFromJsonFileAsync<T>(FileInfo fileInfo) => ReadFromJsonFileAsync<T>(fileInfo.FullName);
}
