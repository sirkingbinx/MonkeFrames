using UnityEngine;

namespace MonkeFrames.Editor.Models;

public struct MapData
{
    public string Name;
    public Vector3 Spawn;

    public MapData(string n, float x, float y, float z)
    {
        Name = n;
        Spawn = new Vector3(x, y, z);
    }
}
