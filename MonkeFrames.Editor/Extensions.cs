using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Utilities;
using UnityEngine;

namespace MonkeFrames.Editor;

public static class Extensions
{
    public static string ToCoordinateString(this Vector3 vector) => UnityUtilities.Vector3ToString(vector);

    public static string ToReadableString(this TimeSpan span)
    {
        var parts = new List<string>();
        if (span.Days > 0) parts.Add($"{span.Days} day{(span.Days > 1 ? "s" : "")}");
        if (span.Hours > 0) parts.Add($"{span.Hours} hour{(span.Hours > 1 ? "s" : "")}");
        if (span.Minutes > 0) parts.Add($"{span.Minutes} minute{(span.Minutes > 1 ? "s" : "")}");
        if (span.Seconds > 0) parts.Add($"{span.Seconds} second{(span.Seconds > 1 ? "s" : "")}");

        return parts.Count > 0 ? string.Join(", ", parts) : "0 seconds";
    }

    public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t != null);
        }
    }

    public static void SetAlpha(this Texture2D tex, float alphaValue)
    {
        Color[] pixels = tex.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i].a = alphaValue;
        }

        tex.SetPixels(pixels);
        tex.Apply();
    }
}