using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI_PowerIndicator), nameof(uGUI_PowerIndicator.IsPowerEnabled))]
internal static class uGUI_PowerIndicator_IsPowerEnabled_Patch
{
    internal static void Postfix(ref bool __result)
    {
        __result &= WristHud.isHudOn;
    }
}