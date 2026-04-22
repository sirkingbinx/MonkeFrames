using MonkeFrames.Editor.Interfaces;
using UnityEngine;

namespace MonkeFrames.Editor.Classes;

public class IEditorWindowManager
{
    public static int WindowIDs = 0;
    public static GUISkin WindowStyle;

    public IEditorWindow Window;
    public Rect WindowPosition;
    public int WindowID;
    public bool Visible = false;

    public IEditorWindowManager(IEditorWindow window)
    {
        Window = window;
        WindowPosition = window.Rect;
        WindowStyle = GUI.skin.box;

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

        GUI.DrawTexture(new Rect(5, 0, 20, 20), UIManager.Instance.titlebarIcon);
        GUI.Label(new Rect(25, 20, WindowPosition.width - 55, 20), Window.Name, WindowStyle);

        if (GUI.Button(new Rect(WindowPosition.width - 25, 0, 20, 20), "X"))
            Visible = false;

        try
        {
            Window.OnDraw();
        } catch (Exception ex)
        {
            Debug.LogError($"Error drawing window \"{Window.Name}\" ({WindowID}): {ex.Message}");
        }
    }

    public void Draw()
    {
        if (Visible) {
            GUI.Window(WindowID, WindowPosition, CreateWindow, GUIContent.none);
            GUI.DragWindow(new Rect(0, 0, WindowPosition.width, 20));
        }
    }
}