using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI_Pings), nameof(uGUI_Pings.IsVisibleNow))]
internal static class uGUI_Pings_IsVisibleNow
{
    internal static void Postfix(ref bool __result)
    {
        __result &= WristHud.isHudOn;
    }
}