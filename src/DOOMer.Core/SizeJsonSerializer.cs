namespace DOOMer.Core;

using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class SizeJsonSerializer : JsonConverter<Size> {
    public override Size Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        string? value = reader.GetString();
        if (value.IsNullEmptyWhitespace()) {
            return default;
        }

        string[] splits = value.Split(',');
        if (splits.Length > 1 && int.TryParse(splits[0], out int w) && int.TryParse(splits[1], out int h)) {
            return new Size(w, h);
        }
        return default;
    }

    public override void Write(Utf8JsonWriter writer, Size value, JsonSerializerOptions options) {
        writer.WriteStringValue($"{value.Width},{value.Height}");
    }
}
