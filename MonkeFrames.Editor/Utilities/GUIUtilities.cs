using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeFrames.Editor.Utilities;

public static class GUIUtilities
{
    public static (T, bool) Dropdown<T>(Rect rect, T current, bool droppedDown, IEnumerable<T> options, Func<T, string> toString)
    {
        T newCurrent = current;
        bool newDroppedDown = droppedDown;

        if (GUI.Button(rect, toString(options[current])))
            newDroppedDown = !newDroppedDown;

        if (droppedDown)
        {
            for (int i = 0; i < options; i++)
            {
                int newY = rect.y + (rect.height + (i * rect.height));

                if (GUI.Button(new Rect(rect.x, newY, rect.width, rect.height), toString(options[current])))
                {
                    newCurrent = options[current];
                    newDroppedDown = false;
                }
            }
        }

        return (newCurrent, newDroppedDown);
    }
}