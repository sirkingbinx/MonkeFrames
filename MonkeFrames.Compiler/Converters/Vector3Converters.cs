using Newtonsoft.Json;
using UnityEngine;
using System;

namespace MonkeFrames.Compiler.Converters;

internal class Vector3Converter : JsonConverter<Vector3>
{
    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WriteEndObject();
    }

    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var dict = serializer.Deserialize<System.Collections.Generic.Dictionary<string, float>>(reader);
        return new Vector3(dict["x"], dict["y"], dict["z"]);
    }
}
