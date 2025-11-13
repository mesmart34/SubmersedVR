using HarmonyLib;
using SubmersedVR.VR;
using UnityEngine;

namespace SubmersedVR.Patches;

//Head based vs Hand based movement
[HarmonyPatch(typeof(PlayerController), nameof(PlayerController.forwardReference), MethodType.Getter)]
internal static class PlayerController_forwardReference_Patch
{
    private static Transform _controllerTransform;
    
    internal static bool Prefix(PlayerController __instance, ref Transform __result)
    {
        if (Settings.HandBasedTurning)
        {
            //Use the Camera's position and the laser pointer's rotation
            //Use a dummy object to hold the transform
            if (_controllerTransform == null)
            {
                _controllerTransform = new GameObject().transform;
            }
            _controllerTransform.position = MainCamera.camera.transform.position;
            _controllerTransform.rotation = Settings.LeftHandBasedTurning ? VRCameraRig.GetLeftTargetTansform().rotation : VRCameraRig.GetTargetTansform().rotation; //the laser pointer transform
            __result = _controllerTransform;
        }
        else
        {
            __result = MainCamera.camera.transform;
        }
        return false;
    }
}