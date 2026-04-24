using UnityEngine;
using MonkeFrames.Compiler.Models;
using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Interfaces;
using MonkeFrames.Editor.Utilities;

using Keyframe = MonkeFrames.Compiler.Models.Keyframe;
using System.Linq;

namespace MonkeFrames.Editor.Windows;

public class KeyframeEditor : IEditorWindow
{
    public string Name => "Keyframe Editor";
    public Rect Rect => new Rect(Screen.width - 620, 100, 600, 550);

    public Project Project = KeyframeManager.Instance.Project;
    public Vector2 KeyframeListScrollPos;

    public void OnDraw()
    {
        GUILayout.BeginArea(new Rect(10, 35, Rect.width - 20, 295));

        KeyframeListScrollPos = GUILayout.BeginScrollView(KeyframeListScrollPos, GUILayout.Width(Rect.width - 20), GUILayout.Height(300));

        for (int i = 0; i < Project.Keyframes.Count; i++)
        {
            Keyframe k = Project.Keyframes.ElementAt(i);

            string displayString = $"Keyframe {i + 1} / P: {UnityUtilities.Vector3ToString(k.Position)} / R: {UnityUtilities.Vector3ToString(k.Rotation)}";
            bool selectionStart = GUILayout.Toggle(UIManager.Instance.Selection == i, displayString);

            if (selectionStart && UIManager.Instance.Selection != i)
                UIManager.Instance.Selection = i;
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();

        int y = 375;

        if (KeyframeManager.Instance.IsCompiling)
        {
            GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
            centeredStyle.alignment = TextAnchor.MiddleCenter;

            GUI.Label(new Rect(10, 385, Rect.width, 20), "Compiling, please wait...", centeredStyle);
        } 
        else if (UIManager.Instance.Selection != -1)
        {
            Keyframe k = KeyframeManager.Instance.Project.Keyframes[UIManager.Instance.Selection];

            GUI.Label(new Rect(10, y, 200, 20), "Position: ");
            float px = CreateNumInputLabel(70, y, 'X', ref k.Position.x);
            float py = CreateNumInputLabel(245, y, 'Y', ref k.Position.y);
            float pz = CreateNumInputLabel(420, y, 'Z', ref k.Position.z);

            GUI.Label(new Rect(10, y + 30, 200, 20), "Rotation: ");
            float rx = CreateNumInputLabel(70, y + 30, 'X', ref k.Rotation.x);
            float ry = CreateNumInputLabel(245, y + 30, 'Y', ref k.Rotation.y);
            float rz = CreateNumInputLabel(420, y + 30, 'Z', ref k.Rotation.z);

            GUI.Label(new Rect(10, y + 60, 200, 20), "FOV: ");
            float fov = CreateNumInputLabel(50, y + 60, 'v', ref k.FieldOfView);

            k.Position.Set(px, py, pz);
            k.Rotation.Set(rx, ry, rz);
            k.FieldOfView = fov;

            // Transition
            GUI.Label(new Rect(10, y + 95, 200, 20), "Transition Style:");

            if (GUI.Toggle(new Rect(120, y + 95, 75, 20), k.Transition.Effect == TransitionEffect.Linear, "Linear"))
                k.Transition.Effect = TransitionEffect.Linear;

            if (GUI.Toggle(new Rect(195, y + 95, 75, 20), k.Transition.Effect == TransitionEffect.Cut, "Cut / None"))
                k.Transition.Effect = TransitionEffect.Cut;

            GUI.Label(new Rect(10, y + 120, 200, 20), "Duration:");
            k.Transition.Duration = GUI.HorizontalSlider(new Rect(75, y + 125, 395, 20), k.Transition.Duration, 0.0f, 30.0f);
            GUI.Label(new Rect(475, y + 120, 25, 20), $"{k.Transition.Duration:F2}s");

            GUI.Label(new Rect(10, Rect.height - 25, Rect.width, 20), $"GUID: {k.GUID} - Duration: {k.Transition.Duration:F2}s");

            KeyframeManager.Instance.Project.Keyframes[UIManager.Instance.Selection] = k;
        } else
        {
            GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
            centeredStyle.alignment = TextAnchor.MiddleCenter;

            GUI.Label(new Rect(0, 10, Rect.width, 40), "Select a keyframe to view it's properties.", centeredStyle);
        }

        GUI.Label(new Rect(10, Rect.height - 20, Rect.width, 20), $"Keyframes: {KeyframeManager.Instance.Project.Keyframes.Count}");
    }

    private float CreateNumInputLabel(float x, float y, char axis, ref float field)
    {
        GUI.Label(new Rect(x, y, 20, 20), $"{axis}: ");
        string newNum = GUI.TextField(new Rect(x + 20, y, 150, 20), field.ToString());

        if (float.TryParse(newNum, out float newValue))
            return newValue;
           
        return field;
    }
}