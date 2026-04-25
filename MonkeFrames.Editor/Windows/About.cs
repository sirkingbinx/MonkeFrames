using UnityEngine;
using MonkeFrames.Compiler.Models;
using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Interfaces;

namespace MonkeFrames.Editor.Windows;

public class About : IEditorWindow
{
    public string Name => "About MonkeFrames";
    public Rect Rect => new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 130);

    public void OnDraw()
    {
        GUI.Label(new Rect(10, 30, Rect.width - 20, 20), $"MonkeFrames.Editor {Constants.VersionID}");
        GUI.Label(new Rect(10, 50, Rect.width - 20, 20), $"MonkeFrames.Compiler {Compiler.Constants.Version}");

        GUI.Label(new Rect(10, 80, Rect.width - 20, 20), $"(C) Copyright 2026 SirKingBinx (bingus)");
        GUI.Label(new Rect(10, 100, Rect.width - 20, 20), $"Made with <3 by Bingus & uhJames");
    }
}