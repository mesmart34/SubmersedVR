using HarmonyLib;
using SubmersedVR.VR;
using static UWE.CoroutineHost;

namespace SubmersedVR.Patches;

// Create the VRCameraRig when ArmsController is started
[HarmonyPatch(typeof(ArmsController), nameof(ArmsController.Start))]
internal static class ArmsController_Start_Patch
{
    [HarmonyPostfix]
    public static void Postfix(ArmsController __instance)
    {
        var mainCamera = SNCameraRoot.main.mainCam;
        VRCameraRig.instance.SetCameraTrackTarget(mainCamera.transform.parent);
        StartCoroutine(VRCameraRig.instance.SetupGameCameras());
        
        // Disable IK
        __instance.ik.enabled = false;
        __instance.leftAim.aimer.enabled = false;
        __instance.rightAim.aimer.enabled = false;

        // Attach
        __instance.gameObject.AddComponent<VRHands>().Setup(__instance.ik);
        // __instance.pda.ui.canvasScaler.vrMode = uGUI_CanvasScaler.Mode.Inversed;
    }
}