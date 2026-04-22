using MonkeFrames.Editor.Attributes;
using MonkeFrames.Editor.Interfaces;

namespace MonkeFrames.Editor.Menus;

public class KeyframeMenu : IEditorMenu
{
    public string Name => "Keyframe";

    [EditorMenuItem("New")]
    public void NewKeyframe()
    {

    }

    [EditorMenuItem("Replace")]
    public void ReplaceKeyframe()
    {
        
    }

    [EditorMenuItem("Delete")]
    public void DeleteKeyframe()
    {
        
    }
}