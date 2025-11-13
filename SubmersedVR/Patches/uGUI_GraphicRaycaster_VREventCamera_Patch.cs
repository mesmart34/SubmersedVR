using HarmonyLib;
using SubmersedVR.VR;
using UnityEngine;

namespace SubmersedVR.Patches;

// Make the uGUI_GraphicRaycaster take the LaserPointers EventCamera when possible
// Have to switch between guiCameraSpace and Worldspace for e.g. Scanner Room and Cyclops UI
[HarmonyPatch(typeof(uGUI_GraphicRaycaster))]
[HarmonyPatch(nameof(uGUI_GraphicRaycaster.eventCamera), MethodType.Getter)]
public static class uGUI_GraphicRaycaster_VREventCamera_Patch
{
    public static bool Prefix(uGUI_GraphicRaycaster __instance, ref Camera __result)
    {
        if (VRCameraRig.instance == null)
        {
            return true;
        }
        
        if (SNCameraRoot.main == null || __instance.guiCameraSpace)
        {
            __result = VRCameraRig.instance.UIControllerCamera;
        }
        else
        {
            __result = VRCameraRig.instance.WorldControllerCamera;
        }
        
        return false;
    }
}