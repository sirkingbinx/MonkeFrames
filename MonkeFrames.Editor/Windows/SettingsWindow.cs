using MonkeFrames.Editor.Classes;
using MonkeFrames.Editor.Interfaces;
using MonkeFrames.Editor.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeFrames.Editor.Windows;

public class SettingsWindow : IEditorWindow
{
    public string Name => "Settings";
    public Rect Rect => new Rect(115, 30, 300, 400);

    bool colorSelectionDropped = false;

    public List<Color> colors = [
        Color.red,
        Color.orange,
        Color.gold,
        Color.green,
        Color.blue,
        Color.purple
    ];

    public void OnDraw()
    {
        // Accent Color
        GUI.Label(new Rect(10, 30, 80, 20),
            new GUIContent(
                "Accent Color: ",
                "The color used for keyframe markers, colored UI elements, and any other colored items.")
        );

        Settings.AccentColor, colorSelectionDropped = GUIUtilities.Dropdown(
            new Rect(100, 30, 100, 20),
            Settings.AccentColor,
            colorSelectionDropped,
            colors,
            UnityUtilities.ColorToString
        );

        // Autosave Projects
        Settings.Autosave = GUI.Toggle(
            new Rect(10, 50, Rect.width - 20, 20),
            new GUIContent(
                "Autosave Projects",
                "Automatically save any named project when it's keyframes are changed.")
        );

        GUI.Label(
            new Rect(10, Rect.height - 25, Rect.width - 20, 20),
            string.IsNullOrEmpty(GUI.tooltip)
                ? "Tooltips will appear here"
                : GUI.tooltip
        );

		GUI.tooltip = "";
    }
}