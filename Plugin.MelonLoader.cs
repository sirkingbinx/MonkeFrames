using System.Linq;
using System.Reflection;
using MelonLoader;
using MonkeFrames;
using MonkeFrames.Components;
using UnityEngine;

[assembly: MelonInfo(typeof(PluginMelonLoader), MonkeFrames.Constants.Name, MonkeFrames.Constants.Version + ".0", MonkeFrames.Constants.Author)]
[assembly: MelonGame("Another Axiom", "Gorilla Tag")]
[assembly: HarmonyDontPatchAll]

namespace MonkeFrames;

public class PluginMelonLoader : MelonMod
{
    public override void OnLateInitializeMelon()
    {
        Debug.Log("[MonkeFrames::Initialize] Initializing MonkeFrames...");

        Constants.Loader = "MelonLoader";

        GameObject tpc = GameObject.Find("Shoulder Camera");

        tpc.AddComponent<CameraManager>();
        tpc.AddComponent<KeyframeManager>();
        tpc.AddComponent<UIManager>();

        Debug.Log("[MonkeFrames::Initialize] All components added");
    }
}
