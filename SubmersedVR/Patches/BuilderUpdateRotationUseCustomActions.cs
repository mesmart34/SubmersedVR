extern alias SteamVRActions;
using HarmonyLib;
using SteamVRActions::Valve.VR;

namespace SubmersedVR.Patches;

extern alias SteamVRRef;

[HarmonyPatch(typeof(Builder), nameof(Builder.UpdateRotation))]
public static class BuilderUpdateRotationUseCustomActions
{
    static bool Prefix(int max, ref bool __result)
    {
        if (SteamVR_Actions.subnautica_BuilderRotateRight.GetStateDown(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any))
        {
            Builder.lastRotation = (Builder.lastRotation + max - 1) % max;
            __result = true;
            return false;
        }
        if (SteamVR_Actions.subnautica_BuilderRotateLeft.GetStateDown(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any))
        {
            Builder.lastRotation = (Builder.lastRotation + 1) % max;
            __result = true;
            return false;
        }
        __result = false;
        return false;
    }
}