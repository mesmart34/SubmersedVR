using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(Vehicle), nameof(Vehicle.OnPilotModeBegin))]
internal static class Vehicle_OnPilotModeBegin_Patch
{
    internal static void Postfix(Vehicle __instance)
    {
        if (__instance is SeaMoth || __instance is Exosuit)
        {
            VRHud.OnEnterVehicle();
        }
    }
}