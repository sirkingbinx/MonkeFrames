using System;
using BepInEx;

namespace MonkeFrames;

[BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
public class Main : BaseUnityPlugin
{
    public static Main Instance;

    private void Start()
    {
        Instance = this;
        GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);
    }

    private void OnPlayerSpawned()
    {
        try
        {
            Logger.LogInfo($"[{Constants.Name}] I have loaded!");
        }
        catch (Exception e)
        {
            Logger.LogError($"[{Constants.Name}] Player start failed:{e.Message + e.StackTrace}");
        }
    }
}
