using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(Vehicle), nameof(Vehicle.OnPilotModeBegin))]
public static class SetHudStaticInVehicles
{
    public static void Postfix(Vehicle __instance)
    {
        if (__instance is SeaMoth || __instance is Exosuit)
        {
            VRHud.OnEnterVehicle();
        }
    }
}