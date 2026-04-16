using MonkeFrames.Editor.Utilities;
using UnityEngine;

namespace MonkeFrames.Editor;

public static class Extensions
{
    public static string ToCoordinateString(this Vector3 vector) => UnityUtilities.Vector3ToString(vector);
}