using BepInEx;

namespace MonkeFrames.Editor;

[BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
public class PluginBepInEx : BaseUnityPlugin
{
    private void Start()
    {
        Constants.Loader = "BepInEx";
        Main.Start();
    }
}
