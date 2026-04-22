using System.Collections.Generic;
using MonkeFrames.Editor.Interfaces;
using UnityEngine;

namespace MonkeFrames.Editor.Classes;

public class IEditorMenuManager
{
    public static GUIStyle ContextMenuButtonStyle
    {
        get {
            if (field == null)
            {
                Texture2D whiteTexture = new Texture2D(1, 1);
                whiteTexture.SetPixel(0, 0, Color.white);
                whiteTexture.Apply();

                field = new GUIStyle(GUI.skin.button);
                field.alignment = TextAnchor.MiddleLeft;
                field.padding.left = 10;
            }

            return field;
        }
    }

    public IEditorMenu Menu;
    public List<EditorMenuItem> Items;

    public IEditorWindowManager(IEditorMenu menu)
    {
        Menu = menu;
    }
    
    // return index of menu to open, or -1 to hide all
    public EditorActionType Draw(int openedIndex)
    {
        int x = Menu.Index * 100;
        int y = 20;
        int width = 300;

        if (openedIndex == Menu.Index)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                EditorMenuItem item = Items[i];
                
                if (GUI.Button(new Rect(x, y, width, 20), item.Name, ContextMenuButtonStyle))
                {
                    item.Action();
                    return -1;
                }

                y += 20;
            }
        }

        if (GUI.Button(new Rect(x, 0, 100, 20), menu.Name))
        {
            if (openedIndex == menu.Name)
                return -1;
            else
                return Menu.Index;
        }

        return openedIndex;
    }
}