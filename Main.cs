using BepInEx;
using GorillaLocomotion;
using MonkeFrames.Components;
using UnityEngine;

namespace MonkeFrames;

[BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
public class Main : BaseUnityPlugin
{
    public static Main Instance;

    private void Start()
    {
        Instance = this;

        // Add all our components to the TPC
        GameObject tpc = GameObject.Find("Shoulder Camera");

        tpc.AddComponent<CameraManager>();
        tpc.AddComponent<KeyframeManager>();
        tpc.AddComponent<UIManager>();
    }
}
