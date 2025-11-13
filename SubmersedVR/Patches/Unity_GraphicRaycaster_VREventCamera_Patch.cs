using HarmonyLib;
using SubmersedVR.VR;
using UnityEngine;
using UnityEngine.UI;

namespace SubmersedVR.Patches;

// Same Patch as above but for the UnityEngine GraphicRaycaster
// Turns out some canvases like the left panel on the cyclops don't use the uGUI_GraphicRaycaster
// TODO: They seem to be in world space only though, have to double check.
[HarmonyPatch(typeof(GraphicRaycaster))]
[HarmonyPatch(nameof(GraphicRaycaster.eventCamera), MethodType.Getter)]
internal static class Unity_GraphicRaycaster_VREventCamera_Patch
{
    internal static bool Prefix(GraphicRaycaster __instance, ref Camera __result)
    {
        // TODO: Clean this up
        var canvas = __instance.GetComponent<Canvas>();
        if (canvas == null)
        {
            return true;
        }
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null))
        {
            return true;
        }
        if (VRCameraRig.instance == null)
        {
            return true;
        }
        __result = VRCameraRig.instance.WorldControllerCamera;
        return false;
    }
}
