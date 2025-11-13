using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(VROptions), nameof(VROptions.GetUseGazeBasedCursor))]
public static class ForceGazeBasedCursor
{
    public static bool Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}