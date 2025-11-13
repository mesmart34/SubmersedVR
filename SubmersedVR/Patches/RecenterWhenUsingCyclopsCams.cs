using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(CyclopsExternalCams), nameof(CyclopsExternalCams.SetActive))]
static class RecenterWhenUsingCyclopsCams
{
    public static void Postfix()
    {
        VRUtil.Recenter();
    }
}