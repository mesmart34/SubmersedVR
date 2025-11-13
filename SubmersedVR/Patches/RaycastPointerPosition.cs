using HarmonyLib;
using SubmersedVR.VR;
using UnityEngine;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(FPSInputModule), nameof(FPSInputModule.GetCursorScreenPosition))]
internal static class RaycastPointerPosition
{
    internal static void Postfix(ref Vector2 __result, FPSInputModule __instance)
    {
        if (VRCameraRig.instance == null || VRCameraRig.instance.UIControllerCamera == null)
        {
            return;
        }

        var eventCamera = VRCameraRig.instance.UIControllerCamera;
        __result = new Vector2(eventCamera.pixelWidth * 0.5f, eventCamera.pixelHeight * 0.5f);
    }
}