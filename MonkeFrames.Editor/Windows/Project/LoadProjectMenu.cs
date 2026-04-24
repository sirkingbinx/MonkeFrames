using MonkeFrames.Editor.Interfaces;
using UnityEngine;

namespace MonkeFrames.Editor.Windows.Project;

public class LoadProject : IEditorWindow
{
    public string Name => "Load Project";
    public Rect Rect => new Rect((int)(Screen.width / 2) - 200, (int)(Screen.width / 2) - 150, 400, 300);

    public Vector2 ScrollPosition;

    public void OnDraw()
    {
        GUILayout.Begin
        if (SaveUtilities.LoadableProjects.Count == 0) {
            GUI.Label(new Rect(10, 30, Rect.width - 20, 20), "No projects saved");
        } else {
            GUILayout.BeginArea(new Rect(10, 20, Rect.width - 10, Rect.height - 55));
            
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, GUILayout.Width(Rect.width - 10), GUILayout.Height(Rect.height - 55));
            
            foreach (KeyValuePair<string, Project> set in SaveUtilities.LoadableProjects)
            {
                if (GUILayout.Button(set.Key))
                {
                    KeyframeManager.Instance.LoadProject(set.Value);
                    UIManager.Instance.CloseWindow("Load Project");
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();

            if (GUI.Button(new Rect(10, Rect.height - 30, 100, 25), "Projects Folder")) {
                Process.Start(new ProcessStartInfo
                {
                    FileName = SaveUtilities.ProjectDirectory,
                    UseShellExecute = true
                });
            }
        }
    }
}