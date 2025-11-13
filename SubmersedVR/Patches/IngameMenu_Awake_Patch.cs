using HarmonyLib;

namespace SubmersedVR.Patches;

// Makes the ingame menu spawn infront of you in vr
[HarmonyPatch(typeof(IngameMenu), nameof(IngameMenu.Awake))]
internal static class IngameMenu_Awake_Patch
{
    internal static void Postfix(IngameMenu __instance)
    {
        var scalar = __instance.GetComponent<uGUI_CanvasScaler>();
        scalar.vrMode = uGUI_CanvasScaler.Mode.Static;
    }
}