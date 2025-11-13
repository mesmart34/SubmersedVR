using HarmonyLib;

namespace SubmersedVR.Patches;

// Makes the builder menu spawn infront of you in vr
// TODO: Could make those more general patches?
[HarmonyPatch(typeof(uGUI_BuilderMenu), nameof(uGUI_BuilderMenu.Awake))]
internal static class uGUI_BuilderMenu_Awake_Patch
{
    internal static void Postfix(uGUI_BuilderMenu __instance)
    {
        var scalar = __instance.GetComponent<uGUI_CanvasScaler>();
        scalar.vrMode = uGUI_CanvasScaler.Mode.Static;
    }
}