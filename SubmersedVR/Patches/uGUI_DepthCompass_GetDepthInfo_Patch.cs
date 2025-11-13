using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI_DepthCompass), nameof(uGUI_DepthCompass.GetDepthInfo))]
internal static class uGUI_DepthCompass_GetDepthInfo_Patch
{
    internal static void Postfix(ref uGUI_DepthCompass.DepthMode __result)
    {
        if (!WristHud.isHudOn)
        {
            __result = uGUI_DepthCompass.DepthMode.None;
        }
    }
}