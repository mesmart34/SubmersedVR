using HarmonyLib;

namespace SubmersedVR.Patches;

// This removes the animation that inspects the object/tool when equipped for the first time
[HarmonyPatch(typeof(ArmsController), nameof(ArmsController.StartInspectObjectAsync))]
internal static class ArmsController_StartInspectObjectAsync_Patch
{
    internal static bool Prefix(ArmsController __instance)
    {
        return false;
    }
}