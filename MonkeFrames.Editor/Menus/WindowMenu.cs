using MonkeFrames.Editor.Attributes;
using MonkeFrames.Editor.Interfaces;

namespace MonkeFrames.Editor.Menus;

public class WindowMenu : IEditorMenu
{
    public string Name => "Window";

    [EditorMenuItem("Keyframe Manager")]
    public void KeyframeManager()
    {

    }

    [EditorMenuItem("Room Manager")]
    public void RoomManager()
    {
        
    }

    [EditorMenuItem("Map Loader")]
    public void MapLoader()
    {
        
    }

    #if DEBUG

    [EditorMenuItem("Diagnostics")]
    public void Diagnostics()
    {
        
    }

    #endif
}