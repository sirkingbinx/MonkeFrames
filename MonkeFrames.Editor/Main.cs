using System;
using MonkeFrames.Editor.Classes;
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

            GameObject tpc = GorillaTagger.Instance.thirdPersonCamera.transform.Find("Shoulder Camera").gameObject;

            tpc.SetActive(true);

            tpc.AddComponent<CameraManager>();
            tpc.AddComponent<KeyframeManager>();
            tpc.AddComponent<UIManager>();
            tpc.AddComponent<ConditionManager>();

            Debug.Log("[MonkeFrames::Initialize] All components added");

            Application.quitting += OnMonkeFramesUnloaded;
        }

        public static Action OnMonkeFramesLoaded = () =>
        {
            Debug.Log($"[MonkeFrames::Initialize] Welcome to MonkeFrames version {Constants.Version}");

            Settings.Load();

            CameraManager.Instance.SetModEnabled(true);
        };

        public static Action OnMonkeFramesUnloaded = () =>
        {
            Settings.Save();
        };
    }
}
