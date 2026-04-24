using MonkeFrames.Editor.Attributes;
using MonkeFrames.Editor.Interfaces;

namespace MonkeFrames.Editor.Menus;

public class ProjectMenu : IEditorMenu
{
    public string Name => "Project";
    public int Index => 3;

    [EditorMenuItem("Project Settings")]
    public void OpenProjectSettings()
    {
        // bro made a fake error message :skull:
        // Think you're funny?
        UIManager.Instance.Status = "NotImplementedException";
    }

    [EditorMenuItem("Load Project")]
    public void LoadProject()
    {
        UIManager.Instance.OpenWindow("Load Project");
    }

    [EditorMenuItem("Save Project")]
    public void SaveProject() 
    {
        SaveUtilities.Save();
    }

    [EditorMenuItem("Compile")]
    public void CompileProject()
    {
        KeyframeManager.Instance.StartBuild();
    }

    [EditorMenuItem("Play")]
    public void PlayProject()
    {
        KeyframeManager.Instance.StartBuildAndRun();
    }
}