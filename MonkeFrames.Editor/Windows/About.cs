using System.Collections.Generic;
using System.Diagnostics;
using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Interfaces;
using MonkeFrames.Editor.Utilities;
using UnityEngine;

namespace MonkeFrames.Editor.Windows;

public class About : IEditorWindow
{
    public string Name => "About MonkeFrames";
    public Rect Rect => new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 500, 400);

    private Vector2 ScrollPosition;

    public void OnDraw()
    {
        GUI.Label(new Rect(10, 30, Rect.width - 20, 20), $"MonkeFrames.Editor {Constants.VersionID}");
        GUI.Label(new Rect(10, 50, Rect.width - 20, 20), $"MonkeFrames.Compiler {Compiler.Constants.Version}");

        GUI.Label(new Rect(10, 80, Rect.width - 20, 20), $"(C) Copyright 2026 SirKingBinx (bingus)");

        GUILayout.BeginArea(new Rect(10, 110, Rect.width - 20, 360));

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, GUILayout.Width(Rect.width - 20), GUILayout.Height(Rect.height - 145));

        foreach (var credit in Constants.Contributors)
            GUILayout.Label($"{credit.Key}     {credit.Value}", GUILayout.Height(20));

        GUILayout.EndScrollView();
        GUILayout.EndArea();

        if (GUI.Button(new Rect(Rect.width - 110, Rect.height - 30, 100, 20), "Ok"))
            UIManager.Instance.CloseWindow("About MonkeFrames");

        if (GUI.Button(new Rect(Rect.width - 220, Rect.height - 30, 100, 20), "Source Code"))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/sirkingbinx/MonkeFrames",
                UseShellExecute = true
            });
        }
    }
}