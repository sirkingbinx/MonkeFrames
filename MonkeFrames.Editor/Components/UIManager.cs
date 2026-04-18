using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using MonkeFrames.Compiler.Models;
using MonkeFrames.Editor.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Debug = UnityEngine.Debug;

using Keyframe = MonkeFrames.Compiler.Models.Keyframe;

namespace MonkeFrames.Editor.Components;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public string CurrentTask;

    public bool ShowingUI = true;
    public bool ShowingEditorUI = false;
    public bool ShowingJoinerUI = false;
    public bool ShowingCompilerUI = false;

    public Vector2 ScreenDimensions;
    public Vector2 KeyframeWindowSize = new(600, 550);

    private Vector2 keyframesScrollPosition;

    public int SelectedKeyframeIndex = -1;
    public bool AllowKeybinds = true;

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
        Keyframe k = KeyframeManager.Instance.Project.Keyframes[SelectedKeyframeIndex];
        CameraManager.Instance.Position = k.Position;
        CameraManager.Instance.Rotation = k.QuatRotation;
        CameraManager.Instance.FieldOfView = k.FieldOfView;
    }

    public void LateUpdate()
    {
        if (ShowingUI && AllowKeybinds) {
            if (Keyboard.current.vKey.wasPressedThisFrame)
                KeyframeManager.Instance.CreateKeyframe();

            if (Keyboard.current.tKey.wasPressedThisFrame)
                KeyframeManager.Instance.CreateKeyframe(lookAtPlayer: true);

            if (Keyboard.current.xKey.wasPressedThisFrame && SelectedKeyframeIndex != -1)
                KeyframeManager.Instance.CreateKeyframe(replaceKeyframeIdx: SelectedKeyframeIndex);

            if (Keyboard.current.deleteKey.wasPressedThisFrame && SelectedKeyframeIndex != -1)
                KeyframeManager.Instance.DeleteKeyframe(SelectedKeyframeIndex);

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

        bool button = false;

        if (ShowingUI)
        {
            if (c == null)
                button = GUI.Button(new Rect(x, 0, 100, 20), text);
            else
                button = GUI.Button(new Rect(x, 0, 150, 20), c);
        }

        return key.wasPressedThisFrame || button;
    }

    GUIContent primaryButton;
    CurrentMenu menu = CurrentMenu.Closed;
    GUIStyle left;

    void MenuTo(CurrentMenu newMenu) => menu = (menu == newMenu ? CurrentMenu.Closed : newMenu);

    public string CurrentStatus = "";

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

            GUI.Button(new Rect(0, 60, 300, 20), $"v{Constants.VersionID}");
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

#if DEBUG
            if (GUI.Button(new Rect(start, 60, 300, 20), "Diagnostics", left))
            {
                ShowingCompilerUI = !ShowingCompilerUI;
                menu = CurrentMenu.Closed;
            }
#endif
        }

        if (menu == CurrentMenu.F3)
        {
            const float start = 250f;

            if (GUI.Button(new Rect(start, 20, 300, 20), "To Selected Keyframe", left) && SelectedKeyframeIndex != -1)
            {
                GoToSelectedKeyframe();
                menu = CurrentMenu.Closed;

                CurrentStatus = $"Moved to Keyframe";
            }

            if (GUI.Button(new Rect(start, 40, 300, 20), "To Monke", left))
            {
                Vector3 headPos = GorillaTagger.Instance.headCollider.transform.position;
                Vector3 headRot = GorillaTagger.Instance.headCollider.transform.rotation.eulerAngles;

                CameraManager.Instance.Position = headPos;
                CameraManager.Instance.Rotation = Quaternion.Euler(headRot);
                menu = CurrentMenu.Closed;

                CurrentStatus = $"Teleported to Monke (Position: {headPos.ToCoordinateString()} Rotation: {headRot.ToCoordinateString()})";
            }
        }

        if (menu == CurrentMenu.F4 || menu == CurrentMenu.F4LoadMenu)
        {
            const float start = 350f;

            GUI.SetNextControlName("projectName");
            KeyframeManager.Instance.Project.Name = GUI.TextField(new Rect(start, 20, 300, 20), KeyframeManager.Instance.Project.Name);

            AllowKeybinds = GUI.GetNameOfFocusedControl() != "projectName";

            if (GUI.Button(new Rect(start, 40, 300, 20), $"FPS: {KeyframeManager.Instance.Project.FPS}", left))
            {
                if (KeyframeManager.Instance.Project.FPS == 30)
                    KeyframeManager.Instance.Project.FPS = 60;
                else if (KeyframeManager.Instance.Project.FPS == 60)
                    KeyframeManager.Instance.Project.FPS = 120;
                else
                    KeyframeManager.Instance.Project.FPS = 30;
            }

            if (GUI.Button(new Rect(start, 60, 300, 20), $"Load Project", left))
            {
                menu = (menu == CurrentMenu.F4LoadMenu ? CurrentMenu.F4 : CurrentMenu.F4LoadMenu);
            }

            if (GUI.Button(new Rect(start, 80, 300, 20), $"Save Project", left))
            {
                menu = CurrentMenu.Closed;
                SaveUtilities.Save();
                CurrentStatus = $"Saved project {KeyframeManager.Instance.Project.Name}";
            }

            if (GUI.Button(new Rect(start, 100, 300, 20), $"Compile", left))
            {
                menu = CurrentMenu.Closed;
                KeyframeManager.Instance.StartBuild();
            }

            if (GUI.Button(new Rect(start, 120, 300, 20), $"Compile & Play", left))
            {
                menu = CurrentMenu.Closed;
                KeyframeManager.Instance.StartBuildAndRun();
            }
        }

        if (menu == CurrentMenu.F4LoadMenu)
        {
            float startX = 650f;
            float startY = 60f;
            
            if (SaveUtilities.LoadableProjects.Count == 0) {
                GUI.Button(new Rect(startX, startY, 300, 20), $"no projects to load (save one first)", left);
            } else {
                float i = startY;

                foreach (KeyValuePair<string, Project> set in SaveUtilities.LoadableProjects)
                {
                    if (GUI.Button(new Rect(startX, i, 300, 20), set.Key, left))
                    {
                        menu = CurrentMenu.Closed;
                        KeyframeManager.Instance.LoadProject(set.Value);
                    }

                    i += 20;
                }
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

        ScreenDimensions = new Vector2(Screen.width, Screen.height);

        if (!ShowingUI)
            return;

        // Status bar
        GUI.Label(new Rect(10, ScreenDimensions.y - 30, ScreenDimensions.x - 20, 20), CurrentStatus);

        if (ShowingCompilerUI)
        {
            float x = 10f;
            float y = ScreenDimensions.y - 300;

            GUI.Box(new Rect(x, y, 300, 290), "");

            // Titlebar
            GUI.DrawTexture(new Rect(x + 10, y + 5, 35, 35), titlebarIcon);
            GUI.Label(
                new Rect(x + 60, y + 15, KeyframeWindowSize.x - 65, 29),
                "MonkeFrames.Compiler Diagnostics"
            );

            if (!KeyframeManager.Instance.Project.IsCompiled) {
                GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
                centeredStyle.alignment = TextAnchor.MiddleCenter;

                GUI.Label(new Rect(x + 10, y + 50, 280, 20), "Compile the project first.", centeredStyle);
            } else
            {
                GUI.Label(new Rect(x + 10, y + 50, 280, 20), $"made Frames: {KeyframeManager.Instance.Project.CompiledKeyframes.Count}");
                GUI.Label(new Rect(x + 10, y + 70, 280, 20), $"at      FPS: {KeyframeManager.Instance.Project.FPS}");
            }
        }

        if (ShowingEditorUI)
        {
            float x = ScreenDimensions.x - KeyframeWindowSize.x - 20;
            float y = 20f;
    
            GUI.Box(new Rect(x, y, KeyframeWindowSize.x, KeyframeWindowSize.y), "");

            // Titlebar
            GUI.DrawTexture(new Rect(x + 10, y + 5, 40, 40), titlebarIcon);
            GUI.Label(
                new Rect(x + 55, y + 15, KeyframeWindowSize.x - 65, 29),
                "Keyframe Editor"
            );

            // Start keyframes list
            GUILayout.BeginArea(new Rect(x + 10, y + 40, KeyframeWindowSize.x - 20, 300));

            keyframesScrollPosition = GUILayout.BeginScrollView(keyframesScrollPosition, GUILayout.Width(KeyframeWindowSize.x - 20), GUILayout.Height(300));

            for (int i = 0; i < KeyframeManager.Instance.Project.Keyframes.Count; i++)
            {
                Keyframe k = KeyframeManager.Instance.Project.Keyframes[i];

                string displayString = $"Keyframe {i + 1} p:{UnityUtilities.Vector3ToString(k.Position)}, r:{UnityUtilities.Vector3ToString(k.Rotation)}";
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
                Keyframe k = KeyframeManager.Instance.Project.Keyframes.ElementAt(SelectedKeyframeIndex);

                // Position
                GUI.Label(new Rect(x + 10, y, 200, 20), "Position: ");
                float px = CreateNumInputLabel(x + 70, y, 'X', ref k.Position.x);
                float py = CreateNumInputLabel(x + 245, y, 'Y', ref k.Position.y);
                float pz = CreateNumInputLabel(x + 420, y, 'Z', ref k.Position.z);

                // Rotation
                GUI.Label(new Rect(x + 10, y + 30, 200, 20), "Rotation: ");
                float rx = CreateNumInputLabel(x + 70, y + 30, 'X', ref k.Rotation.x);
                float ry = CreateNumInputLabel(x + 245, y + 30, 'Y', ref k.Rotation.y);
                float rz = CreateNumInputLabel(x + 420, y + 30, 'Z', ref k.Rotation.z);

                // FOV
                GUI.Label(new Rect(x + 10, y + 60, 200, 20), "FOV: ");
                float fov = CreateNumInputLabel(x + 50, y + 60, 'v', ref k.FieldOfView);

                k.Position.Set(px, py, pz);
                k.Rotation.Set(rx, ry, rz);
                k.FieldOfView = fov;

                // Transition
                GUI.Label(new Rect(x + 10, y + 95, 200, 20), "Transition Style:");

                if (GUI.Toggle(new Rect(x + 120, y + 95, 75, 20), k.Transition.Effect == TransitionEffect.Linear, "Linear"))
                    k.Transition.Effect = TransitionEffect.Linear;

                if (GUI.Toggle(new Rect(x + 195, y + 95, 75, 20), k.Transition.Effect == TransitionEffect.Sine, "Sine"))
                    k.Transition.Effect = TransitionEffect.Sine;

                if (GUI.Toggle(new Rect(x + 270, y + 95, 75, 20), k.Transition.Effect == TransitionEffect.Cut, "Cut / None"))
                    k.Transition.Effect = TransitionEffect.Cut;

                GUI.Label(new Rect(x + 10, y + 120, 200, 20), "Duration:");
                k.Transition.Duration = GUI.HorizontalSlider(new Rect(x + 75, y + 125, 395, 20), k.Transition.Duration, 0.0f, 30.0f);
                GUI.Label(new Rect(x + 475, y + 120, 25, 20), $"{k.Transition.Duration:F2}s");

                GUI.Label(new Rect(x + 10, KeyframeWindowSize.y - 25, KeyframeWindowSize.x, 20), $"GUID: {k.GUID} - Duration: {k.Transition.Duration:F2}s");

                KeyframeManager.Instance.Project.Keyframes[SelectedKeyframeIndex] = k;
            } else
            {
                GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
                centeredStyle.alignment = TextAnchor.MiddleCenter;

                GUI.Label(new Rect(x, y + 10, KeyframeWindowSize.x, 20), "Select a keyframe to view it's properties.", centeredStyle);
            }

            GUI.Label(new Rect(x + 10, KeyframeWindowSize.y - 5, KeyframeWindowSize.x, 20), $"Keyframes: {KeyframeManager.Instance.Project.Keyframes.Count}");
        }
    }

    // Width: 180
    private float CreateNumInputLabel(float x, float y, char axis, ref float field)
    {
        GUI.Label(new Rect(x, y, 20, 20), $"{axis}: ");
        string newNum = GUI.TextField(new Rect(x + 20, y, 150, 20), field.ToString());

        if (float.TryParse(newNum, out float newValue))
            return newValue;
           
        return field;
    }

    private enum CurrentMenu
    {
        Closed = -1,
        F1 = 0,
        F2,
        F3,
        F4,
        F4LoadMenu,
        F5
    }
}