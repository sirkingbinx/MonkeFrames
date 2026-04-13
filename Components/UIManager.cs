using System;
using System.Globalization;
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
    public Vector2 WindowSize = new(600, 800);

    private Vector2 keyframesScrollPosition;

    public int SelectedKeyframeIndex = -1;

    public Vector2 titlebarIcon;

    public void Start()
    {
        Instance = this;

        Debug.Log("[MonkeFrames::UIManager] Loading titlebar icon");
        
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("icon");
        using MemoryStream ms = new MemoryStream();
        
        stream.CopyTo(ms);
        byte[] iconData = ms.ToArray();

        titlebarIcon = UnityUtilities.CreateTexture(iconData);

        Debug.Log("[MonkeFrames::UIManager] UI manager is running");
    }

    public void LateUpdate()
    {
        if (Keyboard.current.vKey.wasPressedThisFrame)
            KeyframeManager.Instance.CreateKeyframe();

        if (Keyboard.current.xKey.wasPressedThisFrame && SelectedKeyframeIndex != -1)
            KeyframeManager.Instance.CreateKeyframe(SelectedKeyframeIndex);

        if (SelectedKeyframeIndex != -1 && Keyboard.current.fKey.wasPressedThisFrame)
        {
            Keyframe k = KeyframeManager.Instance.Keyframes[SelectedKeyframeIndex];
            CameraManager.Instance.Position = k.Position;
            CameraManager.Instance.Rotation = k.QuatRotation;
        }
    }

    public void OnGUI()
    {
        // Input
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

        // Thingy
        ScreenDimensions = new Vector2(Screen.width, Screen.height);

        float x = ScreenDimensions.x - WindowSize.x - 20;
        float y = 20f;
    
        GUI.Box(new Rect(x, y, WindowSize.x, WindowSize.y), "");

        // Titlebar
        GUI.DrawTexture(new Rect(x + 20, y + 5, 30, 30), titlebarIcon);
        GUI.Label(
            new Rect(x + 55, y + 7, WindowSize.x - 65, 20),
            $"Keyframe Editor"
        );

        // Start keyframes list
        GUILayout.BeginArea(new Rect(x + 10, y + 40, WindowSize.x - 20, 300));

        keyframesScrollPosition = GUILayout.BeginScrollView(keyframesScrollPosition, GUILayout.Width(WindowSize.x - 20), GUILayout.Height(300));

        for (int i = 0; i < KeyframeManager.Instance.Keyframes.Count; i++)
        {
            Keyframe k = KeyframeManager.Instance.Keyframes[i];

            string displayString = $"K{i} p:{UnityUtilities.Vector3ToString(k.Position)}, r:{UnityUtilities.Vector3ToString(k.Rotation)}";
            bool selectionStart = GUILayout.Toggle(SelectedKeyframeIndex == i, displayString);

            if (selectionStart && SelectedKeyframeIndex != i)
                SelectedKeyframeIndex = i;
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();

        // modify y to add all the stuff from the keyframe list and title
        y = 375f;

        if (SelectedKeyframeIndex != -1)
        {
            Keyframe k = KeyframeManager.Instance.Keyframes[SelectedKeyframeIndex];

            // Position
            GUI.Label(new Rect(x + 10, y, 200, 20), "Position: ");
            float positionX = CreateNumInputLabel(x + 70, y, 'X', ref k.Position.x);
            float positionY = CreateNumInputLabel(x + 245, y, 'Y', ref k.Position.y);
            float positionZ = CreateNumInputLabel(x + 420, y, 'Z', ref k.Position.z);
            k.Position = new Vector3(positionX, positionY, positionZ);

            // Rotation
            GUI.Label(new Rect(x + 10, y + 30, 200, 20), "Rotation: ");
            float rotationX = CreateNumInputLabel(x + 70, y + 30, 'X', ref k.Rotation.x);
            float rotationY = CreateNumInputLabel(x + 245, y + 30, 'Y', ref k.Rotation.y);
            float rotationZ = CreateNumInputLabel(x + 420, y + 30, 'Z', ref k.Rotation.z);
            k.Rotation = new Vector3(rotationX, rotationY, rotationZ);
        } else
        {
            GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
            centeredStyle.alignment = TextAnchor.MiddleCenter;

            GUI.Label(new Rect(x, y + 10, WindowSize.x, 20), "Select a keyframe to modify it.", centeredStyle);
        }
    }

    // Width: 180
    private float CreateNumInputLabel(float x, float y, char axis, ref float field)
    {
        GUI.Label(new Rect(x, y, 20, 20), $"{axis}: ");
        string input = float.ToString();
        input = GUI.TextField(new Rect(x + 20, y, 150, 20), input).Trim();

        if (float.TryParse(input, out float value))
            return MathF.Round(value, 2);

        return field;
    }
}