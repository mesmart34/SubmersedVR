using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI_MainMenu), nameof(uGUI_MainMenu.Start))]
internal static class MainMenu_SetupVR_Patch
{
    [HarmonyPostfix]
    internal static void Postfix()
    {
        VRMainMenu.SetupMainMenu();
    }
}