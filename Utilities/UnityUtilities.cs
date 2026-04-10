using UnityEngine;

namespace MonkeFrames.Utilities
{
    public static class UnityUtilities
    {
        public static string Vector3ToString(Vector3 vec)
        {
            return $"({vec.x}, {vec.y}, {vec.z})";
        }
    }
}
