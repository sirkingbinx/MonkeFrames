using MelonLoader;
using MonkeFrames.Editor;

[assembly: MelonInfo(typeof(PluginMelonLoader), MonkeFrames.Editor.Constants.Name, MonkeFrames.Editor.Constants.Version + ".0", MonkeFrames.Editor.Constants.Author)]
[assembly: MelonGame("Another Axiom", "Gorilla Tag")]
[assembly: HarmonyDontPatchAll]

namespace MonkeFrames.Editor;

public class PluginMelonLoader : MelonMod
{
    public override void OnLateInitializeMelon()
    {
        Constants.Loader = "MelonLoader";
        Main.Start();
    }
}
