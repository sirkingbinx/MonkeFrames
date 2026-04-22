using MonkeFrames.Editor.Attributes;
using MonkeFrames.Editor.Interfaces;

namespace MonkeFrames.Editor.Menus;

public class ProjectMenu : IEditorMenu
{
    public string Name => "Project";

    [EditorMenuItem("Project Settings..")]
    public void OpenProjectSettings()
    {

    }

    [EditorMenuItem("Load Project")]
    public void LoadProject()
    {
        
    }

    [EditorMenuItem("Save Project")]
    public void SaveProject() 
    {
        
    }

    [EditorMenuItem("Compile")]
    public void CompileProject()
    {
        
    }

    [EditorMenuItem("Play")]
    public void PlayProject()
    {
        
    }
}