extern alias SteamVRRef;
using HarmonyLib;
using SteamVRRef::Valve.VR;
using SubmersedVR.Input;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(GameInput), nameof(GameInput.GetButtonHeld))]
internal static class GameInput_GetButtonHeld_Patch
{
    internal static bool Prefix(GameInput.Button action, ref bool __result)
    {
        if (SteamVrGameInput.ShouldIgnore(action))
        {
            return false;
        }

        var actionName = $"{action}";
        __result = SteamVR_Input.GetState(actionName, SteamVR_Input_Sources.Any);
        return false;
    }
}