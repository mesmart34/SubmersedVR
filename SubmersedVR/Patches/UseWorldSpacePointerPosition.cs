using HarmonyLib;
using UnityEngine.EventSystems;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(FPSInputModule), nameof(FPSInputModule.UpdateMouseState))]
internal static class UseWorldSpacePointerPosition
{
    internal static void Prefix(FPSInputModule __instance, PointerEventData leftData)
    {
        leftData.position = __instance.lastRaycastResult.worldPosition;
    }
}