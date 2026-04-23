using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeFrames.Editor.Windows;

public class MapLoader : IEditorWindow
{
    public string Name => "Map Loader";
    public Rect Rect => new Rect(400, 300, 400, 300);

    public Dictionary<string, GorillaSetZoneTrigger> Maps;
    public Vector2 ScrollPos;

    public void OnDraw()
    {
        GUILayout.BeginArea(new Rect(10, 30, Rect.width - 20, Rect.height - 65));

        ScrollPos = GUILayout.BeginScrollView(ScrollPos,
            GUILayout.Width(Rect.width - 20),
            GUILayout.Height(Rect.height - 65));
        
        foreach (var map in Maps)
        {
            if (GUILayout.Button(map.Key))
                map.Value.OnBoxTriggered();
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();

        GUI.Label(new Rect(10, Rect.height - 25, Rect.width - 20, 20), "Note: May fall out of the world", UIManager.Instance.CenterText);
    }

    public void OnOpen()
    {
        Maps = new();
        var triggers = Object.FindObjectsByType<GorillaSetZoneTrigger>(FindObjectsSortMode.None);

        foreach (GorillaSetZoneTrigger trigger in triggers)
        {
            string name = trigger.gameObject.name;
            int mapNameStart = name.IndexOf("To");

            if (mapNameStart == -1)
                continue;
            
            Maps.TryAdd(name[mapNameStart ..], trigger);
        }
    }
}