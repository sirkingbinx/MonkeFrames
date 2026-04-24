using MonkeFrames.Editor.Attributes;
using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Interfaces;

namespace MonkeFrames.Editor.Menus;

public class MonkeFramesMenu : IEditorMenu
{
    public string Name => "MonkeFrames";
    public int Index => 0;

    [EditorMenuItem("Disable/Enable (F1)")]
    public void About()
    {
        CameraManager.Instance.SetModEnabled(false);
    }

    [EditorMenuItem("Settings")]
    public void About()
    {
        UIManager.Instance.ToggleWindow("Settings");
    }

    [EditorMenuItem("About MonkeFrames")]
    public void About()
    {
        UIManager.Instance.ToggleWindow("About MonkeFrames");
    }
}