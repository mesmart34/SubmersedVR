using HarmonyLib;
using SubmersedVR.Tweaks;
using UnityEngine;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(PlayerCinematicController), nameof(PlayerCinematicController.UpdatePlayerPosition))]
internal static class PlayerCinematicController_UpdatePlayerPosition_Patch
{
    internal static bool Prefix(PlayerCinematicController __instance)
    {
        if (__instance.gameObject.name != "Life_Pod_damaged_03" || VROptions.enableCinematics)
        {
            return true;
        }
        
        IntroFixerVR.animTime += Time.deltaTime;
        var component = __instance.player.GetComponent<Transform>();
        var component2 = MainCameraControl.main.GetComponent<Transform>();
        component.position = __instance.animatedTransform.position;
        component2.position = __instance.player.camAnchor.position;
        if (IntroFixerVR.animTime >= IntroFixerVR.animTimeTimeout)
        {
            return false;
        }
        
        component.rotation = __instance.animatedTransform.rotation;
        component2.rotation = __instance.animatedTransform.rotation;
        return false;

    }
}