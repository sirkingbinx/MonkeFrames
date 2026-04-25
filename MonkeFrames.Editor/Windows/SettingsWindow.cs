using MonkeFrames.Editor.Classes;
using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Interfaces;
using MonkeFrames.Editor.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeFrames.Editor.Windows;

public class SettingsWindow : IEditorWindow
{
    public string Name => "Settings";
    public Rect Rect => new Rect(115, 30, 500, 200);

    bool colorSelectionDropped = false;

    public List<Color> colors = [
        Color.red,
        Color.orange,
        Color.yellow,
        Color.green,
        Color.blue,
        Color.purple
    ];

    public void OnDraw()
    {
        // Autosave Projects
        Settings.current.Autosave = GUI.Toggle(
            new Rect(10, 55, 150, 20),
            Settings.current.Autosave,
            new GUIContent(
                "Autosave Projects",
                "Automatically save any named project when it's keyframes are changed.")
        );

        // Accent color
        GUI.Label(new Rect(10, 30, 100, 20),
            new GUIContent(
                "Accent Color:",
                "Your favorite color (used for colored things.)")
        );

        var colorPreference = GUIUtilities.Dropdown(
            new Rect(120, 30, Rect.width - 130, 20),
            Settings.current.AccentColor,
            colorSelectionDropped,
            colors,
            UnityUtilities.ColorToString
        );

        Settings.current.AccentColor = colorPreference.Item1;
        colorSelectionDropped = colorPreference.Item2;

        GUI.Label(
            new Rect(10, Rect.height - 25, Rect.width - 20, 20),
            string.IsNullOrEmpty(GUI.tooltip)
                ? "Changes saved when closing settings. Hints will appear here."
                : GUI.tooltip
        );

		GUI.tooltip = "";
    }

    public void OnClose()
    {
        KeyframeManager.Instance.RefreshOrbs();
        Settings.Save();
    }
}