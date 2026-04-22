using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MonkeFrames.Editor.Components;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public string Status = "";
    public Vector2 Screen;
    public Texture2D Icon;

    public GUIStyle CenterText
    {
        get {
            if (field == null) {
                field = new GUIStyle(GUI.skin.label);
                field.alignment = TextAnchor.MiddleCenter;
            }

            return field;
        }
    }

    public List<IEditorMenu> Menus;
    public List<IEditorWindow> Windows;

    public bool Drawing;

    public UIManager()
    {
        Instance = this;
    }

    public void Start()
    {
        Debug.Log("[MonkeFrames::UIManager] Loading titlebar icon");
        
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("icon");
        using MemoryStream data = new MemoryStream();
        
        stream.CopyTo(data);
        Icon = UnityUtilities.CreateTexture(data.ToArray());

        Debug.Log("[MonkeFrames::UIManager] UI manager is running");
    }

    public void OnGUI()
    {
        Screen = new Vector2(Screen.width, Screen.height);

        if (!Drawing)
            return;

        GUI.Label(new Rect(10, ScreenDimensions.y - 30, ScreenDimensions.x - 10, 20), Status);
    }
}