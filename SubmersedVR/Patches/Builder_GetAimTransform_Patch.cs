using HarmonyLib;
using SubmersedVR.VR;
using UnityEngine;

namespace SubmersedVR.Patches;

// This makes it so the Builder tool aims with the laser pointer
[HarmonyPatch(typeof(Builder), nameof(Builder.GetAimTransform))]
internal static class Builder_GetAimTransform_Patch
{
    [HarmonyPrefix]
    internal static bool Prefix(ref Transform __result)
    {
        __result = Aiming.GetAimTransform();
        return false;
    }
}