using MonkeFrames.Editor.Attributes;
using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Interfaces;

namespace MonkeFrames.Editor.Menus;

public class WindowMenu : IEditorMenu
{
    public string Name => "Window";
    public int Index => 1;

    [EditorMenuItem("Keyframe Editor")]
    public void KeyframeManager()
    {
        UIManager.Instance.ToggleWindow("Keyframe Editor");
    }

    [EditorMenuItem("Room Manager")]
    public void RoomManager()
    {
        UIManager.Instance.ToggleWindow("Room Manager");
    }

    [EditorMenuItem("Map Loader")]
    public void MapLoader()
    {
        UIManager.Instance.ToggleWindow("Map Loader");
    }
}