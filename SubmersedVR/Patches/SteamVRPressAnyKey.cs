extern alias SteamVRRef;
using HarmonyLib;
using SteamVRRef::Valve.VR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(GameInput), nameof(GameInput.AnyKeyDown), MethodType.Getter)]
public static class SteamVRPressAnyKey
{
    static void Postfix(ref bool __result)
    {
        if (__result)
        {
            return;
        }

        foreach (var action in SteamVR_Input.actionsBoolean)
        {
            if (action.GetStateDown(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any))
            {
                __result = true;
                break;
            }
        }
    }
}