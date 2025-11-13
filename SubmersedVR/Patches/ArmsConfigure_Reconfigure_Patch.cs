using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

// Reconfigure the aiming
[HarmonyPatch(typeof(ArmsController), nameof(ArmsController.Reconfigure))]
internal static class ArmsConfigure_Reconfigure_Patch
{
    [HarmonyPostfix]
    public static void Postfix(PlayerTool tool)
    {
        VRHands.instance?.OnToolEquipped(tool);
    }
}