using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI), nameof(uGUI.UpdateLevelIdentifier))]
internal static class uGUI_UpdateLevelIdentifier_Patch
{
    internal static void Postfix(uGUI __instance)
    {
        VRCameraRig.instance?.UpdateShowControllers();
    }
}