using System.IO;
using MonkeFrames.Editor.Classes.NewtonsoftConverters;
using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Utilities;
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

        if (!File.Exists(SystemUtilities.Combine(Constants.DataFolder, "config.json")))
        {
            current = new Settings();
            return;
        }

        string prefs = File.ReadAllText(SystemUtilities.Combine(Constants.DataFolder, "config.json"));
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
        File.WriteAllText(SystemUtilities.Combine(Constants.DataFolder, "config.json"), json);

        KeyframeManager.Instance.RefreshOrbs();
    }
}