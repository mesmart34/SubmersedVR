using HarmonyLib;

namespace SubmersedVR.Patches;

// Makes the builder menu spawn infront of you in vr
[HarmonyPatch(typeof(uGUI_BuilderMenu), nameof(uGUI_BuilderMenu.Open))]
internal static class uGUI_BuilderMenu_Open_Patch
{
    internal static void Postfix(uGUI_BuilderMenu __instance)
    {
        var scalar = __instance.GetComponent<uGUI_CanvasScaler>();
        scalar.SetDirty();
        scalar.UpdateTransform(SNCameraRoot.main.guiCamera);
    }
}