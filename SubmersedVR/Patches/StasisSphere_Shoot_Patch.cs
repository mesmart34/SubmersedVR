using HarmonyLib;
using SubmersedVR.VR;
using UnityEngine;

namespace SubmersedVR.Patches;

// Make the stasis rifle/or sphere shoot with the hands rotation
// NOTE: This is easier than patching StasisRifle.Fire() but but might break some mods?
[HarmonyPatch(typeof(StasisSphere), nameof(StasisSphere.Shoot))]
internal static class StasisSphere_Shoot_Patch
{
    internal static bool Prefix(ref Quaternion rotation)
    {
        rotation = Aiming.GetAimTransform().rotation;
        return true;
    }
}