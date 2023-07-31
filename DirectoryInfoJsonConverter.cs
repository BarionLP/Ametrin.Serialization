using System.Text.Json.Serialization;
using System.Text.Json;

namespace Ametrin.Serialization;
public sealed class DirectoryInfoJsonConverter : JsonConverter<DirectoryInfo> {
    public override DirectoryInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if(reader.TokenType == JsonTokenType.Null) return null;
        var value = reader.GetString();
        return string.IsNullOrWhiteSpace(value) ? null : new(value);
    }

    public override void Write(Utf8JsonWriter writer, DirectoryInfo value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.FullName);
    }
}
