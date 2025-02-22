namespace DOOMer.Core;

using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class PointJsonSerializer : JsonConverter<Point> {
    public override Point Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        string? value = reader.GetString();
        if (value.IsNullEmptyWhitespace()) {
            return default;
        }

        string[] splits = value.Split(',');
        if (splits.Length > 1 && int.TryParse(splits[0], out int x) && int.TryParse(splits[1], out int y)) {
            return new Point(x, y);
        }
        return default;
    }

    public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options) {
        writer.WriteStringValue($"{value.X},{value.Y}");
    }
}
