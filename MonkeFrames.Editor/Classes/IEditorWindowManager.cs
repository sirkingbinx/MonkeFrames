using System;
using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Interfaces;
using UnityEngine;

namespace MonkeFrames.Editor.Classes;

public class IEditorWindowManager
{
    public static int WindowIDs = 0;

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

        GUI.DragWindow();
    }

    public void Draw()
    {
        if (Visible)
            GUI.Window(WindowID, WindowPosition, CreateWindow, GUIContent.none, GUI.skin.box);

        if (Visible != LastVisible) {
            if (Visible)
                Window.OnOpen();
            else
                Window.OnClose();
            
            LastVisible = Visible;
        }
    }
}