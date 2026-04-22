using MonkeFrames.Editor.Attributes;
using MonkeFrames.Editor.Interfaces;

namespace MonkeFrames.Editor.Menus;

public class GoMenu : IEditorMenu
{
    public string Name => "Go";

    [EditorMenuItem("Gorilla")]
    public void ToGorilla()
    {

    }

    [EditorMenuItem("Selected Keyframe")]
    public void ToSelectedKeyframe()
    {
        
    }
}