using HarmonyLib;
using SubmersedVR.Tweaks;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(PlayerCinematicController), nameof(PlayerCinematicController.EndCinematicMode))]
public static class IntroSequenceFixerCompletion
{
    public static void Postfix(PlayerCinematicController __instance)
    {
        if (__instance.gameObject.name == "Life_Pod_damaged_03")
        {
            IntroFixerVR.animTime = 0f;
        }
    }
}