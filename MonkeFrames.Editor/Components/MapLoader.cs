using System.Collections.Generic;
using GorillaLocomotion;
using GorillaNetworking;
using MonkeFrames.Editor.Models;
using UnityEngine;

namespace MonkeFrames.Editor.Components;

public class MapLoader
{
    public static List<MapData> maps = [
        new("Forest", -71.32f, 11.79f, -90.67f),
        new("Basement", -41.10f, 16.673f, -95.34f),
        new("Canyons", -74.59f, 11.5f, -92.38f),
        new("City", -66.19f, 13.78f, -95.64f),
        new("Mountains", -27.39f, 17.19f, -103.29f),
        new("Ranked", -65.05f, 7.573f, -163.80f),
    ];

    public static GameObject transformGeneratingObject;

    public static void Load(MapData map)
    {
        if (transformGeneratingObject == null)
            transformGeneratingObject = new GameObject("MonkeFrames Transform Finding Thingymabob");

        if (!NetworkSystem.Instance.InRoom || NetworkSystem.Instance.GameModeString.Contains("MODDED"))
        {
            transformGeneratingObject.transform.position = map.Spawn;
            GTPlayer.Instance.TeleportTo(transformGeneratingObject.transform, false, false);
        } else
        {
            UIManager.Instance.CurrentStatus = "You must either be in a modded lobby or disconnected to change the current map.";
        }
    }
}
