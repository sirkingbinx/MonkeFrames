using MonkeFrames.Editor.Components;
using UnityEngine;

namespace MonkeFrames.Editor
{
    public static class Main
    {
        public static void Start()
        {
            GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);
        }

        public static void OnPlayerSpawned()
        {
            Debug.Log("[MonkeFrames::Initialize] Initializing MonkeFrames...");

            GameObject tpc = GameObject.Find("Shoulder Camera") ?? GorillaTagger.Instance.thirdPersonCamera;

            tpc.AddComponent<CameraManager>();
            tpc.AddComponent<KeyframeManager>();
            tpc.AddComponent<UIManager>();

            Debug.Log("[MonkeFrames::Initialize] All components added");
        }
    }
}
