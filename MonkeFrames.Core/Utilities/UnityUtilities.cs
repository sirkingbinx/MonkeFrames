using System;
using UnityEngine;

namespace MonkeFrames.Editor.Utilities
{
    public static class UnityUtilities
    {
        public static string Vector3ToString(Vector3 vec)
        {
            return $"({vec.x}, {vec.y}, {vec.z})";
        }

        public static Texture2D CreateTexture(byte[] img)
        {
            Texture2D tex = new Texture2D(2, 2);
            bool isLoaded = tex.LoadImage(img);

            tex.LoadImage(img);
            return tex;
            
            throw new Exception("Image failed to load");
        }
    }
}
