using HarmonyLib;
using UnityEngine;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(FPSInputModule), nameof(FPSInputModule.ShouldStartDrag))]
class SetDragThresholdHacky
{
    public static bool Prefix(ref bool __result, Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
    {
        // TODO: This has to be dependent on canvas scale, way to high for big pda, too low for small pda
        var newThreshold = 0.04f;
        __result = !useDragThreshold || (pressPos - currentPos).sqrMagnitude >= newThreshold * newThreshold;
        return false;
    }
}