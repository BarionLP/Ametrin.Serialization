﻿using Ametrin.Utils;
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
    public static Task WriteToJsonFileAsync<T>(this T data, string path, JsonSerializerOptions? options = null) => Task.Run(() => data.WriteToJsonFile(path, options));

    public static Result<T> ReadFromJsonFile<T>(string path, JsonSerializerOptions? options = null) {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, options ?? DefaultOptions);
    }
    public static Task<Result<T>> ReadFromJsonFileAsync<T>(string path, JsonSerializerOptions? options = null) => Task.Run(() => ReadFromJsonFile<T>(path, options));

    public static void WriteToJsonFile<T>(this T data, FileInfo fileInfo, JsonSerializerOptions? options = null){
        using var stream = fileInfo.Create();
        JsonSerializer.Serialize(stream, data, options ?? DefaultOptions);
    }
    public static Task WriteToJsonFileAsync<T>(this T data, FileInfo fileInfo, JsonSerializerOptions? options = null) => Task.Run(() => data.WriteToJsonFile(fileInfo, options));

    public static Result<T> ReadFromJsonFile<T>(FileInfo fileInfo, JsonSerializerOptions? options = null){
        using var stream = fileInfo.OpenRead();
        return JsonSerializer.Deserialize<T>(stream, options ?? DefaultOptions);
    }
    public static Task<Result<T>> ReadFromJsonFileAsync<T>(FileInfo fileInfo, JsonSerializerOptions? options = null) => Task.Run(() => ReadFromJsonFile<T>(fileInfo, options));
}
