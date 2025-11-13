using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(VROptions), nameof(VROptions.GetUseGazeBasedCursor))]
internal static class VROptions_GetUseGazeBasedCursor_Patch
{
    internal static bool Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}