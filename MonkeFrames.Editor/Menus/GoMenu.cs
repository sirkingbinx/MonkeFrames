using MonkeFrames.Editor.Attributes;
using MonkeFrames.Editor.Interfaces;

namespace MonkeFrames.Editor.Menus;

public class GoMenu : IEditorMenu
{
    public string Name => "Go";
    public int Index => 2;

    [EditorMenuItem("Gorilla")]
    public void ToGorilla()
    {

    }

    [EditorMenuItem("Selected Keyframe")]
    public void ToSelectedKeyframe()
    {
        
    }
}