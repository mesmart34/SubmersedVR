extern alias SteamVRActions;
using HarmonyLib;
using SteamVRActions::Valve.VR;

namespace SubmersedVR.Patches;

extern alias SteamVRRef;

[HarmonyPatch(typeof(Builder), nameof(Builder.CalculateAdditiveRotationFromInput))]
internal static class Builder_CalculateAdditiveRotationFromInput_Patch
{
    internal static bool Prefix(float additiveRotation, ref float __result)
    {
        if (SteamVR_Actions.subnautica_BuilderRotateRight.GetState(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any))
        {
#pragma warning disable Harmony003
            additiveRotation = MathExtensions.RepeatAngle(additiveRotation -
                                                          Builder.GetDeltaTimeForAdditiveRotation() *
                                                          Builder.additiveRotationSpeed);
        }
        else if (SteamVR_Actions.subnautica_BuilderRotateLeft.GetState(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any))
        {
            additiveRotation = MathExtensions.RepeatAngle(additiveRotation +
                                                          Builder.GetDeltaTimeForAdditiveRotation() *
                                                          Builder.additiveRotationSpeed);
        }
#pragma warning restore Harmony003
        __result = additiveRotation;
        return false;
    }
}