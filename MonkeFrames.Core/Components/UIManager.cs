using System.Diagnostics;
using System.IO;
using System.Reflection;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using MonkeFrames.Editor.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static GorillaTelemetry;
using Debug = UnityEngine.Debug;
using Keyframe = MonkeFrames.Compiler.Models.Keyframe;

namespace MonkeFrames.Editor.Components;

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

    public Texture2D titlebarIcon;

    public void Start()
    {
        Instance = this;

        Debug.Log("[MonkeFrames::UIManager] Loading titlebar icon");
        
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("icon");
        using MemoryStream ms = new MemoryStream();
        
        stream.CopyTo(ms);
        byte[] iconData = ms.ToArray();

        titlebarIcon = UnityUtilities.CreateTexture(iconData);

        primaryButton = new GUIContent("MonkeFrames", titlebarIcon);

        Debug.Log("[MonkeFrames::UIManager] UI manager is running");
    }

    void GoToSelectedKeyframe()
    {
        Keyframe k = KeyframeManager.Instance.Keyframes[SelectedKeyframeIndex];
        CameraManager.Instance.Position = k.Position;
        CameraManager.Instance.Rotation = k.QuatRotation;
        CameraManager.Instance.FieldOfView = k.FieldOfView;
    }

    public void LateUpdate()
    {
        if (ShowingEditorUI) {
            if (Keyboard.current.vKey.wasPressedThisFrame)
                KeyframeManager.Instance.CreateKeyframe();

            if (Keyboard.current.tKey.wasPressedThisFrame)
                KeyframeManager.Instance.CreateKeyframe(lookAtPlayer: true);

            if (Keyboard.current.xKey.wasPressedThisFrame && SelectedKeyframeIndex != -1)
                KeyframeManager.Instance.CreateKeyframe(replaceKeyframeIdx: SelectedKeyframeIndex);

            if (SelectedKeyframeIndex != -1 && Keyboard.current.fKey.wasPressedThisFrame)
            {
                GoToSelectedKeyframe();
            }
        }
    }

    bool Menu(KeyControl key, string text, int x, GUIContent c = null)
    {
        if (key.wasPressedThisFrame && CameraManager.Instance.CinemachineState)
        {
            CameraManager.Instance.SetModEnabled(true);
            return true;
        }

        if (CameraManager.Instance.CinemachineState)
            return false;

        bool button;

        if (c == null)
            button = GUI.Button(new Rect(x, 0, 100, 20), text);
        else
            button = GUI.Button(new Rect(x, 0, 150, 20), c);

        return key.wasPressedThisFrame || button;
    }

    GUIContent primaryButton;
    CurrentMenu menu;
    GUIStyle left;

    void MenuTo(CurrentMenu newMenu) => menu = (menu == newMenu ? CurrentMenu.Closed : newMenu);

    public void OnGUI()
    {
        if (left == null)
        {
            left = new GUIStyle(GUI.skin.button);
            left.alignment = TextAnchor.MiddleLeft;
            left.padding.left = 10;
        }

        if (Menu(Keyboard.current.f1Key, "MonkeFrames", 0, primaryButton))
            MenuTo(CurrentMenu.F1);

        if (Menu(Keyboard.current.f2Key, "View", 150))
            MenuTo(CurrentMenu.F2);

        if (Menu(Keyboard.current.f3Key, "Go", 250))
            MenuTo(CurrentMenu.F3);

        if (Menu(Keyboard.current.f4Key, "Project", 350))
            MenuTo(CurrentMenu.F4);

        if (Menu(Keyboard.current.f5Key, "Keyframe", 450))
            MenuTo(CurrentMenu.F5);

        if (menu == CurrentMenu.F1)
        {
            if (GUI.Button(new Rect(0, 20, 300, 20), "Disable (F1 to enable)", left))
            {
                CameraManager.Instance.SetModEnabled(false);
                menu = CurrentMenu.Closed;
            }

            if (GUI.Button(new Rect(0, 40, 300, 20), "Source (GitHub)", left))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/sirkingbinx/MonkeFrames",
                    UseShellExecute = true
                });
                menu = CurrentMenu.Closed;
            }

            GUI.Button(new Rect(0, 60, 300, 20), $"MonkeFrames v{Constants.Version} - {Constants.Loader}");
        }

        if (menu == CurrentMenu.F2)
        {
            const float start = 150f;

            if (GUI.Button(new Rect(start, 20, 300, 20), "Keyframe Editor", left))
            {
                ShowingEditorUI = !ShowingEditorUI;
                menu = CurrentMenu.Closed;
            }

            if (GUI.Button(new Rect(start, 40, 300, 20), "Room Joiner", left))
            {
                ShowingJoinerUI = !ShowingJoinerUI;
                menu = CurrentMenu.Closed;
            }
        }

        if (menu == CurrentMenu.F3)
        {
            const float start = 250f;

            if (GUI.Button(new Rect(start, 20, 300, 20), "To Selected Keyframe", left) && SelectedKeyframeIndex != -1)
            {
                GoToSelectedKeyframe();
                menu = CurrentMenu.Closed;
            }

            if (GUI.Button(new Rect(start, 40, 300, 20), "To Monke", left))
            {
                CameraManager.Instance.Position = GorillaTagger.Instance.headCollider.transform.position;
                CameraManager.Instance.Rotation = GorillaTagger.Instance.headCollider.transform.rotation;
                menu = CurrentMenu.Closed;
            }
        }

        if (menu == CurrentMenu.F4)
        {
            const float start = 350f;

            KeyframeManager.Instance.CurrentProject.Name = GUI.TextField(new Rect(start, 20, 300, 20), KeyframeManager.Instance.CurrentProject.Name);

            if (GUI.Button(new Rect(start, 40, 300, 20), $"FPS: {KeyframeManager.Instance.CurrentProject.FPS}", left))
            {
                KeyframeManager.Instance.CurrentProject.FPS = (KeyframeManager.Instance.CurrentProject.FPS == 30 ? 60 : 30);
            }
        }

        if (menu == CurrentMenu.F5)
        {
            const float start = 450f;
            if (GUI.Button(new Rect(start, 20, 300, 20), "Create New", left))
            {
                KeyframeManager.Instance.CreateKeyframe();
                menu = CurrentMenu.Closed;
            }

            if (GUI.Button(new Rect(start, 40, 300, 20), "Create New towards Monke", left))
            {
                KeyframeManager.Instance.CreateKeyframe(lookAtPlayer: true);
                menu = CurrentMenu.Closed;
            }

            if (GUI.Button(new Rect(start, 60, 300, 20), "Replace Selection with New", left) && SelectedKeyframeIndex != -1)
            {
                KeyframeManager.Instance.CreateKeyframe(replaceKeyframeIdx: SelectedKeyframeIndex);
                menu = CurrentMenu.Closed;
            }

            if (GUI.Button(new Rect(start, 80, 300, 20), "Delete Selection", left) && SelectedKeyframeIndex != -1)
            {
                KeyframeManager.Instance.DeleteKeyframe(SelectedKeyframeIndex);
                menu = CurrentMenu.Closed;
            }
        }

        if (!ShowingUI)
            return;

        // Thingy
        ScreenDimensions = new Vector2(Screen.width, Screen.height);

        if (ShowingEditorUI) {
            float x = ScreenDimensions.x - WindowSize.x - 20;
            float y = 20f;
    
            GUI.Box(new Rect(x, y, WindowSize.x, WindowSize.y), "");

            // Titlebar
            GUI.DrawTexture(new Rect(x + 10, y + 5, 40, 40), titlebarIcon);
            GUI.Label(
                new Rect(x + 55, y + 15, WindowSize.x - 65, 29),
                "Keyframe Editor"
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
                CreateNumInputLabel(x + 70, y, 'X', ref k.Position.x);
                CreateNumInputLabel(x + 245, y, 'Y', ref k.Position.y);
                CreateNumInputLabel(x + 420, y, 'Z', ref k.Position.z);

                // Rotation
                GUI.Label(new Rect(x + 10, y + 30, 200, 20), "Rotation: ");
                CreateNumInputLabel(x + 70, y + 30, 'X', ref k.Rotation.x);
                CreateNumInputLabel(x + 245, y + 30, 'Y', ref k.Rotation.y);
                CreateNumInputLabel(x + 420, y + 30, 'Z', ref k.Rotation.z);

                // FOV
                GUI.Label(new Rect(x + 10, y + 55, 200, 20), "FOV: ");
                CreateNumInputLabel(x + 50, y + 55, 'v', ref k.FieldOfView);

                GUI.Label(new Rect(x + 10, WindowSize.y - 45, WindowSize.x, 20), $"guid: {k.GUID}");
            } else
            {
                GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
                centeredStyle.alignment = TextAnchor.MiddleCenter;

                GUI.Label(new Rect(x, y + 10, WindowSize.x, 20), "Select a keyframe to view it's properties.", centeredStyle);
            }

            GUI.Label(new Rect(x + 10, WindowSize.y - 25, WindowSize.x, 20), $"in project: {KeyframeManager.Instance.CurrentProject.Name} - keyframes: {KeyframeManager.Instance.Keyframes.Count}");
            GUI.Label(new Rect(x + 10, WindowSize.y - 5, WindowSize.x, 20), $"MonkeFrames {Constants.Version} ({Constants.Loader})");
        }
    }

    // Width: 180
    private void CreateNumInputLabel(float x, float y, char axis, ref float field)
    {
        GUI.Label(new Rect(x, y, 20, 20), $"{axis}: ");
        GUI.TextField(new Rect(x + 20, y, 150, 20), field.ToString());
    }

    private enum CurrentMenu
    {
        Closed = -1,
        F1 = 0,
        F2,
        F3,
        F4,
        F5
    }
}