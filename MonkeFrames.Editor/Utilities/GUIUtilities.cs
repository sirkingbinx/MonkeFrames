using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonkeFrames.Editor.Utilities;

public static class GUIUtilities
{
    public static (T, bool) Dropdown<T>(Rect rect, T current, bool droppedDown, IEnumerable<T> options, Func<T, string> toString)
    {
        T newCurrent = current;
        bool newDroppedDown = droppedDown;

        if (GUI.Button(rect, toString(current)))
            newDroppedDown = !newDroppedDown;

        if (!droppedDown)
            return (newCurrent, newDroppedDown);

        foreach (var option in options.Select((item, index) => new { item, index }))
        {
            int newY = (int)(rect.y + (rect.height + (option.index * rect.height)));

            if (GUI.Button(new Rect(rect.x, newY, rect.width, rect.height), toString(option.item)))
            {
                newCurrent = option.item;
                newDroppedDown = false;
            }
        }

        return (newCurrent, newDroppedDown);
    }
}