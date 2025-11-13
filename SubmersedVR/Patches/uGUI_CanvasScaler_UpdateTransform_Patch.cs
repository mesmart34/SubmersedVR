using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI_CanvasScaler), nameof(uGUI_CanvasScaler.UpdateTransform))]
public static class uGUI_CanvasScalerPDA_Attach
{
    public static void Postfix(uGUI_CanvasScaler __instance)
    {
        // TODO: There gotta be a better way to attach this only to the PDA, maybe custom behaviour, disabling the Scalar?
        if (__instance.gameObject.GetComponent<uGUI_PDA>() == null)
        {
            return;
        }

        if (VRCameraRig.instance == null)
        {
            return;
        }
        
        var rigWorldPos = SNCameraRoot.main.transform;

        var worldPos = __instance._anchor.transform.position;
        var worldRot = __instance._anchor.transform.rotation;
        var uiSpacePos = worldPos - rigWorldPos.position;
        var uiSpaceRotation = worldRot;
        
        __instance.rectTransform.position = uiSpacePos;
        __instance.rectTransform.rotation = uiSpaceRotation;
    }
}
