using MelonLoader;
using MonkeFrames.Editor;
using MonkeFrames.Editor.Components;
using UnityEngine;

[assembly: MelonInfo(typeof(PluginMelonLoader), MonkeFrames.Editor.Constants.Name, MonkeFrames.Editor.Constants.Version + ".0", MonkeFrames.Editor.Constants.Author)]
[assembly: MelonGame("Another Axiom", "Gorilla Tag")]
[assembly: HarmonyDontPatchAll]

namespace MonkeFrames.Editor;

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
