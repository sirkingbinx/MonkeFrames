using System.Linq;
using System.Reflection;
using BepInEx;
using MonkeFrames.Core.Components;
using UnityEngine;

namespace MonkeFrames.Core;

[BepInPlugin(Constants.Guid, Constants.Name, Constants.Version + ".0")]
public class PluginBepInEx : BaseUnityPlugin
{
    private void Start()
    {
        Debug.Log("[MonkeFrames::Initialize] Initializing MonkeFrames...");

        Constants.Loader = "BepInEx";

        GameObject tpc = GameObject.Find("Shoulder Camera");

        tpc.AddComponent<CameraManager>();
        tpc.AddComponent<KeyframeManager>();
        tpc.AddComponent<UIManager>();

        Debug.Log("[MonkeFrames::Initialize] All components added");
    }
}
