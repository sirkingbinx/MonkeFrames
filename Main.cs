using BepInEx;
using GorillaLocomotion;
using MonkeFrames.Components;
using UnityEngine;

namespace MonkeFrames;

[BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
public class Main : BaseUnityPlugin
{
    private void Start()
    {
        Debug.Log("[MonkeFrames::Initialize] Initializing MonkeFrames...");

        GameObject tpc = GameObject.Find("Shoulder Camera");

        tpc.AddComponent<CameraManager>();
        tpc.AddComponent<KeyframeManager>();
        tpc.AddComponent<UIManager>();

        Debug.Log("[MonkeFrames::Initialize] All components added");
    }
}
