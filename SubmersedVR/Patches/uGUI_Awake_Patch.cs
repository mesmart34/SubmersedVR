using HarmonyLib;
using SubmersedVR.VR;
using UnityEngine;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI), nameof(uGUI.Awake))]
internal static class uGUI_AwakeS_Patch
{
    [HarmonyPostfix]
    internal static void Postfix()
    {
        // TODO: Should use proper singleton pattern?
        var rig = new GameObject(nameof(VRCameraRig)).AddComponent<VRCameraRig>();
        VRCameraRig.instance = rig;
        Object.DontDestroyOnLoad(rig);
    }
}