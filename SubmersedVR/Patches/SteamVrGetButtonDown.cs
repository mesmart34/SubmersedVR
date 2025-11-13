extern alias SteamVRRef;
using HarmonyLib;
using SteamVRRef::Valve.VR;
using SubmersedVR.Input;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(GameInput), nameof(GameInput.GetButtonDown))]
public static class SteamVrGetButtonDown
{
    static bool Prefix(GameInput.Button action, ref bool __result)
    {
        if (SteamVrGameInput.ShouldIgnore(action))
        {
            return false;
        }

        var actionName = $"{action}";
        __result = SteamVR_Input.GetStateDown(actionName, SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
        return false;
    }
}