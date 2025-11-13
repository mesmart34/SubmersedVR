using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

// Switch to the vehicle quickslots when entering a vehicle and back to the player quickslots when exiting
[HarmonyPatch]
internal static class QuickSlots_Patch
{
    [HarmonyPatch(typeof(Vehicle), nameof(Vehicle.OnPilotModeBegin))]
    internal static void Postfix(Vehicle __instance)
    {
        VRCameraRig.instance?.VrQuickSlots?.SetTarget(__instance);
    }

    [HarmonyPatch(typeof(Vehicle), nameof(Vehicle.OnPilotModeEnd))]
    internal static void Postfix()
    {
        VRCameraRig.instance?.VrQuickSlots?.SetTarget(null);
    }
}