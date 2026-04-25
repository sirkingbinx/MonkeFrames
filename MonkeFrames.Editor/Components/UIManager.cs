using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MonkeFrames.Editor.Classes;
using MonkeFrames.Editor.Interfaces;
using MonkeFrames.Editor.Utilities;
using UnityEngine;

namespace MonkeFrames.Editor.Components;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public int Selection = -1;

    public string Status = "";
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

    public List<IEditorMenuManager> Menus = [];
    public List<IEditorWindowManager> Windows = [];

    public int CurrentMenuIndex = -1;

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

        Debug.Log("[MonkeFrames::UIManager] Initializing managers...");

        List<Type> windowTypes = Assembly.GetExecutingAssembly().GetLoadableTypes()
            .Where(t => typeof(IEditorWindow).IsAssignableFrom(t) && t.IsClass).ToList();
        List<Type> menuTypes = Assembly.GetExecutingAssembly().GetLoadableTypes()
            .Where(t => typeof(IEditorMenu).IsAssignableFrom(t) && t.IsClass).ToList();

        foreach (Type windowType in windowTypes)
        {
            if (Activator.CreateInstance(windowType) is not IEditorWindow window)
                return;

            Windows.Add(new IEditorWindowManager(window));
        }

        foreach (Type menuType in menuTypes)
        {
            if (Activator.CreateInstance(menuType) is not IEditorMenu menu)
                return;

            Menus.Add(new IEditorMenuManager(menu));
        }

        Debug.Log("[MonkeFrames::UIManager] UI manager is running");

        Main.OnMonkeFramesLoaded.Invoke();
    }

    public void OpenWindow(string menuName)
    {
        Windows.First(w => w.Window.Name == menuName).Visible = true;
    }

    public void CloseWindow(string menuName)
    {
        Windows.First(w => w.Window.Name == menuName).Visible = false;
    }

    public void ToggleWindow(string menuName)
    {
        var w = Windows.First(w => w.Window.Name == menuName);
        w?.Visible = !w.Visible;
    }

    public void OnGUI()
    {
        if (!Drawing)
            return;

        Menus.ForEach(menu => { CurrentMenuIndex = menu.Draw(CurrentMenuIndex); });
        Windows.ForEach(window => {
            if (window.Visible)
                window.Draw();
        });

        GUI.Label(new Rect(10, Screen.width - 30, Screen.height - 10, 20), Status);
    }
}