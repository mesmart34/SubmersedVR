extern alias SteamVRRef;
using HarmonyLib;
using SteamVRRef::Valve.VR;
using SubmersedVR.Input;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(GameInput), nameof(GameInput.GetButtonUp))]
internal static class SteamVrGetButtonUp
{
    internal static bool Prefix(GameInput.Button action, ref bool __result)
    {
        if (SteamVrGameInput.ShouldIgnore(action))
        {
            return false;
        }

        var actionName = $"{action}";
        __result = SteamVR_Input.GetStateUp(actionName, SteamVR_Input_Sources.Any);
        return false;
    }
}