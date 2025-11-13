using HarmonyLib;
using UnityEngine.EventSystems;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(FPSInputModule), nameof(FPSInputModule.UpdateMouseState))]
static class UseWorldSpacePointerPosition
{
    public static void Prefix(FPSInputModule __instance, PointerEventData leftData)
    {
        leftData.position = __instance.lastRaycastResult.worldPosition;
    }
}