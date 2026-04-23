using MonkeFrames.Editor.Interfaces;
using UnityEngine;

namespace MonkeFrames.Editor.Windows;

public class FramesDebug : IEditorWindow
{
    public string Name => "Diagnostics";
    public Rect Rect => new Rect(115, 30, 400, 400);

    public void OnDraw()
    {
        GUI.Label(new Rect(0, 20, Rect.width, 20), "WIP");
    }
}