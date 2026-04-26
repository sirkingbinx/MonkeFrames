using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Interfaces;
using UnityEngine;

namespace MonkeFrames.Editor.Windows.Project;

public class ProjectSettings : IEditorWindow
{
    public string Name => "Project Settings";
    public Rect Rect => new Rect(115, 30, 500, 200);

    public Compiler.Models.Project Project => KeyframeManager.Instance.Project;

    public void OnDraw()
    {
        GUI.Label(new Rect(10, 35, 40, 20), new GUIContent("FPS", "The amount of frames per second in your project"));
        Project.FPS = (int)GUI.HorizontalSlider(new Rect(55, 40, Rect.width - 120, 20), Project.FPS, 30, 120);
        GUI.Label(new Rect(Rect.width - 45, 35, 45, 20), Project.FPS.ToString());

        GUI.Label(new Rect(10, 55, 55, 20), new GUIContent("Name", "The display name of your project"));
        Project.Name = GUI.TextField(new Rect(70, 55, Rect.width - 80, 20), Project.Name);

        GUI.Label(
            new Rect(10, Rect.height - 25, Rect.width - 20, 20),
            string.IsNullOrEmpty(GUI.tooltip)
                ? "Changes saved when closing settings. Hints will appear here."
                : GUI.tooltip
        );

		GUI.tooltip = "";
    }
}