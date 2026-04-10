using GorillaLocomotion;
using HarmonyLib;

namespace MonkeFrames.Patches;

public static class HarmonyPatches
{
    private static Harmony _harmonyInstance;
    
    /// <summary>
    ///     The current instance of Harmony that is patching the assembly.
    ///     If there is no Harmony instance, it will create one and return it.
    ///     You do not need to touch this section
    /// </summary>
    private static Harmony HarmonyInstance
    {
        get
        {
            _harmonyInstance ??= new Harmony(Constants.Guid);
            return _harmonyInstance;
        }
    }
    
    /// <summary>
    ///     Patch the assembly.
    /// </summary>
    public static void Patch()
    {
        HarmonyInstance.PatchAll();
    }
    
    /// <summary>
    ///     Unpatch the assembly.
    /// </summary>
    public static void Unpatch()
    {
        HarmonyInstance.UnpatchSelf();
    }
}
