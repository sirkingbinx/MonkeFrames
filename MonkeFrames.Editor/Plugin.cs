using BepInEx;
using MonkeFrames.Editor.Classes;
using MonkeFrames.Editor.Components;
using System;
using UnityEngine;

#if DEBUG
using System.Runtime.InteropServices;
#endif

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
        Console.WriteLine("[MonkeFrames::Initialize] Initializing MonkeFrames...");

        Constants.Init();

        GameObject tpc = GorillaTagger.Instance.thirdPersonCamera.transform.Find("Shoulder Camera").gameObject;

        tpc.SetActive(true);

        tpc.AddComponent<CameraManager>();
        tpc.AddComponent<KeyframeManager>();
        tpc.AddComponent<UIManager>();
        tpc.AddComponent<ConditionManager>();

        Console.WriteLine("[MonkeFrames::Initialize] All components added");

        Application.quitting += OnMonkeFramesUnloaded;
    }

    public static Action OnMonkeFramesLoaded = () =>
    {
        Console.WriteLine($"[MonkeFrames::Initialize] Welcome to MonkeFrames version {Constants.Version}");

        Settings.Load();

        CameraManager.Instance.SetModEnabled(true);
    };

    public static Action OnMonkeFramesUnloaded = () =>
    {
        Settings.Save();
    };

#if DEBUG
    Plugin()
    {
        AllocConsole();

        Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        Console.SetError(new System.IO.StreamWriter(Console.OpenStandardError()) { AutoFlush = true });

        Console.Title = $"MonkeFrames {Constants.VersionID} (Build {Constants.BuildDate})";

        Console.WriteLine($"MonkeFrames Debug Build {Constants.VersionID} (Build {Constants.BuildDate})");

        Application.logMessageReceived += HandleLogMsg;

        Application.quitting += () => {
            Application.logMessageReceived -= HandleLogMsg;
            FreeConsole();
        };
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FreeConsole();

    private static void HandleLogMsg(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception && stackTrace.Contains("MonkeFrames"))
        {
            Console.Error.WriteLine("An unhandled exception occured.");
            Console.Error.WriteLine($"Message:     {logString}");
            Console.Error.WriteLine($"Stack Trace: {stackTrace}");
        }
    }
#endif
}
