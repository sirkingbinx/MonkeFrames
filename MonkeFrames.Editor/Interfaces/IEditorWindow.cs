using UnityEngine;

namespace MonkeFrames.Editor.Interfaces;

public interface IEditorWindow
{
    public string Name => "Window";
    public Rect Rect => new Rect(10, 10, 400, 100);

    public void OnDraw() { }
    public void OnOpen() { }
    public void OnClose() { }
}