using MonkeFrames.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using Keyframe = MonkeFrames.Models.Keyframe;

namespace MonkeFrames.Components;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public string CurrentTask;

    public bool ShowingUI = true;
    public bool ShowingEditorUI = true;
    public bool ShowingJoinerUI = true;

    public Vector2 ScreenDimensions;
    public Vector2 WindowSize = new(600, 1150);

    private Vector2 keyframesScrollPosition;

    public int SelectedKeyframeIndex = -1;

    public void Update()
    {
        if (Keyboard.current.f1Key.wasPressedThisFrame)
            ShowingUI = false;

        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            ShowingUI = true;
            ShowingEditorUI = true;
        }

        if (Keyboard.current.f3Key.wasPressedThisFrame)
        {
            ShowingUI = true;
            ShowingJoinerUI = true;
        }

        if (Keyboard.current.vKey.wasPressedThisFrame)
            KeyframeManager.Instance.CreateKeyframe();

        if (Keyboard.current.xKey.wasPressedThisFrame && SelectedKeyframeIndex != -1)
            KeyframeManager.Instance.CreateKeyframe(SelectedKeyframeIndex);
    }

    public void EditorGUI()
    {
        float x = ScreenDimensions.x - WindowSize.x - 20;
        float y = 20f;

        GUI.Box(new Rect(x, y, WindowSize.x, WindowSize.y), "");
        GUI.Label(
            new Rect(x + 10, y, WindowSize.x, 20),
            "MonkeFrames Editor"
        );

        // keyframes list
        keyframesScrollPosition = GUILayout.BeginScrollView(keyframesScrollPosition, GUILayout.Width(WindowSize.x - 20), GUILayout.Height(400));

        foreach (Keyframe k in KeyframeManager.Instance.Keyframes)
        {
            string displayString = $"{k.KeyframeID} p:{UnityUtilities.Vector3ToString(k.Position)}, r:{UnityUtilities.Vector3ToString(k.Rotation)}";
            bool selectionStart = GUILayout.Toggle(k.KeyframeIndex == SelectedKeyframeIndex, displayString);

            if (selectionStart)
                SelectedKeyframeIndex = k.KeyframeIndex;
        }

        GUILayout.EndScrollView();

        // modify y to add all the stuff from the keyframe list and title
        y = 450f;

        GUI.Label(new Rect(x, y, 200, 20), "Stuff goes here");
    }

    public void OnGUI()
    {
        if (ScreenDimensions == null)
            ScreenDimensions = new Vector2(Screen.width, Screen.height);

        EditorGUI();
    }
}