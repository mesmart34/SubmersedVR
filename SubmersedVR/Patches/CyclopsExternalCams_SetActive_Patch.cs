using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(CyclopsExternalCams), nameof(CyclopsExternalCams.SetActive))]
internal static class CyclopsExternalCams_SetActive_Patch
{
    internal static void Postfix()
    {
        VRUtil.Recenter();
    }
}