using System.IO;
using MonkeFrames.Editor.Classes.NewtonsoftConverters;
using MonkeFrames.Editor.Components;
using Newtonsoft.Json;
using UnityEngine;

namespace MonkeFrames.Editor.Classes;

public class Settings
{
    public static Settings current;

    public Color AccentColor = Color.blue;
    public bool Autosave = true;

    public static void Load()
    {
        var settings = new JsonSerializerSettings
        {
            Converters = { new ColorConverter() }
        };

        if (!File.Exists(Path.Combine(Constants.DataFolder, "settings.json")))
        {
            current = new Settings();
            return;
        }

        string prefs = File.ReadAllText(Path.Combine(Constants.DataFolder, "settings.json"));
        current = JsonConvert.DeserializeObject<Settings>(prefs, settings);
    }

    public static void Save()
    {
        var settings = new JsonSerializerSettings
        {
            Converters = { new ColorConverter() },
            Formatting = Formatting.Indented,
        };

        string json = JsonConvert.SerializeObject(current, settings);

        Directory.CreateDirectory(Constants.DataFolder);
        File.WriteAllText(Path.Combine(Constants.DataFolder, "settings.json"), json);

        KeyframeManager.Instance.RefreshOrbs();
    }
}