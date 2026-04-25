using System;
using UnityEngine;
using Newtonsoft.Json;

namespace MonkeFrames.Editor.Classes.NewtonsoftConverters;

public class ColorConverter : JsonConverter<Color>
{
    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("r"); writer.WriteValue(value.r);
        writer.WritePropertyName("g"); writer.WriteValue(value.g);
        writer.WritePropertyName("b"); writer.WriteValue(value.b);
        writer.WritePropertyName("a"); writer.WriteValue(value.a);
        writer.WriteEndObject();
    }

    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = Newtonsoft.Json.Linq.JObject.Load(reader);
        float r = (float)jsonObject["r"];
        float g = (float)jsonObject["g"];
        float b = (float)jsonObject["b"];
        float a = (float)jsonObject["a"];
        return new Color(r, g, b, a);
    }
}