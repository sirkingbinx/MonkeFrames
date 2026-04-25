using System;
using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Interfaces;
using UnityEngine;

namespace MonkeFrames.Editor.Classes;

public class IEditorWindowManager
{
    public static int WindowIDs = 0;
    public static GUIStyle WindowStyle;

    public IEditorWindow Window;
    public Rect WindowPosition;
    public int WindowID;

    public bool Visible = false;
    public bool LastVisible = false;

    public IEditorWindowManager(IEditorWindow window)
    {
        Window = window;
        WindowPosition = window.Rect;

        WindowIDs++;
        WindowID = WindowIDs;
    }

    private void CreateWindow(int windowId)
    {
        // Window Creation ( done in Draw() )
        //  _________________________________
        // |  [>|] Window Name           [X] | <-- Topbar (done here)
        // |                                 |   <
        // |          hello world!           |   <
        // |                                 |   <
        // |                                 |   <
        // |                                 |   <
        // |                                 |   <   Content (done in Window.OnDraw() )
        // |                                 |   <
        // |                                 |   <
        // |                                 |   <
        // |                                 |   <
        // |_________________________________|   <

        GUI.DrawTexture(new Rect(5, 5, 20, 20), UIManager.Instance.Icon);
        GUI.Label(new Rect(30, 5, WindowPosition.width - 60, 20), Window.Name);

        if (GUI.Button(new Rect(WindowPosition.width - 25, 5, 20, 20), "X"))
            Visible = false;

        try
        {
            Window.OnDraw();
        } catch (Exception ex)
        {
            Debug.LogError($"Error drawing window \"{Window.Name}\" ({WindowID}): {ex.Message}");
        }

        GUI.DragWindow(new Rect(0, 0, WindowPosition.width, 30));
    }

    public void Draw()
    {
        WindowStyle ??= GUI.skin.box;

        GUI.backgroundColor = new Color(1, 1, 1, 0.95f);

        if (Visible)
            WindowPosition = GUI.Window(WindowID, WindowPosition, CreateWindow, GUIContent.none, WindowStyle);

        if (WindowPosition.x >= Screen.width - 20)
            WindowPosition.x = 0;

        if (WindowPosition.y >= Screen.height - 20)
            WindowPosition.y = 20;

        if (Visible != LastVisible) {
            if (Visible)
                Window.OnOpen();
            else
                Window.OnClose();
            
            LastVisible = Visible;
        }
    }
}