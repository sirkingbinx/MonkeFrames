using MonkeFrames.Editor.Attributes;
using MonkeFrames.Editor.Interfaces;

namespace MonkeFrames.Editor.Menus;

public class KeyframeMenu : IEditorMenu
{
    public string Name => "Keyframe";
    public int Index => 4;

    [EditorMenuItem("New")]
    public void NewKeyframe()
    {
        KeyframeManager.Instance.CreateKeyframe();
    }

    [EditorMenuItem("New towards Monke")]
    public void NewKeyframe()
    {
        KeyframeManager.Instance.CreateKeyframe(lookAtPlayer: true);
    }

    [EditorMenuItem("Replace Selection")]
    public void ReplaceKeyframe()
    {
        if (UIManager.Instance.Selection == -1)
        {
            UIManager.Instance.Status = "Please select a keyframe in the keyframe editor before performing this action.";
            return;
        }
        
        KeyframeManager.Instance.CreateKeyframe(replaceKeyframeIdx: UIManager.Instance.Selection);
    }

    [EditorMenuItem("Delete Selection")]
    public void DeleteKeyframe()
    {
        if (UIManager.Instance.Selection == -1)
        {
            UIManager.Instance.Status = "Please select a keyframe in the keyframe editor before performing this action.";
            return;
        }

        KeyframeManager.Instance.DeleteKeyframe(UIManager.Instance.Selection);
    }
}