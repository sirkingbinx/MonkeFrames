using BepInEx;
using MonkeFrames.Editor.Classes;
using MonkeFrames.Editor.Components;
using System;
using UnityEngine;

namespace MonkeFrames.Editor;

[BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
public class Plugin : BaseUnityPlugin
{
    public void Start()
    {
        HarmonyLib.Harmony.CreateAndPatchAll(typeof(Plugin).Assembly, Constants.Guid);
        GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);
    }

    public static void OnPlayerSpawned()
    {
        Debug.Log("[MonkeFrames::Initialize] Initializing MonkeFrames...");

        Constants.Init();

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
