using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI_PinnedRecipes), nameof(uGUI_PinnedRecipes.GetMode))]
internal static class uGUI_PinnedRecipes_GetMode_Patch
{
    internal static void Postfix(ref uGUI_PinnedRecipes.Mode __result)
    {
        if (!WristHud.isHudOn)
        {
            __result = uGUI_PinnedRecipes.Mode.Off;
        }
    }
}